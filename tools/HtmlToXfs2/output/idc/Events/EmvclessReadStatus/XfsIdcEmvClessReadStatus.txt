using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Interop.Extensions;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Events;

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
