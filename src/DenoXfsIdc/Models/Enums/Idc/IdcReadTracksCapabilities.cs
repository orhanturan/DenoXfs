namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcReadTracksCapabilities : ushort
{
    Notsupp = 0x0000,
    Track1 = 0x0001,
    Track2 = 0x0002,
    Track3 = 0x0004,
    TrackWm = 0x8000,
    FrontTrack1 = 0x0080,
    Frontimage = 0x0100,
    Backimage = 0x0200,
    Track1Jis1 = 0x0400,
    Track3Jis1 = 0x0800,
    Ddi = 0x4000
}
