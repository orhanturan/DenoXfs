namespace DenoXfs.Core.Models.Enums.Idc;

public enum IdcStatus : ushort
{
    NotReady = 0,
    Idle = 1,
    ReadyToread = 2,
    Processing = 3,
    CardReadOk = 4,
    Processingerror = 5
}
