using System.Runtime.InteropServices;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcPowerSaveChange
{
    private ushort usPowerSaveRecoveryTime;

    public ushort PowerSaveRecoveryTime
    {
        get => usPowerSaveRecoveryTime;
        set => usPowerSaveRecoveryTime = value;
    }

}
