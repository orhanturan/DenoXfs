using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcEmvclessTxDataOutput
{
    private IdcDataSource wDataSource;
    private IdcTxOutcome wTxOutcome;
    private IdcCardholderAction wCardholderAction;
    private IntPtr lpDataRead;
    private IntPtr lpClessOutcome;

    public IdcDataSource DataSource
    {
        get => wDataSource;
        set => wDataSource = value;
    }

    public IdcTxOutcome TxOutcome
    {
        get => wTxOutcome;
        set => wTxOutcome = value;
    }

    public IdcCardholderAction CardholderAction
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
