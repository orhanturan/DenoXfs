using System.Text.Json;
using System.Text.Json.Nodes;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Infos;
using DenoXfs.Sim.Idc.Layouts;
using DenoXfs.Sim.Idc.Scenarios;

namespace DenoXfs.Sim.Tests;

public class ScenarioTests
{
    private const string Address = "10.0.0.7";
    private static readonly CancellationToken Ct = CancellationToken.None;
    private readonly IdcTestRig _rig = new();

    [Fact]
    public void Merge_combines_objects_replaces_arrays_and_scalars_and_keeps_the_overlay()
    {
        var target = Json("""{ "status": { "fwDevice": 0, "fwMedia": 2 }, "lights": [1, 2, 3], "keep": true }""");
        var overlay = Json("""{ "status": { "fwDevice": "HWERROR" }, "lights": [9], "extra": null }""");

        new JsonMerger().Merge(target, overlay);

        Assert.Equal("""{"status":{"fwDevice":"HWERROR","fwMedia":2},"lights":[9],"keep":true,"extra":null}""", target.ToJsonString());
        Assert.Equal("""{"status":{"fwDevice":"HWERROR"},"lights":[9],"extra":null}""", overlay.ToJsonString());
    }

    [Fact]
    public async Task Effective_section_is_default_then_based_on_chain_then_the_profile()
    {
        _rig.Catalog
            .Add(ProfileKind.Scenario, "Default", """{ "status": { "fwDevice": "ONLINE", "fwSecurity": "NOTSUPP" } }""")
            .Add(ProfileKind.Scenario, "Faulty", """{ "status": { "fwDevice": "HWERROR", "fwChipModule": "INOP" } }""")
            .Add(ProfileKind.Scenario, "FaultyChipOk", """{ "status": { "fwChipModule": "OK" } }""", basedOn: "Faulty");

        var section = await _rig.Resolver.ResolveAsync(ProfileKind.Scenario, "faultychipok", IdcProfiles.DeviceClass, Ct);

        Assert.Equal("""{"status":{"fwDevice":"HWERROR","fwSecurity":"NOTSUPP","fwChipModule":"OK"}}""", section.ToJsonString());
        Assert.Equal("""{"status":{"fwDevice":"ONLINE","fwSecurity":"NOTSUPP"}}""",
            (await _rig.Resolver.ResolveAsync(ProfileKind.Scenario, "Unknown", IdcProfiles.DeviceClass, Ct)).ToJsonString());
    }

    [Fact]
    public async Task Based_on_cycle_stops()
    {
        _rig.Catalog
            .Add(ProfileKind.Scenario, "A", """{ "a": 1 }""", basedOn: "B")
            .Add(ProfileKind.Scenario, "B", """{ "b": 2 }""", basedOn: "A");

        Assert.Equal("""{"b":2,"a":1}""", (await _rig.Resolver.ResolveAsync(ProfileKind.Scenario, "A", IdcProfiles.DeviceClass, Ct)).ToJsonString());
    }

    [Fact]
    public async Task Selecting_an_unknown_name_is_refused_and_changes_nothing()
    {
        DefaultsAndDeviceError();

        Assert.Equal("Unknown scenario 'Nope'.", await _rig.SelectionService.SelectAsync(Address, new ProfileSelection("Nope", "Default"), Ct));
        Assert.Equal("Unknown device profile 'Nope'.", await _rig.SelectionService.SelectAsync(Address, new ProfileSelection("Default", "Nope"), Ct));
        Assert.Empty(_rig.SelectionStore.Saved);
    }

    [Fact]
    public async Task Status_follows_the_selected_scenario_and_the_switch_publishes_device_status()
    {
        DefaultsAndDeviceError();
        var status = new StatusInfo(_rig.Devices, _rig.Profiles, _rig.Merger);

        Assert.Null(await _rig.SelectionService.SelectAsync(Address, new ProfileSelection("deviceerror", "Default"), Ct));

        Assert.Equal(new ProfileSelection("DeviceError", "Default"), _rig.SelectionStore.Saved[Address]);
        var fields = Bytes(await status.GetInfoAsync(new InfoContext(Address, TestDevices.Of(Address), IdcConstants.StatusCategory, Empty), Ct));
        Assert.Equal(4, Word(fields, IdcStructs.Status["fwDevice"].Offset));     // HWERROR
        Assert.Equal(2, Word(fields, IdcStructs.Status["fwChipModule"].Offset)); // INOP
        Assert.Equal(2, Word(fields, IdcStructs.Status["fwMedia"].Offset));      // still from the device state: no card

        var published = Assert.Single(_rig.Publisher.Published);
        Assert.Equal((XfsEventKind.System, ApiLayouts.DeviceStatusEvent), (published.Kind, published.EventId));
        Assert.Equal(4u, published.Payload!.Value!["dwState"]!.GetValue<uint>());

        // Back to Default: ONLINE again, one more device status event; selecting the same again sends nothing.
        await _rig.SelectionService.SelectAsync(Address, ProfileSelection.Default, Ct);
        await _rig.SelectionService.SelectAsync(Address, ProfileSelection.Default, Ct);
        Assert.Equal(2, _rig.Publisher.Published.Count);
        fields = Bytes(await status.GetInfoAsync(new InfoContext(Address, TestDevices.Of(Address), IdcConstants.StatusCategory, Empty), Ct));
        Assert.Equal(0, Word(fields, IdcStructs.Status["fwDevice"].Offset));
    }

    [Fact]
    public async Task Device_profile_sets_capabilities_without_a_device_status_event()
    {
        DefaultsAndDeviceError();
        _rig.Catalog.Add(ProfileKind.DeviceProfile, "Swipe", """{ "capabilities": { "fwType": "SWIPE", "fwChipProtocols": "NOTSUPP" } }""");

        await _rig.SelectionService.SelectAsync(Address, new ProfileSelection("Default", "Swipe"), Ct);
        var caps = Bytes(await new CapabilitiesInfo(_rig.Devices, _rig.Profiles, _rig.Merger)
            .GetInfoAsync(new InfoContext(Address, TestDevices.Of(Address), IdcConstants.CapabilitiesCategory, Empty), Ct));

        Assert.Equal(2, Word(caps, IdcStructs.Caps["fwType"].Offset));
        Assert.Equal(0, Word(caps, IdcStructs.Caps["fwChipProtocols"].Offset));
        Assert.Equal(7, Word(caps, IdcStructs.Caps["fwReadTracks"].Offset)); // not in the profile: the built-in value
        Assert.Empty(_rig.Publisher.Published);
    }

    private void DefaultsAndDeviceError() => _rig.Catalog
        .Add(ProfileKind.Scenario, "Default", "{}")
        .Add(ProfileKind.Scenario, "DeviceError", """{ "status": { "fwDevice": "HWERROR", "fwChipModule": "INOP" } }""")
        .Add(ProfileKind.DeviceProfile, "Default", "{}");

    private static readonly JsonElement Empty = JsonDocument.Parse("{}").RootElement;

    private static JsonObject Json(string json) => (JsonObject)JsonNode.Parse(json)!;

    private static byte[] Bytes(CommandOutcome outcome) =>
        Convert.FromBase64String(new NativeLayoutWriter(new NativeBufferBuilderFactory()).Write(outcome.Output)!.Data);

    private static ushort Word(byte[] data, int offset) => BitConverter.ToUInt16(data, offset);
}
