using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services;

public sealed record SectionDoc(string File, string Type, string Text, string? Command = null);

/// <summary>Loads the spec sections of a device so struct usage can be counted per section.</summary>
public static class SectionUsage
{
    private static readonly HashSet<string> CommandTypes = new() { "info_command", "execute_command", "event" };
    private static readonly Regex TypeRegex = new(@"^type:\s*(\w+)\s*$", RegexOptions.Multiline | RegexOptions.Compiled);
    private static readonly Regex TitleRegex = new(@"^title:\s*""([^""]+)""", RegexOptions.Multiline | RegexOptions.Compiled);
    private static readonly Regex CommandTitleRegex = new(@"^WFS_(?:INF|CMD|EXEE|SRVE|USRE|SYSE)_([A-Za-z0-9]+)_(.+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static IReadOnlyList<SectionDoc> Load(string sectionsDirectory)
    {
        var docs = new List<SectionDoc>();
        if (!Directory.Exists(sectionsDirectory))
            return docs;

        foreach (var file in Directory.GetFiles(sectionsDirectory, "*.md").OrderBy(f => f, StringComparer.Ordinal))
        {
            var text = File.ReadAllText(file);
            var match = TypeRegex.Match(text);
            if (!match.Success || !CommandTypes.Contains(match.Groups[1].Value))
                continue;

            var title = TitleRegex.Match(text);
            docs.Add(new SectionDoc(Path.GetFileName(file), match.Groups[1].Value, text, title.Success ? title.Groups[1].Value : null));
        }

        return docs;
    }

    /// <summary>WFS_INF_IDC_STATUS → Status. A value that is already a folder name is returned unchanged.</summary>
    public static string? CommandFolder(string? command, string device)
    {
        if (string.IsNullOrWhiteSpace(command))
            return null;

        var match = CommandTitleRegex.Match(command.Trim());
        if (!match.Success)
            return command;

        if (!match.Groups[1].Value.Equals(device, StringComparison.OrdinalIgnoreCase))
            return null;

        return NameConverter.ToPascalCase(match.Groups[2].Value);
    }

    public static IReadOnlyList<SectionDoc> SectionsMentioning(IReadOnlyList<SectionDoc> docs, HeaderTypedef header)
    {
        var pattern = new Regex($@"\b(?:LP)?{Regex.Escape(header.TypedefName)}\b|\b_?{Regex.Escape(header.Tag)}\b");
        return docs.Where(d => pattern.IsMatch(d.Text)).ToList();
    }
}
