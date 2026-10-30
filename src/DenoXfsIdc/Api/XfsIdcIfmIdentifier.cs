using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcIfmIdentifier
{
    private IdcIFMAuthority wIFMAuthority;
    private IntPtr lpszIFMIdentifier;

    public IdcIFMAuthority IFMAuthority
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
