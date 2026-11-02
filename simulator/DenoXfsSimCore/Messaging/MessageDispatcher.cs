using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

public sealed class MessageDispatcher : IMessageDispatcher
{
    private readonly Dictionary<string, IMessageHandler> _handlers;
    private readonly ILogger<MessageDispatcher> _logger;

    public MessageDispatcher(IEnumerable<IMessageHandler> handlers, ILogger<MessageDispatcher> logger)
    {
        _handlers = handlers.ToDictionary(h => h.MessageType, StringComparer.Ordinal);
        _logger = logger;
    }

    public Task DispatchAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken)
    {
        var type = WireJson.GetString(message, "type");
        if (_handlers.TryGetValue(type, out var handler))
            return handler.HandleAsync(channel, message, cancellationToken);

        _logger.LogWarning("unknown message type={Type} from={Remote}", type, channel.RemoteAddress);
        return Task.CompletedTask;
    }
}
