using System;

namespace DenoXfs.Core.Models.Responses;

public class GetInfoResponse
{
    public int Result { get; set; }
    public uint RequestId { get; set; }
    public IntPtr ResultPtr { get; set; }
}






