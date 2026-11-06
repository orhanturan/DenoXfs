using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Names;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Ui;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

/// <summary>Routes WFPGetInfo to the <see cref="IInfoCommand"/> of the session's device class; unknown categories are unsupported.</summary>
public sealed class GetInfoMessageHandler : IMessageHandler
{
    private readonly IClassHandlers<IInfoCommand> _categories;
    private readonly ISessionRegistry _sessions;
    private readonly IRequestCancellation _cancellation;
    private readonly INativeLayoutWriter _writer;
    private readonly ITrafficRecorder _traffic;
    private readonly ILogger<GetInfoMessageHandler> _logger;

    public GetInfoMessageHandler(IClassHandlers<IInfoCommand> categories, ISessionRegistry sessions, IRequestCancellation cancellation,
        INativeLayoutWriter writer, ITrafficRecorder traffic, ILogger<GetInfoMessageHandler> logger)
    {
        _categories = categories;
        _sessions = sessions;
        _cancellation = cancellation;
        _writer = writer;
        _traffic = traffic;
        _logger = logger;
    }

    public string MessageType => WireTypes.GetInfo;

    public async Task HandleAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken)
    {
        var reqId = WireJson.GetUInt(message, "reqId");
        var category = WireJson.GetUInt(message, "category");
        message.TryGetProperty("input", out var input);
        _traffic.Request(channel.RemoteAddress, reqId, XfsCodeKind.Info, category);

        var cancelled = _cancellation.Register(channel, reqId);
        CommandOutcome outcome;
        try
        {
            outcome = await RunAsync(channel, reqId, category, input, cancelled, cancellationToken);
        }
        finally
        {
            _cancellation.Complete(channel, reqId);
        }

        var (hResult, result) = Marshal(channel, reqId, category, outcome);
        _traffic.Completion(channel.RemoteAddress, reqId, XfsCodeKind.Info, category, hResult);
        _logger.LogInformation("getinfo from={Remote} reqId={ReqId} category={Category} hResult={HResult} hasResult={HasResult}",
            channel.RemoteAddress, reqId, category, hResult, result is not null);
        await channel.SendAsync(WireMessageFactory.Completion(reqId, WireTypes.GetInfo, hResult, category, result), cancellationToken);
    }

    /// <summary>Writes the output block; an output that does not fit its layout completes the request with WFS_ERR_INTERNAL_ERROR.</summary>
    private (int HResult, ResultBuffer? Result) Marshal(ISessionChannel channel, uint reqId, uint category, CommandOutcome outcome)
    {
        try
        {
            return (outcome.HResult, _writer.Write(outcome.Output));
        }
        catch (NativeValueException ex)
        {
            _logger.LogError("getinfo output invalid from={Remote} reqId={ReqId} category={Category} reason={Reason}", channel.RemoteAddress, reqId, category, ex.Message);
            return (XfsResultCodes.InternalError, null);
        }
    }

    private async Task<CommandOutcome> RunAsync(ISessionChannel channel, uint reqId, uint category, JsonElement input,
        CancellationToken cancelled, CancellationToken sessionToken)
    {
        if (_sessions.ServiceOf(channel) is not { } service || !_categories.TryGet(service.XfsClass, category, out var handler))
            return new CommandOutcome(XfsResultCodes.UnsupportedCategory, null);

        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancelled, sessionToken);
        try
        {
            return await handler.GetInfoAsync(new InfoContext(channel.RemoteAddress, new DeviceRef(service.TerminalId, service.DeviceId), category, input), linked.Token);
        }
        catch (OperationCanceledException) when (cancelled.IsCancellationRequested && !sessionToken.IsCancellationRequested)
        {
            return new CommandOutcome(XfsResultCodes.Canceled, null);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "getinfo failed from={Remote} reqId={ReqId} category={Category}", channel.RemoteAddress, reqId, category);
            return new CommandOutcome(XfsResultCodes.InternalError, null);
        }
    }
}
