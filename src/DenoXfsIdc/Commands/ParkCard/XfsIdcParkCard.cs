using System.Runtime.InteropServices;

namespace DenoXfs.Idc.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcParkCard
{
    private IdcDirectionParkCard wDirection;
    private ushort usParkingStation;

    public IdcDirectionParkCard Direction
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
