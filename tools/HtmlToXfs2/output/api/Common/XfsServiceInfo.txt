using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;

namespace DenoXfs.Api.Common;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsServiceInfo
{
    private IntPtr lppDeviceInformation;
    private IntPtr lpVendorModeInformation;
    private IntPtr lpServiceInterface;
    private IntPtr lppCompoundDevices;
    private IntPtr lpszServiceProviderVersion;
    private IntPtr lpszExtra;

    public IntPtr DeviceInformation
    {
        get => lppDeviceInformation;
        set => lppDeviceInformation = value;
    }

    public XfsVendorModeInfo? VendorModeInformation
    {
        get => lpVendorModeInformation.ToStructSafe<XfsVendorModeInfo>();
        set => StructureConvert.ToIntPtr(ref lpVendorModeInformation, value);
    }

    public XfsServiceInterface? ServiceInterface
    {
        get => lpServiceInterface.ToStructSafe<XfsServiceInterface>();
        set => StructureConvert.ToIntPtr(ref lpServiceInterface, value);
    }

    public IntPtr CompoundDevices
    {
        get => lppCompoundDevices;
        set => lppCompoundDevices = value;
    }

    public string? ServiceProviderVersion
    {
        get => IntPtrConvert.ToWString(lpszServiceProviderVersion);
        set => WStringConvert.ToIntPtr(ref lpszServiceProviderVersion, value);
    }

    public string? Extra
    {
        get => IntPtrConvert.ToString(lpszExtra);
        set => StringConvert.ToIntPtr(ref lpszExtra, value);
    }

}
