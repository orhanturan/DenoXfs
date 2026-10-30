namespace DenoXfs.Core.Models.Enums.Idc;

[Flags]
public enum IdcChipProtocolsCapabilities : ushort
{
    Notsupp = 0x0000,
    T0 = 0x0001,
    T1 = 0x0002,
    ProtocolNotRequired = 0x0004,
    TypeaPart3 = 0x0008,
    TypeaPart4 = 0x0010,
    Typeb = 0x0020,
    Nfc = 0x0040
}
