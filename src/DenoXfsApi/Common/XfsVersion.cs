using System.Runtime.InteropServices;

namespace DenoXfs.Api.Common;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsVersion
{
    private ushort wVersion;
    private ushort wLowVersion;
    private ushort wHighVersion;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 257)]
    private byte[] szDescription;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 257)]
    private byte[] szSystemStatus;

    public ushort Version
    {
        get => wVersion;
        set => wVersion = value;
    }

    public ushort LowVersion
    {
        get => wLowVersion;
        set => wLowVersion = value;
    }

    public ushort HighVersion
    {
        get => wHighVersion;
        set => wHighVersion = value;
    }

    public byte[] Description
    {
        get => szDescription;
        set => szDescription = value;
    }

    public byte[] SystemStatus
    {
        get => szSystemStatus;
        set => szSystemStatus = value;
    }

}
