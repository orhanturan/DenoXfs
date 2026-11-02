namespace DenoXfs.Sim.Data;

/// <summary>A terminal, identified by the remote address of its DLL connections.</summary>
public class Terminal
{
    public int Id { get; set; }

    public string Address1 { get; set; } = string.Empty;

    public string? Address2 { get; set; }

    public List<DeviceState> DeviceStates { get; set; } = new();
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

/// <summary>Device class data shared by all terminals as JSON, e.g. DeviceClass "IDC", Category "Forms".</summary>
public class DeviceData
{
    public int Id { get; set; }

    public string DeviceClass { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Json { get; set; } = string.Empty;
}

/// <summary>Per terminal state of one device class as JSON (settings, counters) that survives a host restart.</summary>
public class DeviceState
{
    public int Id { get; set; }

    public int TerminalId { get; set; }

    public string DeviceClass { get; set; } = string.Empty;

    public string Json { get; set; } = string.Empty;
}
