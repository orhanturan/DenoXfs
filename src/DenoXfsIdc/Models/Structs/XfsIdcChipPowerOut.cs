using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Idc.Models.Structs;

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
        get => IntPtrConvert.ToByteArray(lpbChipData);
        set => ByteArrayConvert.ToIntPtr(ref lpbChipData, value);
    }

}
