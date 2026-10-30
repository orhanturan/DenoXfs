namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcEjectPositionCapabilities : ushort
{
    Exitposition = 0x0001,
    Transportposition = 0x0002
}
