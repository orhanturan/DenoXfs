using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcKeyData
{
    private IntPtr lpRID;
    private ushort wCAPublicKeyIndex;
    private ushort wAPublicKeyAlgorithmIndicator;
    private IntPtr lpCAPublicKeyExponent;
    private IntPtr lpCAPublicKeyModulus;
    private IntPtr lpbCAPublicKeyCheckSum;

    public XfsIdcHexData? RID
    {
        get => lpRID.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpRID, value);
    }

    public ushort CAPublicKeyIndex
    {
        get => wCAPublicKeyIndex;
        set => wCAPublicKeyIndex = value;
    }

    public ushort APublicKeyAlgorithmIndicator
    {
        get => wAPublicKeyAlgorithmIndicator;
        set => wAPublicKeyAlgorithmIndicator = value;
    }

    public XfsIdcHexData? CAPublicKeyExponent
    {
        get => lpCAPublicKeyExponent.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpCAPublicKeyExponent, value);
    }

    public XfsIdcHexData? CAPublicKeyModulus
    {
        get => lpCAPublicKeyModulus.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpCAPublicKeyModulus, value);
    }

    public byte[]? CAPublicKeyCheckSum
    {
        get => PrimitiveArray.ToManaged<byte>(lpbCAPublicKeyCheckSum);
        set => PrimitiveArray.ToNative(ref lpbCAPublicKeyCheckSum, value);
    }

}
