using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;

namespace DenoXfs.Api.Common;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsApiTransactionState
{
    private TransactionState fwState;
    private IntPtr lpTransactionInfo;

    public TransactionState State
    {
        get => fwState;
        set => fwState = value;
    }

    public XfsApiTransactionInfo? TransactionInfo
    {
        get => lpTransactionInfo.ToStructSafe<XfsApiTransactionInfo>();
        set => StructureConvert.ToIntPtr(ref lpTransactionInfo, value);
    }

}
