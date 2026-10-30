using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
        get => IntPtrConvert.ToByteArray(lpbCAPublicKeyCheckSum);
        set => ByteArrayConvert.ToIntPtr(ref lpbCAPublicKeyCheckSum, value);
    }

}
