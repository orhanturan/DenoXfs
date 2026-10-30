namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcReadData : ushort
{
    Track1 = 0x0001,
    Track2 = 0x0002,
    Track3 = 0x0004,
    Chip = 0x0008,
    Security = 0x0010,
    Fluxinactive = 0x0020,
    TrackWm = 0x8000,
    MemoryChip = 0x0040,
    FrontTrack1 = 0x0080,
    Frontimage = 0x0100,
    Backimage = 0x0200,
    Track1Jis1 = 0x0400,
    Track3Jis1 = 0x0800,
    Ddi = 0x4000
}
