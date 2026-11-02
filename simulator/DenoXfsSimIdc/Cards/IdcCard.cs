namespace DenoXfs.Sim.Idc.Cards;

/// <summary>
/// Test card. Tracks are ASCII text sent as their Latin1 bytes ('4' = 0x34); a track may also hold binary bytes.
/// Any track and the ATR may be absent. <see cref="ChipResponses"/> maps a command APDU to its response
/// (hex, upper case, no spaces) for WFS_CMD_IDC_CHIP_IO.
/// </summary>
public sealed record IdcCard(
    string Name,
    string? Track1,
    string? Track2,
    string? Track3,
    byte[]? Atr,
    IReadOnlyDictionary<string, string> ChipResponses)
{
    /// <summary>Answer to an APDU that has no stored response: INS not supported.</summary>
    public const string UnknownApduResponse = "6D00";

    public bool HasChip => Atr is { Length: > 0 };

    public byte[] RespondTo(byte[] apdu) =>
        Convert.FromHexString(ChipResponses.TryGetValue(Convert.ToHexString(apdu), out var response) ? response : UnknownApduResponse);
}

/// <summary>The test cards the user can insert and edit on the web page (stored in the simulator database).</summary>
public interface ICardCatalog
{
    Task<IReadOnlyList<IdcCard>> AllAsync(CancellationToken cancellationToken);

    Task<IdcCard?> FindAsync(string name, CancellationToken cancellationToken);

    /// <summary>Adds the card or replaces the card with the same name (or with <paramref name="originalName"/> when it is renamed).</summary>
    Task SaveAsync(IdcCard card, string? originalName, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string name, CancellationToken cancellationToken);
}
