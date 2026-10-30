using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcRetainCard
{
    private ushort usCount;
    private IdcPosition fwPosition;

    public ushort Count
    {
        get => usCount;
        set => usCount = value;
    }

    public IdcPosition Position
    {
        get => fwPosition;
        set => fwPosition = value;
    }

}
