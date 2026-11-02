namespace DenoXfs.Sim.Core.Protocol;

/// <summary>Message type names shared by the Service Provider DLL and the host.</summary>
public static class WireTypes
{
    public const string Hello = "hello";
    public const string HelloAck = "helloAck";
    public const string Execute = "execute";
    public const string GetInfo = "getinfo";
    public const string Close = "close";
    public const string Completion = "completion";
    public const string Event = "event";
    public const string Cancel = "cancel";
}

/// <summary>Event classes; the wire names are the lower case names ("execute", "service", "user", "system").</summary>
public enum XfsEventKind
{
    Execute,
    Service,
    User,
    System
}

public sealed record HelloAckMessage(
    string Type, int HResult,
    ushort SpiVersion, ushort SpiLow, ushort SpiHigh,
    ushort SrvcVersion, ushort SrvcLow, ushort SrvcHigh);

public sealed record CompletionMessage(string Type, uint ReqId, string Kind, int HResult, uint Command, ResultBuffer? Result);

/// <summary>An event for the DLL. ReqId is the request of an execute event (0 for the other kinds).</summary>
public sealed record EventMessage(string Type, string Kind, uint ReqId, uint EventId, int HResult, ResultBuffer? Result);

public static class WireMessageFactory
{
    public static HelloAckMessage HelloAck(int hResult, ushort spiVersion, ushort srvcVersion, Versions.SupportedVersions supported) =>
        new(WireTypes.HelloAck, hResult,
            spiVersion, supported.Spi.Low, supported.Spi.High,
            srvcVersion, supported.Service.Low, supported.Service.High);

    public static CompletionMessage Completion(uint reqId, string kind, int hResult, uint command, ResultBuffer? result) =>
        new(WireTypes.Completion, reqId, kind, hResult, command, result);

    public static EventMessage Event(XfsEventKind kind, uint reqId, uint eventId, ResultBuffer? result) =>
        new(WireTypes.Event, kind.ToString().ToLowerInvariant(), reqId, eventId, XfsResultCodes.Success, result);
}
