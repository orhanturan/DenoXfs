using System.Text.Json.Nodes;
using DenoXfs.Sim.Core.Scenarios;
using Microsoft.EntityFrameworkCore;

namespace DenoXfs.Sim.Data;

/// <summary>Scenarios and device profiles from the Profile / ProfileSection tables (Default first, then by name).</summary>
public sealed class EfProfileCatalog : IProfileCatalog
{
    private readonly IDbContextFactory<SimDbContext> _contexts;

    public EfProfileCatalog(IDbContextFactory<SimDbContext> contexts)
    {
        _contexts = contexts;
    }

    public async Task<IReadOnlyList<ProfileDefinition>> AllAsync(ProfileKind kind, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var profiles = await db.Profiles.AsNoTracking().Include(p => p.Sections).Where(p => p.Kind == kind).ToListAsync(cancellationToken);
        return profiles
            .OrderBy(p => !string.Equals(p.Name, ProfileNames.Default, StringComparison.OrdinalIgnoreCase))
            .ThenBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .Select(ToDefinition)
            .ToList();
    }

    public async Task<ProfileDefinition?> FindAsync(ProfileKind kind, string name, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var profile = await db.Profiles.AsNoTracking().Include(p => p.Sections)
            .FirstOrDefaultAsync(p => p.Kind == kind && p.Name == name, cancellationToken); // Name has NOCASE collation
        return profile is null ? null : ToDefinition(profile);
    }

    private static ProfileDefinition ToDefinition(Profile profile) => new(
        profile.Kind, profile.Name, profile.Description, profile.BasedOn,
        profile.Sections.ToDictionary(s => s.XfsClass, s => JsonNode.Parse(s.Json) as JsonObject ?? new JsonObject()));
}

/// <summary>Terminal.ScenarioId / DeviceProfileId of the terminal of an IP; an IP without a terminal has Default and cannot save.</summary>
public sealed class EfTerminalSelectionStore : ITerminalSelectionStore
{
    private readonly IDbContextFactory<SimDbContext> _contexts;

    public EfTerminalSelectionStore(IDbContextFactory<SimDbContext> contexts)
    {
        _contexts = contexts;
    }

    public async Task<ProfileSelection> LoadAsync(string address, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var ids = await db.Terminals.AsNoTracking().OfAddress(address)
            .Select(t => new { t.ScenarioId, t.DeviceProfileId }).FirstOrDefaultAsync(cancellationToken);
        if (ids is null)
            return ProfileSelection.Default;

        return new ProfileSelection(await NameAsync(db, ids.ScenarioId, cancellationToken), await NameAsync(db, ids.DeviceProfileId, cancellationToken));
    }

    public async Task<bool> SaveAsync(string address, ProfileSelection selection, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var terminal = await db.Terminals.OfAddress(address).FirstOrDefaultAsync(cancellationToken);
        if (terminal is null)
            return false;

        terminal.ScenarioId = await IdAsync(db, ProfileKind.Scenario, selection.Scenario, cancellationToken);
        terminal.DeviceProfileId = await IdAsync(db, ProfileKind.DeviceProfile, selection.DeviceProfile, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static async Task<string> NameAsync(SimDbContext db, int? id, CancellationToken cancellationToken) =>
        id is null
            ? ProfileNames.Default
            : await db.Profiles.Where(p => p.Id == id).Select(p => p.Name).FirstOrDefaultAsync(cancellationToken) ?? ProfileNames.Default;

    /// <summary>Default is stored as null, so a terminal follows whatever Default becomes.</summary>
    private static async Task<int?> IdAsync(SimDbContext db, ProfileKind kind, string name, CancellationToken cancellationToken) =>
        string.Equals(name, ProfileNames.Default, StringComparison.OrdinalIgnoreCase)
            ? null
            : await db.Profiles.Where(p => p.Kind == kind && p.Name == name).Select(p => (int?)p.Id).FirstOrDefaultAsync(cancellationToken);
}
