using System.Buffers.Binary;
using DenoXfs.Sim.Core.Protocol;

namespace DenoXfs.Sim.Core.Buffers;

public sealed class NativeBufferBuilder : INativeBufferBuilder
{
    private readonly int _ptrSize;
    private readonly List<byte> _data = new();
    private readonly List<int[]> _fixups = new();

    public NativeBufferBuilder(int ptrSize)
    {
        _ptrSize = ptrSize;
    }

    public int Reserve(int size)
    {
        var offset = _data.Count;
        _data.AddRange(new byte[size]);
        return offset;
    }

    public int AddBytes(ReadOnlySpan<byte> bytes)
    {
        var offset = _data.Count;
        _data.AddRange(bytes.ToArray());
        return offset;
    }

    public int AddAnsiString(string text)
    {
        var offset = _data.Count;
        _data.AddRange(System.Text.Encoding.Latin1.GetBytes(text));
        _data.Add(0);
        return offset;
    }

    public void WriteByte(int offset, byte value) => Write(offset, [value]);

    public void WriteUInt16(int offset, ushort value)
    {
        Span<byte> bytes = stackalloc byte[2];
        BinaryPrimitives.WriteUInt16LittleEndian(bytes, value);
        Write(offset, bytes);
    }

    public void WriteUInt32(int offset, uint value)
    {
        Span<byte> bytes = stackalloc byte[4];
        BinaryPrimitives.WriteUInt32LittleEndian(bytes, value);
        Write(offset, bytes);
    }

    public void WritePointer(int fieldOffset, int targetOffset)
    {
        if (fieldOffset < 0 || fieldOffset + _ptrSize > _data.Count)
            throw new ArgumentOutOfRangeException(nameof(fieldOffset));
        if (targetOffset < 0 || targetOffset > _data.Count)
            throw new ArgumentOutOfRangeException(nameof(targetOffset));
        _fixups.Add(new[] { fieldOffset, targetOffset });
    }

    public ResultBuffer Build() =>
        new(_ptrSize, Convert.ToBase64String(_data.ToArray()), _fixups.ToArray());

    private void Write(int offset, ReadOnlySpan<byte> bytes)
    {
        if (offset < 0 || offset + bytes.Length > _data.Count)
            throw new ArgumentOutOfRangeException(nameof(offset));
        for (var i = 0; i < bytes.Length; i++)
            _data[offset + i] = bytes[i];
    }
}

public sealed class NativeBufferBuilderFactory : INativeBufferBuilderFactory
{
    /// <summary>Pointer size of the target process (the XFS Manager here is 32 bit).</summary>
    public const int DefaultPtrSize = 4;

    public INativeBufferBuilder Create() => new NativeBufferBuilder(DefaultPtrSize);
}
