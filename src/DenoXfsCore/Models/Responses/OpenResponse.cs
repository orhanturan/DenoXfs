using DenoXfs.Api.Common;

namespace DenoXfs.Core.Models.Responses;

public class OpenResponse
{
    public int Result { get; set; }
    public ushort HService { get; set; }
    public uint RequestId { get; set; }
    public XfsVersion SrvcVersion { get; set; }
    public XfsVersion SPIVersion { get; set; }
}











