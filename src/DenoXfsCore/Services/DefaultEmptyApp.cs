using System;
using DenoXfs.Core.Abstractions;

namespace DenoXfs.Core.Services;

public class DefaultEmptyApp : IXfsApp
{
    public string Name => string.Empty;

    public IntPtr Handle => IntPtr.Zero;

    public IntPtr Create() => IntPtr.Zero;

    public void Destroy() { }
}






