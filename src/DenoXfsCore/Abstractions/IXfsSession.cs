using System;
using DenoXfs.Core.Models.Requests;
using DenoXfs.Core.Models.Responses;

namespace DenoXfs.Core.Abstractions;

public interface IXfsSession : IDisposable
{
    /// <summary>WFSStartUp. A session that is already started reports success without calling the Manager again.</summary>
    StartUpResponse Startup(StartUpRequest request);
    void Stop();
}
