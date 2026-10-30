using System.Runtime.InteropServices;
using DenoXfs.Core.Extensions;
using DenoXfs.Core.Convert;
using DenoXfs.Core.Models.Enums.Idc;

namespace DenoXfs.Core.Idc;

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
