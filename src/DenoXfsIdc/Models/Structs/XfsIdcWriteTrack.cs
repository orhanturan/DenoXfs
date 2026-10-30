using System.Runtime.InteropServices;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcWriteTrack
{
    private IntPtr lpstrFormName;
    private IntPtr lpstrTrackData;
    private IdcWriteMethod fwWriteMethod;

    public string? FormName
    {
        get => IntPtrConvert.ToString(lpstrFormName);
        set => StringConvert.ToIntPtr(ref lpstrFormName, value);
    }

    public string? TrackData
    {
        get => IntPtrConvert.ToString(lpstrTrackData);
        set => StringConvert.ToIntPtr(ref lpstrTrackData, value);
    }

    public IdcWriteMethod WriteMethod
    {
        get => fwWriteMethod;
        set => fwWriteMethod = value;
    }

}
