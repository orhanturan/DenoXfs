using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Idc.Commands;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcParseData
{
    private IntPtr lpstrFormName;
    private IntPtr lppCardData;

    public string? FormName
    {
        get => IntPtrConvert.ToString(lpstrFormName);
        set => StringConvert.ToIntPtr(ref lpstrFormName, value);
    }

    public IntPtr CardData
    {
        get => lppCardData;
        set => lppCardData = value;
    }

}
