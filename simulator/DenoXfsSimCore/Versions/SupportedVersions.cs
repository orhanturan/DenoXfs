namespace DenoXfs.Sim.Core.Versions;

/// <summary>Version ranges the simulator supports. Spec 3.40 only for now (3.40 = 0x2803).</summary>
public sealed record SupportedVersions(XfsVersionRange Spi, XfsVersionRange Service)
{
    public const ushort Version340 = 0x2803;
    public const ushort Version300 = 0x0003;

    public static SupportedVersions Spec340 { get; } =
        new(new XfsVersionRange(Version300, Version340), new XfsVersionRange(Version300, Version340));
}
