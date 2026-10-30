using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsSoftware
{
    private IntPtr lpszSoftwareName;
    private IntPtr lpszSoftwareVersion;

    public string? SoftwareName
    {
        get => IntPtrConvert.ToWString(lpszSoftwareName);
        set => WStringConvert.ToIntPtr(ref lpszSoftwareName, value);
    }

    public string? SoftwareVersion
    {
        get => IntPtrConvert.ToWString(lpszSoftwareVersion);
        set => WStringConvert.ToIntPtr(ref lpszSoftwareVersion, value);
    }

}
