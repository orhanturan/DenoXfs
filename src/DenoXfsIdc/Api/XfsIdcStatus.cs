using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcStatus
{
    private IdcDeviceStatus fwDevice;
    private IdcMediaStatus fwMedia;
    private IdcRetainBinStatus fwRetainBin;
    private IdcSecurityStatus fwSecurity;
    private ushort usCards;
    private IdcChipPowerCapabilities fwChipPower;
    private IntPtr lpszExtra;
    private IdcGuidLightsCapabilities dwGuidLights;
    private IdcChipModuleStatus fwChipModule;
    private IdcMagReadModuleStatus fwMagReadModule;
    private IdcMagWriteModuleStatus fwMagWriteModule;
    private IdcFrontImageModuleStatus fwFrontImageModule;
    private IdcBackImageModuleStatus fwBackImageModule;
    private IdcDevicePositionStatus wDevicePosition;
    private ushort usPowerSaveRecoveryTime;
    private IdcParkingStationMediaStatus lpwParkingStationMedia;
    private IdcAntiFraudModuleStatus wAntiFraudModule;

    public IdcDeviceStatus Device
    {
        get => fwDevice;
        set => fwDevice = value;
    }

    public IdcMediaStatus Media
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

    public IdcGuidLightsCapabilities GuidLights
    {
        get => dwGuidLights;
        set => dwGuidLights = value;
    }

    public IdcChipModuleStatus ChipModule
    {
        get => fwChipModule;
        set => fwChipModule = value;
    }

    public IdcMagReadModuleStatus MagReadModule
    {
        get => fwMagReadModule;
        set => fwMagReadModule = value;
    }

    public IdcMagWriteModuleStatus MagWriteModule
    {
        get => fwMagWriteModule;
        set => fwMagWriteModule = value;
    }

    public IdcFrontImageModuleStatus FrontImageModule
    {
        get => fwFrontImageModule;
        set => fwFrontImageModule = value;
    }

    public IdcBackImageModuleStatus BackImageModule
    {
        get => fwBackImageModule;
        set => fwBackImageModule = value;
    }

    public IdcDevicePositionStatus DevicePosition
    {
        get => wDevicePosition;
        set => wDevicePosition = value;
    }

    public ushort PowerSaveRecoveryTime
    {
        get => usPowerSaveRecoveryTime;
        set => usPowerSaveRecoveryTime = value;
    }

    public IdcParkingStationMediaStatus ParkingStationMedia
    {
        get => lpwParkingStationMedia;
        set => lpwParkingStationMedia = value;
    }

    public IdcAntiFraudModuleStatus AntiFraudModule
    {
        get => wAntiFraudModule;
        set => wAntiFraudModule = value;
    }

}
