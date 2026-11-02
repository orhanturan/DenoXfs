using System;
using System.Threading.Tasks;
using DenoXfs.Api;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Commands;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Services;

public class XfsService : IXfsService
{
    private readonly IXfsApp xfsApp;
    private readonly IXfsMessageProcessor messageProcessor;
    private readonly ICommandBuilderFactory builderFactory;

    public XfsService(IXfsApp xfsApp, IXfsMessageProcessor messageProcessor, ICommandBuilderFactory builderFactory)
    {
        this.xfsApp = xfsApp ?? throw new ArgumentNullException(nameof(xfsApp));
        this.messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
        this.builderFactory = builderFactory ?? throw new ArgumentNullException(nameof(builderFactory));
    }

    public async Task<OpenResponse> OpenAsync(OpenRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        EnsureMessageWindow();

        var tcs = new TaskCompletionSource<OpenResponse>();

        var command = builderFactory.Create<IOpenCommandBuilder>()
            .WithOpenRequest(request)
            .WithXfsApp(xfsApp)
            .WithWindowHandle(messageProcessor.WindowHandle)
            .WithTaskCompletionSource(tcs)
            .Build();

        messageProcessor.EnqueueCommand(command);
        return await tcs.Task;
    }

    public async Task<CloseResponse> CloseAsync(CloseRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        EnsureMessageWindow();

        var tcs = new TaskCompletionSource<CloseResponse>();

        var command = builderFactory.Create<ICloseCommandBuilder>()
            .WithCloseRequest(request)
            .WithXfsApp(xfsApp)
            .WithWindowHandle(messageProcessor.WindowHandle)
            .WithTaskCompletionSource(tcs)
            .Build();

        messageProcessor.EnqueueCommand(command);
        return await tcs.Task;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        EnsureMessageWindow();

        var tcs = new TaskCompletionSource<RegisterResponse>();

        var command = builderFactory.Create<IRegisterCommandBuilder>()
            .WithRegisterRequest(request)
            .WithXfsApp(xfsApp)
            .WithWindowHandle(messageProcessor.WindowHandle)
            .WithTaskCompletionSource(tcs)
            .Build();

        messageProcessor.EnqueueCommand(command);
        return await tcs.Task;
    }

    public async Task<ExecuteResponse> ExecuteAsync(ExecuteRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        EnsureMessageWindow();

        var tcs = new TaskCompletionSource<ExecuteResponse>();

        var command = builderFactory.Create<IExecuteCommandBuilder>()
            .WithExecuteRequest(request)
            .WithXfsApp(xfsApp)
            .WithWindowHandle(messageProcessor.WindowHandle)
            .WithTaskCompletionSource(tcs)
            .Build();

        messageProcessor.EnqueueCommand(command);
        return await tcs.Task;
    }

    public int CancelAsyncRequest(ushort hService, uint requestId) => XfsApi.CancelAsyncRequest(hService, requestId);

    public async Task<GetInfoResponse> GetInfoAsync(GetInfoRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        EnsureMessageWindow();

        var tcs = new TaskCompletionSource<GetInfoResponse>();

        var command = builderFactory.Create<IGetInfoCommandBuilder>()
            .WithGetInfoRequest(request)
            .WithXfsApp(xfsApp)
            .WithWindowHandle(messageProcessor.WindowHandle)
            .WithTaskCompletionSource(tcs)
            .Build();

        messageProcessor.EnqueueCommand(command);
        return await tcs.Task;
    }

    // Without the hidden message window nothing would ever process a queued command, so fail instead of waiting forever.
    private void EnsureMessageWindow()
    {
        if (messageProcessor.WindowHandle == IntPtr.Zero)
            throw new InvalidOperationException("The XFS message window is not available (see the log for the window creation error).");
    }
}
