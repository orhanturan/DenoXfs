namespace DenoXfs.Sim.Core.Names;

public enum XfsCodeKind
{
    Info,
    Command,
    Event,
    Result
}

/// <summary>Spec names of one device class (or of XFSAPI.H); hand written until HtmlToXfs2 generates them.</summary>
public interface IXfsCodeNameSource
{
    IEnumerable<(XfsCodeKind Kind, int Code, string Name)> Names { get; }
}

/// <summary>All spec names of the registered sources (codes of the device classes do not overlap: IDC is 2xx, CDM 3xx, ...).</summary>
public interface IXfsCodeNames
{
    string? NameOf(XfsCodeKind kind, int code);

    /// <summary>"WFS_CMD_IDC_READ_RAW_DATA (207)", or the bare code when it has no name.</summary>
    string Describe(XfsCodeKind kind, int code);

    bool TryCode(XfsCodeKind kind, string name, out int code);

    IReadOnlyList<(int Code, string Name)> All(XfsCodeKind kind);
}

public sealed class XfsCodeNames : IXfsCodeNames
{
    private readonly Dictionary<(XfsCodeKind, int), string> _names = new();
    private readonly Dictionary<(XfsCodeKind, string), int> _codes = new();

    public XfsCodeNames(IEnumerable<IXfsCodeNameSource> sources)
    {
        foreach (var (kind, code, name) in sources.SelectMany(s => s.Names))
        {
            _names.TryAdd((kind, code), name);
            _codes.TryAdd((kind, name.ToUpperInvariant()), code);
        }
    }

    public string? NameOf(XfsCodeKind kind, int code) => _names.GetValueOrDefault((kind, code));

    public string Describe(XfsCodeKind kind, int code) => NameOf(kind, code) is { } name ? $"{name} ({code})" : code.ToString();

    public bool TryCode(XfsCodeKind kind, string name, out int code) => _codes.TryGetValue((kind, name.Trim().ToUpperInvariant()), out code);

    public IReadOnlyList<(int Code, string Name)> All(XfsCodeKind kind) =>
        _names.Where(n => n.Key.Item1 == kind).Select(n => (n.Key.Item2, n.Value)).OrderBy(n => Math.Abs(n.Item1)).ToList();
}

/// <summary>XFSAPI.H: the API errors and the system events.</summary>
public sealed class ApiCodeNames : IXfsCodeNameSource
{
    private static readonly string[] Errors =
    [
        "ALREADY_STARTED", "API_VER_TOO_HIGH", "API_VER_TOO_LOW", "CANCELED", "CFG_INVALID_HKEY", "CFG_INVALID_NAME", "CFG_INVALID_SUBKEY",
        "CFG_INVALID_VALUE", "CFG_KEY_NOT_EMPTY", "CFG_NAME_TOO_LONG", "CFG_NO_MORE_ITEMS", "CFG_VALUE_TOO_LONG", "DEV_NOT_READY",
        "HARDWARE_ERROR", "INTERNAL_ERROR", "INVALID_ADDRESS", "INVALID_APP_HANDLE", "INVALID_BUFFER", "INVALID_CATEGORY", "INVALID_COMMAND",
        "INVALID_EVENT_CLASS", "INVALID_HSERVICE", "INVALID_HPROVIDER", "INVALID_HWND", "INVALID_HWNDREG", "INVALID_POINTER", "INVALID_REQ_ID",
        "INVALID_RESULT", "INVALID_SERVPROV", "INVALID_TIMER", "INVALID_TRACELEVEL", "LOCKED", "NO_BLOCKING_CALL", "NO_SERVPROV",
        "NO_SUCH_THREAD", "NO_TIMER", "NOT_LOCKED", "NOT_OK_TO_UNLOAD", "NOT_STARTED", "NOT_REGISTERED", "OP_IN_PROGRESS", "OUT_OF_MEMORY",
        "SERVICE_NOT_FOUND", "SPI_VER_TOO_HIGH", "SPI_VER_TOO_LOW", "SRVC_VER_TOO_HIGH", "SRVC_VER_TOO_LOW", "TIMEOUT", "UNSUPP_CATEGORY",
        "UNSUPP_COMMAND", "VERSION_ERROR_IN_SRVC", "INVALID_DATA", "SOFTWARE_ERROR", "CONNECTION_LOST", "USER_ERROR", "UNSUPP_DATA",
        "FRAUD_ATTEMPT", "SEQUENCE_ERROR", "AUTH_REQUIRED"
    ];

    private static readonly string[] SystemEvents =
    [
        "UNDELIVERABLE_MSG", "HARDWARE_ERROR", "VERSION_ERROR", "DEVICE_STATUS", "APP_DISCONNECT", "SOFTWARE_ERROR", "USER_ERROR",
        "LOCK_REQUESTED", "FRAUD_ATTEMPT"
    ];

    public IEnumerable<(XfsCodeKind Kind, int Code, string Name)> Names =>
        Errors.Select((name, i) => (XfsCodeKind.Result, -(i + 1), $"WFS_ERR_{name}"))
            .Append((XfsCodeKind.Result, 0, "WFS_SUCCESS"))
            .Concat(SystemEvents.Select((name, i) => (XfsCodeKind.Event, i + 1, $"WFS_SYSE_{name}")));
}
