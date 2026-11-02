using DenoXfs.TestTool.Logging;
using DenoXfs.TestTool.Session;
using Microsoft.Extensions.Logging;

namespace DenoXfs.TestTool.Operations;

public class OperationRunner : IOperationRunner
{
    private readonly ITrackedInvoker _invoker;
    private readonly ICallCanceller _canceller;
    private readonly ISessionController _session;
    private readonly IXfsTrafficLog _log;
    private readonly ILogger<OperationRunner> _logger;

    public OperationRunner(ITrackedInvoker invoker, ICallCanceller canceller, ISessionController session, IXfsTrafficLog log,
        ILogger<OperationRunner> logger)
    {
        _invoker = invoker;
        _canceller = canceller;
        _session = session;
        _log = log;
        _logger = logger;
    }

    public OperationRun Start(IOperationDefinition operation, object parameters)
    {
        var kind = operation.Kind == OperationKind.Info ? XfsCodeKind.Info : XfsCodeKind.Command;
        var call = new RunningCall(_canceller);
        call.CancelSent += (text, result) => _log.CancelSent(kind, operation.Code, text, result);
        var completion = RunAsync(operation, parameters, kind, new CallScopedInvoker(_invoker, call));
        return new OperationRun(completion, call.Cancel);
    }

    private async Task RunAsync(IOperationDefinition operation, object parameters, XfsCodeKind kind, IOperationInvoker invoker)
    {
        _log.Request(kind, operation.Code, _session.HService, parameters);
        try
        {
            var outcome = await operation.InvokeAsync(parameters, invoker);
            _log.Completion(kind, operation.Code, outcome.RequestId, outcome.Result, outcome.Decoded);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "operation failed name={Name}", operation.Name);
            _log.Failure(kind, operation.Code, ex.Message);
        }
    }

    /// <summary>The invoker an operation definition sees: every call it makes reports to the run's <see cref="RunningCall"/>.</summary>
    private sealed class CallScopedInvoker : IOperationInvoker
    {
        private readonly ITrackedInvoker _inner;
        private readonly RunningCall _call;

        public CallScopedInvoker(ITrackedInvoker inner, RunningCall call)
        {
            _inner = inner;
            _call = call;
        }

        public Task<RawResponse> GetInfoAsync(uint category, IntPtr query) => _inner.GetInfoAsync(category, query, _call);

        public Task<RawResponse> ExecuteAsync(uint command, IntPtr input) => _inner.ExecuteAsync(command, input, _call);

        public void Free(IntPtr resultHandle) => _inner.Free(resultHandle);
    }
}
