using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsVendorModeInfo
{
    [MarshalAs(UnmanagedType.Bool)]
    private bool bAllowOpenSessions;
    private IntPtr lpdwAllowedExecuteCommands;

    public bool AllowOpenSessions
    {
        get => bAllowOpenSessions;
        set => bAllowOpenSessions = value;
    }

    public uint[]? AllowedExecuteCommands
    {
        get => IntPtrConvert.ToUInt32Array(lpdwAllowedExecuteCommands);
        set => UInt32ArrayConvert.ToIntPtr(ref lpdwAllowedExecuteCommands, value);
    }

}
