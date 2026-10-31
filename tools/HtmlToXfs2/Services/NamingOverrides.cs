using System.Text.Json;

namespace HtmlToXfs2.Services;

/// <summary>Exceptions to the automatic naming rules, loaded from naming-overrides.json.</summary>
public sealed class NamingOverrides
{
    /// <summary>Struct tag (wfs_idc_set_guidlight) to final C# name.</summary>
    public Dictionary<string, string> StructNames { get; set; } = new();

    /// <summary>Default enum name (StatusMedia) to final C# name (Media).</summary>
    public Dictionary<string, string> EnumNames { get; set; } = new();

    /// <summary>Final enum name to the default names of the enums that are unified into it.</summary>
    public Dictionary<string, List<string>> EnumMerges { get; set; } = new();

    /// <summary>Header comment owner (WFSIDCEMVCLESSDATA.wTxOutcome) to the real struct field.</summary>
    public Dictionary<string, string> OwnerRemap { get; set; } = new();

    /// <summary>#define name to enum member name.</summary>
    public Dictionary<string, string> MemberNames { get; set; } = new();

    public static NamingOverrides Load(string path)
    {
        if (!File.Exists(path))
            return new NamingOverrides();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<NamingOverrides>(File.ReadAllText(path), options) ?? new NamingOverrides();
    }
}
