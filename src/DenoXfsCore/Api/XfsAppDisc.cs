using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsAppDisc
{
    private IntPtr lpszLogicalName;
    private IntPtr lpszWorkstationName;
    private IntPtr lpszAppID;

    public string? LogicalName
    {
        get => IntPtrConvert.ToString(lpszLogicalName);
        set => StringConvert.ToIntPtr(ref lpszLogicalName, value);
    }

    public string? WorkstationName
    {
        get => IntPtrConvert.ToString(lpszWorkstationName);
        set => StringConvert.ToIntPtr(ref lpszWorkstationName, value);
    }

    public string? AppID
    {
        get => IntPtrConvert.ToString(lpszAppID);
        set => StringConvert.ToIntPtr(ref lpszAppID, value);
    }

}
