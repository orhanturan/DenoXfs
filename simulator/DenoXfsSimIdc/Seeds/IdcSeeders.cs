using System.Text.Json;
using DenoXfs.Sim.Data;
using DenoXfs.Sim.Idc.Forms;
using Microsoft.EntityFrameworkCore;

namespace DenoXfs.Sim.Idc.Seeds;

/// <summary>Seed JSON files embedded in this assembly (Seeds/*.json).</summary>
internal static class EmbeddedSeed
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static T Read<T>(string fileName)
    {
        using var stream = typeof(EmbeddedSeed).Assembly.GetManifestResourceStream($"DenoXfs.Sim.Idc.Seeds.{fileName}")
            ?? throw new InvalidOperationException($"Embedded seed {fileName} is missing.");
        return JsonSerializer.Deserialize<T>(stream, Options) ?? throw new InvalidOperationException($"Seed {fileName} is empty.");
    }
}

/// <summary>One card of Seeds/cards.json: ATR and APDUs as hex; binary track bytes as JSON \u escapes.</summary>
public sealed record CardSeed(string Name, string? Track1, string? Track2, string? Track3, string? Atr, Dictionary<string, string>? ChipResponses);

/// <summary>Adds the seed cards that are not in the database yet (by name); cards the user edited stay as they are.</summary>
public sealed class IdcCardSeeder : IDataSeeder
{
    public int Order => 100;

    public async Task SeedAsync(SimDbContext db, CancellationToken cancellationToken)
    {
        var existing = await db.Cards.Select(c => c.Name).ToListAsync(cancellationToken);
        foreach (var seed in EmbeddedSeed.Read<List<CardSeed>>("cards.json").Where(s => !existing.Contains(s.Name)))
        {
            db.Cards.Add(new Card
            {
                Name = seed.Name,
                Track1 = seed.Track1,
                Track2 = seed.Track2,
                Track3 = seed.Track3,
                Atr = seed.Atr is null ? null : Convert.FromHexString(seed.Atr),
                ChipResponses = (seed.ChipResponses ?? new()).Select(r => new CardChipResponse { Command = r.Key, Response = r.Value }).ToList()
            });
        }
        await db.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>Adds the seed forms that are missing (by name) to the IDC "Forms" device data.</summary>
public sealed class IdcFormSeeder : IDataSeeder
{
    public int Order => 110;

    public async Task SeedAsync(SimDbContext db, CancellationToken cancellationToken)
    {
        var row = await db.DeviceData.FirstOrDefaultAsync(
            d => d.DeviceClass == EfFormCatalog.DeviceClass && d.Category == EfFormCatalog.Category, cancellationToken);
        var forms = row is null ? new List<IdcForm>() : EfFormCatalog.Parse(row.Json).ToList();
        var missing = EmbeddedSeed.Read<List<IdcForm>>("forms.json")
            .Where(seed => forms.All(f => !string.Equals(f.Name, seed.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        if (missing.Count == 0)
            return;

        forms.AddRange(missing);
        if (row is null)
        {
            row = new DeviceData { DeviceClass = EfFormCatalog.DeviceClass, Category = EfFormCatalog.Category };
            db.DeviceData.Add(row);
        }
        row.Json = JsonSerializer.Serialize(forms, EmbeddedSeed.Options);
        await db.SaveChangesAsync(cancellationToken);
    }
}
