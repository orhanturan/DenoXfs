using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Idc.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcChipPowerOut
{
    private uint ulChipDataLength;
    private IntPtr lpbChipData;

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
