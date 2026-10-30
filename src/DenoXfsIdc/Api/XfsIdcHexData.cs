using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
