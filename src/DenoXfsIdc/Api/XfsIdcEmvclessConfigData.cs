using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
