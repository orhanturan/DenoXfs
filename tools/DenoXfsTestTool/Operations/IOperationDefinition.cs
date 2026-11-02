namespace DenoXfs.TestTool.Operations;

public enum OperationKind
{
    Info,
    Execute
}

/// <summary>Raw answer of a GetInfo or Execute call. The caller must release <see cref="ResultHandle"/> through <see cref="IOperationInvoker.Free"/>.</summary>
public sealed record RawResponse(int Result, uint RequestId, IntPtr ResultPtr, IntPtr ResultHandle);

/// <summary>What the panel shows after an operation ran.</summary>
public sealed record OperationOutcome(int Result, string ResultName, uint RequestId, object? Decoded, string Summary);

/// <summary>Sends GetInfo and Execute calls on the open session, in the call mode of the settings.</summary>
public interface IOperationInvoker
{
    Task<RawResponse> GetInfoAsync(uint category, IntPtr query);

    Task<RawResponse> ExecuteAsync(uint command, IntPtr input);

    void Free(IntPtr resultHandle);
}

/// <summary>Invoker that reports the request id / blocking thread of each call to a <see cref="RunningCall"/>.</summary>
public interface ITrackedInvoker
{
    Task<RawResponse> GetInfoAsync(uint category, IntPtr query, RunningCall? call);

    Task<RawResponse> ExecuteAsync(uint command, IntPtr input, RunningCall? call);

    void Free(IntPtr resultHandle);
}

/// <summary>One info or execute command shown in the Info or Commands tab. Definitions are registered in DI.</summary>
public interface IOperationDefinition
{
    string Name { get; }

    OperationKind Kind { get; }

    uint Code { get; }

    string Description { get; }

    /// <summary>New parameter object shown in the property grid.</summary>
    object CreateParameters();

    Task<OperationOutcome> InvokeAsync(object parameters, IOperationInvoker invoker);
}
