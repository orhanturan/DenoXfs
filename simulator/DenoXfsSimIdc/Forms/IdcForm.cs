using System.Text.Json.Serialization;

namespace DenoXfs.Sim.Idc.Forms;

/// <summary>
/// A form (spec section 7, simplified): which tracks it reads or writes and the fields it returns per track.
/// <see cref="Tracks"/> is "TRACK2", "TRACK1|TRACK2" (any of them) or "TRACK1&amp;TRACK2" (all of them).
/// Known fields: TRACK1 FORMATCODE, PAN, NAME, EXPIRY, SERVICECODE, DISCRETIONARY; TRACK2 PAN, EXPIRY, SERVICECODE,
/// DISCRETIONARY; TRACK3 FORMATCODE, PAN, ADDITIONAL; every track ALL.
/// </summary>
public sealed record IdcForm(
    string Name,
    ushort Action,
    string Tracks,
    bool Secure,
    IReadOnlyList<string> Track1Fields,
    IReadOnlyList<string> Track2Fields,
    IReadOnlyList<string> Track3Fields)
{
    public const char Track1Separator = '^';
    public const char Track2Separator = '=';
    public const char Track3Separator = '=';

    /// <summary>Track numbers named in <see cref="Tracks"/>, in order.</summary>
    [JsonIgnore]
    public IReadOnlyList<int> TrackNumbers =>
        Tracks.Split('|', '&').Select(t => t.Trim().ToUpperInvariant()).Select(t => t switch
        {
            "TRACK1" => 1,
            "TRACK2" => 2,
            "TRACK3" => 3,
            _ => 0
        }).ToList();

    /// <summary>True for "and" forms: every track must be readable.</summary>
    [JsonIgnore]
    public bool RequiresAllTracks => Tracks.Contains('&');

    [JsonIgnore]
    public bool IsValid => TrackNumbers.Count > 0 && TrackNumbers.All(n => n is >= 1 and <= 3);

    public IReadOnlyList<string> FieldsOf(int track) => track switch
    {
        1 => Track1Fields,
        2 => Track2Fields,
        _ => Track3Fields
    };
}

/// <summary>The forms of the card reader (IDC "Forms" device data in the simulator database).</summary>
public interface IIdcFormCatalog
{
    Task<IReadOnlyList<IdcForm>> AllAsync(CancellationToken cancellationToken);

    Task<IdcForm?> FindAsync(string name, CancellationToken cancellationToken);
}
