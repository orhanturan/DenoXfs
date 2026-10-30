using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
