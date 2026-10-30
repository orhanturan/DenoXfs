using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
