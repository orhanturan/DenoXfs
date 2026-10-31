using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Infos;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcStatus
{
    private IdcDeviceStatus fwDevice;
    private IdcMedia fwMedia;
    private IdcRetainBinStatus fwRetainBin;
    private IdcSecurityStatus fwSecurity;
    private ushort usCards;
    private IdcChipPowerStatus fwChipPower;
    private IntPtr lpszExtra;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    private IdcGuidLight[] dwGuidLights;
    private IdcChipModuleStatus fwChipModule;
    private IdcMagModuleStatus fwMagReadModule;
    private IdcMagModuleStatus fwMagWriteModule;
    private IdcImageModuleStatus fwFrontImageModule;
    private IdcImageModuleStatus fwBackImageModule;
    private IdcDevicePosition wDevicePosition;
    private ushort usPowerSaveRecoveryTime;
    private IntPtr lpwParkingStationMedia;
    private IdcAntiFraudModuleStatus wAntiFraudModule;

    public IdcDeviceStatus Device
    {
        get => fwDevice;
        set => fwDevice = value;
    }

    public IdcMedia Media
    {
        get => fwMedia;
        set => fwMedia = value;
    }

    public IdcRetainBinStatus RetainBin
    {
        get => fwRetainBin;
        set => fwRetainBin = value;
    }

    public IdcSecurityStatus Security
    {
        get => fwSecurity;
        set => fwSecurity = value;
    }

    public ushort Cards
    {
        get => usCards;
        set => usCards = value;
    }

    public IdcChipPowerStatus ChipPower
    {
        get => fwChipPower;
        set => fwChipPower = value;
    }

    public string? Extra
    {
        get => IntPtrConvert.ToString(lpszExtra);
        set => StringConvert.ToIntPtr(ref lpszExtra, value);
    }

    public IdcGuidLight[] GuidLights
    {
        get => dwGuidLights;
        set => dwGuidLights = value;
    }

    public IdcChipModuleStatus ChipModule
    {
        get => fwChipModule;
        set => fwChipModule = value;
    }

    public IdcMagModuleStatus MagReadModule
    {
        get => fwMagReadModule;
        set => fwMagReadModule = value;
    }

    public IdcMagModuleStatus MagWriteModule
    {
        get => fwMagWriteModule;
        set => fwMagWriteModule = value;
    }

    public IdcImageModuleStatus FrontImageModule
    {
        get => fwFrontImageModule;
        set => fwFrontImageModule = value;
    }

    public IdcImageModuleStatus BackImageModule
    {
        get => fwBackImageModule;
        set => fwBackImageModule = value;
    }

    public IdcDevicePosition DevicePosition
    {
        get => wDevicePosition;
        set => wDevicePosition = value;
    }

    public ushort PowerSaveRecoveryTime
    {
        get => usPowerSaveRecoveryTime;
        set => usPowerSaveRecoveryTime = value;
    }

    public IdcMedia?[]? ParkingStationMedia
    {
        get => IntPtrConvert.ToZeroTerminatedArray<IdcMedia>(lpwParkingStationMedia);
        set => ZeroTerminatedArrayConvert.ToIntPtr(ref lpwParkingStationMedia, value);
    }

    public IdcAntiFraudModuleStatus AntiFraudModule
    {
        get => wAntiFraudModule;
        set => wAntiFraudModule = value;
    }

}
