using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;

namespace DenoXfs.Idc.Shared;

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
