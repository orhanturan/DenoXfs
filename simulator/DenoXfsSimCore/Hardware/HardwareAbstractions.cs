namespace DenoXfs.Sim.Core.Hardware;

/// <summary>Persistence of the hardware catalog (vendors, models, devices, services); rules are checked by <see cref="IHardwareCatalog"/>.</summary>
public interface IHardwareStore
{
    Task<HardwareSnapshot> LoadAsync(CancellationToken cancellationToken);

    Task<int> SaveAsync(VendorEdit vendor, CancellationToken cancellationToken);

    Task<int> SaveAsync(ModelEdit model, CancellationToken cancellationToken);

    Task<int> SaveAsync(DeviceEdit device, CancellationToken cancellationToken);

    Task<int> SaveAsync(ServiceEdit service, CancellationToken cancellationToken);

    /// <summary>Deletes the item with everything under it (a vendor's models, a model's devices, a device's services).</summary>
    Task DeleteAsync(HardwareItem item, int id, CancellationToken cancellationToken);
}

/// <summary>Persistence of the terminals; rules are checked by <see cref="ITerminalCatalog"/>.</summary>
public interface ITerminalStore
{
    Task AddAsync(TerminalInfo terminal, CancellationToken cancellationToken);

    Task UpdateAsync(TerminalInfo terminal, CancellationToken cancellationToken);

    /// <summary>Deletes the terminal with its device states.</summary>
    Task DeleteAsync(string terminalId, CancellationToken cancellationToken);
}

/// <summary>The catalog and terminals in memory; read on every WFSOpen and by the web page, reloaded after an edit.</summary>
public interface IHardwareSnapshots
{
    Task<HardwareSnapshot> GetAsync(CancellationToken cancellationToken);

    void Invalidate();
}

/// <summary>Edits of vendors, models, devices and services with the catalog's rules (names, device types, classes).</summary>
public interface IHardwareCatalog
{
    Task<HardwareSnapshot> SnapshotAsync(CancellationToken cancellationToken);

    Task<HardwareResult> SaveAsync(VendorEdit vendor, CancellationToken cancellationToken);

    Task<HardwareResult> SaveAsync(ModelEdit model, CancellationToken cancellationToken);

    Task<HardwareResult> SaveAsync(DeviceEdit device, CancellationToken cancellationToken);

    Task<HardwareResult> SaveAsync(ServiceEdit service, CancellationToken cancellationToken);

    Task<HardwareResult> DeleteAsync(HardwareItem item, int id, CancellationToken cancellationToken);
}

/// <summary>Terminals are defined before use: Terminal Id, addresses and model.</summary>
public interface ITerminalCatalog
{
    Task<IReadOnlyList<TerminalInfo>> AllAsync(CancellationToken cancellationToken);

    Task<HardwareResult> AddAsync(TerminalInfo terminal, CancellationToken cancellationToken);

    /// <summary>Changes the addresses and the model; the Terminal Id is the key and does not change.</summary>
    Task<HardwareResult> UpdateAsync(TerminalInfo terminal, CancellationToken cancellationToken);

    Task<HardwareResult> DeleteAsync(string terminalId, CancellationToken cancellationToken);
}

/// <summary>Finds the terminal of an IP and the service a WFSOpen names in the terminal's model.</summary>
public interface IServiceResolver
{
    Task<TerminalView?> TerminalAsync(string address, CancellationToken cancellationToken);

    Task<ServiceResolution> ResolveAsync(string address, string logicalName, CancellationToken cancellationToken);
}

/// <summary>A device class module (IDC, later CDM, ...); its commands and infos are registered keyed by <see cref="DeviceClass"/>.</summary>
public interface IDeviceClassModule
{
    string DeviceClass { get; }
}

public sealed record DeviceClassModule(string DeviceClass) : IDeviceClassModule;
