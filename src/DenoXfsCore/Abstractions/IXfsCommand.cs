using DenoXfs.Core.Api;

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
    /// Processes the completion result from XFS message
    /// </summary>
    void ProcessResult(XfsResult result);
}



