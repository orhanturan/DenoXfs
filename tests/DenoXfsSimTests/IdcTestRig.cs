using DenoXfs.Sim.Core.Ui;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Scenarios;
using Microsoft.Extensions.Logging.Abstractions;

namespace DenoXfs.Sim.Tests;

/// <summary>Real IDC device service with recording fakes for the event paths.</summary>
internal sealed class IdcTestRig
{
    public IdcTestRig()
    {
        Devices = new IdcDeviceRegistry(Store, Notifier);
        Service = new IdcDeviceService(Devices, new FakeCardCatalog(), Publisher, NullLogger<IdcDeviceService>.Instance);
        Selections = new CachedTerminalSelections(SelectionStore);
        Resolver = new EffectiveProfileResolver(Catalog, Selections, Merger);
        Profiles = new IdcProfiles(Resolver);
        SelectionService = new ProfileSelectionService(Catalog, Selections,
            [new IdcSelectionListener(Profiles, Readers, Publisher, NullLogger<IdcSelectionListener>.Instance)], NullLogger<ProfileSelectionService>.Instance);
    }

    public FakeProfileCatalog Catalog { get; } = new();

    public FakeReaders Readers { get; } = new();

    public FakeSelectionStore SelectionStore { get; } = new();

    public JsonMerger Merger { get; } = new();

    public CachedTerminalSelections Selections { get; }

    public EffectiveProfileResolver Resolver { get; }

    public IdcProfiles Profiles { get; }

    public ProfileSelectionService SelectionService { get; }

    public FakeDeviceStateStore Store { get; } = new();

    public TerminalNotifier Notifier { get; } = new();

    public IdcDeviceRegistry Devices { get; }

    public RecordingPublisher Publisher { get; } = new();

    public RecordingExecuteEvents ExecuteEvents { get; } = new();

    public IdcDeviceService Service { get; }

    public uint[] ExecuteEventIds => ExecuteEvents.Sent.Select(e => e.EventId).ToArray();

    public void Insert(string address, string card) =>
        Assert.Equal(InsertOutcome.Inserted, Service.InsertAsync(TestDevices.Of(address), card, CancellationToken.None).GetAwaiter().GetResult());
}

internal sealed class RecordingPublisher : IEventPublisher
{
    public List<(DenoXfs.Sim.Core.Hardware.DeviceRef Device, string XfsClass, XfsEventKind Kind, uint EventId, NativeValue? Payload)> Published { get; } = new();

    public Task PublishAsync(DenoXfs.Sim.Core.Hardware.DeviceRef device, string xfsClass, XfsEventKind kind, uint eventId, NativeValue? payload,
        CancellationToken cancellationToken)
    {
        Published.Add((device, xfsClass, kind, eventId, payload));
        return Task.CompletedTask;
    }
}

internal sealed class RecordingExecuteEvents : IExecuteEvents
{
    public List<(uint EventId, NativeValue? Payload)> Sent { get; } = new();

    public Task SendAsync(uint eventId, NativeValue? payload, CancellationToken cancellationToken)
    {
        Sent.Add((eventId, payload));
        return Task.CompletedTask;
    }
}

/// <summary>Scenarios and device profiles in a list; <see cref="Add"/> with the IDC section as JSON text.</summary>
internal sealed class FakeProfileCatalog : IProfileCatalog
{
    private readonly List<ProfileDefinition> _profiles = new();

    public FakeProfileCatalog Add(ProfileKind kind, string name, string idcJson, string? basedOn = null)
    {
        var section = (System.Text.Json.Nodes.JsonObject)System.Text.Json.Nodes.JsonNode.Parse(idcJson)!;
        _profiles.Add(new ProfileDefinition(kind, name, null, basedOn,
            new Dictionary<string, System.Text.Json.Nodes.JsonObject> { [IdcProfiles.DeviceClass] = section }));
        return this;
    }

    public Task<IReadOnlyList<ProfileDefinition>> AllAsync(ProfileKind kind, CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<ProfileDefinition>>(_profiles.Where(p => p.Kind == kind).ToList());

    public Task<ProfileDefinition?> FindAsync(ProfileKind kind, string name, CancellationToken cancellationToken) =>
        Task.FromResult(_profiles.FirstOrDefault(p => p.Kind == kind && string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)));
}

internal sealed class FakeSelectionStore : ITerminalSelectionStore
{
    public Dictionary<string, ProfileSelection> Saved { get; } = new();

    public Task<ProfileSelection> LoadAsync(string address, CancellationToken cancellationToken) =>
        Task.FromResult(Saved.GetValueOrDefault(address) ?? ProfileSelection.Default);

    public Task<bool> SaveAsync(string address, ProfileSelection selection, CancellationToken cancellationToken)
    {
        Saved[address] = selection;
        return Task.FromResult(true);
    }
}

/// <summary>The seeded service, as WFPOpen resolves it on the dev terminal.</summary>
internal static class TestServices
{
    public static readonly DenoXfs.Sim.Core.Hardware.ResolvedService Idc340 = new("DEV00001", "Idc340", "IDC", 1, "CardReader", "Deno26", "Deno");

    /// <summary>Idc340 on the reader of <see cref="TestDevices.Of"/>, so a session and the rig share one device.</summary>
    public static DenoXfs.Sim.Core.Hardware.ResolvedService Idc340On(string address) => Idc340 with { TerminalId = TestDevices.Of(address).TerminalId };
}

/// <summary>Tests use the IP as the Terminal Id of a terminal with one card reader (model device 1).</summary>
internal static class TestDevices
{
    public static DenoXfs.Sim.Core.Hardware.DeviceRef Of(string address) => new(address, 1);
}

/// <summary>Every IP is a terminal with one card reader, <see cref="TestDevices.Of"/>.</summary>
internal sealed class FakeReaders : IIdcReaders
{
    public Task<IReadOnlyList<IdcReader>> OfTerminalAsync(string address, CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<IdcReader>>([new IdcReader(TestDevices.Of(address), "CardReader", ["Idc340"])]);
}
