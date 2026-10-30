namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcDIPModeCapabilities : ushort
{
    Notsupp = 0x0000,
    Exit = 0x0002,
    Entry = 0x0004,
    EntryExit = 0x0008,
    Unknown = 0x0001
}
