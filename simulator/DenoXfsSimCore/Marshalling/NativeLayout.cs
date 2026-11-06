using DenoXfs.Sim.Core.Buffers;

namespace DenoXfs.Sim.Core.Marshalling;

/// <summary>Native type of a struct field or of a result root (what <c>lpBuffer</c> points at).</summary>
public enum NativeKind
{
    Byte,
    Char,
    Word,
    DWord,
    Bool,

    /// <summary>LPSTR, value: string.</summary>
    AnsiString,

    /// <summary>LPSTR holding "a\0b\0\0", value: array of strings.</summary>
    MultiString,

    /// <summary>LPBYTE, value: hex string or <c>{ "text": "..." }</c>; the length goes to <see cref="NativeType.LengthField"/>.</summary>
    Bytes,

    /// <summary>Pointer to one struct, value: object.</summary>
    StructPointer,

    /// <summary>NULL terminated array of struct pointers, value: array of objects.</summary>
    PointerArray,

    /// <summary>Inline DWORD[<see cref="NativeType.Count"/>], value: array (missing elements stay 0).</summary>
    DWordArray,

    /// <summary>LPWORD list, value: array; zero terminated unless <see cref="NativeType.Terminated"/> is false.</summary>
    WordList,

    /// <summary>LPDWORD list, value: array; zero terminated unless <see cref="NativeType.Terminated"/> is false.</summary>
    DWordList
}

/// <summary>A native type with what the writer needs to know about it.</summary>
public sealed record NativeType(NativeKind Kind)
{
    /// <summary>Names accepted for numbers (flags: an array of names is OR'ed).</summary>
    public ConstantSet? Constants { get; init; }

    public StructLayout? Struct { get; init; }

    /// <summary>Bytes, WordList, DWordList: field of the same struct that holds the element count (filled when the value lacks it).</summary>
    public string? LengthField { get; init; }

    /// <summary>Bytes: one null byte after the data.</summary>
    public bool NullTerminated { get; init; }

    /// <summary>WordList, DWordList: a zero value ends the list.</summary>
    public bool Terminated { get; init; } = true;

    /// <summary>DWordArray: number of elements.</summary>
    public int Count { get; init; }

    /// <summary>PointerArray: element field that identifies an element when a scenario patches the array.</summary>
    public string? KeyField { get; init; }

    public bool IsPointer => Kind is NativeKind.AnsiString or NativeKind.MultiString or NativeKind.Bytes or NativeKind.StructPointer
        or NativeKind.PointerArray or NativeKind.WordList or NativeKind.DWordList;

    public int Size(int ptrSize) => Kind switch
    {
        NativeKind.Byte or NativeKind.Char => 1,
        NativeKind.Word => 2,
        NativeKind.DWord or NativeKind.Bool => 4,
        NativeKind.DWordArray => 4 * Count,
        _ => ptrSize
    };
}

/// <summary>A field of a packed struct; the offset is set by <see cref="StructLayout"/>.</summary>
public sealed record NativeField(string Name, NativeType Type)
{
    public int Offset { get; init; }

    public static NativeField Byte(string name) => new(name, new NativeType(NativeKind.Byte));

    public static NativeField Char(string name) => new(name, new NativeType(NativeKind.Char));

    public static NativeField Word(string name, ConstantSet? constants = null) => new(name, new NativeType(NativeKind.Word) { Constants = constants });

    public static NativeField DWord(string name, ConstantSet? constants = null) => new(name, new NativeType(NativeKind.DWord) { Constants = constants });

    public static NativeField Bool(string name) => new(name, new NativeType(NativeKind.Bool));

    public static NativeField AnsiString(string name) => new(name, new NativeType(NativeKind.AnsiString));

    public static NativeField MultiString(string name) => new(name, new NativeType(NativeKind.MultiString));

    public static NativeField Bytes(string name, string lengthField, bool nullTerminated = false) =>
        new(name, new NativeType(NativeKind.Bytes) { LengthField = lengthField, NullTerminated = nullTerminated });

    public static NativeField StructPointer(string name, StructLayout layout) => new(name, new NativeType(NativeKind.StructPointer) { Struct = layout });

    public static NativeField PointerArray(string name, StructLayout layout, string? keyField = null) =>
        new(name, new NativeType(NativeKind.PointerArray) { Struct = layout, KeyField = keyField });

    public static NativeField DWordArray(string name, int count, ConstantSet? constants = null) =>
        new(name, new NativeType(NativeKind.DWordArray) { Count = count, Constants = constants });

    public static NativeField WordList(string name, ConstantSet? constants = null, string? countField = null) =>
        new(name, new NativeType(NativeKind.WordList) { Constants = constants, LengthField = countField, Terminated = countField is null });

    public static NativeField DWordList(string name, ConstantSet? constants = null, string? countField = null) =>
        new(name, new NativeType(NativeKind.DWordList) { Constants = constants, LengthField = countField, Terminated = countField is null });
}

/// <summary>A packed (pack 1) struct; field offsets follow from the field order and sizes (32 bit pointers).</summary>
public sealed class StructLayout
{
    private readonly Dictionary<string, NativeField> _byName;

    public StructLayout(string name, params NativeField[] fields)
    {
        Name = name;
        var offset = 0;
        var placed = new List<NativeField>(fields.Length);
        foreach (var field in fields)
        {
            placed.Add(field with { Offset = offset });
            offset += field.Type.Size(NativeBufferBuilderFactory.DefaultPtrSize);
        }
        Fields = placed;
        Size = offset;
        _byName = placed.ToDictionary(f => f.Name, StringComparer.Ordinal);
    }

    public string Name { get; }

    public int Size { get; }

    public IReadOnlyList<NativeField> Fields { get; }

    public NativeField? Find(string name) => _byName.GetValueOrDefault(name);

    public NativeField this[string name] => _byName[name];

    public override string ToString() => Name;
}

/// <summary>What <c>lpBuffer</c> of a result or event points at.</summary>
public sealed record NativeLayout(string Name, NativeType Type)
{
    /// <summary>Pointer to one struct (most results).</summary>
    public static NativeLayout Of(StructLayout layout) => new(layout.Name, new NativeType(NativeKind.StructPointer) { Struct = layout });

    /// <summary>NULL terminated array of struct pointers.</summary>
    public static NativeLayout ArrayOf(StructLayout layout, string? keyField = null) =>
        new($"{layout.Name}[]", new NativeType(NativeKind.PointerArray) { Struct = layout, KeyField = keyField });

    /// <summary>Pointer to a single WORD.</summary>
    public static NativeLayout Word(string name, ConstantSet? constants = null) => new(name, new NativeType(NativeKind.Word) { Constants = constants });

    /// <summary>LPSTR "a\0b\0\0" list.</summary>
    public static NativeLayout MultiString(string name) => new(name, new NativeType(NativeKind.MultiString));

    /// <summary>Raw characters/bytes, written as given (the value holds its own terminators).</summary>
    public static NativeLayout Bytes(string name) => new(name, new NativeType(NativeKind.Bytes));
}
