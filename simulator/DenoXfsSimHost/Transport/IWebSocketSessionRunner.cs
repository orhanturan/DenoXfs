using System.Net.WebSockets;

namespace DenoXfs.Sim.Host.Transport;

/// <summary>Runs the receive loop of one Service Provider DLL connection (= one WFPOpen session).</summary>
public interface IWebSocketSessionRunner
{
    Task RunAsync(WebSocket socket, string remoteAddress, CancellationToken cancellationToken);
}
