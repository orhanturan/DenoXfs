using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
