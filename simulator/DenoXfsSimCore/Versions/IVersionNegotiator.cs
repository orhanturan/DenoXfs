using DenoXfs.Sim.Core.Protocol;

namespace DenoXfs.Sim.Core.Versions;

public sealed record VersionNegotiation(int HResult, ushort Version);

/// <summary>Negotiates one version range pair as in WFPOpen (spec section 6.7).</summary>
public interface IVersionNegotiator
{
    /// <param name="requested">Range requested by the caller.</param>
    /// <param name="supported">Range this simulator supports.</param>
    /// <param name="tooLow">HRESULT when the caller's range lies below the supported range.</param>
    /// <param name="tooHigh">HRESULT when the caller's range lies above the supported range.</param>
    VersionNegotiation Negotiate(XfsVersionRange requested, XfsVersionRange supported, int tooLow, int tooHigh);
}

public sealed class VersionNegotiator : IVersionNegotiator
{
    public VersionNegotiation Negotiate(XfsVersionRange requested, XfsVersionRange supported, int tooLow, int tooHigh)
    {
        if (XfsVersionRange.Rank(requested.High) < XfsVersionRange.Rank(supported.Low))
            return new VersionNegotiation(tooLow, 0);
        if (XfsVersionRange.Rank(requested.Low) > XfsVersionRange.Rank(supported.High))
            return new VersionNegotiation(tooHigh, 0);

        var chosen = XfsVersionRange.Rank(requested.High) < XfsVersionRange.Rank(supported.High)
            ? requested.High
            : supported.High;
        return new VersionNegotiation(XfsResultCodes.Success, chosen);
    }
}
