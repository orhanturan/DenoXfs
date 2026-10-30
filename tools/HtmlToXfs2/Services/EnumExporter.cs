using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services;

/// <summary>
/// Enums.json dosyalarını C# enum dosyalarına dönüştürür.
/// </summary>
public class EnumExporter
{
    private readonly string _deviceName;
    private readonly Dictionary<string, ConstantEntry> _constants = new();
    private readonly Dictionary<string, ConstantEntry> _apiConstants = new();

    public EnumExporter(string deviceName)
    {
        _deviceName = deviceName.ToLower();
    }

    /// <summary>
    /// Enums.json dosyasını okur ve her enum için ayrı C# dosyası oluşturur.
    /// </summary>
    public void Export()
    {
        var enumsPath = $"int/json/{_deviceName}/Enums.json";
        var constantsPath = $"int/json/{_deviceName}/Constants.json";
        var outputDir = $"output/{_deviceName}/Enums";

        // Dizini oluştur
        Directory.CreateDirectory(outputDir);

        // Constants.json oku (değer çözümlemesi için)
        LoadConstants(constantsPath);

        // API constants'ı yükle (referans çözümlemesi için)
        if (_deviceName != "api")
        {
            LoadApiConstants();
        }

        // Enums.json oku
        if (!File.Exists(enumsPath))
        {
            Console.WriteLine($"  Warning: {enumsPath} not found");
            return;
        }

        var json = File.ReadAllText(enumsPath);
        var enums = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        if (enums == null || enums.Count == 0)
        {
            Console.WriteLine($"  No enums found in {enumsPath}");
            return;
        }

        int exportedCount = 0;
        foreach (var kvp in enums)
        {
            var enumName = kvp.Key;
            var enumData = kvp.Value;

            var enumEntry = new EnumEntry
            {
                Name = enumData.GetProperty("Name").GetString() ?? enumName,
                OriginalName = enumData.GetProperty("OriginalName").GetString() ?? "",
                Type = enumData.GetProperty("Type").GetString() ?? "DWORD",
                Values = new List<string>()
            };

            var valuesArray = enumData.GetProperty("Values");
            foreach (var val in valuesArray.EnumerateArray())
            {
                enumEntry.Values.Add(val.GetString() ?? "");
            }

            // Enum dosyası oluştur
            var outputPath = Path.Combine(outputDir, $"{enumEntry.Name}.txt");
            var content = GenerateEnumFile(enumEntry);
            File.WriteAllText(outputPath, content);
            exportedCount++;
        }

        Console.WriteLine($"  {exportedCount} enum(s) exported to {outputDir}");
    }

    private void LoadConstants(string path)
    {
        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var constants = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        if (constants == null) return;

        foreach (var kvp in constants)
        {
            var entry = new ConstantEntry
            {
                OriginalName = kvp.Key,
                Value = kvp.Value.GetProperty("Value").GetString() ?? "",
                DataType = kvp.Value.GetProperty("DataType").GetString() ?? ""
            };
            _constants[kvp.Key] = entry;
        }
    }

    private void LoadApiConstants()
    {
        var apiPath = "int/json/api/Constants.json";
        if (!File.Exists(apiPath)) return;

        var json = File.ReadAllText(apiPath);
        var constants = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        if (constants == null) return;

        foreach (var kvp in constants)
        {
            var entry = new ConstantEntry
            {
                OriginalName = kvp.Key,
                Value = kvp.Value.GetProperty("Value").GetString() ?? "",
                DataType = kvp.Value.GetProperty("DataType").GetString() ?? ""
            };
            _apiConstants[kvp.Key] = entry;
        }
    }

    private string GenerateEnumFile(EnumEntry enumEntry)
    {
        var sb = new StringBuilder();

        // Namespace - API için base, device'lar için alt namespace
        if (_deviceName == "api")
        {
            sb.AppendLine("namespace DenoXfs.Core.Models.Enums;");
        }
        else
        {
            var deviceCapitalized = char.ToUpper(_deviceName[0]) + _deviceName.Substring(1).ToLower();
            sb.AppendLine($"namespace DenoXfs.Core.Models.Enums.{deviceCapitalized};");
        }
        sb.AppendLine();

        // Flags tespiti
        bool isFlags = IsFlagsEnum(enumEntry);

        if (isFlags)
        {
            sb.AppendLine("[Flags]");
        }

        // Enum tanımı
        var baseType = GetBaseType(enumEntry.Type);
        sb.AppendLine($"public enum {enumEntry.Name} : {baseType}");
        sb.AppendLine("{");

        // Common prefix'i hesapla (NOTSUPP değerleri hariç)
        var commonPrefix = FindCommonPrefix(enumEntry.Values);

        // Enum değerleri
        for (int i = 0; i < enumEntry.Values.Count; i++)
        {
            var valueName = enumEntry.Values[i];
            var convertedName = ConvertEnumValueName(valueName, commonPrefix);
            var resolvedValue = ResolveEnumValue(valueName);

            if (resolvedValue == null)
            {
                Console.WriteLine($"  Warning: Could not resolve value for {valueName}, skipping");
                continue;
            }

            sb.Append($"    {convertedName} = {resolvedValue}");

            // Son değer değilse virgül ekle
            if (i < enumEntry.Values.Count - 1)
            {
                sb.Append(",");
            }

            sb.AppendLine();
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private bool IsFlagsEnum(EnumEntry enumEntry)
    {
        // Sadece 0x (hex) değeri olan enumlara [Flags] ekle
        foreach (var valueName in enumEntry.Values)
        {
            var resolved = ResolveEnumValue(valueName);
            if (resolved != null && resolved.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private string GetBaseType(string type)
    {
        return type.ToUpper() switch
        {
            "WORD" => "ushort",
            "DWORD" => "uint",
            "HRESULT" => "int",
            "CHAR" => "byte",
            _ => "uint"
        };
    }

    private string FindCommonPrefix(List<string> values)
    {
        if (values == null || values.Count == 0)
            return "";

        // NOTSUPP ve UNKNOWN değerlerini hariç tut (case-insensitive)
        var filteredValues = values.Where(v => 
            !v.Contains("NOTSUPP", StringComparison.OrdinalIgnoreCase) &&
            !v.Contains("UNKNOWN", StringComparison.OrdinalIgnoreCase)
        ).ToList();

        if (filteredValues.Count == 0)
            return "";

        if (filteredValues.Count == 1)
            return "";

        // İlk değeri prefix olarak al
        var prefix = filteredValues[0];

        // Diğer değerlerle karşılaştır ve karakter bazlı ortak prefix'i bul
        for (int i = 1; i < filteredValues.Count; i++)
        {
            var currentValue = filteredValues[i];
            var minLength = Math.Min(prefix.Length, currentValue.Length);
            
            int commonLength = 0;
            for (int j = 0; j < minLength; j++)
            {
                if (char.ToUpperInvariant(prefix[j]) == char.ToUpperInvariant(currentValue[j]))
                {
                    commonLength++;
                }
                else
                {
                    break;
                }
            }

            // Prefix'i ortak uzunluğa kısalt
            if (commonLength < prefix.Length)
            {
                prefix = prefix.Substring(0, commonLength);
            }
        }

        // Son underscore sonrası 1-2 karakter varsa, prefix'i son underscore'a kadar geri çek
        // Örnek: WFS_IDC_DIP_E → WFS_IDC_DIP_
        if (!string.IsNullOrEmpty(prefix))
        {
            var lastUnderscore = prefix.LastIndexOf('_');
            if (lastUnderscore >= 0)
            {
                var afterUnderscore = prefix.Length - lastUnderscore - 1;
                if (afterUnderscore > 0 && afterUnderscore <= 2)
                {
                    prefix = prefix.Substring(0, lastUnderscore + 1);
                }
            }
        }

        return prefix;
    }

    private string ConvertEnumValueName(string originalName, string commonPrefix)
    {
        var name = originalName;

        // 1. Common prefix ile başlıyorsa → prefix'i sil
        if (!string.IsNullOrEmpty(commonPrefix) && name.StartsWith(commonPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var withoutPrefix = name.Substring(commonPrefix.Length);
            
            // Prefix silindikten sonra boş veya rakamla başlıyorsa → silme işlemini geri al
            if (!string.IsNullOrEmpty(withoutPrefix) && !char.IsDigit(withoutPrefix[0]))
            {
                name = withoutPrefix;
            }
        }
        // 2. Common prefix ile başlamıyorsa (NOTSUPP/UNKNOWN gibi) → en azından WFS_{DEVICE}_ kısmını sil
        else
        {
            // WFS_ prefix'ini kaldır
            if (name.StartsWith("WFS_", StringComparison.OrdinalIgnoreCase))
            {
                name = name.Substring(4);
            }

            // Device prefix'ini kaldır (IDC_, vb.)
            var deviceUpper = _deviceName.ToUpper();
            if (_deviceName != "api")
            {
                if (name.StartsWith($"{deviceUpper}_", StringComparison.OrdinalIgnoreCase))
                {
                    name = name.Substring(deviceUpper.Length + 1);
                }
            }
        }

        // Underscore ile başlıyorsa kaldır
        name = name.TrimStart('_');

        // 3. PascalCase'e çevir
        return NameConverter.ToPascalCase(name);
    }

    private string? ResolveEnumValue(string valueName)
    {
        // Önce kendi constants'ında ara
        if (_constants.TryGetValue(valueName, out var entry))
        {
            return ResolveConstantValue(entry);
        }

        // API constants'ında ara
        if (_apiConstants.TryGetValue(valueName, out entry))
        {
            return ResolveConstantValue(entry);
        }

        Console.WriteLine($"  Warning: Could not find constant: {valueName}");
        return null;
    }

    private string? ResolveConstantValue(ConstantEntry entry)
    {
        var value = entry.Value;

        // Referans çözümle (örn: WFS_STAT_DEVONLINE)
        if (IsReference(value))
        {
            var resolved = ResolveReference(value);
            if (resolved == null) return null;
            value = resolved;
        }

        // Matematiksel ifade çözümle (örn: WM_USER + 1)
        if (IsExpression(value))
        {
            var resolved = ResolveExpression(value);
            if (resolved == null) return null;
            value = resolved;
        }

        // Hex formatla
        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            return value.ToLower();
        }

        // Numeric
        if (int.TryParse(value, out _))
        {
            return value;
        }

        return value;
    }

    private bool IsReference(string value)
    {
        return Regex.IsMatch(value, @"^[A-Z_][A-Z0-9_]*$") && !IsNumeric(value);
    }

    private bool IsExpression(string value)
    {
        return value.Contains("+") || value.Contains("-") || value.Contains("*");
    }

    private bool IsNumeric(string value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) return true;
        if (value.StartsWith("-")) return int.TryParse(value, out _);
        return int.TryParse(value, out _);
    }

    private string? ResolveReference(string refName)
    {
        if (_constants.TryGetValue(refName, out var entry))
        {
            if (IsNumeric(entry.Value))
                return entry.Value;
            return ResolveReference(entry.Value);
        }

        if (_apiConstants.TryGetValue(refName, out entry))
        {
            if (IsNumeric(entry.Value))
                return entry.Value;
            return ResolveReference(entry.Value);
        }

        return null;
    }

    private string? ResolveExpression(string expression)
    {
        var match = Regex.Match(expression, @"([A-Z_][A-Z0-9_]*)\s*([+\-*])\s*(\d+)");
        if (!match.Success) return null;

        var refName = match.Groups[1].Value;
        var op = match.Groups[2].Value;
        var operand = int.Parse(match.Groups[3].Value);

        var refValue = ResolveReference(refName);
        if (refValue == null) return null;

        int baseValue;
        if (refValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            baseValue = Convert.ToInt32(refValue, 16);
        }
        else
        {
            baseValue = int.Parse(refValue);
        }

        var result = op switch
        {
            "+" => baseValue + operand,
            "-" => baseValue - operand,
            "*" => baseValue * operand,
            _ => baseValue
        };

        return result.ToString();
    }

    private class ConstantEntry
    {
        public string OriginalName { get; set; } = "";
        public string Value { get; set; } = "";
        public string DataType { get; set; } = "";
    }

    private class EnumEntry
    {
        public string Name { get; set; } = "";
        public string OriginalName { get; set; } = "";
        public string Type { get; set; } = "";
        public List<string> Values { get; set; } = new();
    }
}

