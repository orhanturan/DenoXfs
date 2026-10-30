using System.Runtime.InteropServices;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcParkCard
{
    private IdcDirection wDirection;
    private ushort usParkingStation;

    public IdcDirection Direction
    {
        get => wDirection;
        set => wDirection = value;
    }

    public ushort ParkingStation
    {
        get => usParkingStation;
        set => usParkingStation = value;
    }

}
