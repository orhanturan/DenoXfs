using System.Text.Json;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Marshalling;

namespace DenoXfs.Sim.Core.Abstractions;

/// <summary>
/// Input of an execute command as decoded by the DLL, plus the way back for its execute events. Device is the physical
/// device of the session's service (its state), ServiceName the logical service (e.g. Idc340).
/// </summary>
/// <remarks>RemoteAddress is the IP of the connection: scenario and device profile are found by it.</remarks>
public sealed record ExecuteContext(string RemoteAddress, DeviceRef Device, string ServiceName, uint Command, JsonElement Input, IExecuteEvents Events);

/// <summary>Result of a command or info category: XFS HRESULT plus the optional output, marshalled by the core.</summary>
public sealed record CommandOutcome(int HResult, NativeValue? Output);

/// <summary>
/// One simulated execute command of a device class (registered once per command). The cancellation token fires when
/// the request times out (the handler then reports WFS_ERR_TIMEOUT) or the session ends.
/// </summary>
public interface IExecuteCommand
{
    uint Command { get; }

    Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken);
}

/// <summary>Execute events (WFS_EXECUTE_EVENT) of the running request; they go to the window that sent it.</summary>
public interface IExecuteEvents
{
    Task SendAsync(uint eventId, NativeValue? payload, CancellationToken cancellationToken);
}
