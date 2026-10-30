using System;
using DenoXfs.Core.Abstractions;

namespace DenoXfs.Core.Models.Requests;

public class RegisterRequest : IXfsRequest
{
    public ushort HService { get; set; }
    public uint EventClass { get; set; }
    public IntPtr HWndReg { get; set; }
    
    public IXfsApp XfsApp { get; set; } = default!;
    public IntPtr WindowHandle { get; set; } = IntPtr.Zero;
}

