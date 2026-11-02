using System.Text.Json;
using DenoXfs.Sim.Data;
using Microsoft.EntityFrameworkCore;

namespace DenoXfs.Sim.Idc.Forms;

/// <summary>Forms stored as a JSON array in DeviceData (DeviceClass "IDC", Category "Forms").</summary>
public sealed class EfFormCatalog : IIdcFormCatalog
{
    public const string DeviceClass = "IDC";
    public const string Category = "Forms";
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);
    private readonly IDbContextFactory<SimDbContext> _contexts;

    public EfFormCatalog(IDbContextFactory<SimDbContext> contexts)
    {
        _contexts = contexts;
    }

    public async Task<IReadOnlyList<IdcForm>> AllAsync(CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var json = await db.DeviceData.AsNoTracking()
            .Where(d => d.DeviceClass == DeviceClass && d.Category == Category)
            .Select(d => d.Json)
            .FirstOrDefaultAsync(cancellationToken);
        return json is null ? [] : Parse(json);
    }

    public async Task<IdcForm?> FindAsync(string name, CancellationToken cancellationToken) =>
        (await AllAsync(cancellationToken)).FirstOrDefault(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase));

    public static IReadOnlyList<IdcForm> Parse(string json) => JsonSerializer.Deserialize<List<IdcForm>>(json, Options) ?? [];
}
