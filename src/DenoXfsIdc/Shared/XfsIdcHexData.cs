using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Idc.Shared;

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
