using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
