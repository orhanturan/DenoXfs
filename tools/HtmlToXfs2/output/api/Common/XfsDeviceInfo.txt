using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Api.Common;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsDeviceInfo
{
    private IntPtr lpszModelName;
    private IntPtr lpszSerialNumber;
    private IntPtr lpszRevisionNumber;
    private IntPtr lpszModelDescription;
    private IntPtr lppFirmware;
    private IntPtr lppSoftware;

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

    public IntPtr Firmware
    {
        get => lppFirmware;
        set => lppFirmware = value;
    }

    public IntPtr Software
    {
        get => lppSoftware;
        set => lppSoftware = value;
    }

}
