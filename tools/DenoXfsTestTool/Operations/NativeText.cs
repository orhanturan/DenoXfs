using System.Runtime.InteropServices;
using System.Text;

namespace DenoXfs.TestTool.Operations;

/// <summary>
/// Reads lpstrTrackData, the one XFS string format the generated code does not cover (lists are read by
/// <c>MultiStringConvert</c>).
/// </summary>
public static class NativeText
{
    private const int MaxLength = 64 * 1024;

    /// <summary>
    /// lpstrTrackData of READ_TRACK / PARSE_DATA: "TRACK2:PAN=..\0EXPIRY=..\0\0TRACK3:..\0\0\0" (fields end with 0x00, a
    /// track with one more, the buffer with one more). Returns one line per track: "TRACK2: PAN=.. | EXPIRY=..".
    /// </summary>
    public static IReadOnlyList<string> TrackData(IntPtr pointer)
    {
        var tracks = new List<string>();
        if (pointer == IntPtr.Zero)
            return tracks;

        var offset = 0;
        while (offset < MaxLength)
        {
            var fields = new List<string>();
            while (offset < MaxLength)
            {
                var field = Marshal.PtrToStringAnsi(pointer + offset) ?? string.Empty;
                offset += field.Length + 1;
                if (field.Length == 0)
                    break;
                fields.Add(field);
            }
            if (fields.Count == 0)
                break;

            var keyword = fields[0].IndexOf(':');
            tracks.Add(keyword < 0
                ? string.Join(" | ", fields)
                : $"{fields[0][..(keyword + 1)]} {string.Join(" | ", fields.Skip(1).Prepend(fields[0][(keyword + 1)..]))}");
        }
        return tracks;
    }

    public static byte[] Latin1(string text) => Encoding.Latin1.GetBytes(text);
}
