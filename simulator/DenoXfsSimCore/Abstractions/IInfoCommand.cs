using System.Text.Json;
using DenoXfs.Sim.Core.Hardware;

namespace DenoXfs.Sim.Core.Abstractions;

/// <summary>Query details of a WFPGetInfo as decoded by the DLL (empty object when there are none) for the session's device.</summary>
/// <remarks>RemoteAddress is the IP of the connection: scenario and device profile are found by it.</remarks>
public sealed record InfoContext(string RemoteAddress, DeviceRef Device, uint Category, JsonElement Input);

/// <summary>One simulated info category of a device class (registered once per category).</summary>
public interface IInfoCommand
{
    uint Category { get; }

    Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken);
}
