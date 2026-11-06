using DenoXfs.Sim.Core.Scenarios;

namespace DenoXfs.Sim.Data;

// Table and column names come from SimDbContext's naming convention: deno_ + singular snake_case (deno_model_device.model_id).

/// <summary>A hardware vendor, e.g. Deno.</summary>
public class Vendor
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<TerminalModel> Models { get; set; } = new();
}

/// <summary>A terminal model of a vendor, e.g. Deno26.</summary>
public class TerminalModel
{
    public int Id { get; set; }

    public int VendorId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<ModelDevice> Devices { get; set; } = new();
}

/// <summary>A physical module of a model; DeviceType is one of the code's DeviceTypes (CardReader, Recycler, ...).</summary>
public class ModelDevice
{
    public int Id { get; set; }

    public int ModelId { get; set; }

    public string DeviceType { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public List<XfsService> Services { get; set; } = new();
}

/// <summary>An XFS logical service of a device (WFSOpen name, e.g. Idc340); one per XFS class and device.</summary>
public class XfsService
{
    public int Id { get; set; }

    public int ModelDeviceId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string XfsClass { get; set; } = string.Empty;
}

/// <summary>A terminal, defined before use: ISO 8583 Terminal Id (char(8), padded), the IPs of its DLL connections, its model.</summary>
public class Terminal
{
    public string TerminalId { get; set; } = string.Empty;

    public string Address1 { get; set; } = string.Empty;

    public string? Address2 { get; set; }

    public int ModelId { get; set; }

    public List<DeviceState> DeviceStates { get; set; } = new();

    /// <summary>Selected scenario (null = Default).</summary>
    public int? ScenarioId { get; set; }

    /// <summary>Selected device profile (null = Default).</summary>
    public int? DeviceProfileId { get; set; }
}

/// <summary>A scenario or a device profile (<see cref="Kind"/>); the name is unique per kind, case insensitive.</summary>
public class Profile
{
    public int Id { get; set; }

    public ProfileKind Kind { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? BasedOn { get; set; }

    public List<ProfileSection> Sections { get; set; } = new();
}

/// <summary>The JSON of one device class (e.g. "IDC") in a profile.</summary>
public class ProfileSection
{
    public int Id { get; set; }

    public int ProfileId { get; set; }

    public string XfsClass { get; set; } = string.Empty;

    public string Json { get; set; } = string.Empty;
}

/// <summary>Test card. Tracks are text (Latin1, may hold binary bytes); the ATR is raw bytes.</summary>
public class Card
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Track1 { get; set; }

    public string? Track2 { get; set; }

    public string? Track3 { get; set; }

    public byte[]? Atr { get; set; }

    public List<CardChipResponse> ChipResponses { get; set; } = new();
}

/// <summary>A chip command APDU and the card's response (hex, upper case, no spaces) for WFS_CMD_IDC_CHIP_IO.</summary>
public class CardChipResponse
{
    public int Id { get; set; }

    public int CardId { get; set; }

    public string Command { get; set; } = string.Empty;

    public string Response { get; set; } = string.Empty;
}

/// <summary>Device class data shared by all terminals as JSON, e.g. XfsClass "IDC", Category "Forms".</summary>
public class DeviceData
{
    public int Id { get; set; }

    public string XfsClass { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Json { get; set; } = string.Empty;
}

/// <summary>
/// State of one device of a terminal as JSON (settings, counters) that survives a host restart, per XFS class module:
/// the device (model device) is the key, so two card readers keep their own state and a recycler's modules share the device.
/// </summary>
public class DeviceState
{
    public int Id { get; set; }

    public string TerminalId { get; set; } = string.Empty;

    public int ModelDeviceId { get; set; }

    public string XfsClass { get; set; } = string.Empty;

    public string Json { get; set; } = string.Empty;
}
