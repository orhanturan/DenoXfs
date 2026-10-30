namespace DenoXfs.Core.Models.Enums.Idc;

public enum IdcDeviceStatus : ushort
{
    Online = 0,
    Offline = 1,
    PowerOff = 2,
    Nodevice = 3,
    HwError = 4,
    UserError = 5,
    Busy = 6,
    FraudAttempt = 7,
    PotentialFraud = 8
}
