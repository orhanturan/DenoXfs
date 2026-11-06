using System.Text.Json.Nodes;

namespace DenoXfs.Sim.Core.Marshalling;

/// <summary>
/// A result or event payload before marshalling: its layout plus the value as JSON with the spec field names
/// (<c>{ "fwDevice": 0, "lpszExtra": null, ... }</c>). Commands produce it; a scenario may patch it; the writer turns it into
/// the native block. A null <see cref="Value"/> means a NULL <c>lpBuffer</c>.
/// </summary>
public sealed record NativeValue(NativeLayout Layout, JsonNode? Value);

/// <summary>Helpers for building <see cref="NativeValue"/> JSON.</summary>
public static class NativeJson
{
    /// <summary>LPBYTE value: bytes as a hex string.</summary>
    public static string Hex(ReadOnlySpan<byte> bytes) => Convert.ToHexString(bytes);

    public static JsonArray Array<T>(IEnumerable<T> items, Func<T, JsonNode?> select) => new(items.Select(select).ToArray());

    public static JsonArray Strings(IEnumerable<string> items) => new(items.Select(s => (JsonNode?)JsonValue.Create(s)).ToArray());
}

/// <summary>A value that does not fit its layout (unknown field, name not in the constant set, bad hex, out of range).</summary>
public sealed class NativeValueException : FormatException
{
    public NativeValueException(string path, string message) : base($"{path}: {message}")
    {
        Path = path;
    }

    public string Path { get; }
}
