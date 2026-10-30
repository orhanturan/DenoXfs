using System.Runtime.InteropServices;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct XfsResult
{
    [FieldOffset(0)]
    private uint requestID;

    [FieldOffset(4)]
    private ushort hService;

    [FieldOffset(6)]
    private SystemTime tsTimestamp;

    [FieldOffset(22)]
    private int hResult;

    // UNION - commandCode and eventID share the same memory location
    [FieldOffset(26)]
    private uint dwCommandCode;

    [FieldOffset(26)]
    private uint dwEventID;

    [FieldOffset(30)]
    private IntPtr lpBuffer;

    public uint RequestID
    {
        get => requestID;
        set => requestID = value;
    }

    public ushort Service
    {
        get => hService;
        set => hService = value;
    }

    public SystemTime Timestamp
    {
        get => tsTimestamp;
        set => tsTimestamp = value;
    }

    public int Result
    {
        get => hResult;
        set => hResult = value;
    }

    public uint CommandCode
    {
        get => dwCommandCode;
        set => dwCommandCode = value;
    }

    public uint EventID
    {
        get => dwEventID;
        set => dwEventID = value;
    }

    public IntPtr Buffer
    {
        get => lpBuffer;
        set => lpBuffer = value;
    }
}
