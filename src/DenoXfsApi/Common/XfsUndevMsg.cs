using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;

namespace DenoXfs.Api.Common;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsUndevMsg
{
    private IntPtr lpszLogicalName;
    private IntPtr lpszWorkstationName;
    private IntPtr lpszAppID;
    private uint dwSize;
    private IntPtr lpbDescription;
    private uint dwMsg;
    private IntPtr lpWFSResult;

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
        get => IntPtrConvert.ToByteArray(lpbDescription);
        set => ByteArrayConvert.ToIntPtr(ref lpbDescription, value);
    }

    public uint Msg
    {
        get => dwMsg;
        set => dwMsg = value;
    }

    public XfsResult? WFSResult
    {
        get => lpWFSResult.ToStructSafe<XfsResult>();
        set => StructureConvert.ToIntPtr(ref lpWFSResult, value);
    }

}
