using System.Collections.Concurrent;
using System.Text.Json.Nodes;

namespace DenoXfs.Sim.Core.Scenarios;

/// <summary>The effective section of a profile for one device class: Default ⊕ BasedOn chain ⊕ the profile.</summary>
public interface IEffectiveProfileResolver
{
    /// <summary>A copy the caller may change. An unknown name resolves like Default.</summary>
    Task<JsonObject> ResolveAsync(ProfileKind kind, string name, string deviceClass, CancellationToken cancellationToken);

    /// <summary>The section of the profile the terminal has selected.</summary>
    Task<JsonObject> ResolveForTerminalAsync(string address, ProfileKind kind, string deviceClass, CancellationToken cancellationToken);

    /// <summary>Forgets the cached sections (after a profile was edited).</summary>
    void Invalidate();
}

public sealed class EffectiveProfileResolver : IEffectiveProfileResolver
{
    private readonly IProfileCatalog _catalog;
    private readonly ITerminalSelections _selections;
    private readonly IJsonMerger _merger;
    private readonly ConcurrentDictionary<(ProfileKind, string, string), JsonObject> _cache = new();

    public EffectiveProfileResolver(IProfileCatalog catalog, ITerminalSelections selections, IJsonMerger merger)
    {
        _catalog = catalog;
        _selections = selections;
        _merger = merger;
    }

    public async Task<JsonObject> ResolveAsync(ProfileKind kind, string name, string deviceClass, CancellationToken cancellationToken)
    {
        var key = (kind, name.ToUpperInvariant(), deviceClass);
        if (!_cache.TryGetValue(key, out var section))
        {
            section = await BuildAsync(kind, name, deviceClass, cancellationToken);
            _cache[key] = section;
        }
        return (JsonObject)section.DeepClone();
    }

    public async Task<JsonObject> ResolveForTerminalAsync(string address, ProfileKind kind, string deviceClass, CancellationToken cancellationToken)
    {
        var selection = await _selections.GetAsync(address, cancellationToken);
        return await ResolveAsync(kind, selection.NameOf(kind), deviceClass, cancellationToken);
    }

    public void Invalidate() => _cache.Clear();

    private async Task<JsonObject> BuildAsync(ProfileKind kind, string name, string deviceClass, CancellationToken cancellationToken)
    {
        // Walk name -> BasedOn -> ... (stops at Default, an unknown name or a cycle), then apply from Default down to name.
        var chain = new List<ProfileDefinition>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ProfileNames.Default };
        for (var current = name; seen.Add(current);)
        {
            var profile = await _catalog.FindAsync(kind, current, cancellationToken);
            if (profile is null)
                break;
            chain.Add(profile);
            if (profile.BasedOn is not { } basedOn)
                break;
            current = basedOn;
        }
        if (await _catalog.FindAsync(kind, ProfileNames.Default, cancellationToken) is { } defaults)
            chain.Add(defaults);

        var result = new JsonObject();
        for (var i = chain.Count - 1; i >= 0; i--)
        {
            if (chain[i].Sections.TryGetValue(deviceClass, out var section))
                _merger.Merge(result, section);
        }
        return result;
    }
}
