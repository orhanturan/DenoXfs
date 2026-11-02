using System.Text.Json;

namespace DenoXfs.Sim.Idc.Commands;

/// <summary>Reads the command input decoded by the DLL (a JSON object; binary fields are base64).</summary>
internal static class IdcInput
{
    public static ushort? GetUShort(JsonElement input, string name) =>
        TryGet(input, name, out var value) && value.TryGetUInt16(out var number) ? number : null;

    public static uint? GetUInt(JsonElement input, string name) =>
        TryGet(input, name, out var value) && value.TryGetUInt32(out var number) ? number : null;

    public static string? GetString(JsonElement input, string name) =>
        TryGet(input, name, out var value) && value.ValueKind == JsonValueKind.String ? value.GetString() : null;

    public static byte[]? GetBytes(JsonElement input, string name) =>
        TryGet(input, name, out var value) && value.ValueKind == JsonValueKind.String ? value.GetBytesFromBase64() : null;

    /// <summary>A NULL terminated LPWFSIDCCARDDATA array as decoded by the DLL: [{dataSource, status, data, writeMethod}].</summary>
    public static IReadOnlyList<(ushort Source, byte[] Data, ushort WriteMethod)>? GetCardData(JsonElement input, string name)
    {
        if (!TryGet(input, name, out var array) || array.ValueKind != JsonValueKind.Array)
            return null;
        return array.EnumerateArray()
            .Select(item => (GetUShort(item, "dataSource") ?? 0, GetBytes(item, "data") ?? [], GetUShort(item, "writeMethod") ?? 0))
            .ToList();
    }

    private static bool TryGet(JsonElement input, string name, out JsonElement value)
    {
        value = default;
        return input.ValueKind == JsonValueKind.Object && input.TryGetProperty(name, out value);
    }
}
