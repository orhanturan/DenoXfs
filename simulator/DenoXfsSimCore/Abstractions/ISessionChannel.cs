namespace DenoXfs.Sim.Core.Abstractions;

/// <summary>Outgoing side of one Service Provider session (one WebSocket connection).</summary>
public interface ISessionChannel
{
    /// <summary>Remote address of the DLL; the terminal is resolved from it.</summary>
    string RemoteAddress { get; }

    Task SendAsync(object message, CancellationToken cancellationToken);
}
