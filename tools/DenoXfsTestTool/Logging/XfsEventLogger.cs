using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models;
using DenoXfs.TestTool.Operations;
using Microsoft.Extensions.Hosting;

namespace DenoXfs.TestTool.Logging;

/// <summary>Writes every XFS event the session registered for, with its decoded payload, to the XFS log.</summary>
public class XfsEventLogger : IHostedService
{
    private readonly IXfsEventSource _events;
    private readonly IXfsTrafficLog _log;
    private readonly IEventPayloadDecoders _decoders;

    public XfsEventLogger(IXfsEventSource events, IXfsTrafficLog log, IEventPayloadDecoders decoders)
    {
        _events = events;
        _log = log;
        _decoders = decoders;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _events.EventReceived += OnEvent;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _events.EventReceived -= OnEvent;
        return Task.CompletedTask;
    }

    // Runs on the message thread while the WFSRESULT is still allocated: decode and dump before returning.
    private void OnEvent(XfsEventMessage message)
    {
        object? payload;
        try
        {
            payload = _decoders.Decode(Kind(message.Kind), message.EventId, message.Buffer);
        }
        catch (Exception ex)
        {
            payload = $"(payload decode failed: {ex.Message})";
        }
        _log.Event(message, payload);
    }

    private static XfsCodeKind Kind(XfsEventKind kind) => kind switch
    {
        XfsEventKind.Execute => XfsCodeKind.ExecuteEvent,
        XfsEventKind.Service => XfsCodeKind.ServiceEvent,
        XfsEventKind.User => XfsCodeKind.UserEvent,
        _ => XfsCodeKind.SystemEvent
    };
}
