namespace DenoXfs.Core.Models.Enums.Idc;

public enum IdcChipPowerStatus : ushort
{
    Online = 0,
    PoweredOff = 1,
    Busy = 2,
    Nodevice = 3,
    HwError = 4,
    Nocard = 5,
    Notsupp = 6,
    Unknown = 7
}
