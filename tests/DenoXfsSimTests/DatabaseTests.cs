using DenoXfs.Sim.Core.Ui;
using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Data;
using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Forms;
using DenoXfs.Sim.Idc.Infos;
using DenoXfs.Sim.Idc.Scenarios;
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
    public async Task Device_state_round_trips_per_terminal_and_is_not_stored_without_a_terminal()
    {
        await SeedAsync();
        await AddTerminalAsync("ATM00001", "10.0.0.1");
        await AddTerminalAsync("ATM00002", "10.0.0.2");
        var store = new EfDeviceStateStore(_contexts);
        int readerId;
        using (var seeded = _contexts.CreateDbContext())
            readerId = seeded.ModelDevices.Single().Id;
        DeviceRef first = new("ATM00001", readerId), second = new("ATM00002", readerId);
        Assert.Null(store.Load(first));
        store.Save(new DeviceRef("ATM00003", readerId), new IdcDeviceState(false, 30, 25, 4));
        Assert.Null(store.Load(new DeviceRef("ATM00003", readerId)));
        store.Save(new DeviceRef("ATM00001", readerId + 100), new IdcDeviceState(false, 30, 25, 4));
        Assert.Null(store.Load(new DeviceRef("ATM00001", readerId + 100)));

        store.Save(first, new IdcDeviceState(false, 30, 25, 4));
        store.Save(first, new IdcDeviceState(true, 30, 25, 5));
        store.Save(second, new IdcDeviceState(true, 50, 40, 0));

        Assert.Equal(new IdcDeviceState(true, 30, 25, 5), store.Load(first));
        Assert.Equal(new IdcDeviceState(true, 50, 40, 0), store.Load(second));
        using var db = _contexts.CreateDbContext();
        Assert.Equal(2, db.DeviceStates.Count());
    }

    [Fact]
    public async Task Profile_seeder_adds_missing_profiles_and_keys_but_keeps_edits()
    {
        await SeedAsync();
        await using (var db = _contexts.CreateDbContext())
        {
            var section = db.ProfileSections.Single(s => db.Profiles.Any(p => p.Id == s.ProfileId && p.Name == "DeviceError"));
            section.Json = """{ "status": { "fwDevice": "OFFLINE" } }""";
            await db.SaveChangesAsync();
        }
        await SeedAsync();

        var catalog = new EfProfileCatalog(_contexts);
        Assert.Equal(["Default", "DeviceError"], (await catalog.AllAsync(ProfileKind.Scenario, CancellationToken.None)).Select(p => p.Name));
        Assert.Equal(["Default", "Swipe"], (await catalog.AllAsync(ProfileKind.DeviceProfile, CancellationToken.None)).Select(p => p.Name));
        var edited = await catalog.FindAsync(ProfileKind.Scenario, "deviceerror", CancellationToken.None);
        Assert.Equal("""{"status":{"fwDevice":"OFFLINE"}}""", edited!.Sections[IdcProfiles.DeviceClass].ToJsonString());
        Assert.Equal("Default", edited.BasedOn);
    }

    /// <summary>Every seeded scenario and device profile must give a Status and Capabilities the writer accepts.</summary>
    [Fact]
    public async Task Seeded_profiles_write_valid_status_and_capabilities()
    {
        await SeedAsync();
        var catalog = new EfProfileCatalog(_contexts);
        var selections = new CachedTerminalSelections(new EfTerminalSelectionStore(_contexts));
        var profiles = new IdcProfiles(new EffectiveProfileResolver(catalog, selections, new JsonMerger()));
        var devices = new IdcDeviceRegistry(new FakeDeviceStateStore(), new TerminalNotifier());
        var writer = new NativeLayoutWriter(new NativeBufferBuilderFactory());
        var input = JsonDocument.Parse("{}").RootElement;
        await AddTerminalAsync("ATM00011", "10.1.1.1");

        foreach (var scenario in await catalog.AllAsync(ProfileKind.Scenario, CancellationToken.None))
        foreach (var device in await catalog.AllAsync(ProfileKind.DeviceProfile, CancellationToken.None))
        {
            Assert.True(await selections.SetAsync("10.1.1.1", new ProfileSelection(scenario.Name, device.Name), CancellationToken.None));
            var status = await new StatusInfo(devices, profiles, new JsonMerger()).GetInfoAsync(new InfoContext("10.1.1.1", TestDevices.Of("ATM00011"), IdcConstants.StatusCategory, input), CancellationToken.None);
            var caps = await new CapabilitiesInfo(devices, profiles, new JsonMerger()).GetInfoAsync(new InfoContext("10.1.1.1", TestDevices.Of("ATM00011"), IdcConstants.CapabilitiesCategory, input), CancellationToken.None);
            Assert.NotNull(writer.Write(status.Output));
            Assert.NotNull(writer.Write(caps.Output));
        }
    }

    [Fact]
    public async Task Terminal_selection_round_trips_and_default_is_stored_as_null()
    {
        await SeedAsync();
        var store = new EfTerminalSelectionStore(_contexts);
        Assert.Equal(ProfileSelection.Default, await store.LoadAsync("10.0.0.9", CancellationToken.None));
        Assert.False(await store.SaveAsync("10.0.0.9", new ProfileSelection("DeviceError", "Swipe"), CancellationToken.None));

        await AddTerminalAsync("ATM00009", "10.0.0.8", "10.0.0.9");
        Assert.True(await store.SaveAsync("10.0.0.9", new ProfileSelection("DeviceError", "Swipe"), CancellationToken.None));
        Assert.Equal(new ProfileSelection("DeviceError", "Swipe"), await store.LoadAsync("10.0.0.9", CancellationToken.None));

        await store.SaveAsync("10.0.0.9", new ProfileSelection("Default", "Swipe"), CancellationToken.None);
        using var db = _contexts.CreateDbContext();
        Assert.Null(db.Terminals.Single(t => t.Address2 == "10.0.0.9").ScenarioId);
    }

    private async Task AddTerminalAsync(string terminalId, string address1, string? address2 = null)
    {
        await using var db = _contexts.CreateDbContext();
        db.Terminals.Add(new Terminal { TerminalId = terminalId, Address1 = address1, Address2 = address2, ModelId = db.TerminalModels.First().Id });
        await db.SaveChangesAsync();
    }

    private async Task SeedAsync()
    {
        await using var db = _contexts.CreateDbContext();
        await new HardwareSeeder().SeedAsync(db, CancellationToken.None);
        await new IdcCardSeeder().SeedAsync(db, CancellationToken.None);
        await new IdcFormSeeder().SeedAsync(db, CancellationToken.None);
        await new IdcProfileSeeder().SeedAsync(db, CancellationToken.None);
    }

    internal sealed class TestContextFactory : IDbContextFactory<SimDbContext>
    {
        private readonly DbContextOptions<SimDbContext> _options;

        public TestContextFactory(DbContextOptions<SimDbContext> options)
        {
            _options = options;
        }

        public SimDbContext CreateDbContext() => new(_options);
    }
}
