namespace DenoXfs.Sim.Core.Messaging;

/// <summary>A command or info handler of one device class with its code.</summary>
public sealed record ClassHandler<THandler>(string XfsClass, uint Code, THandler Handler);

/// <summary>
/// Command or info handlers by device class and code: IDC 207 is READ_RAW_DATA only for a session of an IDC service.
/// Built once in the composition root from the handlers each module registers keyed by its class.
/// </summary>
public interface IClassHandlers<THandler>
{
    bool TryGet(string xfsClass, uint code, out THandler handler);
}

public sealed class ClassHandlers<THandler> : IClassHandlers<THandler>
{
    private readonly Dictionary<(string Class, uint Code), THandler> _handlers;

    public ClassHandlers(IEnumerable<ClassHandler<THandler>> handlers)
    {
        _handlers = handlers.ToDictionary(h => (h.XfsClass.ToUpperInvariant(), h.Code), h => h.Handler);
    }

    public bool TryGet(string xfsClass, uint code, out THandler handler) =>
        _handlers.TryGetValue((xfsClass.ToUpperInvariant(), code), out handler!);
}
