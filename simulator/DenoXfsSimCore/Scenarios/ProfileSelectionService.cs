using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Scenarios;

/// <summary>The selection of every terminal, read often (each Status, each command), so kept in memory over the store.</summary>
public interface ITerminalSelections
{
    Task<ProfileSelection> GetAsync(string address, CancellationToken cancellationToken);

    /// <summary>False when no terminal is defined for the address (nothing is stored).</summary>
    Task<bool> SetAsync(string address, ProfileSelection selection, CancellationToken cancellationToken);

    /// <summary>Drops the cached selection of an address, e.g. after its terminal was added, moved or deleted.</summary>
    void Forget(string address);
}

public sealed class CachedTerminalSelections : ITerminalSelections
{
    private readonly ITerminalSelectionStore _store;
    private readonly ConcurrentDictionary<string, ProfileSelection> _cache = new();

    public CachedTerminalSelections(ITerminalSelectionStore store)
    {
        _store = store;
    }

    public async Task<ProfileSelection> GetAsync(string address, CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(address, out var selection))
            return selection;
        selection = await _store.LoadAsync(address, cancellationToken);
        return _cache.GetOrAdd(address, selection);
    }

    public async Task<bool> SetAsync(string address, ProfileSelection selection, CancellationToken cancellationToken)
    {
        if (!await _store.SaveAsync(address, selection, cancellationToken))
            return false;
        _cache[address] = selection;
        return true;
    }

    public void Forget(string address) => _cache.TryRemove(address, out _);
}

/// <summary>Web page: a terminal selects its scenario and device profile.</summary>
public interface IProfileSelectionService
{
    /// <summary>Null when done; otherwise why not (unknown name, terminal not defined).</summary>
    Task<string?> SelectAsync(string address, ProfileSelection selection, CancellationToken cancellationToken);
}

public sealed class ProfileSelectionService : IProfileSelectionService
{
    private readonly IProfileCatalog _catalog;
    private readonly ITerminalSelections _selections;
    private readonly IEnumerable<IProfileSelectionListener> _listeners;
    private readonly ILogger<ProfileSelectionService> _logger;

    public ProfileSelectionService(IProfileCatalog catalog, ITerminalSelections selections, IEnumerable<IProfileSelectionListener> listeners,
        ILogger<ProfileSelectionService> logger)
    {
        _catalog = catalog;
        _selections = selections;
        _listeners = listeners;
        _logger = logger;
    }

    public async Task<string?> SelectAsync(string address, ProfileSelection selection, CancellationToken cancellationToken)
    {
        var scenario = await _catalog.FindAsync(ProfileKind.Scenario, selection.Scenario, cancellationToken);
        if (scenario is null)
            return $"Unknown scenario '{selection.Scenario}'.";
        var deviceProfile = await _catalog.FindAsync(ProfileKind.DeviceProfile, selection.DeviceProfile, cancellationToken);
        if (deviceProfile is null)
            return $"Unknown device profile '{selection.DeviceProfile}'.";

        var before = await _selections.GetAsync(address, cancellationToken);
        var after = new ProfileSelection(scenario.Name, deviceProfile.Name);
        if (before == after)
            return null;

        if (!await _selections.SetAsync(address, after, cancellationToken))
            return $"No terminal is defined for {address}.";
        _logger.LogInformation("selection terminal={Terminal} scenario={Scenario} deviceProfile={DeviceProfile} (was {OldScenario}/{OldDeviceProfile})",
            address, after.Scenario, after.DeviceProfile, before.Scenario, before.DeviceProfile);
        foreach (var listener in _listeners)
            await listener.OnSelectionChangedAsync(address, before, after, cancellationToken);
        return null;
    }
}
