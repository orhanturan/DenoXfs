namespace DenoXfs.Idc.Shared;

[Flags]
public enum IdcDataSource : ushort
{
    NotSupp = 0x0000,
    Track1 = 0x0001,
    Track2 = 0x0002,
    Track3 = 0x0004,
    FrontTrack1 = 0x0080,
    Track1Jis1 = 0x0400,
    Track3Jis1 = 0x0800,
    Chip = 0x0008,
    Security = 0x0010,
    FluxInactive = 0x0020,
    TrackWm = 0x8000,
    MemoryChip = 0x0040,
    FrontImage = 0x0100,
    BackImage = 0x0200,
    Ddi = 0x4000
}
