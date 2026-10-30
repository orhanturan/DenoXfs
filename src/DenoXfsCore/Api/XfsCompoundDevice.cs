using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsCompoundDevice
{
    private IntPtr lpszLogicalServiceName;
    private IntPtr lpszProviderName;
    private IntPtr lpszPhysicalServiceName;
    private IntPtr lpszDeviceClass;

    public string? LogicalServiceName
    {
        get => IntPtrConvert.ToWString(lpszLogicalServiceName);
        set => WStringConvert.ToIntPtr(ref lpszLogicalServiceName, value);
    }

    public string? ProviderName
    {
        get => IntPtrConvert.ToWString(lpszProviderName);
        set => WStringConvert.ToIntPtr(ref lpszProviderName, value);
    }

    public string? PhysicalServiceName
    {
        get => IntPtrConvert.ToWString(lpszPhysicalServiceName);
        set => WStringConvert.ToIntPtr(ref lpszPhysicalServiceName, value);
    }

    public string? DeviceClass
    {
        get => IntPtrConvert.ToWString(lpszDeviceClass);
        set => WStringConvert.ToIntPtr(ref lpszDeviceClass, value);
    }

}
