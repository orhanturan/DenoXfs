using System;
using System.Threading.Tasks;
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

    public async Task<ExecuteResponse> ExecuteAsync(ExecuteRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

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

    public async Task<GetInfoResponse> GetInfoAsync(GetInfoRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

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
}

