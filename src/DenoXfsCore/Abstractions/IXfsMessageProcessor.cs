using System;
using Microsoft.Extensions.Hosting;

namespace DenoXfs.Core.Abstractions;

public interface IXfsMessageProcessor : IHostedService
{
    IntPtr WindowHandle { get; }
    void EnqueueCommand(IXfsCommand command);
}

