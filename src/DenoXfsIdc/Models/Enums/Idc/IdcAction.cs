namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcAction : ushort
{
    Read = 0x0001,
    Write = 0x0002
}
