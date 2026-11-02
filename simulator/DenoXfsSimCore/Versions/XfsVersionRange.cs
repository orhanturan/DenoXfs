namespace DenoXfs.Sim.Core.Versions;

/// <summary>
/// XFS version range. A version word has the major number in the low byte and the minor number in the high byte
/// (3.40 = 0x2803). A "versions required" DWORD carries the highest version in its low word and the lowest in its high word.
/// </summary>
public readonly record struct XfsVersionRange(ushort Low, ushort High)
{
    public static XfsVersionRange FromRequired(uint versionsRequired) =>
        new((ushort)(versionsRequired >> 16), (ushort)(versionsRequired & 0xFFFF));

    /// <summary>Comparable number (major * 1000 + minor) of an encoded version word.</summary>
    public static int Rank(ushort version) => (version & 0xFF) * 1000 + (version >> 8);
}
