using System;

namespace DenoXfs.Core.Abstractions;

/// <summary>Releases a WFSRESULT (and the blocks linked to it) with WFSFreeResult.</summary>
public interface IXfsResultReleaser
{
    /// <returns>The XFS result code; success when <paramref name="resultHandle"/> is zero.</returns>
    int Free(IntPtr resultHandle);
}
