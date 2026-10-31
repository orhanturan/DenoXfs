using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;

namespace DenoXfs.Idc.Shared;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcEmvclessOutcome
{
    private IdcCVMEmvclessOutcome wCVM;
    private IdcAlternateInterfaceEmvclessOutcome wAlternateInterface;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bReceipt;
    private IntPtr lpClessUIOutcome;
    private IntPtr lpClessUIRestart;
    private uint ulClessFieldOffHoldTime;
    private uint ulCardRemovalTimeoutValue;
    private IntPtr lpDiscretionaryData;

    public IdcCVMEmvclessOutcome CVM
    {
        get => wCVM;
        set => wCVM = value;
    }

    public IdcAlternateInterfaceEmvclessOutcome AlternateInterface
    {
        get => wAlternateInterface;
        set => wAlternateInterface = value;
    }

    public bool Receipt
    {
        get => bReceipt;
        set => bReceipt = value;
    }

    public XfsIdcEmvclessUi? ClessUIOutcome
    {
        get => lpClessUIOutcome.ToStructSafe<XfsIdcEmvclessUi>();
        set => StructureConvert.ToIntPtr(ref lpClessUIOutcome, value);
    }

    public XfsIdcEmvclessUi? ClessUIRestart
    {
        get => lpClessUIRestart.ToStructSafe<XfsIdcEmvclessUi>();
        set => StructureConvert.ToIntPtr(ref lpClessUIRestart, value);
    }

    public uint ClessFieldOffHoldTime
    {
        get => ulClessFieldOffHoldTime;
        set => ulClessFieldOffHoldTime = value;
    }

    public uint CardRemovalTimeoutValue
    {
        get => ulCardRemovalTimeoutValue;
        set => ulCardRemovalTimeoutValue = value;
    }

    public XfsIdcHexData? DiscretionaryData
    {
        get => lpDiscretionaryData.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpDiscretionaryData, value);
    }

}
