using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Infos;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcCaps
{
    private ushort wClass;
    private IdcTypeCapability fwType;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bCompound;
    private IdcDataSource fwReadTracks;
    private IdcDataSource fwWriteTracks;
    private IdcChipProtocolsCapabilities fwChipProtocols;
    private ushort usCards;
    private IdcSecTypeCapability fwSecType;
    private IdcPowerOptionCapability fwPowerOnOption;
    private IdcPowerOptionCapability fwPowerOffOption;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bFluxSensorProgrammable;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bReadWriteAccessFollowingEject;
    private IdcWriteMethod fwWriteMode;
    private IdcChipPowerCapabilities fwChipPower;
    private IntPtr lpszExtra;
    private IdcDIPModeCapabilities fwDIPMode;
    private IntPtr lpwMemoryChipProtocols;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    private IdcGuidLight[] dwGuidLights;
    private IdcEjectPosition fwEjectPosition;
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

    public IdcTypeCapability Type
    {
        get => fwType;
        set => fwType = value;
    }

    public bool Compound
    {
        get => bCompound;
        set => bCompound = value;
    }

    public IdcDataSource ReadTracks
    {
        get => fwReadTracks;
        set => fwReadTracks = value;
    }

    public IdcDataSource WriteTracks
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

    public IdcSecTypeCapability SecType
    {
        get => fwSecType;
        set => fwSecType = value;
    }

    public IdcPowerOptionCapability PowerOnOption
    {
        get => fwPowerOnOption;
        set => fwPowerOnOption = value;
    }

    public IdcPowerOptionCapability PowerOffOption
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

    public IdcWriteMethod WriteMode
    {
        get => fwWriteMode;
        set => fwWriteMode = value;
    }

    public IdcChipPowerCapabilities ChipPower
    {
        get => fwChipPower;
        set => fwChipPower = value;
    }

    public string? Extra
    {
        get => IntPtrConvert.ToString(lpszExtra);
        set => StringConvert.ToIntPtr(ref lpszExtra, value);
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

    public IdcGuidLight[] GuidLights
    {
        get => dwGuidLights;
        set => dwGuidLights = value;
    }

    public IdcEjectPosition EjectPosition
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
