using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Extensions;

namespace DenoXfs.Core.Api;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsApiTransactionState
{
    private ushort fwState;
    private IntPtr lpTransactionInfo;

    public ushort State
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
