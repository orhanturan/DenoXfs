using System.Net.WebSockets;
using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Messaging;

namespace DenoXfs.Sim.Host.Transport;

/// <summary>Sends JSON text frames over one WebSocket; sends are serialized.</summary>
public sealed class WebSocketSessionChannel : ISessionChannel
{
    private readonly WebSocket _socket;
    private readonly SemaphoreSlim _sendLock = new(1, 1);

    public WebSocketSessionChannel(WebSocket socket, string remoteAddress)
    {
        _socket = socket;
        RemoteAddress = remoteAddress;
    }

    public string RemoteAddress { get; }

    public async Task SendAsync(object message, CancellationToken cancellationToken)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(message, message.GetType(), WireJson.Options);
        await _sendLock.WaitAsync(cancellationToken);
        try
        {
            await _socket.SendAsync(bytes, WebSocketMessageType.Text, true, cancellationToken);
        }
        finally
        {
            _sendLock.Release();
        }
    }
}
