namespace HtmlToXfs2.Services;

/// <summary>Turns #define names (WFS_IDC_MEDIAPRESENT) into enum member names (Present).</summary>
public static class EnumMemberNamer
{
    public static string CommonPrefix(IReadOnlyList<string> defineNames)
    {
        // NOTSUPP and UNKNOWN members rarely share the group prefix, so they must not shorten it.
        var values = defineNames
            .Where(v => !v.Contains("NOTSUPP", StringComparison.OrdinalIgnoreCase)
                        && !v.Contains("UNKNOWN", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (values.Count < 2)
            return "";

        var prefix = values[0];
        foreach (var value in values.Skip(1))
        {
            var common = 0;
            var max = Math.Min(prefix.Length, value.Length);
            while (common < max && char.ToUpperInvariant(prefix[common]) == char.ToUpperInvariant(value[common]))
                common++;

            prefix = prefix.Substring(0, common);
        }

        // A prefix that ends inside a word (WFS_IDC_DIP_E) is pulled back to the last underscore.
        var lastUnderscore = prefix.LastIndexOf('_');
        if (lastUnderscore >= 0)
        {
            var tail = prefix.Length - lastUnderscore - 1;
            if (tail > 0 && tail <= 2)
                prefix = prefix.Substring(0, lastUnderscore + 1);
        }

        return prefix;
    }

    public static string MemberName(string defineName, string commonPrefix, string device)
    {
        var name = defineName;

        if (commonPrefix.Length > 0 && name.StartsWith(commonPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var rest = name.Substring(commonPrefix.Length);
            if (rest.Length > 0 && !char.IsDigit(rest[0]))
                name = rest;
        }
        else
        {
            name = StripFamilyPrefix(name, device);
        }

        return NameConverter.ToPascalCase(name.TrimStart('_'));
    }

    public static string StripFamilyPrefix(string name, string device)
    {
        if (name.StartsWith("WFS_", StringComparison.OrdinalIgnoreCase))
            name = name.Substring(4);

        var devicePrefix = device.ToUpperInvariant() + "_";
        if (device != "api" && name.StartsWith(devicePrefix, StringComparison.OrdinalIgnoreCase))
            name = name.Substring(devicePrefix.Length);

        return name;
    }
}
