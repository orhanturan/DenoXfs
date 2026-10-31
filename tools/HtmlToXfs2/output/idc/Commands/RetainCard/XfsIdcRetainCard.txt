using System.Runtime.InteropServices;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcRetainCard
{
    private ushort usCount;
    private IdcMedia fwPosition;

    public ushort Count
    {
        get => usCount;
        set => usCount = value;
    }

    public IdcMedia Position
    {
        get => fwPosition;
        set => fwPosition = value;
    }

}
