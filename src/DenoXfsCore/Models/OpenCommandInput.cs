using System;
using System.Threading.Tasks;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Models;

public class OpenCommandInput
{
    public OpenRequest OpenRequest { get; set; } = default!;
    public IXfsApp XfsApp { get; set; } = default!;
    public IntPtr WindowHandle { get; set; } = IntPtr.Zero;
    public TaskCompletionSource<OpenResponse> TaskCompletionSource { get; set; } = new TaskCompletionSource<OpenResponse>();
}

