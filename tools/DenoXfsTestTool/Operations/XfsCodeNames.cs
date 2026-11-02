namespace DenoXfs.TestTool.Operations;

/// <summary>Merges every registered <see cref="IXfsCodeNameSource"/>; unknown codes are shown as the number only.</summary>
public class XfsCodeNames : IXfsCodeNames
{
    private readonly Dictionary<(XfsCodeKind, uint), string> _names = new();

    public XfsCodeNames(IEnumerable<IXfsCodeNameSource> sources)
    {
        foreach (var (kind, code, name) in sources.SelectMany(source => source.Names))
            _names[(kind, code)] = name;
    }

    public string Describe(XfsCodeKind kind, uint code) =>
        _names.TryGetValue((kind, code), out var name) ? $"{name} ({code})" : code.ToString();
}
