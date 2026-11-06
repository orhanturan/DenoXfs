using System.Runtime.CompilerServices;
using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Names;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Ui;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

/// <summary>
/// Routes WFPExecute to the <see cref="IExecuteCommand"/> of the session's device class and sends the completion. Commands of one session
/// run one at a time (like a real device). A request ends with WFS_ERR_CANCELED when WFPCancelAsyncRequest arrives (also
/// while it still waits in the queue) and with WFS_ERR_TIMEOUT when its timeout (0 = WFS_INDEFINITE_WAIT) passes.
/// </summary>
public sealed class ExecuteMessageHandler : IMessageHandler
{
    private readonly IClassHandlers<IExecuteCommand> _commands;
    private readonly ISessionRegistry _sessions;
    private readonly IRequestCancellation _cancellation;
    private readonly INativeLayoutWriter _writer;
    private readonly ITrafficRecorder _traffic;
    private readonly ConditionalWeakTable<ISessionChannel, SemaphoreSlim> _queues = new();
    private readonly ILogger<ExecuteMessageHandler> _logger;

    public ExecuteMessageHandler(IClassHandlers<IExecuteCommand> commands, ISessionRegistry sessions, IRequestCancellation cancellation,
        INativeLayoutWriter writer, ITrafficRecorder traffic, ILogger<ExecuteMessageHandler> logger)
    {
        _commands = commands;
        _sessions = sessions;
        _cancellation = cancellation;
        _writer = writer;
        _traffic = traffic;
        _logger = logger;
    }

    public string MessageType => WireTypes.Execute;

    public async Task HandleAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken)
    {
        var reqId = WireJson.GetUInt(message, "reqId");
        var command = WireJson.GetUInt(message, "command");
        var timeoutMs = WireJson.GetUInt(message, "timeoutMs");
        message.TryGetProperty("input", out var input);
        _traffic.Request(channel.RemoteAddress, reqId, XfsCodeKind.Command, command);

        var cancelled = _cancellation.Register(channel, reqId);
        CommandOutcome outcome;
        try
        {
            outcome = await QueueAndRunAsync(channel, reqId, command, timeoutMs, input, cancelled, cancellationToken);
        }
        finally
        {
            _cancellation.Complete(channel, reqId);
        }

        var (hResult, result) = Marshal(channel, reqId, command, outcome);
        _traffic.Completion(channel.RemoteAddress, reqId, XfsCodeKind.Command, command, hResult);
        _logger.LogInformation("execute from={Remote} reqId={ReqId} command={Command} hResult={HResult} hasResult={HasResult}",
            channel.RemoteAddress, reqId, command, hResult, result is not null);
        await channel.SendAsync(WireMessageFactory.Completion(reqId, WireTypes.Execute, hResult, command, result), cancellationToken);
    }

    /// <summary>Writes the output block; an output that does not fit its layout completes the request with WFS_ERR_INTERNAL_ERROR.</summary>
    private (int HResult, ResultBuffer? Result) Marshal(ISessionChannel channel, uint reqId, uint command, CommandOutcome outcome)
    {
        try
        {
            return (outcome.HResult, _writer.Write(outcome.Output));
        }
        catch (NativeValueException ex)
        {
            _logger.LogError("execute output invalid from={Remote} reqId={ReqId} command={Command} reason={Reason}", channel.RemoteAddress, reqId, command, ex.Message);
            return (XfsResultCodes.InternalError, null);
        }
    }

    private async Task<CommandOutcome> QueueAndRunAsync(ISessionChannel channel, uint reqId, uint command, uint timeoutMs, JsonElement input,
        CancellationToken cancelled, CancellationToken sessionToken)
    {
        var queue = _queues.GetValue(channel, _ => new SemaphoreSlim(1, 1));
        using var waiting = CancellationTokenSource.CreateLinkedTokenSource(cancelled, sessionToken);
        try
        {
            await queue.WaitAsync(waiting.Token);
        }
        catch (OperationCanceledException) when (cancelled.IsCancellationRequested && !sessionToken.IsCancellationRequested)
        {
            _logger.LogInformation("execute cancelled in queue from={Remote} reqId={ReqId} command={Command}", channel.RemoteAddress, reqId, command);
            return new CommandOutcome(XfsResultCodes.Canceled, null);
        }

        try
        {
            return await RunAsync(channel, reqId, command, timeoutMs, input, cancelled, sessionToken);
        }
        finally
        {
            queue.Release();
        }
    }

    private async Task<CommandOutcome> RunAsync(ISessionChannel channel, uint reqId, uint command, uint timeoutMs, JsonElement input,
        CancellationToken cancelled, CancellationToken sessionToken)
    {
        if (_sessions.ServiceOf(channel) is not { } service || !_commands.TryGet(service.XfsClass, command, out var handler))
            return new CommandOutcome(XfsResultCodes.UnsupportedCommand, null);

        using var timeout = new CancellationTokenSource();
        if (timeoutMs > 0)
            timeout.CancelAfter(TimeSpan.FromMilliseconds(timeoutMs));
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancelled, timeout.Token, sessionToken);

        var context = new ExecuteContext(channel.RemoteAddress, new DeviceRef(service.TerminalId, service.DeviceId), service.ServiceName, command, input, new ChannelExecuteEvents(channel, reqId, _writer, _traffic, _logger));
        try
        {
            return await handler.ExecuteAsync(context, linked.Token);
        }
        catch (OperationCanceledException) when (!sessionToken.IsCancellationRequested)
        {
            var hResult = cancelled.IsCancellationRequested ? XfsResultCodes.Canceled : XfsResultCodes.Timeout;
            _logger.LogInformation("execute ended from={Remote} reqId={ReqId} command={Command} reason={Reason} timeoutMs={TimeoutMs}",
                channel.RemoteAddress, reqId, command, hResult == XfsResultCodes.Canceled ? "cancelled" : "timeout", timeoutMs);
            return new CommandOutcome(hResult, null);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "execute failed from={Remote} reqId={ReqId} command={Command}", channel.RemoteAddress, reqId, command);
            return new CommandOutcome(XfsResultCodes.InternalError, null);
        }
    }
}
