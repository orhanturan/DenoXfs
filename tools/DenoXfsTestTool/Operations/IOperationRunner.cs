namespace DenoXfs.TestTool.Operations;

/// <summary>A started operation: completes when the XFS call completed (or was cancelled).</summary>
public sealed class OperationRun
{
    private readonly Action _cancel;

    public OperationRun(Task completion, Action cancel)
    {
        Completion = completion;
        _cancel = cancel;
    }

    public Task Completion { get; }

    /// <summary>WFSCancelAsyncRequest (async mode) or WFSCancelBlockingCall (sync mode) for the running call.</summary>
    public void Cancel() => _cancel();
}

/// <summary>Runs an operation and writes its request, completion, cancel or failure to the XFS log.</summary>
public interface IOperationRunner
{
    OperationRun Start(IOperationDefinition operation, object parameters);
}
