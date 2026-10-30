using System.Runtime.InteropServices;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsVersion
{
    private ushort wVersion;
    private ushort wLowVersion;
    private ushort wHighVersion;

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

}
