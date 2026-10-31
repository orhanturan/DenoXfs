namespace DenoXfs.Idc.Infos;

[Flags]
public enum IdcChipPowerCapabilities : ushort
{
    NotSupp = 0x0000,
    Cold = 0x0002,
    Warm = 0x0004,
    Off = 0x0008
}
