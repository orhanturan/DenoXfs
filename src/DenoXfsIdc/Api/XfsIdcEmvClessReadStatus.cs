using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
