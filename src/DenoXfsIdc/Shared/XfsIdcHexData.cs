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
        get => PrimitiveArray.ToManaged<byte>(lpbData, (int)ulLength);
        set => PrimitiveArray.ToNative(ref lpbData, value, out ulLength);
    }

}
