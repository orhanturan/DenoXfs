using System.Text.Json.Nodes;

namespace DenoXfs.Sim.Core.Scenarios;

/// <summary>
/// The two things a terminal selects: the <see cref="Scenario"/> (behaviour and health: status, command rules, card source)
/// and the <see cref="DeviceProfile"/> (hardware: capabilities, forms).
/// </summary>
public enum ProfileKind
{
    Scenario,
    DeviceProfile
}

public static class ProfileNames
{
    /// <summary>The base of every profile of a kind; a terminal without a selection uses it.</summary>
    public const string Default = "Default";
}

/// <summary>
/// A scenario or device profile: one JSON section per device class (key e.g. "IDC"). A section holds only what differs
/// from <see cref="BasedOn"/> (or from Default); the effective section is the merge of the chain.
/// </summary>
public sealed record ProfileDefinition(
    ProfileKind Kind, string Name, string? Description, string? BasedOn, IReadOnlyDictionary<string, JsonObject> Sections);

/// <summary>What a terminal has selected (names; Default when it never selected anything).</summary>
public sealed record ProfileSelection(string Scenario, string DeviceProfile)
{
    public static readonly ProfileSelection Default = new(ProfileNames.Default, ProfileNames.Default);

    public string NameOf(ProfileKind kind) => kind == ProfileKind.Scenario ? Scenario : DeviceProfile;
}

public interface IProfileCatalog
{
    Task<IReadOnlyList<ProfileDefinition>> AllAsync(ProfileKind kind, CancellationToken cancellationToken);

    Task<ProfileDefinition?> FindAsync(ProfileKind kind, string name, CancellationToken cancellationToken);
}

/// <summary>Stores the selection of each terminal (survives a host restart).</summary>
public interface ITerminalSelectionStore
{
    /// <summary>The stored selection; names that no longer exist come back as Default.</summary>
    Task<ProfileSelection> LoadAsync(string address, CancellationToken cancellationToken);

    /// <summary>False when no terminal is defined for the address.</summary>
    Task<bool> SaveAsync(string address, ProfileSelection selection, CancellationToken cancellationToken);
}

/// <summary>Told after a terminal changed its selection, e.g. to publish WFS_SYSE_DEVICE_STATUS when the device status changed.</summary>
public interface IProfileSelectionListener
{
    Task OnSelectionChangedAsync(string address, ProfileSelection before, ProfileSelection after, CancellationToken cancellationToken);
}
