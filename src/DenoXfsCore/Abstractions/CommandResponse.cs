namespace DenoXfs.Core.Abstractions;

/// <summary>
/// Response returned by command execution containing result code and request ID
/// </summary>
public class CommandResponse
{
    public int Result { get; }
    public uint RequestId { get; }

    public CommandResponse(int result, uint requestId)
    {
        Result = result;
        RequestId = requestId;
    }
}



