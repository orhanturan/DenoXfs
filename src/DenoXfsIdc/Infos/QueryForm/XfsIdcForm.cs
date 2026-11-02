using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;

namespace DenoXfs.Idc.Infos;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcForm
{
    private IntPtr lpszFormName;
    private byte cFieldSeparatorTrack1;
    private byte cFieldSeparatorTrack2;
    private byte cFieldSeparatorTrack3;
    private IdcActionForm fwAction;
    private IntPtr lpszTracks;
    [MarshalAs(UnmanagedType.Bool)]
    private bool bSecure;
    private IntPtr lpszTrack1Fields;
    private IntPtr lpszTrack2Fields;
    private IntPtr lpszTrack3Fields;
    private IntPtr lpszFrontTrack1Fields;
    private byte cFieldSeparatorFrontTrack1;
    private IntPtr lpszJIS1Track1Fields;
    private IntPtr lpszJIS1Track3Fields;
    private byte cFieldSeparatorJIS1Track1;
    private byte cFieldSeparatorJIS1Track3;

    public string? FormName
    {
        get => IntPtrConvert.ToString(lpszFormName);
        set => StringConvert.ToIntPtr(ref lpszFormName, value);
    }

    public byte FieldSeparatorTrack1
    {
        get => cFieldSeparatorTrack1;
        set => cFieldSeparatorTrack1 = value;
    }

    public byte FieldSeparatorTrack2
    {
        get => cFieldSeparatorTrack2;
        set => cFieldSeparatorTrack2 = value;
    }

    public byte FieldSeparatorTrack3
    {
        get => cFieldSeparatorTrack3;
        set => cFieldSeparatorTrack3 = value;
    }

    public IdcActionForm Action
    {
        get => fwAction;
        set => fwAction = value;
    }

    public string? Tracks
    {
        get => IntPtrConvert.ToString(lpszTracks);
        set => StringConvert.ToIntPtr(ref lpszTracks, value);
    }

    public bool Secure
    {
        get => bSecure;
        set => bSecure = value;
    }

    public IReadOnlyList<string>? Track1Fields
    {
        get => MultiStringConvert.ToManaged(lpszTrack1Fields);
        set => MultiStringConvert.ToIntPtr(ref lpszTrack1Fields, value);
    }

    public IReadOnlyList<string>? Track2Fields
    {
        get => MultiStringConvert.ToManaged(lpszTrack2Fields);
        set => MultiStringConvert.ToIntPtr(ref lpszTrack2Fields, value);
    }

    public IReadOnlyList<string>? Track3Fields
    {
        get => MultiStringConvert.ToManaged(lpszTrack3Fields);
        set => MultiStringConvert.ToIntPtr(ref lpszTrack3Fields, value);
    }

    public IReadOnlyList<string>? FrontTrack1Fields
    {
        get => MultiStringConvert.ToManaged(lpszFrontTrack1Fields);
        set => MultiStringConvert.ToIntPtr(ref lpszFrontTrack1Fields, value);
    }

    public byte FieldSeparatorFrontTrack1
    {
        get => cFieldSeparatorFrontTrack1;
        set => cFieldSeparatorFrontTrack1 = value;
    }

    public IReadOnlyList<string>? JIS1Track1Fields
    {
        get => MultiStringConvert.ToManaged(lpszJIS1Track1Fields);
        set => MultiStringConvert.ToIntPtr(ref lpszJIS1Track1Fields, value);
    }

    public IReadOnlyList<string>? JIS1Track3Fields
    {
        get => MultiStringConvert.ToManaged(lpszJIS1Track3Fields);
        set => MultiStringConvert.ToIntPtr(ref lpszJIS1Track3Fields, value);
    }

    public byte FieldSeparatorJIS1Track1
    {
        get => cFieldSeparatorJIS1Track1;
        set => cFieldSeparatorJIS1Track1 = value;
    }

    public byte FieldSeparatorJIS1Track3
    {
        get => cFieldSeparatorJIS1Track3;
        set => cFieldSeparatorJIS1Track3 = value;
    }

}
