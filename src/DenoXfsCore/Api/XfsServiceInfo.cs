using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Extensions;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsServiceInfo
{
    private IntPtr lpVendorModeInformation;
    private IntPtr lpServiceInterface;
    private IntPtr lpszServiceProviderVersion;
    private IntPtr lpszExtra;

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
