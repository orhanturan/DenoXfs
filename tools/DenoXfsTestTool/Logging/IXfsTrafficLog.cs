using DenoXfs.Core.Models;
using DenoXfs.TestTool.Operations;

namespace DenoXfs.TestTool.Logging;

/// <summary>
/// The XFS log shown on screen: requests with their input, completions with their output, events and session steps.
/// Each line is also written to the file log, the dumps at Debug level.
/// </summary>
public interface IXfsTrafficLog
{
    void Request(XfsCodeKind kind, uint code, ushort hService, object? input);

    void Completion(XfsCodeKind kind, uint code, uint requestId, int result, object? output);

    void Failure(XfsCodeKind kind, uint code, string message);

    /// <summary>A cancel was sent for the running call (<paramref name="call"/> names the API call, <paramref name="result"/> its return code).</summary>
    void CancelSent(XfsCodeKind kind, uint code, string call, int result);

    /// <summary>An event with its decoded payload (null when there is none or no decoder); dumped at once.</summary>
    void Event(XfsEventMessage message, object? payload);

    void Session(string text);
}
