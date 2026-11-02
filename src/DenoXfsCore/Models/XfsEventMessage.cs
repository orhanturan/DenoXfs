namespace DenoXfs.Core.Models;

public enum XfsEventKind
{
    Execute,
    Service,
    User,
    System
}

/// <summary>
/// An event message (WFS_*_EVENT) received from a Service Provider. <see cref="Buffer"/> is the WFSRESULT.lpBuffer
/// (the event payload, may be zero); it is valid only while the event handler runs, the result is freed afterwards.
/// </summary>
public sealed record XfsEventMessage(XfsEventKind Kind, uint EventId, ushort HService, uint RequestId, int Result, System.IntPtr Buffer);
