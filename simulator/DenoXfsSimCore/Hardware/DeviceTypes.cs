namespace DenoXfs.Sim.Core.Hardware;

/// <summary>A device type and the XFS classes a device of it may open (one service per class).</summary>
public sealed record DeviceTypeInfo(string Name, string Description, IReadOnlyList<string> XfsClasses)
{
    public bool Allows(string xfsClass) => XfsClasses.Contains(xfsClass, StringComparer.OrdinalIgnoreCase);
}

/// <summary>
/// The device types, fixed in code: a type's behaviour needs code anyway (e.g. the shared cassettes of a recycler that
/// opens both CDM and CIM).
/// </summary>
public static class DeviceTypes
{
    public static readonly IReadOnlyList<DeviceTypeInfo> All =
    [
        new("CardReader", "Card reader (motorised, dip or swipe)", ["IDC"]),
        new("PinPad", "Encrypting PIN pad", ["PIN"]),
        new("ReceiptPrinter", "Receipt printer", ["PTR"]),
        new("Dispenser", "Cash dispenser", ["CDM"]),
        new("Recycler", "Cash recycler: dispenses (CDM) and accepts (CIM) with shared cassettes", ["CDM", "CIM"])
    ];

    public static DeviceTypeInfo? Find(string? name) =>
        All.FirstOrDefault(t => string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase));
}

/// <summary>
/// ISO 8583 field 41 (card acceptor terminal identification), "ans 8": exactly 8 printable ASCII characters including
/// space. Shorter input is right padded with spaces so every database stores and compares the same value.
/// </summary>
public static class TerminalIds
{
    public const int Length = 8;

    /// <summary>Null when valid, with the padded Terminal Id; otherwise why not.</summary>
    public static string? Normalize(string? input, out string terminalId)
    {
        terminalId = string.Empty;
        var value = input?.TrimEnd() ?? string.Empty;
        if (value.Length == 0)
            return "The Terminal Id is required.";
        if (value.Length > Length)
            return $"The Terminal Id has at most {Length} characters (ISO 8583 field 41).";
        if (value.Any(c => c < ' ' || c > '~'))
            return "The Terminal Id holds printable ASCII characters only (ISO 8583 'ans').";
        terminalId = value.PadRight(Length);
        return null;
    }

    /// <summary>Without the padding, for the screen and the logs.</summary>
    public static string Display(string terminalId) => terminalId.TrimEnd();
}
