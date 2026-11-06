using System.Text.Json;
using System.Text.Json.Nodes;

namespace DenoXfs.Sim.Core.Marshalling;

/// <summary>
/// Named values of one field (e.g. fwDevice). A name may be written short (<c>HWERROR</c>) or with the header prefix
/// (<c>WFS_IDC_DEVHWERROR</c>), case insensitive.
/// </summary>
public sealed class ConstantSet
{
    private readonly Dictionary<string, uint> _values = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<(string Name, uint Value)> _names = new();

    public ConstantSet(string prefix, params (string Name, uint Value)[] values)
    {
        Prefix = prefix;
        foreach (var (name, value) in values)
        {
            _names.Add((name, value));
            _values[name] = value;
            _values[prefix + name] = value;
        }
    }

    public string Prefix { get; }

    public IReadOnlyList<(string Name, uint Value)> Values => _names;

    public bool TryResolve(string name, out uint value) => _values.TryGetValue(name.Trim(), out value);

    /// <summary>A JSON value as written in a profile: a number or a name of this set; null when it is neither.</summary>
    public uint? Read(JsonNode? node) => node switch
    {
        JsonValue value when value.GetValueKind() == JsonValueKind.Number && uint.TryParse(value.ToJsonString(), out var number) => number,
        JsonValue value when value.GetValueKind() == JsonValueKind.String && TryResolve(value.GetValue<string>(), out var named) => named,
        _ => null
    };

    /// <summary>The short name of a value, or the number as text.</summary>
    public string NameOf(uint value) => _names.FirstOrDefault(n => n.Value == value).Name ?? value.ToString();

    public override string ToString() => $"{Prefix}*";
}
