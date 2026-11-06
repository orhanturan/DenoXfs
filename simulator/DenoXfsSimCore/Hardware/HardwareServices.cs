using System.Net;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Core.Ui;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Hardware;

public sealed class HardwareSnapshots : IHardwareSnapshots
{
    private readonly IHardwareStore _store;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private HardwareSnapshot? _current;
    private int _version;

    public HardwareSnapshots(IHardwareStore store)
    {
        _store = store;
    }

    public async Task<HardwareSnapshot> GetAsync(CancellationToken cancellationToken)
    {
        if (Volatile.Read(ref _current) is { } current)
            return current;

        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (_current is { } loaded)
                return loaded;
            var version = Volatile.Read(ref _version);
            var snapshot = await _store.LoadAsync(cancellationToken);
            // An edit while loading makes this snapshot stale: return it, but load again next time.
            if (version == Volatile.Read(ref _version))
                Volatile.Write(ref _current, snapshot);
            return snapshot;
        }
        finally
        {
            _lock.Release();
        }
    }

    public void Invalidate()
    {
        Interlocked.Increment(ref _version);
        Volatile.Write(ref _current, null);
    }
}

/// <summary>Checks the catalog rules against the current snapshot, saves, reloads and redraws the terminal pages.</summary>
public sealed class HardwareCatalog : IHardwareCatalog
{
    private readonly IHardwareStore _store;
    private readonly IHardwareSnapshots _snapshots;
    private readonly IReadOnlySet<string> _simulatedClasses;
    private readonly ITerminalNotifier _notifier;
    private readonly ILogger<HardwareCatalog> _logger;

    public HardwareCatalog(IHardwareStore store, IHardwareSnapshots snapshots, IEnumerable<IDeviceClassModule> modules, ITerminalNotifier notifier,
        ILogger<HardwareCatalog> logger)
    {
        _store = store;
        _snapshots = snapshots;
        _simulatedClasses = modules.Select(m => m.DeviceClass).ToHashSet(StringComparer.OrdinalIgnoreCase);
        _notifier = notifier;
        _logger = logger;
    }

    public Task<HardwareSnapshot> SnapshotAsync(CancellationToken cancellationToken) => _snapshots.GetAsync(cancellationToken);

    public async Task<HardwareResult> SaveAsync(VendorEdit vendor, CancellationToken cancellationToken)
    {
        var snapshot = await _snapshots.GetAsync(cancellationToken);
        var name = vendor.Name.Trim();
        if (vendor.Id != 0 && snapshot.Vendors.All(v => v.Id != vendor.Id))
            return HardwareResult.Fail("The vendor does not exist.");
        if (name.Length == 0)
            return HardwareResult.Fail("The vendor name is required.");
        if (snapshot.Vendors.Any(v => v.Id != vendor.Id && SameName(v.Name, name)))
            return HardwareResult.Fail($"A vendor named '{name}' exists.");

        return await SaveAndReloadAsync("vendor", name, () => _store.SaveAsync(vendor with { Name = name }, cancellationToken), cancellationToken);
    }

    public async Task<HardwareResult> SaveAsync(ModelEdit model, CancellationToken cancellationToken)
    {
        var snapshot = await _snapshots.GetAsync(cancellationToken);
        var name = model.Name.Trim();
        var vendor = snapshot.Vendors.FirstOrDefault(v => v.Id == model.VendorId);
        if (vendor is null)
            return HardwareResult.Fail("The vendor does not exist.");
        if (model.Id != 0 && vendor.Models.All(m => m.Id != model.Id))
            return HardwareResult.Fail("The model does not exist at this vendor.");
        if (name.Length == 0)
            return HardwareResult.Fail("The model name is required.");
        if (vendor.Models.Any(m => m.Id != model.Id && SameName(m.Name, name)))
            return HardwareResult.Fail($"{vendor.Name} has a model named '{name}'.");

        return await SaveAndReloadAsync("model", name, () => _store.SaveAsync(model with { Name = name }, cancellationToken), cancellationToken);
    }

    public async Task<HardwareResult> SaveAsync(DeviceEdit device, CancellationToken cancellationToken)
    {
        var snapshot = await _snapshots.GetAsync(cancellationToken);
        var name = device.Name.Trim();
        var model = snapshot.Model(device.ModelId);
        if (model is null)
            return HardwareResult.Fail("The model does not exist.");
        var existing = model.Devices.FirstOrDefault(d => d.Id == device.Id);
        if (device.Id != 0 && existing is null)
            return HardwareResult.Fail("The device does not exist in this model.");
        var type = DeviceTypes.Find(device.DeviceType);
        if (type is null)
            return HardwareResult.Fail($"Unknown device type '{device.DeviceType}'.");
        if (name.Length == 0)
            return HardwareResult.Fail("The device name is required.");
        if (model.Devices.Any(d => d.Id != device.Id && SameName(d.Name, name)))
            return HardwareResult.Fail($"{model.Name} has a device named '{name}'.");
        if (existing?.Services.FirstOrDefault(s => !type.Allows(s.XfsClass)) is { } orphan)
            return HardwareResult.Fail($"A {type.Name} cannot open {orphan.XfsClass}; delete the service '{orphan.Name}' first.");

        return await SaveAndReloadAsync("device", name, () => _store.SaveAsync(device with { Name = name, DeviceType = type.Name }, cancellationToken),
            cancellationToken);
    }

    public async Task<HardwareResult> SaveAsync(ServiceEdit service, CancellationToken cancellationToken)
    {
        var snapshot = await _snapshots.GetAsync(cancellationToken);
        var name = service.Name.Trim();
        var xfsClass = service.XfsClass.Trim().ToUpperInvariant();
        var device = snapshot.Device(service.DeviceId);
        if (device is null)
            return HardwareResult.Fail("The device does not exist.");
        if (service.Id != 0 && device.Services.All(s => s.Id != service.Id))
            return HardwareResult.Fail("The service does not exist on this device.");
        if (name.Length == 0)
            return HardwareResult.Fail("The service name is required.");
        var type = DeviceTypes.Find(device.DeviceType);
        if (type is null || !type.Allows(xfsClass))
            return HardwareResult.Fail($"A {device.DeviceType} cannot open {xfsClass}.");
        if (!_simulatedClasses.Contains(xfsClass))
            return HardwareResult.Fail($"{xfsClass} is not simulated yet.");
        if (device.Services.FirstOrDefault(s => s.Id != service.Id && string.Equals(s.XfsClass, xfsClass, StringComparison.OrdinalIgnoreCase)) is { } sibling)
            return HardwareResult.Fail($"{device.Name} already opens a {xfsClass} service ('{sibling.Name}'); a device opens one service per class.");
        var model = snapshot.Model(device.ModelId)!;
        if (model.Services.Any(s => s.Id != service.Id && SameName(s.Name, name)))
            return HardwareResult.Fail($"{model.Name} has a service named '{name}'; WFSOpen finds a service by its name in the model.");

        return await SaveAndReloadAsync("service", name, () => _store.SaveAsync(service with { Name = name, XfsClass = xfsClass }, cancellationToken),
            cancellationToken);
    }

    public async Task<HardwareResult> DeleteAsync(HardwareItem item, int id, CancellationToken cancellationToken)
    {
        var snapshot = await _snapshots.GetAsync(cancellationToken);
        var usedModels = snapshot.Terminals.Select(t => t.ModelId).ToHashSet();
        var error = item switch
        {
            HardwareItem.Vendor when snapshot.Vendors.FirstOrDefault(v => v.Id == id) is null => "The vendor does not exist.",
            HardwareItem.Vendor when snapshot.Vendors.First(v => v.Id == id).Models.FirstOrDefault(m => usedModels.Contains(m.Id)) is { } used =>
                $"The model {used.Name} is used by a terminal.",
            HardwareItem.Model when snapshot.Model(id) is null => "The model does not exist.",
            HardwareItem.Model when usedModels.Contains(id) => "The model is used by a terminal; give the terminal another model first.",
            HardwareItem.Device when snapshot.Device(id) is null => "The device does not exist.",
            HardwareItem.Service when snapshot.Service(id) is null => "The service does not exist.",
            _ => null
        };
        if (error is not null)
            return HardwareResult.Fail(error);

        await _store.DeleteAsync(item, id, cancellationToken);
        _logger.LogInformation("hardware deleted item={Item} id={Id}", item, id);
        await ReloadAsync(snapshot, cancellationToken);
        return HardwareResult.Ok;
    }

    private async Task<HardwareResult> SaveAndReloadAsync(string item, string name, Func<Task<int>> save, CancellationToken cancellationToken)
    {
        var before = await _snapshots.GetAsync(cancellationToken);
        var id = await save();
        _logger.LogInformation("hardware saved item={Item} id={Id} name={Name}", item, id, name);
        await ReloadAsync(before, cancellationToken);
        return HardwareResult.Created(id);
    }

    private async Task ReloadAsync(HardwareSnapshot before, CancellationToken cancellationToken)
    {
        _snapshots.Invalidate();
        var after = await _snapshots.GetAsync(cancellationToken);
        TerminalNotifications.NotifyAll(_notifier, before, after);
    }

    private static bool SameName(string a, string b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
}

/// <summary>Checks Terminal Id, addresses and model, saves, reloads and redraws the terminal pages.</summary>
public sealed class TerminalCatalog : ITerminalCatalog
{
    private readonly ITerminalStore _store;
    private readonly IHardwareSnapshots _snapshots;
    private readonly ITerminalSelections _selections;
    private readonly ITerminalNotifier _notifier;
    private readonly ILogger<TerminalCatalog> _logger;

    public TerminalCatalog(ITerminalStore store, IHardwareSnapshots snapshots, ITerminalSelections selections, ITerminalNotifier notifier,
        ILogger<TerminalCatalog> logger)
    {
        _store = store;
        _snapshots = snapshots;
        _selections = selections;
        _notifier = notifier;
        _logger = logger;
    }

    public async Task<IReadOnlyList<TerminalInfo>> AllAsync(CancellationToken cancellationToken) =>
        (await _snapshots.GetAsync(cancellationToken)).Terminals;

    public async Task<HardwareResult> AddAsync(TerminalInfo terminal, CancellationToken cancellationToken)
    {
        var snapshot = await _snapshots.GetAsync(cancellationToken);
        if (TerminalIds.Normalize(terminal.TerminalId, out var terminalId) is { } idError)
            return HardwareResult.Fail(idError);
        if (snapshot.Terminal(terminalId) is not null)
            return HardwareResult.Fail($"The terminal {TerminalIds.Display(terminalId)} exists.");
        if (Check(snapshot, terminal with { TerminalId = terminalId }, out var normalized) is { } error)
            return HardwareResult.Fail(error);

        await _store.AddAsync(normalized, cancellationToken);
        return await ReloadAsync("added", snapshot, normalized, cancellationToken);
    }

    public async Task<HardwareResult> UpdateAsync(TerminalInfo terminal, CancellationToken cancellationToken)
    {
        var snapshot = await _snapshots.GetAsync(cancellationToken);
        if (snapshot.Terminal(terminal.TerminalId) is null)
            return HardwareResult.Fail("The terminal does not exist.");
        if (Check(snapshot, terminal, out var normalized) is { } error)
            return HardwareResult.Fail(error);

        await _store.UpdateAsync(normalized, cancellationToken);
        return await ReloadAsync("updated", snapshot, normalized, cancellationToken);
    }

    public async Task<HardwareResult> DeleteAsync(string terminalId, CancellationToken cancellationToken)
    {
        var snapshot = await _snapshots.GetAsync(cancellationToken);
        if (snapshot.Terminal(terminalId) is not { } terminal)
            return HardwareResult.Fail("The terminal does not exist.");

        await _store.DeleteAsync(terminalId, cancellationToken);
        return await ReloadAsync("deleted", snapshot, terminal, cancellationToken);
    }

    /// <summary>Addresses as normalised IPs, unique over address1 and address2 of every other terminal; the model must exist.</summary>
    private static string? Check(HardwareSnapshot snapshot, TerminalInfo terminal, out TerminalInfo normalized)
    {
        normalized = terminal;
        if (NormalizeAddress(terminal.Address1) is not { } address1)
            return "Address 1 must be an IP address.";
        string? address2 = null;
        if (!string.IsNullOrWhiteSpace(terminal.Address2) && (address2 = NormalizeAddress(terminal.Address2)) is null)
            return "Address 2 must be an IP address or empty.";
        if (address1 == address2)
            return "Address 1 and address 2 are the same.";
        foreach (var address in new[] { address1, address2 }.OfType<string>())
            if (snapshot.Terminals.FirstOrDefault(t => t.TerminalId != terminal.TerminalId && t.Has(address)) is { } other)
                return $"{address} belongs to the terminal {TerminalIds.Display(other.TerminalId)}.";
        if (snapshot.Model(terminal.ModelId) is null)
            return "The model does not exist.";

        normalized = terminal with { Address1 = address1, Address2 = address2 };
        return null;
    }

    private static string? NormalizeAddress(string? text) =>
        IPAddress.TryParse(text?.Trim(), out var address) ? TerminalAddress.Normalize(address) : null;

    private async Task<HardwareResult> ReloadAsync(string action, HardwareSnapshot before, TerminalInfo terminal, CancellationToken cancellationToken)
    {
        _logger.LogInformation("terminal {Action} terminalId={TerminalId} address1={Address1} address2={Address2} modelId={ModelId}",
            action, TerminalIds.Display(terminal.TerminalId), terminal.Address1, terminal.Address2, terminal.ModelId);
        _snapshots.Invalidate();
        var after = await _snapshots.GetAsync(cancellationToken);
        // The selection cache is keyed by IP: an address that moved or went away must read the database again.
        foreach (var address in AddressesOf(before).Concat(AddressesOf(after)).Distinct())
            _selections.Forget(address);
        TerminalNotifications.NotifyAll(_notifier, before, after);
        return HardwareResult.Ok;
    }

    private static IEnumerable<string> AddressesOf(HardwareSnapshot snapshot) =>
        snapshot.Terminals.SelectMany(t => new[] { t.Address1, t.Address2 }).OfType<string>();
}

internal static class TerminalNotifications
{
    /// <summary>Every terminal page (before and after the edit) redraws: model, areas and header may have changed.</summary>
    public static void NotifyAll(ITerminalNotifier notifier, HardwareSnapshot before, HardwareSnapshot after)
    {
        foreach (var address in before.Terminals.Concat(after.Terminals).SelectMany(t => new[] { t.Address1, t.Address2 }).OfType<string>().Distinct())
            notifier.Notify(address);
    }
}

public sealed class ServiceResolver : IServiceResolver
{
    private readonly IHardwareSnapshots _snapshots;

    public ServiceResolver(IHardwareSnapshots snapshots)
    {
        _snapshots = snapshots;
    }

    public async Task<TerminalView?> TerminalAsync(string address, CancellationToken cancellationToken)
    {
        var snapshot = await _snapshots.GetAsync(cancellationToken);
        if (snapshot.TerminalOf(address) is not { } terminal || snapshot.Model(terminal.ModelId) is not { } model)
            return null;
        return new TerminalView(terminal, model, snapshot.VendorOf(model)?.Name ?? string.Empty);
    }

    public async Task<ServiceResolution> ResolveAsync(string address, string logicalName, CancellationToken cancellationToken)
    {
        if (await TerminalAsync(address, cancellationToken) is not { } view)
            return ServiceResolution.NotFound($"no terminal is defined for {address}");

        var match = view.Model.Devices
            .SelectMany(d => d.Services.Select(s => (Device: d, Service: s)))
            .FirstOrDefault(x => string.Equals(x.Service.Name, logicalName, StringComparison.OrdinalIgnoreCase));
        if (match.Service is null)
            return ServiceResolution.NotFound($"the model {view.ModelTitle} has no service '{logicalName}'");

        return ServiceResolution.Found(new ResolvedService(view.Terminal.TerminalId, match.Service.Name, match.Service.XfsClass, match.Device.Id,
            match.Device.Name, view.Model.Name, view.VendorName));
    }
}
