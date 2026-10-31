namespace DenoXfs.Idc.Infos;

public enum IdcChipPowerStatus : ushort
{
    Online = 0,
    PoweredOff = 1,
    Busy = 2,
    NoDevice = 3,
    HwError = 4,
    NoCard = 5,
    NotSupp = 6,
    Unknown = 7
}
