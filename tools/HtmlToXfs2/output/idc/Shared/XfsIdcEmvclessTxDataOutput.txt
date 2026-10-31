using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;

namespace DenoXfs.Idc.Shared;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcEmvclessTxDataOutput
{
    private ushort wDataSource;
    private IdcTxOutcomeEmvclessTxDataOutput wTxOutcome;
    private IdcCardholderActionEmvclessTxDataOutput wCardholderAction;
    private IntPtr lpDataRead;
    private IntPtr lpClessOutcome;

    public ushort DataSource
    {
        get => wDataSource;
        set => wDataSource = value;
    }

    public IdcTxOutcomeEmvclessTxDataOutput TxOutcome
    {
        get => wTxOutcome;
        set => wTxOutcome = value;
    }

    public IdcCardholderActionEmvclessTxDataOutput CardholderAction
    {
        get => wCardholderAction;
        set => wCardholderAction = value;
    }

    public XfsIdcHexData? DataRead
    {
        get => lpDataRead.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpDataRead, value);
    }

    public XfsIdcEmvclessOutcome? ClessOutcome
    {
        get => lpClessOutcome.ToStructSafe<XfsIdcEmvclessOutcome>();
        set => StructureConvert.ToIntPtr(ref lpClessOutcome, value);
    }

}
