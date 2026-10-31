using System.Runtime.InteropServices;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Events;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcDevicePosition
{
    private IdcDevicePosition wPosition;

    public IdcDevicePosition Position
    {
        get => wPosition;
        set => wPosition = value;
    }

}
