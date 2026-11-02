using DenoXfs.Sim.Core.Protocol;

namespace DenoXfs.Sim.Core.Abstractions;

/// <summary>The open Service Provider sessions, grouped by terminal (the remote address of the DLL).</summary>
public interface ISessionRegistry
{
    void Add(ISessionChannel channel);

    void Remove(ISessionChannel channel);

    IReadOnlyList<ISessionChannel> SessionsOf(string remoteAddress);

    IReadOnlyList<string> Terminals { get; }
}

/// <summary>
/// Sends service, user and system events of a terminal to every session of it. Each DLL session posts them only to the
/// windows registered for that event class.
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync(string remoteAddress, XfsEventKind kind, uint eventId, ResultBuffer? payload, CancellationToken cancellationToken);
}
