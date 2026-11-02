using System.Net.WebSockets;
using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;

namespace DenoXfs.Sim.Host.Transport;

/// <summary>
/// Receive loop of one DLL connection. Each message is dispatched on its own task so a command that waits (e.g. for a
/// card) never blocks GetInfo or other messages; the execute handler keeps the commands of the session in order.
/// </summary>
public sealed class WebSocketSessionRunner : IWebSocketSessionRunner
{
    private const int BufferSize = 16 * 1024;
    private readonly IMessageDispatcher _dispatcher;
    private readonly ISessionRegistry _sessions;
    private readonly ILogger<WebSocketSessionRunner> _logger;

    public WebSocketSessionRunner(IMessageDispatcher dispatcher, ISessionRegistry sessions, ILogger<WebSocketSessionRunner> logger)
    {
        _dispatcher = dispatcher;
        _sessions = sessions;
        _logger = logger;
    }

    public async Task RunAsync(WebSocket socket, string remoteAddress, CancellationToken cancellationToken)
    {
        var channel = new WebSocketSessionChannel(socket, remoteAddress);
        using var session = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _sessions.Add(channel);
        _logger.LogInformation("session opened from={Remote}", remoteAddress);
        try
        {
            while (socket.State == WebSocketState.Open && !session.IsCancellationRequested)
            {
                var text = await ReceiveTextAsync(socket, session.Token);
                if (text is null)
                    break;

                // Clone: the message outlives the JsonDocument of this loop iteration.
                using var document = JsonDocument.Parse(text);
                _ = DispatchAsync(channel, document.RootElement.Clone(), session.Token);
            }
        }
        catch (WebSocketException ex)
        {
            _logger.LogWarning("session broken from={Remote} reason={Reason}", remoteAddress, ex.Message);
        }
        catch (OperationCanceledException)
        {
            // The client went away without a closing handshake (process exit, network loss): the request was aborted.
            _logger.LogInformation("session aborted from={Remote} reason=client-disconnected", remoteAddress);
        }
        finally
        {
            // Ends the commands that still wait (their completion can no longer be delivered).
            session.Cancel();
            _sessions.Remove(channel);
            _logger.LogInformation("session closed from={Remote}", remoteAddress);
        }
    }

    private async Task DispatchAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken)
    {
        try
        {
            await _dispatcher.DispatchAsync(channel, message, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("message dropped from={Remote} reason=session-ended", channel.RemoteAddress);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "message failed from={Remote}", channel.RemoteAddress);
        }
    }

    private static async Task<string?> ReceiveTextAsync(WebSocket socket, CancellationToken cancellationToken)
    {
        var buffer = new byte[BufferSize];
        using var message = new MemoryStream();
        WebSocketReceiveResult result;
        do
        {
            result = await socket.ReceiveAsync(buffer, cancellationToken);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                return null;
            }
            message.Write(buffer, 0, result.Count);
        }
        while (!result.EndOfMessage);

        return System.Text.Encoding.UTF8.GetString(message.ToArray());
    }
}
