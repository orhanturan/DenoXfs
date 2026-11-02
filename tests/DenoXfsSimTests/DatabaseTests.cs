using DenoXfs.Sim.Data;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Forms;
using DenoXfs.Sim.Idc.Seeds;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DenoXfs.Sim.Tests;

/// <summary>EF catalogs, seeders and the device state store against an in-memory SQLite database with the real migrations.</summary>
public sealed class DatabaseTests : IDisposable
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    private readonly TestContextFactory _contexts;

    public DatabaseTests()
    {
        _connection.Open(); // the in-memory database lives as long as this connection
        _contexts = new TestContextFactory(new DbContextOptionsBuilder<SimDbContext>().UseSqlite(_connection).Options);
        using var db = _contexts.CreateDbContext();
        db.Database.Migrate();
    }

    public void Dispose() => _connection.Dispose();

    [Fact]
    public async Task Seeders_fill_cards_and_forms_and_running_them_again_adds_nothing()
    {
        await SeedAsync();
        await SeedAsync();

        var cards = await new EfCardCatalog(_contexts).AllAsync(CancellationToken.None);
        Assert.Equal(4, cards.Count);
        var binary = cards.Single(c => c.Name == "Track 2 with binary bytes");
        Assert.Equal('\u0001', binary.Track2![0]);
        Assert.Equal(FakeCardCatalog.TestAtr, cards.Single(c => c.Name == "Chip only").Atr);

        var forms = await new EfFormCatalog(_contexts).AllAsync(CancellationToken.None);
        Assert.Equal(["READTRACK2", "READTRACK1ORTRACK2", "READALLTRACKS", "WRITETRACK3"], forms.Select(f => f.Name));
        Assert.Equal(["PAN", "EXPIRY", "SERVICECODE", "DISCRETIONARY"], forms[0].Track2Fields);
    }

    [Fact]
    public async Task Edited_card_is_kept_by_the_seeder_and_can_be_renamed_and_deleted()
    {
        await SeedAsync();
        var catalog = new EfCardCatalog(_contexts);
        var edited = new IdcCard("Chip only", null, "5555=2912", null, [0x3B, 0x00],
            new Dictionary<string, string> { ["00A4"] = "9000" });

        await catalog.SaveAsync(edited, null, CancellationToken.None);
        await SeedAsync();
        var found = await catalog.FindAsync("Chip only", CancellationToken.None);
        Assert.Equal("5555=2912", found!.Track2);
        Assert.Equal("9000", found.ChipResponses["00A4"]);

        await catalog.SaveAsync(edited with { Name = "Chip renamed" }, "Chip only", CancellationToken.None);
        Assert.Null(await catalog.FindAsync("Chip only", CancellationToken.None));
        Assert.True(await catalog.DeleteAsync("Chip renamed", CancellationToken.None));
        Assert.False(await catalog.DeleteAsync("Chip renamed", CancellationToken.None));
    }

    [Fact]
    public void Device_state_round_trips_per_terminal()
    {
        var store = new EfDeviceStateStore(_contexts);
        Assert.Null(store.Load("10.0.0.1"));

        store.Save("10.0.0.1", new IdcDeviceState(false, 30, 25, 4));
        store.Save("10.0.0.1", new IdcDeviceState(true, 30, 25, 5));
        store.Save("10.0.0.2", new IdcDeviceState(true, 50, 40, 0));

        Assert.Equal(new IdcDeviceState(true, 30, 25, 5), store.Load("10.0.0.1"));
        Assert.Equal(new IdcDeviceState(true, 50, 40, 0), store.Load("10.0.0.2"));
        using var db = _contexts.CreateDbContext();
        Assert.Equal(2, db.Terminals.Count());
    }

    private async Task SeedAsync()
    {
        await using var db = _contexts.CreateDbContext();
        await new IdcCardSeeder().SeedAsync(db, CancellationToken.None);
        await new IdcFormSeeder().SeedAsync(db, CancellationToken.None);
    }

    private sealed class TestContextFactory : IDbContextFactory<SimDbContext>
    {
        private readonly DbContextOptions<SimDbContext> _options;

        public TestContextFactory(DbContextOptions<SimDbContext> options)
        {
            _options = options;
        }

        public SimDbContext CreateDbContext() => new(_options);
    }
}
