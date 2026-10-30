namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcGuidLightsStatus : uint
{
    NotAvailable = 0x00000000,
    Off = 0x00000001,
    SlowFlash = 0x00000004,
    MediumFlash = 0x00000008,
    QuickFlash = 0x00000010,
    Continuous = 0x00000080,
    Red = 0x00000100,
    Green = 0x00000200,
    Yellow = 0x00000400,
    Blue = 0x00000800,
    Cyan = 0x00001000,
    Magenta = 0x00002000,
    White = 0x00004000,
    Entry = 0x00100000,
    Exit = 0x00200000
}
