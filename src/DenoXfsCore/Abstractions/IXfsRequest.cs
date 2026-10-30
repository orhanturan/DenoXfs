using System;

namespace DenoXfs.Core.Abstractions;

public interface IXfsRequest
{
    IXfsApp XfsApp { get; set; }
    IntPtr WindowHandle { get; set; }
}

