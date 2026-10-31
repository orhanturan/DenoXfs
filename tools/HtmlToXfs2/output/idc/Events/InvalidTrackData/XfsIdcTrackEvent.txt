using System.Runtime.InteropServices;
using DenoXfs.Interop.Convert;
using DenoXfs.Idc.Shared;

namespace DenoXfs.Idc.Events;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcTrackEvent
{
    private IdcDataStatus fwStatus;
    private IntPtr lpstrTrack;
    private IntPtr lpstrData;

    public IdcDataStatus Status
    {
        get => fwStatus;
        set => fwStatus = value;
    }

    public string? Track
    {
        get => IntPtrConvert.ToString(lpstrTrack);
        set => StringConvert.ToIntPtr(ref lpstrTrack, value);
    }

    public string? Data
    {
        get => IntPtrConvert.ToString(lpstrData);
        set => StringConvert.ToIntPtr(ref lpstrData, value);
    }

}
