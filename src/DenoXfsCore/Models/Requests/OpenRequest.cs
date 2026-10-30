using System.Threading.Tasks;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Models.Requests;

public class OpenRequest : XfsRequest
{
    public string LogicalName { get; set; } = string.Empty;
    public uint TraceLevel { get; set; }
    public uint SrvcVersionsRequired { get; set; }
    public TaskCompletionSource<OpenResponse> TaskCompletionSource { get; set; } = new TaskCompletionSource<OpenResponse>();
}

