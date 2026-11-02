using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models;
using DenoXfs.Api;
using DenoXfs.Api.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Core.Services;

public class XfsMessageProcessor : IXfsMessageProcessor, IXfsEventSource, IHostedService
{
    private readonly IXfsResultReleaser resultReleaser;
    private readonly ILogger<XfsMessageProcessor> logger;

    private readonly Queue<IXfsCommand> commandQueue = new Queue<IXfsCommand>();
    private readonly object queueLock = new object();
    private readonly ConcurrentDictionary<uint, IXfsCommand> pendingCommands = new ConcurrentDictionary<uint, IXfsCommand>();
    private readonly MessageWindow messageWindow = new MessageWindow();
    private Thread? messageThread;
    private IntPtr windowHandle = IntPtr.Zero;
    private bool isRunning = false;

    public XfsMessageProcessor(IXfsResultReleaser resultReleaser, ILogger<XfsMessageProcessor> logger)
    {
        this.resultReleaser = resultReleaser ?? throw new ArgumentNullException(nameof(resultReleaser));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IntPtr WindowHandle => windowHandle;

    public event Action<XfsEventMessage>? EventReceived;

    public void EnqueueCommand(IXfsCommand command)
    {
        lock (queueLock)
        {
            commandQueue.Enqueue(command);
        }

        if (windowHandle != IntPtr.Zero)
        {
            PostMessage(windowHandle, Xfs.WmXfsProcessRequest, IntPtr.Zero, IntPtr.Zero);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (isRunning)
            return Task.CompletedTask;

        isRunning = true;
        messageThread = new Thread(MessageLoop)
        {
            IsBackground = false
        };
        messageThread.Start();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (!isRunning)
            return Task.CompletedTask;

        isRunning = false;

        messageWindow.PostQuit(windowHandle);

        messageThread?.Join(5000);
        messageThread = null;

        messageWindow.Destroy(windowHandle);
        windowHandle = IntPtr.Zero;

        return Task.CompletedTask;
    }

    private void MessageLoop()
    {
        windowHandle = messageWindow.Create();
        if (windowHandle == IntPtr.Zero)
        {
            logger.LogError("message window could not be created win32Error={Error}", Marshal.GetLastPInvokeError());
            isRunning = false;
            return;
        }

        logger.LogInformation("message window created handle=0x{Handle:X}", windowHandle.ToInt64());

        while (isRunning)
        {
            int result = GetMessage(out MSG msg, windowHandle, 0, 0);
            if (result == 0) // WM_QUIT
            {
                break;
            }

            if (msg.message == Xfs.WmXfsProcessRequest)
            {
                ProcessCommand();
            }
            else if (msg.message >= Xfs.OpenComplete && msg.message <= Xfs.TimerEvent)
            {
                ProcessXfsMessage(msg);
            }

            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
        }

        isRunning = false;
    }

    private void ProcessCommand()
    {
        IXfsCommand? command = null;

        lock (queueLock)
        {
            if (commandQueue.Count > 0)
            {
                command = commandQueue.Dequeue();
            }
        }

        if (command == null)
            return;

        try
        {
            var response = command.Execute();
            
            // If command succeeded (WFS_SUCCESS = 0), add to pending commands
            if (response.Result == 0)
            {
                pendingCommands.TryAdd(response.RequestId, command);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "command failed type={Type}", command.GetType().Name);
        }
    }

    private void ProcessXfsMessage(MSG msg)
    {
        // The WFSRESULT is in msg.lParam (wParam is not used). Whoever ends up with it releases it with WFSFreeResult.
        if (msg.lParam == IntPtr.Zero)
            return;

        bool released = false;
        try
        {
            var wfsResult = Marshal.PtrToStructure<XfsResult>(msg.lParam);

            if (IsCompletionMessage(msg.message))
            {
                if (pendingCommands.TryRemove(wfsResult.RequestID, out var command))
                {
                    released = command.ProcessResult(wfsResult, msg.lParam);
                }
                else
                {
                    logger.LogWarning("completion without pending command message={Message} reqId={ReqId}", msg.message, wfsResult.RequestID);
                }
            }
            else if (TryGetEventKind(msg.message, out var kind))
            {
                var xfsEvent = new XfsEventMessage(kind, wfsResult.EventID, wfsResult.Service, wfsResult.RequestID, wfsResult.Result, wfsResult.Buffer);
                logger.LogInformation("event kind={Kind} eventId={EventId} hService={HService} reqId={ReqId} hResult={HResult}",
                    kind, xfsEvent.EventId, xfsEvent.HService, xfsEvent.RequestId, xfsEvent.Result);
                EventReceived?.Invoke(xfsEvent);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "message handling failed message={Message}", msg.message);
        }
        finally
        {
            if (!released)
                resultReleaser.Free(msg.lParam);
        }
    }

    private static bool TryGetEventKind(uint message, out XfsEventKind kind)
    {
        kind = message switch
        {
            Xfs.ExecuteEvent => XfsEventKind.Execute,
            Xfs.ServiceEvent => XfsEventKind.Service,
            Xfs.UserEvent => XfsEventKind.User,
            Xfs.SystemEvent => XfsEventKind.System,
            _ => default
        };
        return message is Xfs.ExecuteEvent or Xfs.ServiceEvent or Xfs.UserEvent or Xfs.SystemEvent;
    }

    private static bool IsCompletionMessage(uint message)
    {
        return message == Xfs.OpenComplete ||
               message == Xfs.CloseComplete||
               message == Xfs.LockComplete||
               message == Xfs.UnlockComplete||
               message == Xfs.RegisterComplete||
               message == Xfs.DeregisterComplete||
               message == Xfs.GetinfoComplete||
               message == Xfs.ExecuteComplete;
    }

    // Windows API P/Invoke declarations
    [DllImport("user32.dll")]
    private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    private static extern bool TranslateMessage(ref MSG lpMsg);

    [DllImport("user32.dll")]
    private static extern IntPtr DispatchMessage(ref MSG lpMsg);

    [StructLayout(LayoutKind.Sequential)]
    private struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }
}

