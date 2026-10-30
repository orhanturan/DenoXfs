using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcHexData
{
    private uint ulLength;
    private IntPtr lpbData;

    public uint Length
    {
        get => ulLength;
        set => ulLength = value;
    }

    public byte[]? Data
    {
        get => IntPtrConvert.ToByteArray(lpbData, (int)ulLength);
        set => DynamicArrayConvert.ToIntPtr(ref lpbData, value, out ulLength);
    }

}
