namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcWriteModeCapabilities : ushort
{
    Notsupp = 0x0000,
    Loco = 0x0002,
    Hico = 0x0004,
    Auto = 0x0008
}
