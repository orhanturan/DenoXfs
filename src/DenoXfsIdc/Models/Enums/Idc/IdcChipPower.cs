namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcChipPower : ushort
{
    Cold = 0x0002,
    Warm = 0x0004,
    Off = 0x0008
}
