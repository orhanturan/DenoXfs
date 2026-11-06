namespace DenoXfs.Sim.Idc.Cards;

public sealed record ChipResponseDto(string Command, string Response);

/// <summary>A card as the web front end edits it: tracks as text (binary bytes as \u escapes), ATR and APDUs as hex.</summary>
public sealed record CardDto(string Name, string? Track1, string? Track2, string? Track3, string? Atr, IReadOnlyList<ChipResponseDto> ChipResponses,
    string? OriginalName = null);

/// <summary>Card ↔ <see cref="CardDto"/>, shared by the REST API and the card editor of the terminal page.</summary>
public static class CardDtos
{
    public static CardDto ToDto(IdcCard card) => new(
        card.Name, card.Track1, card.Track2, card.Track3, card.Atr is null ? null : Convert.ToHexString(card.Atr),
        card.ChipResponses.Select(r => new ChipResponseDto(r.Key, r.Value)).ToList());

    /// <summary>Validates the input: a name, hex ATR and APDUs (spaces allowed), no duplicate command.</summary>
    public static (IdcCard? Card, string Error) FromDto(CardDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return (null, "The card needs a name.");
        if (!TryHex(dto.Atr, out var atr))
            return (null, "The ATR is not valid hex.");

        var responses = new Dictionary<string, string>();
        foreach (var row in dto.ChipResponses)
        {
            if (!TryHex(row.Command, out var command) || command is not { Length: > 0 } || !TryHex(row.Response, out var response) || response is not { Length: >= 2 })
                return (null, $"APDU row '{row.Command}' is not valid hex (the response needs at least SW1 SW2).");
            if (!responses.TryAdd(Convert.ToHexString(command), Convert.ToHexString(response)))
                return (null, $"Command {Convert.ToHexString(command)} appears twice.");
        }

        return (new IdcCard(dto.Name.Trim(), Empty(dto.Track1), Empty(dto.Track2), Empty(dto.Track3), atr, responses), string.Empty);
    }

    /// <summary>Track text for display and editing: non printable bytes as \xNN, the backslash as \\.</summary>
    public static string EscapeTrack(string? value) => string.Concat((value ?? string.Empty).Select(ch =>
        ch == '\\' ? "\\\\" : ch is >= ' ' and < (char)0x7F ? ch.ToString() : $"\\x{(int)ch:X2}"));

    /// <summary>Reverse of <see cref="EscapeTrack"/>.</summary>
    public static string UnescapeTrack(string value) =>
        System.Text.RegularExpressions.Regex.Replace(value, @"\\x([0-9A-Fa-f]{2})|\\\\",
            m => m.Groups[1].Success ? ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString() : "\\");

    private static string? Empty(string? text) => string.IsNullOrEmpty(text) ? null : text;

    private static bool TryHex(string? text, out byte[]? bytes)
    {
        bytes = null;
        var hex = (text ?? string.Empty).Replace(" ", string.Empty);
        if (hex.Length == 0)
            return true;
        if (hex.Length % 2 != 0 || !hex.All(Uri.IsHexDigit))
            return false;
        bytes = Convert.FromHexString(hex);
        return true;
    }
}
