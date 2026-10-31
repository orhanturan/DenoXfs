namespace DenoXfs.Idc.Infos;

[Flags]
public enum IdcChipProtocolsCapabilities : ushort
{
    NotSupp = 0x0000,
    T0 = 0x0001,
    T1 = 0x0002,
    ProtocolNotRequired = 0x0004,
    TypeAPart3 = 0x0008,
    TypeAPart4 = 0x0010,
    TypeB = 0x0020,
    Nfc = 0x0040
}
