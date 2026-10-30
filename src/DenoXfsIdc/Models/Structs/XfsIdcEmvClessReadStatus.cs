using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Extensions;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcEmvClessReadStatus
{
    private IntPtr lpClessUI;

    public XfsIdcEmvclessUi? ClessUI
    {
        get => lpClessUI.ToStructSafe<XfsIdcEmvclessUi>();
        set => StructureConvert.ToIntPtr(ref lpClessUI, value);
    }

}
