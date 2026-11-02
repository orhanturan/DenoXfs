namespace DenoXfs.Sim.Core.Protocol;

/// <summary>HRESULT values from XFSAPI.H used by the simulator core.</summary>
public static class XfsResultCodes
{
    public const int Success = 0;
    public const int Canceled = -4;
    public const int DeviceNotReady = -13;
    public const int InternalError = -15;
    public const int SpiVersionTooHigh = -44;
    public const int SpiVersionTooLow = -45;
    public const int ServiceVersionTooHigh = -46;
    public const int ServiceVersionTooLow = -47;
    public const int Timeout = -48;
    public const int UnsupportedCategory = -49;
    public const int UnsupportedCommand = -50;
    public const int InvalidData = -52;
    public const int UnsupportedData = -56;
}
