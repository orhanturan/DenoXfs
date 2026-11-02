using System.Runtime.InteropServices;

namespace DenoXfs.TestTool.Operations;

/// <summary>
/// Decodes the payload (WFSRESULT.lpBuffer) of one event. Runs on the message thread before the result is freed, so it
/// must copy everything out (the dump reads every value at once). Registered in DI like the operations.
/// </summary>
public interface IEventPayloadDecoder
{
    XfsCodeKind Kind { get; }

    uint EventId { get; }

    object? Decode(IntPtr buffer);
}

/// <summary>Payload that is one structure.</summary>
public sealed class StructPayloadDecoder<T> : IEventPayloadDecoder where T : struct
{
    public StructPayloadDecoder(XfsCodeKind kind, uint eventId)
    {
        Kind = kind;
        EventId = eventId;
    }

    public XfsCodeKind Kind { get; }

    public uint EventId { get; }

    public object? Decode(IntPtr buffer) => Marshal.PtrToStructure<T>(buffer);
}

/// <summary>Payload that is a pointer to one WORD with values of <typeparamref name="TEnum"/>.</summary>
public sealed class WordPayloadDecoder<TEnum> : IEventPayloadDecoder where TEnum : struct, Enum
{
    public WordPayloadDecoder(XfsCodeKind kind, uint eventId)
    {
        Kind = kind;
        EventId = eventId;
    }

    public XfsCodeKind Kind { get; }

    public uint EventId { get; }

    public object? Decode(IntPtr buffer) => (TEnum)Enum.ToObject(typeof(TEnum), (ushort)Marshal.ReadInt16(buffer));
}

/// <summary>Finds the decoder of an event; events without one are shown with their id only.</summary>
public interface IEventPayloadDecoders
{
    object? Decode(XfsCodeKind kind, uint eventId, IntPtr buffer);
}

public sealed class EventPayloadDecoders : IEventPayloadDecoders
{
    private readonly Dictionary<(XfsCodeKind, uint), IEventPayloadDecoder> _decoders;

    public EventPayloadDecoders(IEnumerable<IEventPayloadDecoder> decoders)
    {
        _decoders = decoders.ToDictionary(d => (d.Kind, d.EventId));
    }

    public object? Decode(XfsCodeKind kind, uint eventId, IntPtr buffer)
    {
        if (buffer == IntPtr.Zero || !_decoders.TryGetValue((kind, eventId), out var decoder))
            return null;
        return decoder.Decode(buffer);
    }
}
