using System;

namespace DenoXfs.Core.Models.Requests;

public class GetInfoRequest : XfsRequest
{
    public ushort HService { get; set; }
    public uint Category { get; set; }
    public IntPtr QueryDetails { get; set; }
}

