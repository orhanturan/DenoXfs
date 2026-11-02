using System;

namespace DenoXfs.Core.Models.Responses;

public class GetInfoResponse
{
    public int Result { get; set; }
    public uint RequestId { get; set; }

    /// <summary>The WFSRESULT.lpBuffer content (valid until the result is released).</summary>
    public IntPtr ResultPtr { get; set; }

    /// <summary>The WFSRESULT itself. The caller releases it with <c>IXfsResultReleaser.Free</c>; zero when there is none.</summary>
    public IntPtr ResultHandle { get; set; }
}
