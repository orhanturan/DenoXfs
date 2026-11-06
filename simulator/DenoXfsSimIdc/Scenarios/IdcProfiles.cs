using System.Text.Json.Nodes;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Layouts;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Idc.Scenarios;

/// <summary>
/// The IDC sections of the terminal's scenario and device profile. Both may hold <c>status</c> (WFSIDCSTATUS fields, spec
/// names); the device profile also <c>capabilities</c> (WFSIDCCAPS fields). Status = device state ⊕ device profile ⊕ scenario.
/// </summary>
public interface IIdcProfiles
{
    Task<JsonObject> ScenarioAsync(string address, CancellationToken cancellationToken);

    Task<JsonObject> DeviceProfileAsync(string address, CancellationToken cancellationToken);

    /// <summary>fwDevice the terminal reports with this selection (ONLINE unless a profile pins it).</summary>
    Task<uint> DeviceStateAsync(ProfileSelection selection, CancellationToken cancellationToken);
}

public sealed class IdcProfiles : IIdcProfiles
{
    public const string DeviceClass = "IDC";
    public const string StatusKey = "status";
    public const string CapabilitiesKey = "capabilities";

    private readonly IEffectiveProfileResolver _resolver;

    public IdcProfiles(IEffectiveProfileResolver resolver)
    {
        _resolver = resolver;
    }

    public Task<JsonObject> ScenarioAsync(string address, CancellationToken cancellationToken) =>
        _resolver.ResolveForTerminalAsync(address, ProfileKind.Scenario, DeviceClass, cancellationToken);

    public Task<JsonObject> DeviceProfileAsync(string address, CancellationToken cancellationToken) =>
        _resolver.ResolveForTerminalAsync(address, ProfileKind.DeviceProfile, DeviceClass, cancellationToken);

    public async Task<uint> DeviceStateAsync(ProfileSelection selection, CancellationToken cancellationToken)
    {
        var scenario = await _resolver.ResolveAsync(ProfileKind.Scenario, selection.Scenario, DeviceClass, cancellationToken);
        var device = await _resolver.ResolveAsync(ProfileKind.DeviceProfile, selection.DeviceProfile, DeviceClass, cancellationToken);
        return Device(scenario) ?? Device(device) ?? IdcConstants.DevOnline;
    }

    private static uint? Device(JsonObject section) => IdcConstantSets.Device.Read(section[StatusKey]?["fwDevice"]);
}

/// <summary>
/// After a terminal switched scenario or device profile: WFS_SYSE_DEVICE_STATUS from every card reader of the terminal
/// when the reported fwDevice changed (e.g. Default → DeviceError).
/// </summary>
public sealed class IdcSelectionListener : IProfileSelectionListener
{
    public const string PhysicalName = "DENOXFS_IDC";

    private readonly IIdcProfiles _profiles;
    private readonly IIdcReaders _readers;
    private readonly IEventPublisher _events;
    private readonly ILogger<IdcSelectionListener> _logger;

    public IdcSelectionListener(IIdcProfiles profiles, IIdcReaders readers, IEventPublisher events, ILogger<IdcSelectionListener> logger)
    {
        _profiles = profiles;
        _readers = readers;
        _events = events;
        _logger = logger;
    }

    public async Task OnSelectionChangedAsync(string address, ProfileSelection before, ProfileSelection after, CancellationToken cancellationToken)
    {
        var old = await _profiles.DeviceStateAsync(before, cancellationToken);
        var now = await _profiles.DeviceStateAsync(after, cancellationToken);
        if (old == now)
            return;

        _logger.LogInformation("device status terminal={Terminal} from={Old} to={New}", address,
            IdcConstantSets.Device.NameOf(old), IdcConstantSets.Device.NameOf(now));
        foreach (var reader in await _readers.OfTerminalAsync(address, cancellationToken))
        {
            var status = new JsonObject { ["lpszPhysicalName"] = PhysicalName, ["lpszWorkstationName"] = address, ["dwState"] = now };
            await _events.PublishAsync(reader.Ref, IdcProfiles.DeviceClass, XfsEventKind.System, ApiLayouts.DeviceStatusEvent,
                new NativeValue(ApiLayouts.DevStatusEvent, status), cancellationToken);
        }
    }
}
