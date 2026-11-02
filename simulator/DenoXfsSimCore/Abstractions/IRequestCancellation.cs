namespace DenoXfs.Sim.Core.Abstractions;

/// <summary>
/// Requests of each session that are still running or waiting in the session queue, so WFPCancelAsyncRequest can end
/// them. The handler registers a request when it arrives and completes it when the completion has been decided.
/// </summary>
public interface IRequestCancellation
{
    /// <summary>Token that fires when the request (or every request of the session) is cancelled.</summary>
    CancellationToken Register(ISessionChannel channel, uint requestId);

    /// <summary>Cancels one request, or every request of the session when <paramref name="requestId"/> is 0. False when none was found.</summary>
    bool Cancel(ISessionChannel channel, uint requestId);

    void Complete(ISessionChannel channel, uint requestId);
}
