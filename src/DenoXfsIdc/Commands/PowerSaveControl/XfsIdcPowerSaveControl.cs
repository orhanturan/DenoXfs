using System.Runtime.InteropServices;

namespace DenoXfs.Idc.Commands;

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
