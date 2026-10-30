namespace DenoXfs.Core.Models.Enums.Idc;

public enum IdcDataStatus : ushort
{
    Ok = 0,
    Missing = 1,
    Invalid = 2,
    Toolong = 3,
    Tooshort = 4,
    Srcnotsupp = 5,
    Srcmissing = 6
}
