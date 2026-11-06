using System.Text.Json;
using System.Text.RegularExpressions;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Messaging;
using DenoXfs.Sim.Core.Names;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Core.Ui;
using DenoXfs.Sim.Core.Versions;
using DenoXfs.Sim.Data;
using DenoXfs.Sim.Idc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace DenoXfs.Sim.Tests;

/// <summary>Hardware catalog, terminals, WFSOpen by name and routing by class, against a PostgreSQL test database.</summary>
public sealed class HardwareTests : IDisposable
{
    private readonly TestDatabase _contexts = new();
    private readonly HardwareSnapshots _snapshots;
    private readonly HardwareCatalog _catalog;
    private readonly TerminalCatalog _terminals;
    private readonly ServiceResolver _resolver;
    private readonly IDeviceClassModule[] _modules = [new DeviceClassModule("IDC")];

    public HardwareTests()
    {
        using (var db = _contexts.CreateDbContext())
        {
            new HardwareSeeder().SeedAsync(db, CancellationToken.None).GetAwaiter().GetResult();
        }

        _snapshots = new HardwareSnapshots(new EfHardwareStore(_contexts));
        var notifier = new TerminalNotifier();
        _catalog = new HardwareCatalog(new EfHardwareStore(_contexts), _snapshots, _modules, notifier, NullLogger<HardwareCatalog>.Instance);
        _terminals = new TerminalCatalog(new EfTerminalStore(_contexts), _snapshots, new CachedTerminalSelections(new FakeSelectionStore()), notifier,
            NullLogger<TerminalCatalog>.Instance);
        _resolver = new ServiceResolver(_snapshots);
    }

    public void Dispose() => _contexts.Dispose();

    private static CancellationToken None => CancellationToken.None;

    [Fact]
    public void Every_table_is_deno_prefixed_and_every_name_is_lower_snake_case()
    {
        using var db = _contexts.CreateDbContext();
        var snake = new Regex("^[a-z][a-z0-9]*(_[a-z0-9]+)*$");
        foreach (var entity in db.Model.GetEntityTypes())
        {
            var table = entity.GetTableName()!;
            Assert.StartsWith("deno_", table);
            Assert.Matches(snake, table);
            Assert.All(entity.GetProperties(), p => Assert.Matches(snake, p.GetColumnName()));
            Assert.All(entity.GetIndexes(), i => Assert.Matches(snake, i.GetDatabaseName()!));
            Assert.All(entity.GetForeignKeys(), f => Assert.Matches(snake, f.GetConstraintName()!));
        }
        Assert.Equal("deno_model_device", db.Model.FindEntityType(typeof(ModelDevice))!.GetTableName());
        Assert.Equal("terminal_id", db.Model.FindEntityType(typeof(Terminal))!.FindPrimaryKey()!.Properties.Single().GetColumnName());
    }

    [Fact]
    public async Task Seeder_creates_the_dev_catalog_once_and_keeps_deletions()
    {
        var snapshot = await _catalog.SnapshotAsync(None);
        var vendor = Assert.Single(snapshot.Vendors);
        Assert.Equal("Deno", vendor.Name);
        var model = Assert.Single(vendor.Models);
        Assert.Equal("Deno26", model.Name);
        var device = Assert.Single(model.Devices);
        Assert.Equal(("CardReader", "CardReader"), (device.Name, device.DeviceType));
        Assert.Equal(("Idc340", "IDC"), (device.Services.Single().Name, device.Services.Single().XfsClass));
        Assert.Equal(new TerminalInfo("DEV00001", "127.0.0.1", null, model.Id), Assert.Single(snapshot.Terminals));

        Assert.True((await _catalog.DeleteAsync(HardwareItem.Service, device.Services.Single().Id, None)).Succeeded);
        await using (var db = _contexts.CreateDbContext())
            await new HardwareSeeder().SeedAsync(db, None);
        _snapshots.Invalidate();
        Assert.Empty((await _catalog.SnapshotAsync(None)).Devices.Single().Services);
    }

    [Fact]
    public async Task Catalog_checks_service_names_classes_and_one_service_per_class_per_device()
    {
        var model = (await _catalog.SnapshotAsync(None)).Models.Single();
        var reader = model.Devices.Single();
        var second = await _catalog.SaveAsync(new DeviceEdit(0, model.Id, "CardReader", "CardReader2"), None);
        Assert.True(second.Succeeded);

        Assert.Contains("has a service named 'IDC340'", (await _catalog.SaveAsync(new ServiceEdit(0, second.Id, "IDC340", "IDC"), None)).Error);
        Assert.Contains("cannot open CDM", (await _catalog.SaveAsync(new ServiceEdit(0, second.Id, "Cdm1", "CDM"), None)).Error);
        Assert.Contains("already opens a IDC service", (await _catalog.SaveAsync(new ServiceEdit(0, reader.Id, "Idc2", "IDC"), None)).Error);
        Assert.Contains("has a device named", (await _catalog.SaveAsync(new DeviceEdit(0, model.Id, "Recycler", "cardreader"), None)).Error);
        Assert.Contains("cannot open IDC", (await _catalog.SaveAsync(new DeviceEdit(reader.Id, model.Id, "Recycler", "CardReader"), None)).Error);

        Assert.True((await _catalog.SaveAsync(new ServiceEdit(0, second.Id, "Idc341", "idc"), None)).Succeeded);
        var services = (await _catalog.SnapshotAsync(None)).Model(model.Id)!.Services.ToList();
        Assert.Equal(["Idc340", "Idc341"], services.Select(s => s.Name).Order());
        Assert.All(services, s => Assert.Equal("IDC", s.XfsClass));
    }

    [Fact]
    public async Task A_model_in_use_cannot_be_deleted_and_a_vendor_delete_cascades()
    {
        var used = (await _catalog.SnapshotAsync(None)).Models.Single();
        Assert.Contains("used by a terminal", (await _catalog.DeleteAsync(HardwareItem.Model, used.Id, None)).Error);
        Assert.Contains("used by a terminal", (await _catalog.DeleteAsync(HardwareItem.Vendor, used.VendorId, None)).Error);

        var vendor = await _catalog.SaveAsync(new VendorEdit(0, "Other"), None);
        var model = await _catalog.SaveAsync(new ModelEdit(0, vendor.Id, "O1"), None);
        var device = await _catalog.SaveAsync(new DeviceEdit(0, model.Id, "CardReader", "Reader"), None);
        Assert.True((await _catalog.SaveAsync(new ServiceEdit(0, device.Id, "Idc340", "IDC"), None)).Succeeded); // same name, other model
        Assert.Contains("exists", (await _catalog.SaveAsync(new VendorEdit(0, "other"), None)).Error);

        Assert.True((await _catalog.DeleteAsync(HardwareItem.Vendor, vendor.Id, None)).Succeeded);
        await using var db = _contexts.CreateDbContext();
        Assert.Equal(1, await db.TerminalModels.CountAsync());
        Assert.Equal(1, await db.ModelDevices.CountAsync());
        Assert.Equal(1, await db.XfsServices.CountAsync());
    }

    [Fact]
    public async Task Terminals_get_padded_ids_and_unique_addresses()
    {
        var modelId = (await _catalog.SnapshotAsync(None)).Models.Single().Id;

        Assert.True((await _terminals.AddAsync(new TerminalInfo("ATM1", "::ffff:10.0.0.5", " 10.0.0.6 ", modelId), None)).Succeeded);
        var added = (await _terminals.AllAsync(None)).Single(t => t.TerminalId.StartsWith("ATM1"));
        Assert.Equal(new TerminalInfo("ATM1    ", "10.0.0.5", "10.0.0.6", modelId), added);

        Assert.Contains("belongs to the terminal DEV00001", (await _terminals.AddAsync(new TerminalInfo("ATM2", "127.0.0.1", null, modelId), None)).Error);
        Assert.Contains("belongs to the terminal ATM1", (await _terminals.AddAsync(new TerminalInfo("ATM2", "10.0.0.7", "10.0.0.6", modelId), None)).Error);
        Assert.Contains("at most 8", (await _terminals.AddAsync(new TerminalInfo("ATM000001", "10.0.0.8", null, modelId), None)).Error);
        Assert.Contains("exists", (await _terminals.AddAsync(new TerminalInfo("ATM1", "10.0.0.8", null, modelId), None)).Error);
        Assert.Contains("IP address", (await _terminals.AddAsync(new TerminalInfo("ATM3", "host", null, modelId), None)).Error);
        Assert.Contains("model does not exist", (await _terminals.AddAsync(new TerminalInfo("ATM3", "10.0.0.8", null, 999), None)).Error);

        Assert.True((await _terminals.UpdateAsync(added with { Address2 = null }, None)).Succeeded);
        Assert.True((await _terminals.AddAsync(new TerminalInfo("ATM2", "10.0.0.6", null, modelId), None)).Succeeded);
        Assert.True((await _terminals.DeleteAsync("ATM1    ", None)).Succeeded);
        Assert.Equal(["ATM2    ", "DEV00001"], (await _terminals.AllAsync(None)).Select(t => t.TerminalId));
    }

    [Fact]
    public async Task Resolver_finds_the_service_by_name_in_the_terminal_model()
    {
        var found = await _resolver.ResolveAsync("127.0.0.1", "idc340", None);
        var reader = (await _catalog.SnapshotAsync(None)).Devices.Single();
        Assert.Equal(new ResolvedService("DEV00001", "Idc340", "IDC", reader.Id, "CardReader", "Deno26", "Deno"), found.Service);

        Assert.Contains("no service 'Idc999'", (await _resolver.ResolveAsync("127.0.0.1", "Idc999", None)).Reason);
        Assert.Contains("no terminal is defined for 10.9.9.9", (await _resolver.ResolveAsync("10.9.9.9", "Idc340", None)).Reason);

        var service = (await _catalog.SnapshotAsync(None)).Devices.Single().Services.Single();
        Assert.True((await _catalog.SaveAsync(new ServiceEdit(service.Id, service.DeviceId, "IdcMain", "IDC"), None)).Succeeded);
        Assert.Null((await _resolver.ResolveAsync("127.0.0.1", "Idc340", None)).Service);
        Assert.Equal("IdcMain", (await _resolver.ResolveAsync("127.0.0.1", "IDCMAIN", None)).Service!.ServiceName);
    }

    [Fact]
    public async Task Open_binds_a_known_service_and_rejects_an_unknown_ip_or_name()
    {
        var (sessions, hello) = Hello();
        var known = new RecordingChannel("127.0.0.1");
        var typo = new RecordingChannel("127.0.0.1");
        var stranger = new RecordingChannel("10.9.9.9");

        await hello.HandleAsync(known, HelloMessage("Idc340"), None);
        await hello.HandleAsync(typo, HelloMessage("IDC"), None);
        await hello.HandleAsync(stranger, HelloMessage("Idc340"), None);

        Assert.Equal(XfsResultCodes.Success, known.HelloAck.HResult);
        Assert.Equal("IDC", sessions.ServiceOf(known)!.XfsClass);
        Assert.Equal(XfsResultCodes.ServiceNotFound, typo.HelloAck.HResult);
        Assert.Null(sessions.ServiceOf(typo));
        Assert.Equal(XfsResultCodes.ServiceNotFound, stranger.HelloAck.HResult);
        Assert.Single(sessions.SessionsOf("127.0.0.1"));
    }

    [Fact]
    public async Task Commands_reach_only_sessions_of_their_class_and_events_only_go_to_that_class()
    {
        var traffic = new TrafficRecorder(new TrafficFeed(new TerminalNotifier()), new XfsCodeNames([new ApiCodeNames()]));
        var sessions = new SessionRegistry(traffic);
        var idc = new RecordingChannel("127.0.0.1");
        var cdm = new RecordingChannel("127.0.0.1");
        var unopened = new RecordingChannel("127.0.0.1");
        foreach (var channel in new[] { idc, cdm, unopened })
            sessions.Add(channel);
        sessions.Bind(idc, TestServices.Idc340);
        sessions.Bind(cdm, TestServices.Idc340 with { ServiceName = "Cdm1", XfsClass = "CDM" });

        var writer = new NativeLayoutWriter(new NativeBufferBuilderFactory());
        var command = new FixedCommand(IdcConstants.EjectCardCommand);
        var handler = new ExecuteMessageHandler(new ClassHandlers<IExecuteCommand>([new ClassHandler<IExecuteCommand>("IDC", command.Command, command)]),
            sessions, new RequestCancellation(), writer, traffic, NullLogger<ExecuteMessageHandler>.Instance);
        var execute = JsonDocument.Parse($"{{\"type\":\"execute\",\"reqId\":1,\"command\":{IdcConstants.EjectCardCommand},\"timeoutMs\":0,\"input\":{{}}}}").RootElement;

        await handler.HandleAsync(idc, execute, None);
        await handler.HandleAsync(cdm, execute, None);
        await handler.HandleAsync(unopened, execute, None);
        Assert.Equal(XfsResultCodes.Success, idc.Completion(1).HResult);
        Assert.Equal("Idc340", command.ServiceName);
        Assert.Equal(XfsResultCodes.UnsupportedCommand, cdm.Completion(1).HResult);
        Assert.Equal(XfsResultCodes.UnsupportedCommand, unopened.Completion(1).HResult);

        var publisher = new EventPublisher(sessions, writer, traffic, NullLogger<EventPublisher>.Instance);
        await publisher.PublishAsync(new DeviceRef(TestServices.Idc340.TerminalId, TestServices.Idc340.DeviceId), "IDC", XfsEventKind.Service, IdcConstants.MediaRemovedEvent, null, None);
        Assert.Single(idc.Events);
        Assert.Empty(cdm.Events);
        Assert.Empty(unopened.Events);
    }

    private (SessionRegistry Sessions, HelloMessageHandler Handler) Hello()
    {
        var traffic = new TrafficRecorder(new TrafficFeed(new TerminalNotifier()), new XfsCodeNames([new ApiCodeNames()]));
        var sessions = new SessionRegistry(traffic);
        var handler = new HelloMessageHandler(new VersionNegotiator(), SupportedVersions.Spec340, _resolver, sessions, _modules, traffic,
            NullLogger<HelloMessageHandler>.Instance);
        return (sessions, handler);
    }

    /// <summary>WFPOpen of the DLL asking for SPI and service 3.00 - 3.40.</summary>
    private static JsonElement HelloMessage(string logicalName) => JsonDocument.Parse(
        $"{{\"type\":\"hello\",\"logicalName\":\"{logicalName}\",\"spiVersionsRequired\":206851,\"srvcVersionsRequired\":206851}}").RootElement;

    /// <summary>Completes with success and remembers the service it ran for.</summary>
    private sealed class FixedCommand : IExecuteCommand
    {
        public FixedCommand(uint command)
        {
            Command = command;
        }

        public uint Command { get; }

        public string? ServiceName { get; private set; }

        public Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
        {
            ServiceName = context.ServiceName;
            return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, null));
        }
    }
}
