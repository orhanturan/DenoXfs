using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsDevStatus
{
    private IntPtr lpszPhysicalName;
    private IntPtr lpszWorkstationName;
    private uint dwState;

    public string? PhysicalName
    {
        get => IntPtrConvert.ToString(lpszPhysicalName);
        set => StringConvert.ToIntPtr(ref lpszPhysicalName, value);
    }

    public string? WorkstationName
    {
        get => IntPtrConvert.ToString(lpszWorkstationName);
        set => StringConvert.ToIntPtr(ref lpszWorkstationName, value);
    }

    public uint State
    {
        get => dwState;
        set => dwState = value;
    }

}
