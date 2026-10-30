using System.Runtime.InteropServices;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Idc.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcTrackDetected
{
    private IdcTracks fwTracks;

    public IdcTracks Tracks
    {
        get => fwTracks;
        set => fwTracks = value;
    }

}
