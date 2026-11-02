using System.Text;
using DenoXfs.Sim.Core.Protocol;

namespace DenoXfs.Sim.Idc.Forms;

/// <summary>
/// Result of reading tracks through a form: the lpstrTrackData bytes, the HRESULT (WFS_ERR_IDC_INVALIDDATA when the
/// form's track rule is not met) and the tracks that could not be read (one WFS_EXEE_IDC_INVALIDTRACKDATA each).
/// </summary>
public sealed record FormattedTracks(int HResult, byte[] TrackData, IReadOnlyList<int> MissingTracks);

/// <summary>Applies a read form to track texts (READ_TRACK on the card inside, PARSE_DATA on the given card data).</summary>
public interface ITrackDataFormatter
{
    FormattedTracks Format(IdcForm form, IReadOnlyDictionary<int, string?> tracks);
}

public sealed class TrackDataFormatter : ITrackDataFormatter
{
    public FormattedTracks Format(IdcForm form, IReadOnlyDictionary<int, string?> tracks)
    {
        // "TRACK2:PAN=...\0EXPIRY=...\0\0TRACK3:...\0\0\0": fields end with 0x00, a track with one more, the buffer with one more.
        var text = new StringBuilder();
        var missing = new List<int>();
        foreach (var number in form.TrackNumbers)
        {
            if (!tracks.TryGetValue(number, out var track) || string.IsNullOrEmpty(track))
            {
                missing.Add(number);
                continue;
            }

            text.Append("TRACK").Append(number).Append(':');
            var fields = Fields(number, track);
            foreach (var name in form.FieldsOf(number))
                text.Append(name).Append('=').Append(fields.GetValueOrDefault(name, string.Empty)).Append('\0');
            text.Append('\0');
        }
        text.Append('\0');

        var read = form.TrackNumbers.Count - missing.Count;
        var ok = form.RequiresAllTracks ? missing.Count == 0 : read > 0;
        return new FormattedTracks(ok ? XfsResultCodes.Success : IdcConstants.ErrInvalidData, Encoding.Latin1.GetBytes(text.ToString()), missing);
    }

    private static Dictionary<string, string> Fields(int number, string track)
    {
        var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { ["ALL"] = track };
        switch (number)
        {
            case 1:
                // B<PAN>^<NAME>^<YYMM><SVC><discretionary>
                var parts = track.Split(IdcForm.Track1Separator);
                if (parts[0].Length > 0)
                {
                    fields["FORMATCODE"] = parts[0][..1];
                    fields["PAN"] = parts[0][1..];
                }
                if (parts.Length > 1)
                    fields["NAME"] = parts[1];
                if (parts.Length > 2)
                    AddExpiryServiceDiscretionary(fields, parts[2]);
                break;
            case 2:
                // <PAN>=<YYMM><SVC><discretionary>
                var separator = track.IndexOf(IdcForm.Track2Separator);
                fields["PAN"] = separator < 0 ? track : track[..separator];
                if (separator >= 0)
                    AddExpiryServiceDiscretionary(fields, track[(separator + 1)..]);
                break;
            case 3:
                // <format code 2><PAN>=<additional data>
                var end = track.IndexOf(IdcForm.Track3Separator);
                fields["FORMATCODE"] = track[..Math.Min(2, track.Length)];
                if (track.Length > 2)
                    fields["PAN"] = end < 0 ? track[2..] : track[2..Math.Max(2, end)];
                if (end >= 0)
                    fields["ADDITIONAL"] = track[(end + 1)..];
                break;
        }
        return fields;
    }

    private static void AddExpiryServiceDiscretionary(Dictionary<string, string> fields, string rest)
    {
        fields["EXPIRY"] = rest[..Math.Min(4, rest.Length)];
        if (rest.Length > 4)
            fields["SERVICECODE"] = rest[4..Math.Min(7, rest.Length)];
        if (rest.Length > 7)
            fields["DISCRETIONARY"] = rest[7..];
    }
}
