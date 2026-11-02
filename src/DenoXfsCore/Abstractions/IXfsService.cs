using System.Threading.Tasks;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Abstractions;

/// <summary>Asynchronous XFS calls (WFSAsync*): each completes when the completion message arrives.</summary>
public interface IXfsService
{
    Task<OpenResponse> OpenAsync(OpenRequest request);
    Task<CloseResponse> CloseAsync(CloseRequest request);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<ExecuteResponse> ExecuteAsync(ExecuteRequest request);
    Task<GetInfoResponse> GetInfoAsync(GetInfoRequest request);

    /// <summary>WFSCancelAsyncRequest (request id 0 = every request of the service); the request then completes with WFS_ERR_CANCELED.</summary>
    int CancelAsyncRequest(ushort hService, uint requestId);
}
