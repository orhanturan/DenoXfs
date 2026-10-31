using System.Runtime.InteropServices;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Commands;

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
