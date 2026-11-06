using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Protocol;

namespace DenoXfs.Sim.Core.Marshalling;

/// <summary>Turns a <see cref="NativeValue"/> into the flat result block the DLL materializes.</summary>
public interface INativeLayoutWriter
{
    /// <summary>Null when there is no value (NULL <c>lpBuffer</c>). Throws <see cref="NativeValueException"/> for a value that does not fit.</summary>
    ResultBuffer? Write(NativeValue? value);
}

/// <summary>
/// Layout driven writer. Pointer targets are appended depth first in field order, so the block looks like one built by
/// hand. Fields missing in the value stay zero / NULL; length and count fields are filled from the data unless given.
/// </summary>
public sealed class NativeLayoutWriter : INativeLayoutWriter
{
    private readonly INativeBufferBuilderFactory _builders;

    public NativeLayoutWriter(INativeBufferBuilderFactory builders)
    {
        _builders = builders;
    }

    public ResultBuffer? Write(NativeValue? value)
    {
        if (value?.Value is null)
            return null;

        var builder = _builders.Create();
        var type = value.Layout.Type;
        if (type.IsPointer)
        {
            AppendTarget(builder, type, value.Value, value.Layout.Name);
        }
        else
        {
            var offset = builder.Reserve(type.Size(NativeBufferBuilderFactory.DefaultPtrSize));
            WriteInline(builder, offset, type, value.Value, value.Layout.Name);
        }
        return builder.Build();
    }

    /// <summary>Appends what a pointer of <paramref name="type"/> points at; returns its offset and the element count.</summary>
    private static (int Offset, int Count) AppendTarget(INativeBufferBuilder builder, NativeType type, JsonNode value, string path)
    {
        var ptrSize = NativeBufferBuilderFactory.DefaultPtrSize;
        switch (type.Kind)
        {
            case NativeKind.StructPointer:
            {
                var layout = type.Struct!;
                var offset = builder.Reserve(layout.Size);
                WriteStruct(builder, offset, layout, AsObject(value, path), path);
                return (offset, 1);
            }
            case NativeKind.PointerArray:
            {
                var layout = type.Struct!;
                var items = AsArray(value, path);
                var array = builder.Reserve((items.Count + 1) * ptrSize);
                for (var i = 0; i < items.Count; i++)
                {
                    var itemPath = $"{path}[{i}]";
                    var item = builder.Reserve(layout.Size);
                    builder.WritePointer(array + i * ptrSize, item);
                    WriteStruct(builder, item, layout, AsObject(items[i] ?? throw new NativeValueException(itemPath, "null element"), itemPath), itemPath);
                }
                return (array, items.Count);
            }
            case NativeKind.AnsiString:
            {
                var text = AsString(value, path);
                return (builder.AddAnsiString(text), text.Length);
            }
            case NativeKind.MultiString:
            {
                var items = AsArray(value, path).Select((item, i) => AsString(item!, $"{path}[{i}]")).ToList();
                return (builder.AddBytes(Encoding.Latin1.GetBytes(string.Concat(items.Select(s => s + "\0")) + "\0")), items.Count);
            }
            case NativeKind.Bytes:
            {
                var data = ParseBytes(value, path);
                var offset = builder.AddBytes(data);
                if (type.NullTerminated)
                    builder.AddBytes([0]);
                return (offset, data.Length);
            }
            case NativeKind.WordList:
            case NativeKind.DWordList:
            {
                var items = AsArray(value, path);
                var size = type.Kind == NativeKind.WordList ? 2 : 4;
                var offset = builder.Reserve((items.Count + (type.Terminated ? 1 : 0)) * size);
                var element = new NativeType(type.Kind == NativeKind.WordList ? NativeKind.Word : NativeKind.DWord) { Constants = type.Constants };
                for (var i = 0; i < items.Count; i++)
                    WriteInline(builder, offset + i * size, element, items[i] ?? 0, $"{path}[{i}]");
                return (offset, items.Count);
            }
            default:
                throw new NativeValueException(path, $"{type.Kind} is not a pointer type");
        }
    }

    private static void WriteStruct(INativeBufferBuilder builder, int offset, StructLayout layout, JsonObject value, string path)
    {
        foreach (var (name, _) in value)
        {
            if (layout.Find(name) is null)
                throw new NativeValueException($"{path}.{name}", $"{layout.Name} has no field '{name}'");
        }

        foreach (var field in layout.Fields)
        {
            var fieldPath = $"{path}.{field.Name}";
            if (!value.TryGetPropertyValue(field.Name, out var fieldValue) || fieldValue is null)
                continue;

            if (!field.Type.IsPointer)
            {
                WriteInline(builder, offset + field.Offset, field.Type, fieldValue, fieldPath);
                continue;
            }

            var (target, count) = AppendTarget(builder, field.Type, fieldValue, fieldPath);
            builder.WritePointer(offset + field.Offset, target);

            if (field.Type.LengthField is { } lengthName && !value.ContainsKey(lengthName))
            {
                var lengthField = layout.Find(lengthName) ?? throw new NativeValueException(fieldPath, $"{layout.Name} has no length field '{lengthName}'");
                WriteInline(builder, offset + lengthField.Offset, lengthField.Type, count, $"{path}.{lengthName}");
            }
        }
    }

    private static void WriteInline(INativeBufferBuilder builder, int offset, NativeType type, JsonNode value, string path)
    {
        switch (type.Kind)
        {
            case NativeKind.Byte:
            case NativeKind.Char:
                builder.WriteByte(offset, (byte)Number(value, type, path, byte.MaxValue));
                break;
            case NativeKind.Word:
                builder.WriteUInt16(offset, (ushort)Number(value, type, path, ushort.MaxValue));
                break;
            case NativeKind.DWord:
            case NativeKind.Bool:
                builder.WriteUInt32(offset, Number(value, type, path, uint.MaxValue));
                break;
            case NativeKind.DWordArray:
            {
                var items = AsArray(value, path);
                if (items.Count > type.Count)
                    throw new NativeValueException(path, $"at most {type.Count} elements");
                var element = new NativeType(NativeKind.DWord) { Constants = type.Constants };
                for (var i = 0; i < items.Count; i++)
                    WriteInline(builder, offset + i * 4, element, items[i] ?? 0, $"{path}[{i}]");
                break;
            }
            default:
                throw new NativeValueException(path, $"{type.Kind} is not an inline type");
        }
    }

    /// <summary>A number, true/false, a constant name, "0x.." hex, a one character string (CHAR), or an array of names (flags, OR'ed).</summary>
    private static uint Number(JsonNode value, NativeType type, string path, uint max)
    {
        ulong result;
        switch (value)
        {
            case JsonArray flags:
                result = 0;
                for (var i = 0; i < flags.Count; i++)
                    result |= Number(flags[i] ?? throw new NativeValueException($"{path}[{i}]", "null flag"), type, $"{path}[{i}]", max);
                break;
            case JsonValue scalar when scalar.GetValueKind() is JsonValueKind.True or JsonValueKind.False:
                result = scalar.GetValue<bool>() ? 1u : 0u;
                break;
            case JsonValue scalar when scalar.GetValueKind() == JsonValueKind.Number:
                if (!scalar.TryGetValue<long>(out var number) && !long.TryParse(scalar.ToJsonString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
                    throw new NativeValueException(path, $"'{scalar.ToJsonString()}' is not an integer");
                if (number < 0)
                    throw new NativeValueException(path, $"{number} is negative");
                result = (ulong)number;
                break;
            case JsonValue scalar when scalar.GetValueKind() == JsonValueKind.String:
                result = Name(scalar.GetValue<string>(), type, path);
                break;
            default:
                throw new NativeValueException(path, $"'{value.ToJsonString()}' is not a number");
        }

        if (result > max)
            throw new NativeValueException(path, $"{result} does not fit {type.Kind}");
        return (uint)result;
    }

    private static uint Name(string text, NativeType type, string path)
    {
        if (type.Constants is { } constants && constants.TryResolve(text, out var named))
            return named;
        if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
            && uint.TryParse(text.AsSpan(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hex))
            return hex;
        if (uint.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var decimalValue))
            return decimalValue;
        if (type.Kind == NativeKind.Char && text.Length == 1 && text[0] <= 0xFF)
            return text[0];

        var known = type.Constants is null ? string.Empty : $" (known: {string.Join(", ", type.Constants.Values.Select(v => v.Name))})";
        throw new NativeValueException(path, $"unknown name '{text}'{known}");
    }

    /// <summary>LPBYTE data: hex string (spaces allowed) or <c>{ "text": "..." }</c> (Latin-1, \u00NN for binary bytes).</summary>
    private static byte[] ParseBytes(JsonNode value, string path)
    {
        if (value is JsonObject obj)
        {
            if (obj.Count != 1 || !obj.TryGetPropertyValue("text", out var text) || text is null)
                throw new NativeValueException(path, "bytes object must be { \"text\": \"...\" }");
            var chars = AsString(text, path);
            if (chars.Any(c => c > 0xFF))
                throw new NativeValueException(path, "text holds characters above \\u00FF");
            return Encoding.Latin1.GetBytes(chars);
        }

        var hex = AsString(value, path).Replace(" ", string.Empty);
        if (hex.Length % 2 != 0 || !hex.All(Uri.IsHexDigit))
            throw new NativeValueException(path, "not valid hex");
        return Convert.FromHexString(hex);
    }

    private static JsonObject AsObject(JsonNode value, string path) =>
        value as JsonObject ?? throw new NativeValueException(path, "expected an object");

    private static JsonArray AsArray(JsonNode value, string path) =>
        value as JsonArray ?? throw new NativeValueException(path, "expected an array");

    private static string AsString(JsonNode value, string path) =>
        value is JsonValue scalar && scalar.GetValueKind() == JsonValueKind.String
            ? scalar.GetValue<string>()
            : throw new NativeValueException(path, "expected a string");
}
