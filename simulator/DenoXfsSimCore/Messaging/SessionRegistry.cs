using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Protocol;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

public sealed class SessionRegistry : ISessionRegistry
{
    private readonly object _lock = new();
    private readonly List<ISessionChannel> _sessions = new();

    public void Add(ISessionChannel channel)
    {
        lock (_lock)
            _sessions.Add(channel);
    }

    public void Remove(ISessionChannel channel)
    {
        lock (_lock)
            _sessions.Remove(channel);
    }

    public IReadOnlyList<ISessionChannel> SessionsOf(string remoteAddress)
    {
        lock (_lock)
            return _sessions.Where(s => s.RemoteAddress == remoteAddress).ToList();
    }

    public IReadOnlyList<string> Terminals
    {
        get
        {
            lock (_lock)
                return _sessions.Select(s => s.RemoteAddress).Distinct().ToList();
        }
    }
}

public sealed class EventPublisher : IEventPublisher
{
    private readonly ISessionRegistry _sessions;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(ISessionRegistry sessions, ILogger<EventPublisher> logger)
    {
        _sessions = sessions;
        _logger = logger;
    }

    public async Task PublishAsync(string remoteAddress, XfsEventKind kind, uint eventId, ResultBuffer? payload, CancellationToken cancellationToken)
    {
        var sessions = _sessions.SessionsOf(remoteAddress);
        _logger.LogInformation("event terminal={Terminal} kind={Kind} eventId={EventId} sessions={Sessions} hasPayload={HasPayload}",
            remoteAddress, kind, eventId, sessions.Count, payload is not null);
        foreach (var session in sessions)
        {
            try
            {
                await session.SendAsync(WireMessageFactory.Event(kind, 0, eventId, payload), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("event send failed terminal={Terminal} eventId={EventId} reason={Reason}", remoteAddress, eventId, ex.Message);
            }
        }
    }
}

/// <summary>Execute events of one request: sent to the session that sent it, with its request id.</summary>
public sealed class ChannelExecuteEvents : IExecuteEvents
{
    private readonly ISessionChannel _channel;
    private readonly uint _requestId;
    private readonly ILogger _logger;

    public ChannelExecuteEvents(ISessionChannel channel, uint requestId, ILogger logger)
    {
        _channel = channel;
        _requestId = requestId;
        _logger = logger;
    }

    public Task SendAsync(uint eventId, ResultBuffer? payload, CancellationToken cancellationToken)
    {
        _logger.LogInformation("execute event from={Remote} reqId={ReqId} eventId={EventId} hasPayload={HasPayload}",
            _channel.RemoteAddress, _requestId, eventId, payload is not null);
        return _channel.SendAsync(WireMessageFactory.Event(XfsEventKind.Execute, _requestId, eventId, payload), cancellationToken);
    }
}
