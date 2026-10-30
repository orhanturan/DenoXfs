using System;
using System.Threading.Tasks;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Models;

public class GetInfoCommandInput
{
    public GetInfoRequest GetInfoRequest { get; set; } = default!;
    public IXfsApp XfsApp { get; set; } = default!;
    public IntPtr WindowHandle { get; set; } = IntPtr.Zero;
    public TaskCompletionSource<GetInfoResponse> TaskCompletionSource { get; set; } = new TaskCompletionSource<GetInfoResponse>();
}

