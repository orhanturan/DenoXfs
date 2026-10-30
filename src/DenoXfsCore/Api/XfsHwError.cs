using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsHwError
{
    private IntPtr lpszLogicalName;
    private IntPtr lpszPhysicalName;
    private IntPtr lpszWorkstationName;
    private IntPtr lpszAppID;
    private uint dwAction;
    private uint dwSize;
    private IntPtr lpbDescription;

    public string? LogicalName
    {
        get => IntPtrConvert.ToString(lpszLogicalName);
        set => StringConvert.ToIntPtr(ref lpszLogicalName, value);
    }

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

    public string? AppID
    {
        get => IntPtrConvert.ToString(lpszAppID);
        set => StringConvert.ToIntPtr(ref lpszAppID, value);
    }

    public uint Action
    {
        get => dwAction;
        set => dwAction = value;
    }

    public uint Size
    {
        get => dwSize;
        set => dwSize = value;
    }

    public byte[]? Description
    {
        get => IntPtrConvert.ToByteArray(lpbDescription);
        set => ByteArrayConvert.ToIntPtr(ref lpbDescription, value);
    }

}
