namespace DenoXfs.Idc.Shared;

[Flags]
public enum IdcGuidLight : uint
{
    NotAvailable = 0x0000,
    Off = 0x0001,
    SlowFlash = 0x0004,
    MediumFlash = 0x0008,
    QuickFlash = 0x0010,
    Continuous = 0x0080,
    Red = 0x0100,
    Green = 0x0200,
    Yellow = 0x0400,
    Blue = 0x0800,
    Cyan = 0x1000,
    MagEnta = 0x2000,
    White = 0x4000,
    Entry = 0x100000,
    Exit = 0x200000
}
