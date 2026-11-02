namespace DenoXfs.TestTool.Operations;

/// <summary>What an XFS code identifies. Info, command and event codes overlap, so a code is only unique per kind.</summary>
public enum XfsCodeKind
{
    Info,
    Command,
    ExecuteEvent,
    ServiceEvent,
    UserEvent,
    SystemEvent
}

/// <summary>Spec names of one device class (or of the API), e.g. (Command, 207, "WFS_CMD_IDC_READ_RAW_DATA").</summary>
public interface IXfsCodeNameSource
{
    IEnumerable<(XfsCodeKind Kind, uint Code, string Name)> Names { get; }
}

/// <summary>Readable text of an XFS code, e.g. "WFS_CMD_IDC_READ_RAW_DATA (207)".</summary>
public interface IXfsCodeNames
{
    string Describe(XfsCodeKind kind, uint code);
}
