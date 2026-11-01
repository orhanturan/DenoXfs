using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;

namespace DenoXfs.Api.Common;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsVrsnError
{
    private IntPtr lpszLogicalName;
    private IntPtr lpszWorkstationName;
    private IntPtr lpszAppID;
    private uint dwSize;
    private IntPtr lpbDescription;
    private IntPtr lpWFSVersion;

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

    public uint Size
    {
        get => dwSize;
        set => dwSize = value;
    }

    public byte[]? Description
    {
        get => PrimitiveArray.ToManaged<byte>(lpbDescription);
        set => PrimitiveArray.ToNative(ref lpbDescription, value);
    }

    public XfsVersion? WFSVersion
    {
        get => lpWFSVersion.ToStructSafe<XfsVersion>();
        set => StructureConvert.ToIntPtr(ref lpWFSVersion, value);
    }

}
