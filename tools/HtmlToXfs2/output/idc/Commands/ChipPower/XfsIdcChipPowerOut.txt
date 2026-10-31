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
        get => IntPtrConvert.ToByteArray(lpbChipData, (int)ulChipDataLength);
        set => DynamicArrayConvert.ToIntPtr(ref lpbChipData, value, out ulChipDataLength);
    }

}
