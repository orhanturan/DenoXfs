using System.Runtime.InteropServices;

namespace DenoXfs.Idc.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcSetGuidLight
{
    private ushort wGuidLight;
    private uint dwCommand;

    public ushort GuidLight
    {
        get => wGuidLight;
        set => wGuidLight = value;
    }

    public uint Command
    {
        get => dwCommand;
        set => dwCommand = value;
    }

}
