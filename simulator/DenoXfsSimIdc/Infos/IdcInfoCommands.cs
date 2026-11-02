using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Idc.Infos;

/// <summary>WFS_INF_IDC_STATUS: WFSIDCSTATUS built from the terminal's device state.</summary>
public sealed class StatusInfo : IInfoCommand
{
    private readonly IIdcDeviceRegistry _devices;
    private readonly INativeBufferBuilderFactory _builders;

    public StatusInfo(IIdcDeviceRegistry devices, INativeBufferBuilderFactory builders)
    {
        _devices = devices;
        _builders = builders;
    }

    public uint Category => IdcConstants.StatusCategory;

    public Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var state = _devices.Get(context.RemoteAddress).Snapshot();
        var builder = _builders.Create();
        var root = builder.Reserve(StatusLayout.Size);

        builder.WriteUInt16(root + StatusLayout.Device, IdcConstants.DevOnline);
        builder.WriteUInt16(root + StatusLayout.Media, state.Media switch
        {
            MediaPosition.Inside => IdcConstants.MediaPresent,
            MediaPosition.AtExit => IdcConstants.MediaEntering,
            _ => IdcConstants.MediaNotPresent
        });
        builder.WriteUInt16(root + StatusLayout.RetainBin, state.RetainBin switch
        {
            RetainBinState.Full => IdcConstants.RetainBinFull,
            RetainBinState.High => IdcConstants.RetainBinHigh,
            _ => IdcConstants.RetainBinOk
        });
        builder.WriteUInt16(root + StatusLayout.Security, IdcConstants.SecNotSupp);
        builder.WriteUInt16(root + StatusLayout.Cards, (ushort)state.RetainCount);
        builder.WriteUInt16(root + StatusLayout.ChipPower, state.Chip switch
        {
            ChipState.Online => IdcConstants.ChipOnline,
            ChipState.PoweredOff => IdcConstants.ChipPoweredOff,
            _ => IdcConstants.ChipNoCard
        });
        // lpszExtra NULL. dwGuidLights: the lights set by SET_GUIDANCE_LIGHT (0 = not available).
        for (var i = 0; i < StatusLayout.GuidLightsCount; i++)
            builder.WriteUInt32(root + StatusLayout.GuidLights + i * sizeof(uint), state.GuidanceLights[i]);
        builder.WriteUInt16(root + StatusLayout.ChipModule, IdcConstants.ChipModOk);
        builder.WriteUInt16(root + StatusLayout.MagReadModule, IdcConstants.MagModOk);
        builder.WriteUInt16(root + StatusLayout.MagWriteModule, IdcConstants.MagModOk);
        builder.WriteUInt16(root + StatusLayout.FrontImageModule, IdcConstants.ImgModNotSupp);
        builder.WriteUInt16(root + StatusLayout.BackImageModule, IdcConstants.ImgModNotSupp);
        builder.WriteUInt16(root + StatusLayout.DevicePosition, IdcConstants.DevicePosNotSupp);
        builder.WriteUInt16(root + StatusLayout.PowerSaveRecoveryTime, state.PowerSaveRecoveryTime);
        // lpwParkingStationMedia NULL (no parking stations).
        builder.WriteUInt16(root + StatusLayout.AntiFraudModule, IdcConstants.AfmNotSupp);
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, builder.Build()));
    }
}

/// <summary>
/// WFS_INF_IDC_CAPABILITIES: a motorised reader that reads and writes tracks 1-3, has a T=0/T=1 chip, a retain bin,
/// a card unit guidance light and power save control.
/// </summary>
public sealed class CapabilitiesInfo : IInfoCommand
{
    private readonly IIdcDeviceRegistry _devices;
    private readonly INativeBufferBuilderFactory _builders;

    public CapabilitiesInfo(IIdcDeviceRegistry devices, INativeBufferBuilderFactory builders)
    {
        _devices = devices;
        _builders = builders;
    }

    public uint Category => IdcConstants.CapabilitiesCategory;

    public Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var settings = _devices.Get(context.RemoteAddress).Settings;
        var builder = _builders.Create();
        var root = builder.Reserve(CapsLayout.Size);

        builder.WriteUInt16(root + CapsLayout.Class, IdcConstants.ServiceClassIdc);
        builder.WriteUInt16(root + CapsLayout.Type, IdcConstants.TypeMotor);
        builder.WriteUInt16(root + CapsLayout.ReadTracks, IdcConstants.Track1 | IdcConstants.Track2 | IdcConstants.Track3);
        builder.WriteUInt16(root + CapsLayout.WriteTracks, IdcConstants.Track1 | IdcConstants.Track2 | IdcConstants.Track3);
        builder.WriteUInt16(root + CapsLayout.WriteMode, IdcConstants.WriteLoco | IdcConstants.WriteHico | IdcConstants.WriteAuto);
        builder.WriteUInt16(root + CapsLayout.ChipProtocols, IdcConstants.ChipT0 | IdcConstants.ChipT1);
        builder.WriteUInt16(root + CapsLayout.Cards, (ushort)settings.RetainCapacity);
        builder.WriteUInt16(root + CapsLayout.SecType, IdcConstants.SecNotSupp);
        builder.WriteUInt16(root + CapsLayout.PowerOnOption, IdcConstants.ResetNoAction);
        builder.WriteUInt16(root + CapsLayout.PowerOffOption, IdcConstants.ResetNoAction);
        builder.WriteUInt16(root + CapsLayout.ChipPower, IdcConstants.ChipPowerCold | IdcConstants.ChipPowerWarm | IdcConstants.ChipPowerOff);
        builder.WriteUInt32(root + CapsLayout.GuidLights + IdcConstants.GuidanceCardUnit * sizeof(uint), Commands.SetGuidanceLightCommand.CardUnitCapabilities);
        builder.WriteUInt16(root + CapsLayout.EjectPosition, IdcConstants.ExitPosition);
        builder.WriteUInt32(root + CapsLayout.PowerSaveControl, 1);
        // Not supported (0 / FALSE / NULL): compound, flux sensor, read/write after eject, DIP mode, memory chips,
        // other guidance lights, parking stations, anti fraud, synchronizable commands.
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, builder.Build()));
    }
}
