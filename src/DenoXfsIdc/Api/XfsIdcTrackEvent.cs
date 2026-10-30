using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcTrackEvent
{
    private IdcInvalidtrackdataStatus fwStatus;
    private IntPtr lpstrTrack;
    private IntPtr lpstrData;

    public IdcInvalidtrackdataStatus Status
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
