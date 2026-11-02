using System.Collections;
using System.Reflection;
using System.Text;

namespace DenoXfs.TestTool.Operations;

/// <summary>
/// Renders a decoded result as indented text. It reads every value immediately, which matters because the generated
/// structs read pointer fields lazily and the native buffer is released right after decoding.
/// </summary>
public static class ObjectDumper
{
    private const int MaxDepth = 4;

    public static string Dump(object? value)
    {
        var text = new StringBuilder();
        Write(text, value, 0);
        return text.ToString().TrimEnd();
    }

    private static void Write(StringBuilder text, object? value, int depth)
    {
        var indent = new string(' ', depth * 2);
        switch (value)
        {
            case null:
                text.AppendLine($"{indent}null");
                return;
            case string or bool or char or Enum or IntPtr:
            case var _ when value.GetType().IsPrimitive:
                text.AppendLine($"{indent}{value}");
                return;
            case byte[] bytes:
                text.AppendLine($"{indent}[{bytes.Length} bytes] {Hex(bytes)}");
                return;
        }

        if (depth >= MaxDepth)
        {
            text.AppendLine($"{indent}...");
            return;
        }

        if (value is IEnumerable items)
        {
            var index = 0;
            foreach (var item in items)
            {
                text.AppendLine($"{indent}[{index++}]");
                Write(text, item, depth + 1);
            }
            if (index == 0)
                text.AppendLine($"{indent}(empty)");
            return;
        }

        foreach (var property in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.GetIndexParameters().Length > 0 || !property.CanRead)
                continue;

            object? propertyValue;
            try
            {
                propertyValue = property.GetValue(value);
            }
            catch (Exception ex)
            {
                text.AppendLine($"{indent}{property.Name} = <{ex.GetBaseException().Message}>");
                continue;
            }

            if (IsSimple(propertyValue))
            {
                text.AppendLine($"{indent}{property.Name} = {Format(propertyValue)}");
            }
            else if (SimpleItems(propertyValue) is { } simpleItems)
            {
                text.AppendLine($"{indent}{property.Name} = [{simpleItems.Count}] {string.Join("; ", simpleItems.Select(Format))}");
            }
            else
            {
                text.AppendLine($"{indent}{property.Name}:");
                Write(text, propertyValue, depth + 1);
            }
        }
    }

    private static bool IsSimple(object? value) =>
        value is null or string or bool or char or Enum or IntPtr or byte[] || value.GetType().IsPrimitive;

    /// <summary>The items of a collection of simple values (e.g. the 32 guidance lights), written on one line; otherwise null.</summary>
    private static List<object?>? SimpleItems(object? value)
    {
        if (value is not IEnumerable enumerable)
            return null;
        var items = enumerable.Cast<object?>().ToList();
        return items.Count > 0 && items.All(IsSimple) ? items : null;
    }

    private static string Format(object? value) => value switch
    {
        null => "null",
        byte[] bytes => $"[{bytes.Length} bytes] {Hex(bytes)}",
        _ => value.ToString() ?? string.Empty
    };

    /// <summary>Bytes as space separated hex, e.g. "34 31 31".</summary>
    public static string Hex(byte[] bytes) => string.Join(' ', bytes.Select(b => b.ToString("X2")));

    /// <summary>Bytes as ASCII exactly as they are; non printable bytes as \xNN.</summary>
    public static string Ascii(byte[] bytes)
    {
        var text = new StringBuilder(bytes.Length);
        foreach (var b in bytes)
        {
            if (b == (byte)'\\')
                text.Append(@"\\");
            else if (b is >= 0x20 and < 0x7F)
                text.Append((char)b);
            else
                text.Append($"\\x{b:X2}");
        }
        return text.ToString();
    }
}
