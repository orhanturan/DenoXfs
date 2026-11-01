using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Idc.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcSetkey
{
    private ushort usKeyLen;
    private IntPtr lpbKeyValue;

    public ushort KeyLen
    {
        get => usKeyLen;
        set => usKeyLen = value;
    }

    public byte[]? KeyValue
    {
        get => PrimitiveArray.ToManaged<byte>(lpbKeyValue, (int)usKeyLen);
        set => PrimitiveArray.ToNative(ref lpbKeyValue, value, out usKeyLen);
    }

}
