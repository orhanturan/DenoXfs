namespace DenoXfs.TestTool.Settings;

/// <summary>Whether a call uses the asynchronous (WFSAsync*) or the synchronous XFS function.</summary>
public enum CallMode
{
    Async,
    Sync
}

/// <summary>Event classes of WFSRegister.</summary>
[Flags]
public enum EventClasses
{
    None = 0,
    Service = 1,
    User = 2,
    System = 4,
    Execute = 8,
    All = Service | User | System | Execute
}
