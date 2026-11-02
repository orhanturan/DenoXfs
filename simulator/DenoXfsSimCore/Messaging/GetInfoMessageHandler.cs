using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Protocol;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

/// <summary>Routes WFPGetInfo to the registered <see cref="IInfoCommand"/>; unknown categories are unsupported.</summary>
public sealed class GetInfoMessageHandler : IMessageHandler
{
    private readonly Dictionary<uint, IInfoCommand> _categories;
    private readonly IRequestCancellation _cancellation;
    private readonly ILogger<GetInfoMessageHandler> _logger;

    public GetInfoMessageHandler(IEnumerable<IInfoCommand> categories, IRequestCancellation cancellation, ILogger<GetInfoMessageHandler> logger)
    {
        _categories = categories.ToDictionary(c => c.Category);
        _cancellation = cancellation;
        _logger = logger;
    }

    public string MessageType => WireTypes.GetInfo;

    public async Task HandleAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken)
    {
        var reqId = WireJson.GetUInt(message, "reqId");
        var category = WireJson.GetUInt(message, "category");
        message.TryGetProperty("input", out var input);

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

        _logger.LogInformation("getinfo from={Remote} reqId={ReqId} category={Category} hResult={HResult} hasResult={HasResult}",
            channel.RemoteAddress, reqId, category, outcome.HResult, outcome.Result is not null);
        await channel.SendAsync(WireMessageFactory.Completion(reqId, WireTypes.GetInfo, outcome.HResult, category, outcome.Result), cancellationToken);
    }

    private async Task<CommandOutcome> RunAsync(ISessionChannel channel, uint reqId, uint category, JsonElement input,
        CancellationToken cancelled, CancellationToken sessionToken)
    {
        if (!_categories.TryGetValue(category, out var handler))
            return new CommandOutcome(XfsResultCodes.UnsupportedCategory, null);

        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancelled, sessionToken);
        try
        {
            return await handler.GetInfoAsync(new InfoContext(channel.RemoteAddress, category, input), linked.Token);
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
