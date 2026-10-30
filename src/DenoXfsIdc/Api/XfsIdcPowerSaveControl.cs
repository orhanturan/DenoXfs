using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
