using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcEmvclessConfigData
{
    private IntPtr lpTerminalData;
    private IntPtr lppAIDData;
    private IntPtr lppKeyData;

    public XfsIdcHexData? TerminalData
    {
        get => lpTerminalData.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpTerminalData, value);
    }

    public IntPtr AIDData
    {
        get => lppAIDData;
        set => lppAIDData = value;
    }

    public IntPtr KeyData
    {
        get => lppKeyData;
        set => lppKeyData = value;
    }

}
