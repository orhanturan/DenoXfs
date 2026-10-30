using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services;

/// <summary>
/// Structs.json dosyalarını C# struct dosyalarına dönüştürür.
/// </summary>
public class StructExporter
{
    private readonly string _deviceName;
    private readonly Dictionary<string, ConstantsExporter.ConstantEntry> _constants = new();
    private readonly Dictionary<string, ConstantsExporter.ConstantEntry> _apiConstants = new();
    private readonly Dictionary<string, string> _structNames = new(); // Pointer type -> Struct name mapping

    public StructExporter(string deviceName)
    {
        _deviceName = deviceName.ToLower();
    }

    /// <summary>
    /// Structs.json dosyasını okur ve her struct için ayrı C# dosyası oluşturur.
    /// </summary>
    public void Export()
    {
        var structsPath = $"int/json/{_deviceName}/Structs.json";
        var constantsPath = $"int/json/{_deviceName}/Constants.json";
        var outputDir = $"output/{_deviceName}/Structs";

        // Dizini oluştur
        Directory.CreateDirectory(outputDir);

        // Constants.json oku (SizeConst çözümlemesi için)
        LoadConstants(constantsPath);

        // API constants'ı yükle (referans çözümlemesi için)
        if (_deviceName != "api")
        {
            LoadApiConstants();
        }

        // Structs.json oku
        if (!File.Exists(structsPath))
        {
            Console.WriteLine($"  Warning: {structsPath} not found");
            return;
        }

        var json = File.ReadAllText(structsPath);
        var structs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        if (structs == null || structs.Count == 0)
        {
            Console.WriteLine($"  No structs found in {structsPath}");
            return;
        }

        // Build pointer type -> struct name mapping
        BuildStructNameMapping(structs);

        int exportedCount = 0;
        foreach (var kvp in structs)
        {
            var structName = kvp.Key;
            var structFields = kvp.Value;

            // Struct dosyası oluştur
            var outputPath = Path.Combine(outputDir, $"{structName}.txt");
            var content = GenerateStructFile(structName, structFields);
            File.WriteAllText(outputPath, content);
            exportedCount++;
        }

        Console.WriteLine($"  {exportedCount} struct(s) exported to {outputDir}");
    }

    private void LoadConstants(string path)
    {
        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var constants = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        if (constants == null) return;

        foreach (var kvp in constants)
        {
            var entry = new ConstantsExporter.ConstantEntry
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
        var apiConstantsPath = "int/json/api/Constants.json";
        if (!File.Exists(apiConstantsPath)) return;

        var json = File.ReadAllText(apiConstantsPath);
        var constants = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        if (constants == null) return;

        foreach (var kvp in constants)
        {
            var entry = new ConstantsExporter.ConstantEntry
            {
                OriginalName = kvp.Key,
                Value = kvp.Value.GetProperty("Value").GetString() ?? "",
                DataType = kvp.Value.GetProperty("DataType").GetString() ?? ""
            };
            _apiConstants[kvp.Key] = entry;
        }
    }

    private void BuildStructNameMapping(Dictionary<string, JsonElement> structs)
    {
        // For each struct, create mapping from pointer type to struct name
        // Example: XfsIdcHexData -> LPWFSIDCHEXDATA
        foreach (var kvp in structs)
        {
            var structName = kvp.Key;
            
            // Convert struct name to pointer type
            // XfsIdcHexData -> XFSIDCHEXDATA -> IDCHEXDATA -> WFSIDCHEXDATA -> LPWFSIDCHEXDATA
            var upperName = structName.ToUpper();
            if (upperName.StartsWith("XFS"))
            {
                var withoutXfs = upperName.Substring(3); // Remove "XFS" -> IDCHEXDATA
                var pointerType = "LPWFS" + withoutXfs; // LPWFSIDCHEXDATA
                _structNames[pointerType] = structName;
            }
        }

        // Also scan all fields to find pointer types and map them to struct names
        // This handles cases where we have LPWFS... in fields but need to find the struct
        foreach (var kvp in structs)
        {
            var structFields = kvp.Value;
            
            foreach (var field in structFields.EnumerateObject())
            {
                var fieldType = field.Value.GetString() ?? "";
                
                // If field type is a pointer type (LPWFS...), try to find matching struct
                if (fieldType.StartsWith("LPWFS", StringComparison.OrdinalIgnoreCase) && !_structNames.ContainsKey(fieldType))
                {
                    // Try to convert pointer type to struct name
                    // LPWFSIDCHEXDATA -> WFSIDCHEXDATA -> XFSIDCHEXDATA -> XfsIdcHexData
                    var withoutLP = fieldType.Substring(2); // Remove "LP" -> WFSIDCHEXDATA
                    var withXfs = "XFS" + withoutLP.Substring(3); // WFS -> XFS -> XFSIDCHEXDATA
                    
                    // Convert to struct name: XFSIDCHEXDATA -> XfsIdcHexData
                    var convertedName = ConvertPointerTypeToStructName(withXfs);
                    
                    // Check if this struct exists
                    if (structs.ContainsKey(convertedName))
                    {
                        _structNames[fieldType] = convertedName;
                    }
                }
            }
        }
    }

    private string ConvertPointerTypeToStructName(string upperName)
    {
        // XFSIDCHEXDATA -> XfsIdcHexData
        // Insert underscores before capital letters
        var withUnderscores = Regex.Replace(upperName, @"([A-Z])([A-Z][a-z])", "$1_$2");
        withUnderscores = Regex.Replace(withUnderscores, @"([a-z\d])([A-Z])", "$1_$2");
        
        // Convert to lowercase with underscores: xfs_idc_hex_data
        var lowerWithUnderscores = withUnderscores.ToLower();
        
        // Use ConvertStructName which expects wfs_ or xfs_ prefix
        return NameConverter.ConvertStructName(lowerWithUnderscores);
    }

    private string GenerateStructFile(string structName, JsonElement structFields)
    {
        // XfsResult için özel generate (union içerdiği için FieldOffset gerekli)
        if (structName == "XfsResult")
        {
            return GenerateXfsResultStruct();
        }
        
        var sb = new StringBuilder();

        // Collect all fields first (needed to determine required usings)
        var fieldList = new List<(string fieldName, string fieldType, ParsedFieldType parsedType)>();
        foreach (var field in structFields.EnumerateObject())
        {
            var fieldName = field.Name;
            var fieldType = field.Value.GetString() ?? "";
            var parsedType = ParseFieldType(fieldType, fieldName, structFields);
            fieldList.Add((fieldName, fieldType, parsedType));
        }

        // Determine required using directives
        bool needsConvertUsing = fieldList.Any(f => !string.IsNullOrEmpty(f.parsedType.GetterExpression));
        bool needsExtensionsUsing = fieldList.Any(f => f.parsedType.GetterExpression?.Contains(".ToStructSafe") == true);
        bool needsEnumUsing = _deviceName != "api" && fieldList.Any(f => IsEnumPrivateType(f.parsedType.PrivateType));

        // Using statements
        sb.AppendLine("using System.Runtime.InteropServices;");
        if (needsConvertUsing)
            sb.AppendLine("using DenoXfs.Core.Convert;");
        if (needsExtensionsUsing)
            sb.AppendLine("using DenoXfs.Core.Extensions;");
        if (needsEnumUsing)
            sb.AppendLine($"using DenoXfs.Core.Models.Enums.{NameConverter.CapitalizeFirstOnly(_deviceName)};");
        sb.AppendLine();

        // Namespace
        var namespaceLine = _deviceName == "api"
            ? "namespace DenoXfs.Core.Api;"
            : $"namespace DenoXfs.{NameConverter.CapitalizeFirstOnly(_deviceName)}.Models.Structs;";
        sb.AppendLine(namespaceLine);
        sb.AppendLine();

        // StructLayout attribute
        sb.AppendLine("[StructLayout(LayoutKind.Sequential, Pack = 1)]");
        sb.AppendLine($"public struct {structName}");
        sb.AppendLine("{");

        // First, generate all private fields
        foreach (var (fieldName, fieldType, parsedType) in fieldList)
        {
            GeneratePrivateField(sb, fieldName, parsedType);
        }

        sb.AppendLine();

        // Then, generate all public properties
        foreach (var (fieldName, fieldType, parsedType) in fieldList)
        {
            GeneratePublicProperty(sb, fieldName, parsedType);
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    private void GeneratePrivateField(StringBuilder sb, string fieldName, ParsedFieldType parsedType)
    {
        var privateFieldName = ToPrivateFieldName(fieldName);

        // Generate private field with MarshalAs attribute if needed
        if (!string.IsNullOrEmpty(parsedType.MarshalAsAttribute))
        {
            sb.AppendLine($"    {parsedType.MarshalAsAttribute}");
        }
        sb.AppendLine($"    private {parsedType.PrivateType} {privateFieldName};");
    }

    private void GeneratePublicProperty(StringBuilder sb, string fieldName, ParsedFieldType parsedType)
    {
        var privateFieldName = ToPrivateFieldName(fieldName);
        var publicPropertyName = ToPublicPropertyName(fieldName);

        // Generate public property
        sb.AppendLine($"    public {parsedType.PublicType} {publicPropertyName}");
        sb.AppendLine("    {");

        // Generate getter/setter
        if (!string.IsNullOrEmpty(parsedType.GetterExpression) && !string.IsNullOrEmpty(parsedType.SetterExpression))
        {
            // Custom getter/setter (pointer or dynamic array)
            sb.AppendLine($"        get => {parsedType.GetterExpression};");
            sb.AppendLine($"        set => {parsedType.SetterExpression};");
        }
        else
        {
            // Simple getter/setter (primitive or fixed array)
            sb.AppendLine($"        get => {privateFieldName};");
            sb.AppendLine($"        set => {privateFieldName} = value;");
        }

        sb.AppendLine("    }");
        sb.AppendLine();
    }

    private string ToPrivateFieldName(string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
            return "";

        // Make first letter lowercase for private field
        // RequestID -> requestID
        // hService -> hService (already lowercase)
        if (char.IsUpper(fieldName[0]))
        {
            return char.ToLower(fieldName[0]) + fieldName.Substring(1);
        }
        
        return fieldName;
    }

    private string ToPublicPropertyName(string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
            return "";

        // If starts with uppercase, use as-is
        // RequestID -> RequestID
        if (char.IsUpper(fieldName[0]))
        {
            return fieldName;
        }
        
        // If starts with lowercase, remove everything until first uppercase letter
        // hService -> Service
        // lpvOldStatus -> OldStatus
        // dwState -> State
        for (int i = 0; i < fieldName.Length; i++)
        {
            if (char.IsUpper(fieldName[i]))
            {
                return fieldName.Substring(i);
            }
        }
        
        // No uppercase found, capitalize first letter
        // status -> Status
        return char.ToUpper(fieldName[0]) + fieldName.Substring(1);
    }

    private ParsedFieldType ParseFieldType(string fieldType, string fieldName, JsonElement allFields)
    {
        var result = new ParsedFieldType();

        // Check for array types
        if (fieldType.Contains("[ZeroTerminated]"))
        {
            return ParseZeroTerminatedArray(fieldType, fieldName);
        }
        else if (fieldType.Contains("[") && fieldType.Contains("]"))
        {
            // Extract the value inside []
            var match = Regex.Match(fieldType, @"\[([^\]]+)\]");
            if (match.Success)
            {
                var arraySize = match.Groups[1].Value.Trim();
                
                // Check if it's numeric (decimal or hex)
                bool isNumeric = int.TryParse(arraySize, out _) || 
                                 arraySize.StartsWith("0x", StringComparison.OrdinalIgnoreCase);
                
                // Check if it's a known constant
                bool isConstant = _constants.ContainsKey(arraySize) || 
                                  _apiConstants.ContainsKey(arraySize);
                
                // Check if it's an expression with constants (e.g., "WFSDDESCRIPTION_LEN + 1")
                bool isExpression = arraySize.Contains("+") || arraySize.Contains("-");
                
                if (isNumeric || isConstant || isExpression)
                {
                    // Fixed size array (number, constant, or expression)
                    return ParseFixedSizeArray(fieldType, fieldName);
                }
                else
                {
                    // Dynamic array (variable name like ulChipDataLength)
                    return ParseDynamicArray(fieldType, fieldName, allFields);
                }
            }
        }

        // Check for pointer types
        if (fieldType.StartsWith("LP"))
        {
            return ParsePointerType(fieldType, fieldName);
        }

        // Check for enum types (not primitive types)
        if (IsEnumType(fieldType))
        {
            result.PrivateType = fieldType;
            result.PublicType = fieldType;
            result.IsPointer = false;
            result.IsArray = false;
            return result;
        }

        // Primitive types
        return ParsePrimitiveType(fieldType, fieldName);
    }

    private ParsedFieldType ParseZeroTerminatedArray(string fieldType, string fieldName)
    {
        var result = new ParsedFieldType();
        var baseType = fieldType.Replace("[ZeroTerminated]", "").Trim();

        // Check if it's LPSTR
        if (baseType == "LPSTR")
        {
            result.PrivateType = "IntPtr";
            result.PublicType = "string?[]?";
            result.IsPointer = true;
            result.GetterExpression = $"IntPtrConvert.ToZeroTerminatedArray({ToPrivateFieldName(fieldName)})";
            result.SetterExpression = $"ZeroTerminatedArrayConvert.ToIntPtr(ref {ToPrivateFieldName(fieldName)}, value)";
        }
        else
        {
            // Enum array
            result.PrivateType = "IntPtr";
            result.PublicType = $"{baseType}?[]?";
            result.IsPointer = true;
            result.GetterExpression = $"IntPtrConvert.ToZeroTerminatedArray<{baseType}>({ToPrivateFieldName(fieldName)})";
            result.SetterExpression = $"ZeroTerminatedArrayConvert.ToIntPtr(ref {ToPrivateFieldName(fieldName)}, value)";
        }

        return result;
    }

    private ParsedFieldType ParseFixedSizeArray(string fieldType, string fieldName)
    {
        var result = new ParsedFieldType();
        
        // Extract base type and size
        var match = Regex.Match(fieldType, @"^(.+)\[([^\]]+)\]$");
        if (!match.Success)
        {
            // Fallback to primitive
            return ParsePrimitiveType(fieldType, fieldName);
        }

        var baseType = match.Groups[1].Value.Trim();
        var sizeConstant = match.Groups[2].Value.Trim();

        // Resolve size constant
        var size = ResolveSizeConstant(sizeConstant);
        if (size == null)
        {
            // If can't resolve, use IntPtr
            result.PrivateType = "IntPtr";
            result.PublicType = $"{baseType}?[]?";
            result.IsPointer = true;
            result.GetterExpression = $"IntPtrConvert.ToArray<{baseType}>({ToPrivateFieldName(fieldName)}, {sizeConstant})";
            result.SetterExpression = $"ArrayConvert.ToIntPtr(ref {ToPrivateFieldName(fieldName)}, value)";
            return result;
        }

        // Fixed size array with MarshalAs
        result.PrivateType = $"{baseType}[]";
        result.PublicType = $"{baseType}[]";
        result.IsPointer = false;
        result.IsArray = true;
        result.MarshalAsAttribute = $"[MarshalAs(UnmanagedType.ByValArray, SizeConst = {size})]";
        // Empty expressions - will use simple getter/setter in GeneratePublicProperty
        result.GetterExpression = "";
        result.SetterExpression = "";

        return result;
    }

    private ParsedFieldType ParseDynamicArray(string fieldType, string fieldName, JsonElement allFields)
    {
        var result = new ParsedFieldType();
        
        // Extract: *BYTE[usKeyLen] or BYTE[usKeyLen] -> baseType=BYTE, countField=usKeyLen
        // Support both formats: with or without leading *
        var match = Regex.Match(fieldType, @"^\*?(\w+)\[(\w+)\]$");
        if (!match.Success)
        {
            // Fallback
            result.PrivateType = "IntPtr";
            result.PublicType = "byte[]?";
            result.IsPointer = true;
            result.GetterExpression = $"IntPtrConvert.ToByteArray({ToPrivateFieldName(fieldName)})";
            result.SetterExpression = $"ByteArrayConvert.ToIntPtr(ref {ToPrivateFieldName(fieldName)}, value)";
            return result;
        }

        var baseTypeStr = match.Groups[1].Value;
        var countFieldName = match.Groups[2].Value;

        // Find count field in struct
        var countField = FindCountField(countFieldName, allFields);
        if (countField == null)
        {
            // Fallback
            result.PrivateType = "IntPtr";
            result.PublicType = "byte[]?";
            result.IsPointer = true;
            result.GetterExpression = $"IntPtrConvert.ToByteArray({ToPrivateFieldName(fieldName)})";
            result.SetterExpression = $"ByteArrayConvert.ToIntPtr(ref {ToPrivateFieldName(fieldName)}, value)";
            return result;
        }

        var csharpBaseType = ConvertBaseType(baseTypeStr);
        var countPrivateName = ToPrivateFieldName(countField);

        // Determine count field's C# type (ushort vs uint) so the setter resolves correctly
        string? countFieldType = null;
        foreach (var f in allFields.EnumerateObject())
        {
            if (f.Name == countField)
            {
                countFieldType = ConvertBaseType(f.Value.GetString() ?? "");
                break;
            }
        }

        result.PrivateType = "IntPtr";
        result.PublicType = $"{csharpBaseType}[]?";
        result.IsPointer = true;

        if (csharpBaseType == "byte")
        {
            // Use ToByteArray(ptr, count) for byte arrays — avoids byte?[] vs byte[] mismatch
            result.GetterExpression = $"IntPtrConvert.ToByteArray({ToPrivateFieldName(fieldName)}, (int){countPrivateName})";
            result.SetterExpression = $"DynamicArrayConvert.ToIntPtr(ref {ToPrivateFieldName(fieldName)}, value, out {countPrivateName})";
        }
        else
        {
            result.GetterExpression = $"IntPtrConvert.ToDynamicArray<{csharpBaseType}>({ToPrivateFieldName(fieldName)}, {countPrivateName})";
            result.SetterExpression = $"DynamicArrayConvert.ToIntPtr(ref {ToPrivateFieldName(fieldName)}, value, out {countPrivateName})";
        }

        return result;
    }

    private string? FindCountField(string countFieldName, JsonElement allFields)
    {
        // First, try exact match (for original field names like ulChipDataLength)
        foreach (var field in allFields.EnumerateObject())
        {
            if (field.Name.Equals(countFieldName, StringComparison.OrdinalIgnoreCase))
            {
                return field.Name;
            }
        }
        
        // If not found, try without prefix (us, ul, dw, etc.)
        var withoutPrefix = countFieldName;
        if (countFieldName.StartsWith("us", StringComparison.OrdinalIgnoreCase))
        {
            withoutPrefix = countFieldName.Substring(2);
        }
        else if (countFieldName.StartsWith("ul", StringComparison.OrdinalIgnoreCase))
        {
            withoutPrefix = countFieldName.Substring(2);
        }
        else if (countFieldName.StartsWith("dw", StringComparison.OrdinalIgnoreCase))
        {
            withoutPrefix = countFieldName.Substring(2);
        }

        // Capitalize first letter
        var searchName = char.ToUpper(withoutPrefix[0]) + withoutPrefix.Substring(1);

        // Search in struct fields
        foreach (var field in allFields.EnumerateObject())
        {
            if (field.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase))
            {
                return field.Name;
            }
        }

        return null;
    }

    private ParsedFieldType ParsePointerType(string fieldType, string fieldName)
    {
        var result = new ParsedFieldType();
        result.PrivateType = "IntPtr";
        result.IsPointer = true;

        var privateFieldName = ToPrivateFieldName(fieldName);

        if (fieldType == "LPSTR")
        {
            result.PublicType = "string?";
            result.GetterExpression = $"IntPtrConvert.ToString({privateFieldName})";
            result.SetterExpression = $"StringConvert.ToIntPtr(ref {privateFieldName}, value)";
        }
        else if (fieldType == "LPWSTR")
        {
            result.PublicType = "string?";
            result.GetterExpression = $"IntPtrConvert.ToWString({privateFieldName})";
            result.SetterExpression = $"WStringConvert.ToIntPtr(ref {privateFieldName}, value)";
        }
        else if (fieldType == "LPBYTE")
        {
            result.PublicType = "byte[]?";
            result.GetterExpression = $"IntPtrConvert.ToByteArray({privateFieldName})";
            result.SetterExpression = $"ByteArrayConvert.ToIntPtr(ref {privateFieldName}, value)";
        }
        else if (fieldType == "LPDWORD")
        {
            result.PublicType = "uint[]?";
            result.GetterExpression = $"IntPtrConvert.ToUInt32Array({privateFieldName})";
            result.SetterExpression = $"UInt32ArrayConvert.ToIntPtr(ref {privateFieldName}, value)";
        }
        else if (fieldType == "LPVOID")
        {
            result.PublicType = "IntPtr";
            result.GetterExpression = privateFieldName;
            result.SetterExpression = $"{privateFieldName} = value";
        }
        else if (fieldType.StartsWith("LPWFS"))
        {
            // Nested struct pointer: LPWFSVERSION -> XfsVersion
            var structName = ConvertStructPointerToName(fieldType);
            result.PublicType = $"{structName}?";
            result.GetterExpression = $"{privateFieldName}.ToStructSafe<{structName}>()";
            result.SetterExpression = $"StructureConvert.ToIntPtr(ref {privateFieldName}, value)";
        }
        else
        {
            // Unknown pointer type, default to IntPtr
            result.PublicType = "IntPtr";
            result.GetterExpression = privateFieldName;
            result.SetterExpression = $"{privateFieldName} = value";
        }

        return result;
    }

    private string ConvertStructPointerToName(string pointerType)
    {
        // First, try to find in struct names mapping (from Structs.json)
        if (_structNames.TryGetValue(pointerType, out var structName))
        {
            return structName;
        }

        // Fallback: convert using pattern matching
        // LPWFSVERSION -> XfsVersion
        // LPWFSIDCHEXDATA -> XfsIdcHexData
        // LPWFSVENDORMODEINFO -> XfsVendorModeInfo

        if (!pointerType.StartsWith("LPWFS"))
            return pointerType;

        var name = pointerType.Substring(2); // Remove LP -> WFSIDCHEXDATA
        
        // Convert WFS -> XFS (first 3 chars)
        if (name.StartsWith("WFS", StringComparison.OrdinalIgnoreCase))
        {
            name = "XFS" + name.Substring(3); // XFSIDCHEXDATA
        }
        
        // Now convert using NameConverter: XFSIDCHEXDATA -> XfsIdcHexData
        // First, insert underscores before capital letters (except first)
        var withUnderscores = Regex.Replace(name, @"([A-Z])([A-Z][a-z])", "$1_$2");
        withUnderscores = Regex.Replace(withUnderscores, @"([a-z\d])([A-Z])", "$1_$2");
        
        // Convert to lowercase with underscores: XFS_IDC_HEX_DATA
        var lowerWithUnderscores = withUnderscores.ToLower(); // xfs_idc_hex_data
        
        // Now use ConvertStructName which expects wfs_ or wfs prefix
        return NameConverter.ConvertStructName(lowerWithUnderscores);
    }

    private ParsedFieldType ParsePrimitiveType(string fieldType, string fieldName)
    {
        var result = new ParsedFieldType();
        var csharpType = ConvertBaseType(fieldType);
        result.PrivateType = csharpType;
        result.PublicType = csharpType;
        result.IsPointer = false;
        result.IsArray = false;

        // Windows BOOL is 4 bytes; C# bool defaults to 1 byte in struct layout
        if (fieldType.Equals("BOOL", StringComparison.OrdinalIgnoreCase))
            result.MarshalAsAttribute = "[MarshalAs(UnmanagedType.Bool)]";

        return result;
    }

    private string ConvertBaseType(string type)
    {
        return type.ToUpper() switch
        {
            // Basic types
            "WORD" => "ushort",
            "USHORT" => "ushort",
            "SHORT" => "short",
            "DWORD" => "uint",
            "ULONG" => "uint",  // ULONG is typically 32-bit in Windows API
            "LONG" => "int",
            "BOOL" => "bool",
            "CHAR" => "byte",   // C char is typically byte in C#
            "BYTE" => "byte",
            "UCHAR" => "byte",
            "INT" => "int",
            "UINT" => "uint",
            
            // Windows types
            "SYSTEMTIME" => "SystemTime",
            "HRESULT" => "int",       // HRESULT is a 32-bit signed integer
            "REQUESTID" => "uint",    // REQUESTID is typically ULONG
            "HSERVICE" => "ushort",   // HSERVICE is typically USHORT
            "HANDLE" => "IntPtr",
            "HWND" => "IntPtr",
            "LPARAM" => "IntPtr",
            "WPARAM" => "IntPtr",
            
            _ => type // Enum or unknown type
        };
    }

    private bool IsEnumPrivateType(string privateType)
    {
        if (string.IsNullOrEmpty(privateType)) return false;
        var csharpPrimitives = new HashSet<string>(StringComparer.Ordinal)
        {
            "ushort", "uint", "int", "short", "bool", "byte", "IntPtr", "SystemTime"
        };
        if (csharpPrimitives.Contains(privateType)) return false;
        if (privateType.EndsWith("[]"))
        {
            // e.g. "IdcGuidLightsCapabilities[]" — check the base type
            var baseType = privateType.Substring(0, privateType.Length - 2);
            if (string.IsNullOrEmpty(baseType) || char.IsLower(baseType[0])) return false;
            if (csharpPrimitives.Contains(baseType)) return false;
            return true;
        }
        if (char.IsLower(privateType[0])) return false; // primitive or unknown
        return true;
    }

    private bool IsEnumType(string type)
    {
        // Check if it's a known enum type (starts with capital letter and not a primitive)
        if (string.IsNullOrEmpty(type))
            return false;

        var upperType = type.ToUpper();
        
        // Known primitive and Windows types (not enums)
        var knownTypes = new[] { 
            // Basic types
            "WORD", "USHORT", "SHORT", "DWORD", "ULONG", "LONG", "BOOL", "CHAR", "BYTE", "UCHAR", "INT", "UINT",
            // Pointer types
            "LPSTR", "LPWSTR", "LPBYTE", "LPDWORD", "LPVOID", "LPLPVOID",
            // Windows types
            "SYSTEMTIME", "HRESULT", "REQUESTID", "HSERVICE", "HANDLE", "HWND", "LPARAM", "WPARAM"
        };
        
        if (knownTypes.Contains(upperType))
            return false;

        // If it starts with LP, it's a pointer
        if (type.StartsWith("LP", StringComparison.OrdinalIgnoreCase))
            return false;

        // If it starts with capital letter and not a known type, assume it's an enum
        return char.IsUpper(type[0]);
    }

    private string? ResolveSizeConstant(string constantName)
    {
        // Numeric literals (decimal or hex) resolve to themselves
        if (int.TryParse(constantName, out _) ||
            constantName.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return constantName;

        // Try device constants first
        if (_constants.TryGetValue(constantName, out var entry))
        {
            return entry.Value;
        }

        // Try API constants
        if (_apiConstants.TryGetValue(constantName, out var apiEntry))
        {
            return apiEntry.Value;
        }

        return null;
    }

    /// <summary>
    /// XfsResult struct'ı union içerdiği için FieldOffset ile explicit layout kullanır.
    /// </summary>
    private string GenerateXfsResultStruct()
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("using System.Runtime.InteropServices;");
        sb.AppendLine();
        sb.AppendLine("namespace DenoXfs.Core.Api;");
        sb.AppendLine();
        sb.AppendLine("[StructLayout(LayoutKind.Explicit, Pack = 1)]");
        sb.AppendLine("public struct XfsResult");
        sb.AppendLine("{");
        sb.AppendLine("    [FieldOffset(0)]");
        sb.AppendLine("    private uint requestID;");
        sb.AppendLine();
        sb.AppendLine("    [FieldOffset(4)]");
        sb.AppendLine("    private ushort hService;");
        sb.AppendLine();
        sb.AppendLine("    [FieldOffset(6)]");
        sb.AppendLine("    private SystemTime tsTimestamp;");
        sb.AppendLine();
        sb.AppendLine("    [FieldOffset(22)]");
        sb.AppendLine("    private int hResult;");
        sb.AppendLine();
        sb.AppendLine("    // UNION - commandCode and eventID share the same memory location");
        sb.AppendLine("    [FieldOffset(26)]");
        sb.AppendLine("    private uint dwCommandCode;");
        sb.AppendLine();
        sb.AppendLine("    [FieldOffset(26)]");
        sb.AppendLine("    private uint dwEventID;");
        sb.AppendLine();
        sb.AppendLine("    [FieldOffset(30)]");
        sb.AppendLine("    private IntPtr lpBuffer;");
        sb.AppendLine();
        sb.AppendLine("    public uint RequestID");
        sb.AppendLine("    {");
        sb.AppendLine("        get => requestID;");
        sb.AppendLine("        set => requestID = value;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    public ushort Service");
        sb.AppendLine("    {");
        sb.AppendLine("        get => hService;");
        sb.AppendLine("        set => hService = value;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    public SystemTime Timestamp");
        sb.AppendLine("    {");
        sb.AppendLine("        get => tsTimestamp;");
        sb.AppendLine("        set => tsTimestamp = value;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    public int Result");
        sb.AppendLine("    {");
        sb.AppendLine("        get => hResult;");
        sb.AppendLine("        set => hResult = value;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    public uint CommandCode");
        sb.AppendLine("    {");
        sb.AppendLine("        get => dwCommandCode;");
        sb.AppendLine("        set => dwCommandCode = value;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    public uint EventID");
        sb.AppendLine("    {");
        sb.AppendLine("        get => dwEventID;");
        sb.AppendLine("        set => dwEventID = value;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    public IntPtr Buffer");
        sb.AppendLine("    {");
        sb.AppendLine("        get => lpBuffer;");
        sb.AppendLine("        set => lpBuffer = value;");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        
        return sb.ToString();
    }

    private class ParsedFieldType
    {
        public string PrivateType { get; set; } = "";
        public string PublicType { get; set; } = "";
        public bool IsPointer { get; set; }
        public bool IsArray { get; set; }
        public string? MarshalAsAttribute { get; set; }
        public string GetterExpression { get; set; } = "";
        public string SetterExpression { get; set; } = "";
    }
}

