using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcAidData
{
    private IntPtr lpAID;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bPartialSelection;
    private uint ulTransactionType;
    private IntPtr lpKernelIdentifier;
    private IntPtr lpConfigData;

    public XfsIdcHexData? AID
    {
        get => lpAID.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpAID, value);
    }

    public bool PartialSelection
    {
        get => bPartialSelection;
        set => bPartialSelection = value;
    }

    public uint TransactionType
    {
        get => ulTransactionType;
        set => ulTransactionType = value;
    }

    public XfsIdcHexData? KernelIdentifier
    {
        get => lpKernelIdentifier.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpKernelIdentifier, value);
    }

    public XfsIdcHexData? ConfigData
    {
        get => lpConfigData.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpConfigData, value);
    }

}
