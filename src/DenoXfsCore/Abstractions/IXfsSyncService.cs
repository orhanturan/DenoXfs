using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Abstractions;

/// <summary>Synchronous XFS calls (WFSOpen, WFSExecute, ...): each blocks the calling thread until the XFS Manager returns.</summary>
public interface IXfsSyncService
{
    StartUpResponse StartUp(StartUpRequest request);
    OpenResponse Open(OpenRequest request);
    CloseResponse Close(CloseRequest request);
    RegisterResponse Register(RegisterRequest request);
    ExecuteResponse Execute(ExecuteRequest request);
    GetInfoResponse GetInfo(GetInfoRequest request);

    /// <summary>WFSCancelBlockingCall for the thread that blocks in a sync call (see <c>BlockingThreadStarted</c>).</summary>
    int CancelBlockingCall(uint threadId);

    /// <summary>WFMSetTraceLevel. Returns the XFS result code.</summary>
    int SetTraceLevel(SetTraceLevelRequest request);
}
