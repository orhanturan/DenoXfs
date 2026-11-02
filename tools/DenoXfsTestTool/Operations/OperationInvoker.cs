using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models.Requests;
using DenoXfs.TestTool.Session;
using DenoXfs.TestTool.Settings;
using Microsoft.Extensions.Logging;

namespace DenoXfs.TestTool.Operations;

public class OperationInvoker : IOperationInvoker, ITrackedInvoker, ICallCanceller
{
    private readonly ISessionController _session;
    private readonly IXfsService _async;
    private readonly IXfsSyncService _sync;
    private readonly IXfsResultReleaser _releaser;
    private readonly TestToolSettings _settings;
    private readonly ILogger<OperationInvoker> _logger;

    public OperationInvoker(ISessionController session, IXfsService asyncService, IXfsSyncService syncService,
        IXfsResultReleaser releaser, TestToolSettings settings, ILogger<OperationInvoker> logger)
    {
        _session = session;
        _async = asyncService;
        _sync = syncService;
        _releaser = releaser;
        _settings = settings;
        _logger = logger;
    }

    public Task<RawResponse> GetInfoAsync(uint category, IntPtr query) => GetInfoAsync(category, query, null);

    public Task<RawResponse> ExecuteAsync(uint command, IntPtr input) => ExecuteAsync(command, input, null);

    public async Task<RawResponse> GetInfoAsync(uint category, IntPtr query, RunningCall? call)
    {
        var request = new GetInfoRequest { HService = RequireOpenSession(), Category = category, QueryDetails = query, TimeOut = _settings.TimeoutMs };
        request.RequestIdAssigned = id => call?.AsyncStarted(request.HService, id);
        request.BlockingThreadStarted = thread => call?.BlockingStarted(thread);
        _logger.LogInformation("getinfo mode={Mode} category={Category} hService={HService}", _settings.GetInfoMode, category, request.HService);

        var response = _settings.GetInfoMode == CallMode.Async
            ? await _async.GetInfoAsync(request)
            : await Task.Run(() => _sync.GetInfo(request));
        _logger.LogInformation("getinfo done category={Category} result={Result} reqId={ReqId}", category, ResultNames.Describe(response.Result), response.RequestId);
        return new RawResponse(response.Result, response.RequestId, response.ResultPtr, response.ResultHandle);
    }

    public async Task<RawResponse> ExecuteAsync(uint command, IntPtr input, RunningCall? call)
    {
        var request = new ExecuteRequest { HService = RequireOpenSession(), Command = command, CmdData = input, TimeOut = _settings.TimeoutMs };
        request.RequestIdAssigned = id => call?.AsyncStarted(request.HService, id);
        request.BlockingThreadStarted = thread => call?.BlockingStarted(thread);
        _logger.LogInformation("execute mode={Mode} command={Command} hService={HService}", _settings.ExecuteMode, command, request.HService);

        var response = _settings.ExecuteMode == CallMode.Async
            ? await _async.ExecuteAsync(request)
            : await Task.Run(() => _sync.Execute(request));
        _logger.LogInformation("execute done command={Command} result={Result} reqId={ReqId}", command, ResultNames.Describe(response.Result), response.RequestId);
        return new RawResponse(response.Result, response.RequestId, response.ResultPtr, response.ResultHandle);
    }

    public int CancelAsyncRequest(ushort hService, uint requestId) => _async.CancelAsyncRequest(hService, requestId);

    public int CancelBlockingCall(uint threadId) => _sync.CancelBlockingCall(threadId);

    public void Free(IntPtr resultHandle)
    {
        if (resultHandle == IntPtr.Zero)
            return;

        int result = _releaser.Free(resultHandle);
        if (result != 0)
            _logger.LogWarning("free result failed result={Result}", ResultNames.Describe(result));
    }

    private ushort RequireOpenSession()
    {
        if (_session.State != SessionState.Open)
            throw new InvalidOperationException("The session is not open. Open it in the Session tab first.");
        return _session.HService;
    }
}
