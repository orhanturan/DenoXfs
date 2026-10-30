namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcChipPowerCapabilities : ushort
{
    Notsupp = 0x0000,
    Cold = 0x0002,
    Warm = 0x0004,
    Off = 0x0008
}
