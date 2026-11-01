using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Idc.Shared;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcChipIo
{
    private ushort wChipProtocol;
    private uint ulChipDataLength;
    private IntPtr lpbChipData;

    public ushort ChipProtocol
    {
        get => wChipProtocol;
        set => wChipProtocol = value;
    }

    public uint ChipDataLength
    {
        get => ulChipDataLength;
        set => ulChipDataLength = value;
    }

    public byte[]? ChipData
    {
        get => PrimitiveArray.ToManaged<byte>(lpbChipData, (int)ulChipDataLength);
        set => PrimitiveArray.ToNative(ref lpbChipData, value, out ulChipDataLength);
    }

}
