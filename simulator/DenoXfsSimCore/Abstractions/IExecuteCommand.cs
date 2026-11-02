using System.Text.Json;
using DenoXfs.Sim.Core.Protocol;

namespace DenoXfs.Sim.Core.Abstractions;

/// <summary>Input of an execute command as decoded by the DLL, plus the way back for its execute events.</summary>
public sealed record ExecuteContext(string RemoteAddress, uint Command, JsonElement Input, IExecuteEvents Events);

/// <summary>Result of a command or info category: XFS HRESULT plus an optional result block.</summary>
public sealed record CommandOutcome(int HResult, ResultBuffer? Result);

/// <summary>
/// One simulated execute command of a device class (registered once per command). The cancellation token fires when
/// the request times out (the handler then reports WFS_ERR_TIMEOUT) or the session ends.
/// </summary>
public interface IExecuteCommand
{
    uint Command { get; }

    Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken);
}

/// <summary>Execute events (WFS_EXECUTE_EVENT) of the running request; they go to the window that sent it.</summary>
public interface IExecuteEvents
{
    Task SendAsync(uint eventId, ResultBuffer? payload, CancellationToken cancellationToken);
}
