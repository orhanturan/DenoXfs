using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Extensions;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcEmvclessConfigData
{
    private IntPtr lpTerminalData;

    public XfsIdcHexData? TerminalData
    {
        get => lpTerminalData.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpTerminalData, value);
    }

}
