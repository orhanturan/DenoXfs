using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Idc.Infos;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcIfmIdentifier
{
    private IdcIfmAuthority wIFMAuthority;
    private IntPtr lpszIFMIdentifier;

    public IdcIfmAuthority IFMAuthority
    {
        get => wIFMAuthority;
        set => wIFMAuthority = value;
    }

    public string? IFMIdentifier
    {
        get => IntPtrConvert.ToString(lpszIFMIdentifier);
        set => StringConvert.ToIntPtr(ref lpszIFMIdentifier, value);
    }

}
