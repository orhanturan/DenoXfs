using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Versions;

namespace DenoXfs.Sim.Tests;

public class VersionNegotiatorTests
{
    private readonly VersionNegotiator _negotiator = new();
    private static readonly XfsVersionRange Supported = new(0x0003, 0x2803); // 3.00 - 3.40

    [Fact]
    public void Overlapping_range_picks_highest_common_version()
    {
        var requested = XfsVersionRange.FromRequired(0x0003_2803); // 3.00 - 3.40
        var result = _negotiator.Negotiate(requested, Supported, XfsResultCodes.SpiVersionTooLow, XfsResultCodes.SpiVersionTooHigh);

        Assert.Equal(XfsResultCodes.Success, result.HResult);
        Assert.Equal(0x2803, result.Version);
    }

    [Fact]
    public void Request_for_lower_maximum_is_honoured()
    {
        var requested = XfsVersionRange.FromRequired(0x0003_1403); // 3.00 - 3.20
        var result = _negotiator.Negotiate(requested, Supported, XfsResultCodes.SpiVersionTooLow, XfsResultCodes.SpiVersionTooHigh);

        Assert.Equal(0x1403, result.Version);
    }

    [Fact]
    public void Request_above_supported_range_is_too_high()
    {
        var requested = XfsVersionRange.FromRequired(0x0004_0004); // 4.00
        var result = _negotiator.Negotiate(requested, Supported, XfsResultCodes.SpiVersionTooLow, XfsResultCodes.SpiVersionTooHigh);

        Assert.Equal(XfsResultCodes.SpiVersionTooHigh, result.HResult);
        Assert.Equal(0, result.Version);
    }

    [Fact]
    public void Request_below_supported_range_is_too_low()
    {
        var narrow = new XfsVersionRange(0x1403, 0x2803); // 3.20 - 3.40
        var requested = XfsVersionRange.FromRequired(0x0003_0003); // 3.00
        var result = _negotiator.Negotiate(requested, narrow, XfsResultCodes.ServiceVersionTooLow, XfsResultCodes.ServiceVersionTooHigh);

        Assert.Equal(XfsResultCodes.ServiceVersionTooLow, result.HResult);
    }
}
