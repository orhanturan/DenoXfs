namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcWriteMethod : ushort
{
    Loco = 0x0002,
    Hico = 0x0004,
    Auto = 0x0008
}
