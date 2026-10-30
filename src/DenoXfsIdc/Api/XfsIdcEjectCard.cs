using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
