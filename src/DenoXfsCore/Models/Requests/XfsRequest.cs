using System;
using DenoXfs.Core.Abstractions;

namespace DenoXfs.Core.Models.Requests;

/// <summary>
/// Base class for all XFS requests that provides common properties
/// </summary>
public abstract class XfsRequest : IXfsRequest
{
    public IXfsApp XfsApp { get; set; } = default!;
    public IntPtr WindowHandle { get; set; } = IntPtr.Zero;
    
    /// <summary>
    /// Timeout value in milliseconds for the request
    /// </summary>
    public uint TimeOut { get; set; }
    
    /// <summary>
    /// User timeout value in milliseconds (optional, for user interaction timeouts)
    /// </summary>
    public uint UserTimeOut { get; set; }
}


