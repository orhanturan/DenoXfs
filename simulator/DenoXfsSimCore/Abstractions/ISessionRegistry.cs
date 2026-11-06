using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;

namespace DenoXfs.Sim.Core.Abstractions;

/// <summary>
/// The Service Provider sessions (one per DLL connection), grouped by terminal (the remote address of the DLL). A session
/// is bound to its service when WFPOpen resolved the logical name; only then commands, infos and events reach it.
/// </summary>
public interface ISessionRegistry
{
    void Add(ISessionChannel channel);

    void Remove(ISessionChannel channel);

    /// <summary>WFPOpen succeeded: the session talks to this service (its class picks the module).</summary>
    void Bind(ISessionChannel channel, ResolvedService service);

    /// <summary>The service of an opened session; null before or after a rejected WFPOpen.</summary>
    ResolvedService? ServiceOf(ISessionChannel channel);

    /// <summary>The opened sessions of a terminal.</summary>
    IReadOnlyList<ISessionChannel> SessionsOf(string remoteAddress);

    /// <summary>The opened sessions of the service of this XFS class on this device (a recycler's CIM events never reach its CDM sessions).</summary>
    IReadOnlyList<ISessionChannel> SessionsOf(DeviceRef device, string xfsClass);

    IReadOnlyList<string> Terminals { get; }
}

/// <summary>
/// Sends service, user and system events of one device and class to the sessions of that device's service of that
/// class (the second card reader's sessions do not see the first one's events). Each DLL session posts them only to the
/// windows registered for that event class.
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync(DeviceRef device, string xfsClass, XfsEventKind kind, uint eventId, NativeValue? payload, CancellationToken cancellationToken);
}
