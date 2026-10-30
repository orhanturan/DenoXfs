using DenoXfs.Core.Api;


namespace DenoXfs.Core.Models.Requests;

public class StartUpRequest
{
    public uint VersionsRequired { get; set; }
    public XfsVersion XfsVersion { get; set; }
}






