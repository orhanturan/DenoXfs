using System;
using DenoXfs.Api.Common;

namespace DenoXfs.Core.Abstractions;

/// <summary>
/// Base interface for all XFS commands (used by factory)
/// </summary>
public interface IXfsCommand
{
    /// <summary>
    /// Executes the command and returns CommandResponse with result and request ID
    /// </summary>
    CommandResponse Execute();

    /// <summary>
    /// Processes the completion result from XFS message.
    /// </summary>
    /// <param name="result">The WFSRESULT content.</param>
    /// <param name="resultHandle">Address of the WFSRESULT; it must be released with WFSFreeResult.</param>
    /// <returns>True when the command passed <paramref name="resultHandle"/> on to its caller, who now releases it;
    /// false when the message processor must release it.</returns>
    bool ProcessResult(XfsResult result, IntPtr resultHandle);
}
