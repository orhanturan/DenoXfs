namespace DenoXfs.Core.Models.Enums.Idc;

public enum IdcPowerOnOptionCapabilities : ushort
{
    Noaction = 1,
    Eject = 2,
    Retain = 3,
    EjectThenretain = 4,
    ReadPosition = 5
}
