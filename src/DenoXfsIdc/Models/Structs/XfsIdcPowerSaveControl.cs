using System.Runtime.InteropServices;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcPowerSaveControl
{
    private ushort usMaxPowerSaveRecoveryTime;

    public ushort MaxPowerSaveRecoveryTime
    {
        get => usMaxPowerSaveRecoveryTime;
        set => usMaxPowerSaveRecoveryTime = value;
    }

}
