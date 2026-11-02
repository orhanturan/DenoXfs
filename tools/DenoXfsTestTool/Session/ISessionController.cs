namespace DenoXfs.TestTool.Session;

public enum SessionState
{
    Closed,
    Opening,
    Open,
    Closing
}

public sealed record SessionResult(bool Success, string Message);

/// <summary>Owns the XFS session: StartUp, Open, SetTraceLevel, Register and the way back.</summary>
public interface ISessionController
{
    SessionState State { get; }

    /// <summary>Service handle of the open session; 0 when closed.</summary>
    ushort HService { get; }

    event Action? StateChanged;

    Task<SessionResult> OpenAsync();

    Task<SessionResult> CloseAsync();
}
