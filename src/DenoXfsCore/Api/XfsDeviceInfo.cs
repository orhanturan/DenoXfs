using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsDeviceInfo
{
    private IntPtr lpszModelName;
    private IntPtr lpszSerialNumber;
    private IntPtr lpszRevisionNumber;
    private IntPtr lpszModelDescription;

    public string? ModelName
    {
        get => IntPtrConvert.ToWString(lpszModelName);
        set => WStringConvert.ToIntPtr(ref lpszModelName, value);
    }

    public string? SerialNumber
    {
        get => IntPtrConvert.ToWString(lpszSerialNumber);
        set => WStringConvert.ToIntPtr(ref lpszSerialNumber, value);
    }

    public string? RevisionNumber
    {
        get => IntPtrConvert.ToWString(lpszRevisionNumber);
        set => WStringConvert.ToIntPtr(ref lpszRevisionNumber, value);
    }

    public string? ModelDescription
    {
        get => IntPtrConvert.ToWString(lpszModelDescription);
        set => WStringConvert.ToIntPtr(ref lpszModelDescription, value);
    }

}
