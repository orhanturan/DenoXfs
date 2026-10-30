using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Extensions;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcEmvclessTxData
{
    private IntPtr lpData;

    public XfsIdcHexData? Data
    {
        get => lpData.ToStructSafe<XfsIdcHexData>();
        set => StructureConvert.ToIntPtr(ref lpData, value);
    }

}
