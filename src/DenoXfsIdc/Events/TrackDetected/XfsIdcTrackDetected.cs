using System.Runtime.InteropServices;

namespace DenoXfs.Idc.Events;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct XfsIdcTrackDetected
{
    private ushort fwTracks;

    public ushort Tracks
    {
        get => fwTracks;
        set => fwTracks = value;
    }

}
