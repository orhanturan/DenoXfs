using System.Runtime.InteropServices;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcSetGuidlight
{
    private ushort wGuidLight;
    private IdcCommand dwCommand;

    public ushort GuidLight
    {
        get => wGuidLight;
        set => wGuidLight = value;
    }

    public IdcCommand Command
    {
        get => dwCommand;
        set => dwCommand = value;
    }

}
