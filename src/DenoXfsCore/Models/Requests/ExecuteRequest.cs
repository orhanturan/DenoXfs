using System;

namespace DenoXfs.Core.Models.Requests;

public class ExecuteRequest : XfsRequest
{
    public ushort HService { get; set; }
    public uint Command { get; set; }
    public IntPtr CmdData { get; set; }
}

