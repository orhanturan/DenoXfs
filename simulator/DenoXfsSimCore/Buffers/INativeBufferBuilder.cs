using DenoXfs.Sim.Core.Protocol;

namespace DenoXfs.Sim.Core.Buffers;

/// <summary>
/// Builds a packed (pack 1, little endian) native block with pointers expressed as fixups.
/// Offset 0 is the root of the block.
/// </summary>
public interface INativeBufferBuilder
{
    /// <summary>Reserves zeroed bytes and returns their offset.</summary>
    int Reserve(int size);

    /// <summary>Appends raw bytes and returns their offset.</summary>
    int AddBytes(ReadOnlySpan<byte> bytes);

    /// <summary>Appends a null terminated ANSI string and returns its offset.</summary>
    int AddAnsiString(string text);

    void WriteByte(int offset, byte value);

    void WriteUInt16(int offset, ushort value);

    void WriteUInt32(int offset, uint value);

    /// <summary>Marks the pointer field at <paramref name="fieldOffset"/> to point at <paramref name="targetOffset"/>.</summary>
    void WritePointer(int fieldOffset, int targetOffset);

    ResultBuffer Build();
}

public interface INativeBufferBuilderFactory
{
    INativeBufferBuilder Create();
}
