using System;
using DenoXfs.Core.Models.Requests;

namespace DenoXfs.Core.Abstractions;

public interface IXfsSession : IDisposable
{
    void Startup(StartUpRequest request);
    void Stop();
}






