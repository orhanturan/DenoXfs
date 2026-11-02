namespace DenoXfs.TestTool.Operations;

/// <summary>Cancels XFS calls: WFSCancelAsyncRequest for async calls, WFSCancelBlockingCall for sync ones.</summary>
public interface ICallCanceller
{
    int CancelAsyncRequest(ushort hService, uint requestId);

    int CancelBlockingCall(uint threadId);
}

/// <summary>
/// One XFS call of an operation that may be cancelled. It learns the request id (async) or the blocking thread (sync)
/// when the call starts; a cancel pressed before that is applied as soon as it is known.
/// </summary>
public sealed class RunningCall
{
    private readonly object _lock = new();
    private readonly ICallCanceller _canceller;
    private ushort _hService;
    private uint? _requestId;
    private uint? _threadId;
    private bool _cancelRequested;

    public RunningCall(ICallCanceller canceller)
    {
        _canceller = canceller;
    }

    /// <summary>Raised with a short description of each cancel that was sent and its XFS result.</summary>
    public event Action<string, int>? CancelSent;

    public void AsyncStarted(ushort hService, uint requestId)
    {
        lock (_lock)
        {
            _hService = hService;
            _requestId = requestId;
            if (!_cancelRequested)
                return;
        }
        Send();
    }

    public void BlockingStarted(uint threadId)
    {
        lock (_lock)
        {
            _threadId = threadId;
            if (!_cancelRequested)
                return;
        }
        Send();
    }

    public void Cancel()
    {
        lock (_lock)
        {
            if (_cancelRequested)
                return;
            _cancelRequested = true;
            if (_requestId is null && _threadId is null)
                return; // sent when the call starts
        }
        Send();
    }

    private void Send()
    {
        uint? requestId, threadId;
        ushort hService;
        lock (_lock)
        {
            requestId = _requestId;
            threadId = _threadId;
            hService = _hService;
        }

        if (requestId is { } id)
            CancelSent?.Invoke($"WFSCancelAsyncRequest hService={hService} reqId={id}", _canceller.CancelAsyncRequest(hService, id));
        else if (threadId is { } thread)
            CancelSent?.Invoke($"WFSCancelBlockingCall thread={thread}", _canceller.CancelBlockingCall(thread));
    }
}
