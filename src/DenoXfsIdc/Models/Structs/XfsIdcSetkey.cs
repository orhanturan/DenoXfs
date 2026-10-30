using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Idc.Models.Structs;

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
        get => IntPtrConvert.ToByteArray(lpbKeyValue, (int)usKeyLen);
        set => DynamicArrayConvert.ToIntPtr(ref lpbKeyValue, value, out usKeyLen);
    }

}
