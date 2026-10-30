using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
        get => IntPtrConvert.ToByteArray(lpbChipData, (int)ulChipDataLength);
        set => DynamicArrayConvert.ToIntPtr(ref lpbChipData, value, out ulChipDataLength);
    }

}
