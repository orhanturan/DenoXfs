using System;
using DenoXfs.Core.Models;

namespace DenoXfs.Core.Abstractions;

/// <summary>Raises the event messages the application registered for. Handlers run on the message thread and must return quickly.</summary>
public interface IXfsEventSource
{
    event Action<XfsEventMessage>? EventReceived;
}
