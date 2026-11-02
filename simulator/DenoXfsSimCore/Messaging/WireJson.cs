using System.Text.Json;

namespace DenoXfs.Sim.Core.Messaging;

/// <summary>JSON settings of the DLL protocol: camelCase, no indentation.</summary>
public static class WireJson
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static uint GetUInt(JsonElement element, string name) =>
        element.TryGetProperty(name, out var value) && value.TryGetUInt32(out var number) ? number : 0;

    public static string GetString(JsonElement element, string name) =>
        element.TryGetProperty(name, out var value) ? value.GetString() ?? string.Empty : string.Empty;
}
