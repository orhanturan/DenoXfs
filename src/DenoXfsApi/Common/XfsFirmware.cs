using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Api.Common;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsFirmware
{
    private IntPtr lpszFirmwareName;
    private IntPtr lpszFirmwareVersion;
    private IntPtr lpszHardwareRevision;

    public string? FirmwareName
    {
        get => IntPtrConvert.ToWString(lpszFirmwareName);
        set => WStringConvert.ToIntPtr(ref lpszFirmwareName, value);
    }

    public string? FirmwareVersion
    {
        get => IntPtrConvert.ToWString(lpszFirmwareVersion);
        set => WStringConvert.ToIntPtr(ref lpszFirmwareVersion, value);
    }

    public string? HardwareRevision
    {
        get => IntPtrConvert.ToWString(lpszHardwareRevision);
        set => WStringConvert.ToIntPtr(ref lpszHardwareRevision, value);
    }

}
