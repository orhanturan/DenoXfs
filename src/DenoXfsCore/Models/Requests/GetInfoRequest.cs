using System;

namespace DenoXfs.Core.Models.Requests;

public class GetInfoRequest : XfsRequest
{
    public ushort HService { get; set; }
    public uint Category { get; set; }
    public IntPtr QueryDetails { get; set; }

    /// <summary>Async calls: called with the request id as soon as WFSAsync* accepted the request (before the completion), so it can be cancelled.</summary>
    public Action<uint>? RequestIdAssigned { get; set; }

    /// <summary>Sync calls: called with the native id of the thread that blocks in the call, so it can be cancelled with WFSCancelBlockingCall.</summary>
    public Action<uint>? BlockingThreadStarted { get; set; }
}

