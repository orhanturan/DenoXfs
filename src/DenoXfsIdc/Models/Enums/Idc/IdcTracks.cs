namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcTracks : ushort
{
    Track1 = 0x0001,
    Track2 = 0x0002,
    Track3 = 0x0004,
    TrackWm = 0x8000,
    FrontTrack1 = 0x0080
}
