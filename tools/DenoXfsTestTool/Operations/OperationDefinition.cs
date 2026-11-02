namespace DenoXfs.TestTool.Operations;

/// <summary>Parameters of operations that take no input.</summary>
public sealed class NoParameters
{
}

/// <summary>
/// Template for an operation: send, decode the result buffer, always release the WFSRESULT.
/// A subclass only says how to send and how to decode.
/// </summary>
public abstract class OperationDefinition<TParameters> : IOperationDefinition where TParameters : class, new()
{
    public abstract string Name { get; }

    public abstract OperationKind Kind { get; }

    public abstract uint Code { get; }

    public virtual string Description => string.Empty;

    public object CreateParameters() => new TParameters();

    public async Task<OperationOutcome> InvokeAsync(object parameters, IOperationInvoker invoker)
    {
        var response = await SendAsync((TParameters)parameters, invoker);
        try
        {
            object? decoded = response.ResultPtr != IntPtr.Zero ? Decode(response.ResultPtr) : null;
            var summary = $"{ResultNames.Describe(response.Result)}, reqId={response.RequestId}, buffer={(response.ResultPtr != IntPtr.Zero ? "yes" : "none")}";
            return new OperationOutcome(response.Result, ResultNames.Describe(response.Result), response.RequestId, decoded, summary);
        }
        finally
        {
            invoker.Free(response.ResultHandle);
        }
    }

    protected abstract Task<RawResponse> SendAsync(TParameters parameters, IOperationInvoker invoker);

    /// <summary>Reads the result buffer (WFSRESULT.lpBuffer); it is released after this returns.</summary>
    protected abstract object? Decode(IntPtr buffer);
}
