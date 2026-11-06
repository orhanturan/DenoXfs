using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Idc.Scenarios;

namespace DenoXfs.Sim.Idc.Device;

/// <summary>A card reader of a terminal: a device of the terminal's model with one or more IDC services (they share its state).</summary>
public sealed record IdcReader(DeviceRef Ref, string Name, IReadOnlyList<string> ServiceNames)
{
    public string Services => string.Join(", ", ServiceNames);
}

/// <summary>The card readers of the terminal of an IP (none for an IP without a terminal), in the catalog's order.</summary>
public interface IIdcReaders
{
    Task<IReadOnlyList<IdcReader>> OfTerminalAsync(string address, CancellationToken cancellationToken);
}

public sealed class IdcReaders : IIdcReaders
{
    private readonly IServiceResolver _resolver;

    public IdcReaders(IServiceResolver resolver)
    {
        _resolver = resolver;
    }

    public async Task<IReadOnlyList<IdcReader>> OfTerminalAsync(string address, CancellationToken cancellationToken)
    {
        if (await _resolver.TerminalAsync(address, cancellationToken) is not { } view)
            return [];
        return view.DevicesOf(IdcProfiles.DeviceClass)
            .Select(d => new IdcReader(new DeviceRef(view.Terminal.TerminalId, d.Id), d.Name,
                d.Services.Where(s => string.Equals(s.XfsClass, IdcProfiles.DeviceClass, StringComparison.OrdinalIgnoreCase)).Select(s => s.Name).ToList()))
            .ToList();
    }
}
