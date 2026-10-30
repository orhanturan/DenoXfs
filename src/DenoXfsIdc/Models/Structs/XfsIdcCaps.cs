using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcCaps
{
    private ushort wClass;
    private IdcTypeCapabilities fwType;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bCompound;
    private IdcReadTracksCapabilities fwReadTracks;
    private ushort fwWriteTracks;
    private IdcChipProtocolsCapabilities fwChipProtocols;
    private ushort usCards;
    private IdcSecTypeCapabilities fwSecType;
    private IdcPowerOnOptionCapabilities fwPowerOnOption;
    private ushort fwPowerOffOption;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bFluxSensorProgrammable;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bReadWriteAccessFollowingEject;
    private IdcWriteModeCapabilities fwWriteMode;
    private IdcChipPowerCapabilities fwChipPower;
    private IntPtr lpszExtra;
    private IdcDIPModeCapabilities fwDIPMode;
    private IntPtr lpwMemoryChipProtocols;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    private IdcGuidLightsCapabilities[] dwGuidLights;
    private IdcEjectPositionCapabilities fwEjectPosition;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bPowerSaveControl;
    private ushort usParkingStations;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bAntiFraudModule;
    private IntPtr lpdwSynchronizableCommands;

    public ushort Class
    {
        get => wClass;
        set => wClass = value;
    }

    public IdcTypeCapabilities Type
    {
        get => fwType;
        set => fwType = value;
    }

    public bool Compound
    {
        get => bCompound;
        set => bCompound = value;
    }

    public IdcReadTracksCapabilities ReadTracks
    {
        get => fwReadTracks;
        set => fwReadTracks = value;
    }

    public ushort WriteTracks
    {
        get => fwWriteTracks;
        set => fwWriteTracks = value;
    }

    public IdcChipProtocolsCapabilities ChipProtocols
    {
        get => fwChipProtocols;
        set => fwChipProtocols = value;
    }

    public ushort Cards
    {
        get => usCards;
        set => usCards = value;
    }

    public IdcSecTypeCapabilities SecType
    {
        get => fwSecType;
        set => fwSecType = value;
    }

    public IdcPowerOnOptionCapabilities PowerOnOption
    {
        get => fwPowerOnOption;
        set => fwPowerOnOption = value;
    }

    public ushort PowerOffOption
    {
        get => fwPowerOffOption;
        set => fwPowerOffOption = value;
    }

    public bool FluxSensorProgrammable
    {
        get => bFluxSensorProgrammable;
        set => bFluxSensorProgrammable = value;
    }

    public bool ReadWriteAccessFollowingEject
    {
        get => bReadWriteAccessFollowingEject;
        set => bReadWriteAccessFollowingEject = value;
    }

    public IdcWriteModeCapabilities WriteMode
    {
        get => fwWriteMode;
        set => fwWriteMode = value;
    }

    public IdcChipPowerCapabilities ChipPower
    {
        get => fwChipPower;
        set => fwChipPower = value;
    }

    public string?[]? Extra
    {
        get => IntPtrConvert.ToZeroTerminatedArray(lpszExtra);
        set => ZeroTerminatedArrayConvert.ToIntPtr(ref lpszExtra, value);
    }

    public IdcDIPModeCapabilities DIPMode
    {
        get => fwDIPMode;
        set => fwDIPMode = value;
    }

    public IdcMemoryChipProtocolsCapabilities?[]? MemoryChipProtocols
    {
        get => IntPtrConvert.ToZeroTerminatedArray<IdcMemoryChipProtocolsCapabilities>(lpwMemoryChipProtocols);
        set => ZeroTerminatedArrayConvert.ToIntPtr(ref lpwMemoryChipProtocols, value);
    }

    public IdcGuidLightsCapabilities[] GuidLights
    {
        get => dwGuidLights;
        set => dwGuidLights = value;
    }

    public IdcEjectPositionCapabilities EjectPosition
    {
        get => fwEjectPosition;
        set => fwEjectPosition = value;
    }

    public bool PowerSaveControl
    {
        get => bPowerSaveControl;
        set => bPowerSaveControl = value;
    }

    public ushort ParkingStations
    {
        get => usParkingStations;
        set => usParkingStations = value;
    }

    public bool AntiFraudModule
    {
        get => bAntiFraudModule;
        set => bAntiFraudModule = value;
    }

    public uint[]? SynchronizableCommands
    {
        get => IntPtrConvert.ToUInt32Array(lpdwSynchronizableCommands);
        set => UInt32ArrayConvert.ToIntPtr(ref lpdwSynchronizableCommands, value);
    }

}
