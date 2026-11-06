using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Ui;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

public sealed class SessionRegistry : ISessionRegistry
{
    private readonly object _lock = new();
    private readonly Dictionary<ISessionChannel, ResolvedService?> _sessions = new();
    private readonly ITrafficRecorder _traffic;

    public SessionRegistry(ITrafficRecorder traffic)
    {
        _traffic = traffic;
    }

    public void Add(ISessionChannel channel)
    {
        lock (_lock)
            _sessions[channel] = null;
    }

    public void Remove(ISessionChannel channel)
    {
        ResolvedService? service;
        lock (_lock)
        {
            _sessions.Remove(channel, out service);
        }
        if (service is not null)
            _traffic.Session(channel.RemoteAddress, $"session closed {service.ServiceName} ({service.XfsClass})");
    }

    public void Bind(ISessionChannel channel, ResolvedService service)
    {
        lock (_lock)
            _sessions[channel] = service;
        _traffic.Session(channel.RemoteAddress, $"session opened {service.ServiceName} ({service.XfsClass}, device {service.DeviceName})");
    }

    public ResolvedService? ServiceOf(ISessionChannel channel)
    {
        lock (_lock)
            return _sessions.GetValueOrDefault(channel);
    }

    public IReadOnlyList<ISessionChannel> SessionsOf(string remoteAddress)
    {
        lock (_lock)
            return _sessions.Where(s => s.Value is not null && s.Key.RemoteAddress == remoteAddress).Select(s => s.Key).ToList();
    }

    public IReadOnlyList<ISessionChannel> SessionsOf(DeviceRef device, string xfsClass)
    {
        lock (_lock)
            return _sessions
                .Where(s => s.Value?.TerminalId == device.TerminalId && s.Value.DeviceId == device.DeviceId &&
                            string.Equals(s.Value.XfsClass, xfsClass, StringComparison.OrdinalIgnoreCase))
                .Select(s => s.Key)
                .ToList();
    }

    public IReadOnlyList<string> Terminals
    {
        get
        {
            lock (_lock)
                return _sessions.Where(s => s.Value is not null).Select(s => s.Key.RemoteAddress).Distinct().ToList();
        }
    }
}

public sealed class EventPublisher : IEventPublisher
{
    private readonly ISessionRegistry _sessions;
    private readonly INativeLayoutWriter _writer;
    private readonly ITrafficRecorder _traffic;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(ISessionRegistry sessions, INativeLayoutWriter writer, ITrafficRecorder traffic, ILogger<EventPublisher> logger)
    {
        _sessions = sessions;
        _writer = writer;
        _traffic = traffic;
        _logger = logger;
    }

    public async Task PublishAsync(DeviceRef device, string xfsClass, XfsEventKind kind, uint eventId, NativeValue? payload,
        CancellationToken cancellationToken)
    {
        var sessions = _sessions.SessionsOf(device, xfsClass);
        _logger.LogInformation("event device={Device} class={Class} kind={Kind} eventId={EventId} sessions={Sessions} hasPayload={HasPayload}",
            device, xfsClass, kind, eventId, sessions.Count, payload?.Value is not null);
        var buffer = _writer.Write(payload);
        foreach (var address in sessions.Select(s => s.RemoteAddress).Distinct())
            _traffic.Event(address, 0, kind.ToString().ToLowerInvariant(), eventId);
        foreach (var session in sessions)
        {
            try
            {
                await session.SendAsync(WireMessageFactory.Event(kind, 0, eventId, buffer), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("event send failed device={Device} eventId={EventId} reason={Reason}", device, eventId, ex.Message);
            }
        }
    }
}

/// <summary>Execute events of one request: sent to the session that sent it, with its request id.</summary>
public sealed class ChannelExecuteEvents : IExecuteEvents
{
    private readonly ISessionChannel _channel;
    private readonly uint _requestId;
    private readonly INativeLayoutWriter _writer;
    private readonly ITrafficRecorder _traffic;
    private readonly ILogger _logger;

    public ChannelExecuteEvents(ISessionChannel channel, uint requestId, INativeLayoutWriter writer, ITrafficRecorder traffic, ILogger logger)
    {
        _channel = channel;
        _requestId = requestId;
        _writer = writer;
        _traffic = traffic;
        _logger = logger;
    }

    public Task SendAsync(uint eventId, NativeValue? payload, CancellationToken cancellationToken)
    {
        _logger.LogInformation("execute event from={Remote} reqId={ReqId} eventId={EventId} hasPayload={HasPayload}",
            _channel.RemoteAddress, _requestId, eventId, payload?.Value is not null);
        _traffic.Event(_channel.RemoteAddress, _requestId, "execute", eventId);
        return _channel.SendAsync(WireMessageFactory.Event(XfsEventKind.Execute, _requestId, eventId, _writer.Write(payload)), cancellationToken);
    }
}
