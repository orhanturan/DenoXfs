using System.Runtime.InteropServices;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcDevicePosition
{
    private IdcDevicepositionPosition wPosition;

    public IdcDevicepositionPosition Position
    {
        get => wPosition;
        set => wPosition = value;
    }

}
