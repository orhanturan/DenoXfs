using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
