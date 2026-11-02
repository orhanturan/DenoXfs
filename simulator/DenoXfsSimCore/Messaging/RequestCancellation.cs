using System.Collections.Concurrent;
using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Protocol;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

public sealed class RequestCancellation : IRequestCancellation
{
    private readonly ConcurrentDictionary<(ISessionChannel Channel, uint RequestId), CancellationTokenSource> _requests = new();

    public CancellationToken Register(ISessionChannel channel, uint requestId) =>
        _requests.GetOrAdd((channel, requestId), _ => new CancellationTokenSource()).Token;

    public bool Cancel(ISessionChannel channel, uint requestId)
    {
        var found = false;
        foreach (var (key, source) in _requests)
        {
            if (key.Channel != channel || (requestId != 0 && key.RequestId != requestId))
                continue;
            source.Cancel();
            found = true;
        }
        return found;
    }

    public void Complete(ISessionChannel channel, uint requestId)
    {
        if (_requests.TryRemove((channel, requestId), out var source))
            source.Dispose();
    }
}

/// <summary>WFPCancelAsyncRequest: {"type":"cancel","reqId":n}; the cancelled request completes with WFS_ERR_CANCELED.</summary>
public sealed class CancelMessageHandler : IMessageHandler
{
    private readonly IRequestCancellation _cancellation;
    private readonly ILogger<CancelMessageHandler> _logger;

    public CancelMessageHandler(IRequestCancellation cancellation, ILogger<CancelMessageHandler> logger)
    {
        _cancellation = cancellation;
        _logger = logger;
    }

    public string MessageType => WireTypes.Cancel;

    public Task HandleAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken)
    {
        var reqId = WireJson.GetUInt(message, "reqId");
        var found = _cancellation.Cancel(channel, reqId);
        _logger.LogInformation("cancel from={Remote} reqId={ReqId} found={Found}", channel.RemoteAddress, reqId, found);
        return Task.CompletedTask;
    }
}
