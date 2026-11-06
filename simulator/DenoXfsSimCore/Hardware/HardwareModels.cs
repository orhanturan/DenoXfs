namespace DenoXfs.Sim.Core.Hardware;

/// <summary>An XFS logical service of a device, opened by its name (WFSOpen("Idc340")); the class picks the module.</summary>
public sealed record ServiceInfo(int Id, int DeviceId, string Name, string XfsClass);

/// <summary>A physical module of a model (e.g. a card reader); its type decides which XFS classes it may open.</summary>
public sealed record DeviceInfo(int Id, int ModelId, string DeviceType, string Name, IReadOnlyList<ServiceInfo> Services);

public sealed record ModelInfo(int Id, int VendorId, string Name, IReadOnlyList<DeviceInfo> Devices)
{
    public IEnumerable<ServiceInfo> Services => Devices.SelectMany(d => d.Services);
}

public sealed record VendorInfo(int Id, string Name, IReadOnlyList<ModelInfo> Models);

/// <summary>A terminal: ISO 8583 Terminal Id (8 characters, see <see cref="TerminalIds"/>), one or two IPs and its model.</summary>
public sealed record TerminalInfo(string TerminalId, string Address1, string? Address2, int ModelId)
{
    public bool Has(string address) => Address1 == address || Address2 == address;
}

/// <summary>Everything of the hardware catalog and the terminals, small enough to be read as a whole and cached.</summary>
public sealed record HardwareSnapshot(IReadOnlyList<VendorInfo> Vendors, IReadOnlyList<TerminalInfo> Terminals)
{
    public static readonly HardwareSnapshot Empty = new([], []);

    public IEnumerable<ModelInfo> Models => Vendors.SelectMany(v => v.Models);

    public IEnumerable<DeviceInfo> Devices => Models.SelectMany(m => m.Devices);

    public ModelInfo? Model(int id) => Models.FirstOrDefault(m => m.Id == id);

    public DeviceInfo? Device(int id) => Devices.FirstOrDefault(d => d.Id == id);

    public ServiceInfo? Service(int id) => Devices.SelectMany(d => d.Services).FirstOrDefault(s => s.Id == id);

    public VendorInfo? VendorOf(ModelInfo model) => Vendors.FirstOrDefault(v => v.Id == model.VendorId);

    public TerminalInfo? Terminal(string terminalId) => Terminals.FirstOrDefault(t => t.TerminalId == terminalId);

    /// <summary>The terminal of an IP (address1 or address2); the first one by Terminal Id if an IP was entered twice.</summary>
    public TerminalInfo? TerminalOf(string address) =>
        Terminals.Where(t => t.Has(address)).OrderBy(t => t.TerminalId, StringComparer.Ordinal).FirstOrDefault();
}

/// <summary>
/// The service a session opened: the terminal, the logical service and the device it belongs to. Device state is kept
/// per device: the CDM and CIM services of one recycler share it, a second card reader or a dispenser has its own.
/// </summary>
public sealed record ResolvedService(string TerminalId, string ServiceName, string XfsClass, int DeviceId, string DeviceName, string ModelName,
    string VendorName);

/// <summary>
/// A physical device of a terminal: the Terminal Id (not an IP: a terminal may have two) and the model device. The key
/// of device state, sessions of the device and its events.
/// </summary>
public sealed record DeviceRef(string TerminalId, int DeviceId)
{
    public override string ToString() => $"{TerminalIds.Display(TerminalId)}#{DeviceId}";
}

/// <summary>Outcome of resolving a WFSOpen: the service, or why it was not found.</summary>
public sealed record ServiceResolution(ResolvedService? Service, string? Reason)
{
    public static ServiceResolution Found(ResolvedService service) => new(service, null);

    public static ServiceResolution NotFound(string reason) => new(null, reason);
}

/// <summary>A terminal with its model, as the terminal page shows it.</summary>
public sealed record TerminalView(TerminalInfo Terminal, ModelInfo Model, string VendorName)
{
    public string DisplayId => TerminalIds.Display(Terminal.TerminalId);

    public string ModelTitle => $"{VendorName} · {Model.Name}";

    public IReadOnlySet<string> XfsClasses => Model.Services.Select(s => s.XfsClass).ToHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>The devices of the model that open a service of this class, e.g. the card readers for IDC.</summary>
    public IReadOnlyList<DeviceInfo> DevicesOf(string xfsClass) =>
        Model.Devices.Where(d => d.Services.Any(s => string.Equals(s.XfsClass, xfsClass, StringComparison.OrdinalIgnoreCase))).ToList();
}

/// <summary>Result of an edit: null error when done, plus the id of a new item.</summary>
public sealed record HardwareResult(string? Error, int Id = 0)
{
    public static readonly HardwareResult Ok = new((string?)null);

    public static HardwareResult Created(int id) => new(null, id);

    public static HardwareResult Fail(string error) => new(error);

    public bool Succeeded => Error is null;
}

/// <summary>Kinds of catalog items, for delete.</summary>
public enum HardwareItem
{
    Vendor,
    Model,
    Device,
    Service
}

/// <summary>Add (Id 0) or rename a vendor.</summary>
public sealed record VendorEdit(int Id, string Name);

/// <summary>Add (Id 0) or rename a model of a vendor.</summary>
public sealed record ModelEdit(int Id, int VendorId, string Name);

/// <summary>Add (Id 0) or change a device of a model.</summary>
public sealed record DeviceEdit(int Id, int ModelId, string DeviceType, string Name);

/// <summary>Add (Id 0) or change a service of a device.</summary>
public sealed record ServiceEdit(int Id, int DeviceId, string Name, string XfsClass);
