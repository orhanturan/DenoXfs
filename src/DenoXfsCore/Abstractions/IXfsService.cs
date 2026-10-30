using System.Threading.Tasks;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Abstractions;

public interface IXfsService
{
    Task<OpenResponse> OpenAsync(OpenRequest request);
    Task<ExecuteResponse> ExecuteAsync(ExecuteRequest request);
    Task<GetInfoResponse> GetInfoAsync(GetInfoRequest request);
}






