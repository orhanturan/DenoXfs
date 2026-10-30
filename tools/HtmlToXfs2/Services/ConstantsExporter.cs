using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services;

/// <summary>
/// Constants.json ve HeaderEnums.json dosyalarını C# static class dosyalarına dönüştürür.
/// </summary>
public class ConstantsExporter
{
    private readonly string _deviceName;
    private readonly Dictionary<string, ConstantEntry> _constants = new();
    private readonly Dictionary<string, ConstantEntry> _apiConstants = new();
    private readonly Dictionary<string, HeaderEnumEntry> _headerEnums = new();

    public ConstantsExporter(string deviceName)
    {
        _deviceName = deviceName.ToLower();
    }
    
    /// <summary>
    /// Constants.json ve HeaderEnums.json dosyalarını okur ve C# dosyası oluşturur.
    /// </summary>
    public void Export()
    {
        var constantsPath = $"int/json/{_deviceName}/Constants.json";
        var headerEnumsPath = $"int/json/{_deviceName}/HeaderEnums.json";
        var outputDir = $"output/{_deviceName}/Constants";
        var className = GetClassName();
        var outputPath = Path.Combine(outputDir, $"{className}.txt");

        // Dizini oluştur
        Directory.CreateDirectory(outputDir);

        // Constants.json oku
        LoadConstants(constantsPath);

        // API constants'ı yükle (referans çözümlemesi için)
        if (_deviceName != "api")
        {
            LoadApiConstants();
        }

        // HeaderEnums.json oku
        LoadHeaderEnums(headerEnumsPath);

        // C# dosyası oluştur
        var content = GenerateCSharpFile(className);
        File.WriteAllText(outputPath, content);

        Console.WriteLine($"  Constants exported to: {outputPath}");
    }

    private void LoadConstants(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"  Warning: {path} not found");
            return;
        }

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

    private void LoadHeaderEnums(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"  Warning: {path} not found");
            return;
        }

        var json = File.ReadAllText(path);
        var enums = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        if (enums == null) return;

        foreach (var kvp in enums)
        {
            var entry = new HeaderEnumEntry
            {
                Name = kvp.Value.GetProperty("Name").GetString() ?? "",
                Type = kvp.Value.GetProperty("Type").GetString() ?? "",
                Values = new List<string>()
            };

            var valuesArray = kvp.Value.GetProperty("Values");
            foreach (var val in valuesArray.EnumerateArray())
            {
                entry.Values.Add(val.GetString() ?? "");
            }

            _headerEnums[kvp.Key] = entry;
        }
    }

    private string GetClassName()
    {
        return _deviceName == "api" ? "Xfs" : Capitalize(_deviceName);
    }

    private string GenerateCSharpFile(string className)
    {
        var sb = new StringBuilder();

        var ns = _deviceName == "api" ? "DenoXfs.Core.Api" : $"DenoXfs.{Capitalize(_deviceName)}.Api";
        sb.AppendLine($"namespace {ns};");
        sb.AppendLine();

        if (_deviceName == "api")
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine("/// XFS (eXtensions for Financial Services) sabit değerleri.");
            sb.AppendLine("/// </summary>");
        }

        sb.AppendLine($"public static class {className}");
        sb.AppendLine("{");

        // Yazılan constant isimlerini takip et (duplicate kontrolü için)
        var writtenConstants = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Constants.json'dan gelen sabitler
        foreach (var kvp in _constants)
        {
            var entry = kvp.Value;
            var name = ConvertName(entry.OriginalName);
            var (type, value) = ResolveTypeAndValue(entry);

            if (value == null) continue; // Çözümlenemeyenler atlanır

            // Duplicate kontrolü: eğer bu constant zaten yazıldıysa atla
            if (writtenConstants.Contains(name))
                continue;

            sb.AppendLine($"    public const {type} {name} = {value};");
            writtenConstants.Add(name);
        }

        // HeaderEnums.json'dan gelen enum değerleri
        if (_headerEnums.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("    // Header Enums");
            
            foreach (var enumEntry in _headerEnums.Values)
            {
                sb.AppendLine();
                sb.AppendLine($"    // {enumEntry.Name}");
                
                foreach (var valueName in enumEntry.Values)
                {
                    var convertedName = ConvertName(valueName);
                    
                    // Duplicate kontrolü: eğer bu constant zaten yazıldıysa atla
                    if (writtenConstants.Contains(convertedName))
                        continue;
                    
                    var resolvedValue = ResolveEnumValue(valueName);
                    
                    if (resolvedValue != null)
                    {
                        var type = DetermineType(enumEntry.Type, resolvedValue);
                        var formattedValue = FormatValue(resolvedValue, type);
                        sb.AppendLine($"    public const {type} {convertedName} = {formattedValue};");
                        writtenConstants.Add(convertedName);
                    }
                }
            }
        }

        // Platform constants (not in XFS spec, required by the runtime)
        if (_deviceName == "api")
        {
            sb.AppendLine();
            sb.AppendLine("    // Platform");
            sb.AppendLine("    public const uint WmXfsProcessRequest = 0x8001u;");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private string? ResolveEnumValue(string valueName)
    {
        // Önce kendi constants'ında ara
        if (_constants.TryGetValue(valueName, out var entry))
        {
            var (_, value) = ResolveTypeAndValue(entry);
            return value;
        }

        // API constants'ında ara
        if (_apiConstants.TryGetValue(valueName, out entry))
        {
            var (_, value) = ResolveTypeAndValue(entry);
            return value;
        }

        Console.WriteLine($"  Warning: Could not resolve enum value: {valueName}");
        return null;
    }

    private string ConvertName(string originalName)
    {
        // WFS_ prefix'ini kaldır
        var name = originalName;
        if (name.StartsWith("WFS_", StringComparison.OrdinalIgnoreCase))
        {
            name = name.Substring(4);
        }

        // Device prefix'ini kaldır (IDC_, CDM_, vb.)
        var deviceUpper = _deviceName.ToUpper() + "_";
        if (_deviceName != "api" && name.StartsWith(deviceUpper, StringComparison.OrdinalIgnoreCase))
        {
            name = name.Substring(deviceUpper.Length);
        }

        // UPPER_CASE → PascalCase (kelime bazlı ayrıştırma ile)
        return NameConverter.ToPascalCase(name);
    }

    private (string type, string? value) ResolveTypeAndValue(ConstantEntry entry)
    {
        var value = entry.Value;
        var dataType = entry.DataType;
        var isWmUserExpression = false;

        // Referans çözümle (örn: WFS_STAT_DEVONLINE)
        if (IsReference(value))
        {
            var resolved = ResolveReference(value);
            if (resolved == null) return ("int", null);
            value = resolved;
        }

        // Matematiksel ifade çözümle (örn: WM_USER + 1)
        if (IsExpression(value))
        {
            // WM_USER içeren expression'ları işaretle
            isWmUserExpression = value.Contains("WM_USER");
            
            var resolved = ResolveExpression(value);
            if (resolved == null) return ("int", null);
            value = resolved;
        }

        // Tip belirle - WM_USER expression'ları için uint (DWORD) kullan
        var type = isWmUserExpression ? "uint" : DetermineType(dataType, value);

        // Value formatla
        var formattedValue = FormatValue(value, type);

        return (type, formattedValue);
    }

    private bool IsReference(string value)
    {
        // Sadece harf ve alt çizgi içeriyorsa referans olabilir
        return Regex.IsMatch(value, @"^[A-Z_][A-Z0-9_]*$") && !IsNumeric(value);
    }

    private bool IsExpression(string value)
    {
        return value.Contains("+") || value.Contains("-") || value.Contains("*");
    }

    private bool IsNumeric(string value)
    {
        if (string.IsNullOrEmpty(value)) return false;

        // Hex
        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return true;

        // Negatif sayı
        if (value.StartsWith("-"))
            return int.TryParse(value, out _);

        return int.TryParse(value, out _);
    }

    private string? ResolveReference(string refName)
    {
        // Windows API sabitleri
        if (refName == "WM_USER")
            return "1024";
        
        // Önce kendi constants'ında ara
        if (_constants.TryGetValue(refName, out var entry))
        {
            if (IsNumeric(entry.Value))
                return entry.Value;
            // Recursive çözümle
            return ResolveReference(entry.Value);
        }

        // API constants'ında ara
        if (_apiConstants.TryGetValue(refName, out entry))
        {
            if (IsNumeric(entry.Value))
                return entry.Value;
            return ResolveReference(entry.Value);
        }

        Console.WriteLine($"  Warning: Could not resolve reference: {refName}");
        return null;
    }

    private string? ResolveExpression(string expression)
    {
        // WM_USER + 1 gibi ifadeleri çözümle
        var match = Regex.Match(expression, @"([A-Z_][A-Z0-9_]*)\s*([+\-*])\s*(\d+)");
        if (!match.Success) return null;

        var refName = match.Groups[1].Value;
        var op = match.Groups[2].Value;
        var operand = int.Parse(match.Groups[3].Value);

        var refValue = ResolveReference(refName);
        if (refValue == null) return null;

        // Hex'i int'e çevir
        int baseValue;
        if (refValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            baseValue = Convert.ToInt32(refValue, 16);
        }
        else
        {
            baseValue = int.Parse(refValue);
        }

        // İşlemi uygula
        var result = op switch
        {
            "+" => baseValue + operand,
            "-" => baseValue - operand,
            "*" => baseValue * operand,
            _ => baseValue
        };

        return result.ToString();
    }

    private string DetermineType(string dataType, string value)
    {
        // DataType belirtilmişse
        if (!string.IsNullOrEmpty(dataType))
        {
            return dataType.ToUpper() switch
            {
                "DWORD" => "int",
                "HRESULT" => "int",
                "WORD" => "ushort",
                "CHAR" => "char",
                _ => "int"
            };
        }

        // Hex değer
        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            return "int";
        }

        // Numeric
        if (IsNumeric(value))
        {
            return "int";
        }

        return "int";
    }

    private string? FormatValue(string value, string type)
    {
        // Hex değer
        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            return value.ToLower();
        }

        // Char tipi
        if (type == "char")
        {
            if (value.Length == 1)
                return $"'{value}'";
            return $"'{value[0]}'";
        }

        // Numeric
        return value;
    }

    private string Capitalize(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return char.ToUpper(text[0]) + text.Substring(1).ToLower();
    }

    public class ConstantEntry
    {
        public string OriginalName { get; set; } = "";
        public string Value { get; set; } = "";
        public string DataType { get; set; } = "";
    }

    private class HeaderEnumEntry
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public List<string> Values { get; set; } = new();
    }
}
