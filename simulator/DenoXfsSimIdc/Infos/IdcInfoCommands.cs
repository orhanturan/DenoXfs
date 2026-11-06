using System.Text.Json.Nodes;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Layouts;
using DenoXfs.Sim.Idc.Scenarios;

namespace DenoXfs.Sim.Idc.Infos;

/// <summary>
/// WFS_INF_IDC_STATUS: WFSIDCSTATUS built from the terminal's device state, then the <c>status</c> fields of the device
/// profile and of the scenario on top (e.g. DeviceError pins fwDevice to HWERROR).
/// </summary>
public sealed class StatusInfo : IInfoCommand
{
    private readonly IIdcDeviceRegistry _devices;
    private readonly IIdcProfiles _profiles;
    private readonly IJsonMerger _merger;

    public StatusInfo(IIdcDeviceRegistry devices, IIdcProfiles profiles, IJsonMerger merger)
    {
        _devices = devices;
        _profiles = profiles;
        _merger = merger;
    }

    public uint Category => IdcConstants.StatusCategory;

    public async Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var state = _devices.Get(context.Device).Snapshot();
        var status = new JsonObject
        {
            ["fwDevice"] = IdcConstants.DevOnline,
            ["fwMedia"] = state.Media switch
            {
                MediaPosition.Inside => IdcConstants.MediaPresent,
                MediaPosition.AtExit => IdcConstants.MediaEntering,
                _ => IdcConstants.MediaNotPresent
            },
            ["fwRetainBin"] = state.RetainBin switch
            {
                RetainBinState.Full => IdcConstants.RetainBinFull,
                RetainBinState.High => IdcConstants.RetainBinHigh,
                _ => IdcConstants.RetainBinOk
            },
            ["fwSecurity"] = IdcConstants.SecNotSupp,
            ["usCards"] = (ushort)state.RetainCount,
            ["fwChipPower"] = state.Chip switch
            {
                ChipState.Online => IdcConstants.ChipOnline,
                ChipState.PoweredOff => IdcConstants.ChipPoweredOff,
                _ => IdcConstants.ChipNoCard
            },
            // lpszExtra NULL. dwGuidLights: the lights set by SET_GUIDANCE_LIGHT (0 = not available).
            ["dwGuidLights"] = NativeJson.Array(state.GuidanceLights.Take(IdcStructs.GuidLightsSize), light => light),
            ["fwChipModule"] = IdcConstants.ChipModOk,
            ["fwMagReadModule"] = IdcConstants.MagModOk,
            ["fwMagWriteModule"] = IdcConstants.MagModOk,
            ["fwFrontImageModule"] = IdcConstants.ImgModNotSupp,
            ["fwBackImageModule"] = IdcConstants.ImgModNotSupp,
            ["wDevicePosition"] = IdcConstants.DevicePosNotSupp,
            ["usPowerSaveRecoveryTime"] = state.PowerSaveRecoveryTime,
            // lpwParkingStationMedia NULL (no parking stations).
            ["wAntiFraudModule"] = IdcConstants.AfmNotSupp
        };
        Apply(status, await _profiles.DeviceProfileAsync(context.RemoteAddress, cancellationToken));
        Apply(status, await _profiles.ScenarioAsync(context.RemoteAddress, cancellationToken));
        return new CommandOutcome(XfsResultCodes.Success, new NativeValue(IdcResults.Status, status));
    }

    private void Apply(JsonObject status, JsonObject section)
    {
        if (section[IdcProfiles.StatusKey] is JsonObject fields)
            _merger.Merge(status, fields);
    }
}

/// <summary>
/// WFS_INF_IDC_CAPABILITIES: by default a motorised reader that reads and writes tracks 1-3, has a T=0/T=1 chip, a retain
/// bin, a card unit guidance light and power save control; the <c>capabilities</c> fields of the device profile go on top.
/// </summary>
public sealed class CapabilitiesInfo : IInfoCommand
{
    private readonly IIdcDeviceRegistry _devices;
    private readonly IIdcProfiles _profiles;
    private readonly IJsonMerger _merger;

    public CapabilitiesInfo(IIdcDeviceRegistry devices, IIdcProfiles profiles, IJsonMerger merger)
    {
        _devices = devices;
        _profiles = profiles;
        _merger = merger;
    }

    public uint Category => IdcConstants.CapabilitiesCategory;

    public async Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var settings = _devices.Get(context.Device).Settings;
        var lights = new uint[IdcConstants.GuidanceCardUnit + 1];
        lights[IdcConstants.GuidanceCardUnit] = Commands.SetGuidanceLightCommand.CardUnitCapabilities;
        var caps = new JsonObject
        {
            ["wClass"] = IdcConstants.ServiceClassIdc,
            ["fwType"] = IdcConstants.TypeMotor,
            ["fwReadTracks"] = IdcConstants.Track1 | IdcConstants.Track2 | IdcConstants.Track3,
            ["fwWriteTracks"] = IdcConstants.Track1 | IdcConstants.Track2 | IdcConstants.Track3,
            ["fwChipProtocols"] = IdcConstants.ChipT0 | IdcConstants.ChipT1,
            ["usCards"] = (ushort)settings.RetainCapacity,
            ["fwSecType"] = IdcConstants.SecNotSupp,
            ["fwPowerOnOption"] = IdcConstants.ResetNoAction,
            ["fwPowerOffOption"] = IdcConstants.ResetNoAction,
            ["fwWriteMode"] = IdcConstants.WriteLoco | IdcConstants.WriteHico | IdcConstants.WriteAuto,
            ["fwChipPower"] = IdcConstants.ChipPowerCold | IdcConstants.ChipPowerWarm | IdcConstants.ChipPowerOff,
            ["dwGuidLights"] = NativeJson.Array(lights, light => light),
            ["fwEjectPosition"] = IdcConstants.ExitPosition,
            ["bPowerSaveControl"] = true
            // Not supported (0 / FALSE / NULL): compound, flux sensor, read/write after eject, DIP mode, memory chips,
            // other guidance lights, parking stations, anti fraud, synchronizable commands.
        };
        if ((await _profiles.DeviceProfileAsync(context.RemoteAddress, cancellationToken))[IdcProfiles.CapabilitiesKey] is JsonObject fields)
            _merger.Merge(caps, fields);
        return new CommandOutcome(XfsResultCodes.Success, new NativeValue(IdcResults.Caps, caps));
    }
}
