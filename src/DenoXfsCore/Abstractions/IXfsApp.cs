using System;

namespace DenoXfs.Core.Abstractions;

public interface IXfsApp
{
    string Name { get; }
    IntPtr Handle { get; }
    IntPtr Create();
    void Destroy();
}






