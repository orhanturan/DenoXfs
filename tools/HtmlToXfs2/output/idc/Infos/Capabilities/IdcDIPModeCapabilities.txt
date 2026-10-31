namespace DenoXfs.Idc.Infos;

[Flags]
public enum IdcDIPModeCapabilities : ushort
{
    Unknown = 0x0001,
    Exit = 0x0002,
    Entry = 0x0004,
    EntryExit = 0x0008
}
