namespace DenoXfs.Idc.Shared;

[Flags]
public enum IdcWriteMethod : ushort
{
    NotSupp = 0x0000,
    Loco = 0x0002,
    Hico = 0x0004,
    Auto = 0x0008
}
