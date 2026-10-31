namespace DenoXfs.Idc.Shared;

public enum IdcDataStatus : ushort
{
    Ok = 0,
    Missing = 1,
    Invalid = 2,
    TooLong = 3,
    TooShort = 4,
    SrcNotSupp = 5,
    SrcMissing = 6
}
