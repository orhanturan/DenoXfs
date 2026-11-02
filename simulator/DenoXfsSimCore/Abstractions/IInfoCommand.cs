using System.Text.Json;

namespace DenoXfs.Sim.Core.Abstractions;

/// <summary>Query details of a WFPGetInfo as decoded by the DLL (empty object when there are none).</summary>
public sealed record InfoContext(string RemoteAddress, uint Category, JsonElement Input);

/// <summary>One simulated info category of a device class (registered once per category).</summary>
public interface IInfoCommand
{
    uint Category { get; }

    Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken);
}
