using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcParseData
{
    private IntPtr lpstrFormName;

    public string? FormName
    {
        get => IntPtrConvert.ToString(lpstrFormName);
        set => StringConvert.ToIntPtr(ref lpstrFormName, value);
    }

}
