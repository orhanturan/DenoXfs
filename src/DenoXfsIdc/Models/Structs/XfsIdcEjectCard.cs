using System.Runtime.InteropServices;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcEjectCard
{
    private IdcEjectPosition wEjectPosition;

    public IdcEjectPosition EjectPosition
    {
        get => wEjectPosition;
        set => wEjectPosition = value;
    }

}
