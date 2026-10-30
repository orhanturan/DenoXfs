using System.Text.Json;
using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services;

/// <summary>
/// Header dosyalarından section'lara göre #define'ları parse edip HeaderEnums.json'a yazar.
/// </summary>
public class HeaderEnumExtractor
{
    private readonly Dictionary<string, EnumRule> _rules = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Enum kuralı ekler.
    /// </summary>
    public void AddRule(string sectionMarker, string enumName, string enumType = "DWORD", params string[] prefixFilters)
    {
        _rules[sectionMarker] = new EnumRule
        {
            Name = enumName,
            Type = enumType,
            PrefixFilters = prefixFilters.Length > 0 ? prefixFilters : null
        };
    }
    
    private class EnumRule
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "DWORD";
        public string[]? PrefixFilters { get; set; } // null = hepsini al, değilse sadece bu prefix'lerle başlayanları al
    }

    /// <summary>
    /// Header dosyasını parse edip HeaderEnums.json'a yazar.
    /// </summary>
    public void ExtractAndSave(string headerPath, string outputPath)
    {
        if (!File.Exists(headerPath))
        {
            Console.WriteLine($"  Header file not found: {headerPath}");
            return;
        }

        var enums = ParseHeader(headerPath);
        
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(outputPath, JsonSerializer.Serialize(enums, options));
        
        Console.WriteLine($"  Saved: {outputPath}");
        foreach (var e in enums)
        {
            Console.WriteLine($"    - {e.Key}: {e.Value.Values.Count} values");
        }
    }

    /// <summary>
    /// Header dosyasını satır satır okur, section bulunca sonraki section'a kadar #define'ları toplar.
    /// </summary>
    private Dictionary<string, EnumEntry> ParseHeader(string headerPath)
    {
        var result = new Dictionary<string, EnumEntry>();
        var lines = File.ReadAllLines(headerPath);
        
        EnumRule? currentRule = null;
        List<string>? currentValues = null;

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();

            // Section header mı?
            if (IsSectionHeader(line))
            {
                // Önceki section'ı kaydet
                SaveCurrentEnum(result, currentRule, currentValues);
                
                // Yeni section'a geç
                currentRule = null;
                currentValues = null;

                // Bu section bizim rule'larımızdan birine uyuyor mu?
                foreach (var rule in _rules)
                {
                    if (line.Contains(rule.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        currentRule = rule.Value;
                        currentValues = new List<string>();
                        break;
                    }
                }
                continue;
            }

            // Aktif section varsa #define'ları topla
            if (currentRule != null && currentValues != null && line.StartsWith("#define"))
            {
                var name = ExtractDefineName(line);
                if (name != null && PassesFilter(name, currentRule.PrefixFilters))
                {
                    currentValues.Add(name);
                }
            }
        }

        // Son section'ı kaydet
        SaveCurrentEnum(result, currentRule, currentValues);

        return result;
    }

    private void SaveCurrentEnum(Dictionary<string, EnumEntry> result, EnumRule? rule, List<string>? values)
    {
        if (rule != null && values != null && values.Count > 0)
        {
            result[rule.Name] = new EnumEntry
            {
                Name = rule.Name,
                Type = rule.Type,
                Values = new List<string>(values)
            };
        }
    }
    
    private bool PassesFilter(string name, string[]? prefixFilters)
    {
        // Filtre yoksa hepsini kabul et
        if (prefixFilters == null || prefixFilters.Length == 0)
            return true;
        
        // En az bir prefix ile eşleşmeli
        foreach (var prefix in prefixFilters)
        {
            if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private bool IsSectionHeader(string line)
    {
        // /****** formatı (XFSAPI.H)
        if (line.StartsWith("/******"))
            return true;

        // /* ... */ formatı (XFSIDC.H)
        // Ama açıklama satırlarını (örn: /* Message-No = (WM_USER + No) */) hariç tut
        if (line.StartsWith("/*") && line.EndsWith("*/") && !line.StartsWith("/*="))
        {
            // Çok uzun satırlar section header değil (açıklama satırları)
            if (line.Length > 60)
                return false;
            
            // İçinde = veya ( varsa muhtemelen açıklama satırı
            var content = line.Substring(2, line.Length - 4).Trim();
            if (content.Contains("=") || content.Contains("("))
                return false;
            
            return true;
        }

        return false;
    }

    private string? ExtractDefineName(string line)
    {
        var match = Regex.Match(line, @"#define\s+([A-Z_][A-Z0-9_]*)\s+");
        return match.Success ? match.Groups[1].Value : null;
    }

    public class EnumEntry
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public List<string> Values { get; set; } = new();
    }
}

