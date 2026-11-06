using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models.Requests;
using DenoXfs.TestTool.Logging;
using DenoXfs.TestTool.Operations;
using DenoXfs.TestTool.Settings;
using Microsoft.Extensions.Logging;

namespace DenoXfs.TestTool.Session;

public class SessionController : ISessionController
{
    private readonly IXfsSession _session;
    private readonly IXfsService _async;
    private readonly IXfsSyncService _sync;
    private readonly IXfsMessageProcessor _messages;
    private readonly TestToolSettings _settings;
    private readonly IXfsApp _app;
    private readonly ILogicalServiceCatalog _logicalServices;
    private readonly IXfsTrafficLog _traffic;
    private readonly ILogger<SessionController> _logger;

    public SessionController(IXfsSession session, IXfsService asyncService, IXfsSyncService syncService,
        IXfsMessageProcessor messages, TestToolSettings settings, IXfsApp app, ILogicalServiceCatalog logicalServices, IXfsTrafficLog traffic,
        ILogger<SessionController> logger)
    {
        _session = session;
        _async = asyncService;
        _sync = syncService;
        _messages = messages;
        _settings = settings;
        _app = app;
        _logicalServices = logicalServices;
        _traffic = traffic;
        _logger = logger;
    }

    public SessionState State { get; private set; } = SessionState.Closed;

    public ushort HService { get; private set; }

    public event Action? StateChanged;

    public async Task<SessionResult> OpenAsync()
    {
        if (State != SessionState.Closed)
            return new SessionResult(false, $"Session is {State}.");

        SetState(SessionState.Opening);
        try
        {
            var startUp = await Task.Run(() => _session.Startup(new StartUpRequest { VersionsRequired = _settings.StartUpVersionsRequired }));
            _logger.LogInformation("startup versions=0x{Versions:X8} result={Result}", _settings.StartUpVersionsRequired, ResultNames.Describe(startUp.Result));
            _traffic.Session($"WFSStartUp versions=0x{_settings.StartUpVersionsRequired:X8} result={ResultNames.Describe(startUp.Result)}");
            if (startUp.Result != 0)
                return Fail($"WFSStartUp failed: {ResultNames.Describe(startUp.Result)}");

            var configured = _logicalServices.All().FirstOrDefault(s => string.Equals(s.Name, _settings.LogicalName, StringComparison.OrdinalIgnoreCase));
            var registry = configured is null ? "not in LOGICAL_SERVICES" : $"class={configured.Class} provider={configured.Provider}";
            var open = await OpenServiceAsync();
            _logger.LogInformation(
                "open mode={Mode} logical={Logical} ({Registry}) appId={AppId} traceLevel=0x{Trace:X8} timeout={Timeout} srvcVersionsRequired=0x{Required:X8} hService={HService} result={Result} service=0x{Service:X4} spi=0x{Spi:X4}",
                _settings.OpenMode, _settings.LogicalName, registry, _app.Name, _settings.TraceLevel, _settings.TimeoutMs, _settings.ServiceVersionsRequired,
                open.HService, ResultNames.Describe(open.Result), open.SrvcVersion.Version, open.SPIVersion.Version);
            _traffic.Session($"WFS{(_settings.OpenMode == CallMode.Async ? "Async" : string.Empty)}Open logical={_settings.LogicalName} ({registry}) appId={_app.Name} " +
                $"traceLevel=0x{_settings.TraceLevel:X8} timeout={_settings.TimeoutMs} srvcVersionsRequired=0x{_settings.ServiceVersionsRequired:X8} " +
                $"→ hService={open.HService} service=0x{open.SrvcVersion.Version:X4} spi=0x{open.SPIVersion.Version:X4} result={ResultNames.Describe(open.Result)}");
            if (open.Result != 0)
            {
                await Task.Run(_session.Stop);
                return Fail($"Open failed: {ResultNames.Describe(open.Result)}");
            }

            HService = open.HService;
            if (_settings.SetTraceLevelAfterOpen)
            {
                int trace = await Task.Run(() => _sync.SetTraceLevel(new SetTraceLevelRequest { HService = HService, TraceLevel = _settings.TraceLevel }));
                _logger.LogInformation("trace level hService={HService} level={Level} result={Result}", HService, _settings.TraceLevel, ResultNames.Describe(trace));
                _traffic.Session($"WFSSetTraceLevel level={_settings.TraceLevel} result={ResultNames.Describe(trace)}");
            }

            if (_settings.RegisterAfterOpen && _settings.EventClasses != EventClasses.None)
            {
                int register = await RegisterAsync();
                _logger.LogInformation("register mode={Mode} classes={Classes} result={Result}", _settings.RegisterMode, _settings.EventClasses, ResultNames.Describe(register));
                _traffic.Session($"WFSRegister classes={_settings.EventClasses} result={ResultNames.Describe(register)}");
                if (register != 0)
                {
                    // The session is usable after a failed register: keep it open and report the problem.
                    SetState(SessionState.Open);
                    return new SessionResult(true, $"Open, hService={HService}; register failed: {ResultNames.Describe(register)}");
                }
            }

            SetState(SessionState.Open);
            return new SessionResult(true, $"Open, hService={HService}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "open failed");
            _traffic.Session($"Open failed: {ex.Message}");
            HService = 0;
            return Fail(ex.Message);
        }
    }

    public async Task<SessionResult> CloseAsync()
    {
        if (State is SessionState.Closed or SessionState.Closing)
            return new SessionResult(false, $"Session is {State}.");

        SetState(SessionState.Closing);
        try
        {
            int close = _settings.CloseMode == CallMode.Async
                ? (await _async.CloseAsync(new CloseRequest { HService = HService })).Result
                : (await Task.Run(() => _sync.Close(new CloseRequest { HService = HService }))).Result;
            _logger.LogInformation("close mode={Mode} hService={HService} result={Result}", _settings.CloseMode, HService, ResultNames.Describe(close));
            _traffic.Session($"WFSClose hService={HService} result={ResultNames.Describe(close)}");

            await Task.Run(_session.Stop);
            HService = 0;
            SetState(SessionState.Closed);
            return new SessionResult(close == 0, close == 0 ? "Closed." : $"Close returned {ResultNames.Describe(close)}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "close failed");
            _traffic.Session($"Close failed: {ex.Message}");
            HService = 0;
            SetState(SessionState.Closed);
            return new SessionResult(false, ex.Message);
        }
    }

    private async Task<DenoXfs.Core.Models.Responses.OpenResponse> OpenServiceAsync()
    {
        var request = new OpenRequest
        {
            LogicalName = _settings.LogicalName,
            TraceLevel = _settings.TraceLevel,
            SrvcVersionsRequired = _settings.ServiceVersionsRequired,
            TimeOut = _settings.TimeoutMs
        };
        return _settings.OpenMode == CallMode.Async
            ? await _async.OpenAsync(request)
            : await Task.Run(() => _sync.Open(request));
    }

    private async Task<int> RegisterAsync()
    {
        var request = new RegisterRequest { HService = HService, EventClass = (uint)_settings.EventClasses, HWndReg = _messages.WindowHandle };
        return _settings.RegisterMode == CallMode.Async
            ? (await _async.RegisterAsync(request)).Result
            : (await Task.Run(() => _sync.Register(request))).Result;
    }

    private SessionResult Fail(string message)
    {
        HService = 0;
        SetState(SessionState.Closed);
        return new SessionResult(false, message);
    }

    private void SetState(SessionState state)
    {
        State = state;
        StateChanged?.Invoke();
    }
}
