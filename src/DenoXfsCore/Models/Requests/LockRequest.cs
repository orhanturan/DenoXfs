using System;
using DenoXfs.Core.Abstractions;

namespace DenoXfs.Core.Models.Requests;

public class LockRequest : IXfsRequest
{
    public ushort HService { get; set; }
    public uint TimeOut { get; set; }
    
    public IXfsApp XfsApp { get; set; } = default!;
    public IntPtr WindowHandle { get; set; } = IntPtr.Zero;
}

