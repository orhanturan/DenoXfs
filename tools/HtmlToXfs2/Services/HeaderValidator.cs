using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using HtmlToXfs2.Model;

namespace HtmlToXfs2.Services;

public class HeaderValidator
{
    private readonly string _headerFilePath;
    private readonly string _structsJsonPath;
    private readonly string _enumsJsonPath;
    
    public HeaderValidator(string headerFilePath, string structsJsonPath, string enumsJsonPath)
    {
        _headerFilePath = headerFilePath;
        _structsJsonPath = structsJsonPath;
        _enumsJsonPath = enumsJsonPath;
    }
    
    public ValidationReport Validate()
    {
        var report = new ValidationReport();
        
        // Parse header file
        var headerContent = File.ReadAllText(_headerFilePath);
        var headerStructs = ParseHeaderStructs(headerContent);
        var headerDefines = ParseHeaderDefines(headerContent);
        
        // Load JSON files
        var jsonStructs = LoadStructsJson();
        var jsonEnums = LoadEnumsJson();
        
        // Compare structs
        CompareStructs(headerStructs, jsonStructs, report);
        
        // Compare enums
        CompareEnums(headerDefines, jsonEnums, report);
        
        // Compare enum order and completeness
        CompareEnumOrder(headerDefines, jsonEnums, report);
        
        // Compare constants: Find header defines not used in any enum
        var allHeaderDefines = headerDefines.Values.SelectMany(g => g).Distinct().ToList();
        var enumValues = GetAllEnumValues();
        CompareConstants(allHeaderDefines, enumValues, report);
        
        // Clean up: Remove enum values not defined in header from all JSON files
        CleanupInvalidEnumValues(headerDefines, jsonEnums);
        
        return report;
    }
    
    private Dictionary<string, HeaderStruct> ParseHeaderStructs(string headerContent)
    {
        var structs = new Dictionary<string, HeaderStruct>(StringComparer.OrdinalIgnoreCase);
        var lines = headerContent.Split('\n');
        
        int i = 0;
        while (i < lines.Length)
        {
            var line = lines[i].Trim();
            
            // typedef struct _name veya typedef struct name bul
            var match = Regex.Match(line, @"typedef\s+struct\s+_?(\w+)");
            if (match.Success)
            {
                var structName = match.Groups[1].Value;
                var structLines = new List<string>();
                int braceCount = 0;
                bool started = false;
                
                // Brace counting ile struct sonunu bul
                while (i < lines.Length)
                {
                    line = lines[i];
                    structLines.Add(line);
                    
                    braceCount += line.Count(c => c == '{');
                    braceCount -= line.Count(c => c == '}');
                    
                    if (line.Contains('{')) started = true;
                    if (started && braceCount == 0) break;
                    
                    i++;
                }
                
                // Struct'ı parse et
                var structContent = string.Join("\n", structLines);
                var fields = ParseStructFieldsWithUnion(structContent);
                
                if (fields.Count > 0)
                {
                    structs[structName] = new HeaderStruct
                    {
                        Name = structName,
                        Fields = fields
                    };
                }
            }
            i++;
        }
        
        return structs;
    }
    
    private Dictionary<string, string> ParseStructFields(string fieldsSection)
    {
        var fields = new Dictionary<string, string>();
        
        // Pattern to match: TYPE fieldName; or TYPE fieldName[SIZE];
        var fieldPattern = @"(\w+)\s+(\w+)(\[[\w_]+\])?\s*;";
        var matches = Regex.Matches(fieldsSection, fieldPattern);
        
        // Keep track of previous field for dynamic array size detection
        string? previousFieldName = null;
        
        foreach (Match match in matches)
        {
            var fieldType = match.Groups[1].Value.Trim();
            var fieldName = match.Groups[2].Value.Trim();
            var arrayNotation = match.Groups[3].Value.Trim();
            
            if (!string.IsNullOrEmpty(arrayNotation))
            {
                fieldType += arrayNotation;
            }
            // Check for dynamic array size (LP field with previous Len/Length/Count field)
            // For header structs, don't require "array" keyword
            else if (IsDynamicArrayField(fieldType, previousFieldName))
            {
                // Convert LP prefix to * (e.g., LPBYTE -> *BYTE)
                var baseType = fieldType.Substring(2); // Remove "LP"
                fieldType = $"*{baseType}[{previousFieldName}]";
            }
            
            fields[fieldName] = fieldType;
            
            // Update previous field name for next iteration
            previousFieldName = fieldName;
        }
        
        return fields;
    }
    
    /// <summary>
    /// Parse struct fields with union support - flattens union fields into the struct
    /// </summary>
    private Dictionary<string, string> ParseStructFieldsWithUnion(string structContent)
    {
        var fields = new Dictionary<string, string>();
        var lines = structContent.Split('\n');
        
        int braceDepth = 0;
        bool inUnion = false;
        string? previousFieldName = null;
        
        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();
            
            // Track brace depth
            int openBraces = line.Count(c => c == '{');
            int closeBraces = line.Count(c => c == '}');
            
            // Check if entering a union
            if (line.Contains("union") && line.Contains("{"))
            {
                inUnion = true;
                braceDepth += openBraces;
                braceDepth -= closeBraces;
                continue;
            }
            
            // Update brace depth
            braceDepth += openBraces;
            braceDepth -= closeBraces;
            
            // Check if exiting union (} u; pattern)
            if (inUnion && braceDepth <= 1 && line.Contains("}"))
            {
                inUnion = false;
                continue;
            }
            
            // Skip struct opening/closing lines
            if (line.StartsWith("typedef") || line.StartsWith("}"))
                continue;
            
            // Skip lines that are just braces
            if (line == "{" || line == "}" || line.StartsWith("} "))
                continue;
            
            // Parse field: TYPE fieldName; or TYPE fieldName[SIZE];
            var fieldMatch = Regex.Match(line, @"(\w+)\s+(\w+)(\[[\w_]+\])?\s*;");
            if (fieldMatch.Success)
            {
                var fieldType = fieldMatch.Groups[1].Value.Trim();
                var fieldName = fieldMatch.Groups[2].Value.Trim();
                var arrayNotation = fieldMatch.Groups[3].Value.Trim();
                
                // Skip if it's a keyword (union, struct, etc.)
                if (fieldType == "union" || fieldType == "struct")
                    continue;
                
                if (!string.IsNullOrEmpty(arrayNotation))
                {
                    fieldType += arrayNotation;
                }
                else if (IsDynamicArrayField(fieldType, previousFieldName))
                {
                    var baseType = fieldType.Substring(2);
                    fieldType = $"*{baseType}[{previousFieldName}]";
                }
                
                // Add field (union fields are flattened into the struct)
                if (!fields.ContainsKey(fieldName))
                {
                    fields[fieldName] = fieldType;
                }
                
                previousFieldName = fieldName;
            }
        }
        
        return fields;
    }

    /// <summary>
    /// Checks if a field is a dynamic array (LP pointer with previous Len/Length/Count field)
    /// </summary>
    private bool IsDynamicArrayField(string fieldType, string? previousFieldName)
    {
        // Must be LP pointer type
        if (!fieldType.StartsWith("LP", StringComparison.OrdinalIgnoreCase))
            return false;

        // Previous field must exist and end with Len, Length, or Count
        if (string.IsNullOrEmpty(previousFieldName))
            return false;

        return previousFieldName.EndsWith("Len", StringComparison.OrdinalIgnoreCase) ||
               previousFieldName.EndsWith("Length", StringComparison.OrdinalIgnoreCase) ||
               previousFieldName.EndsWith("Count", StringComparison.OrdinalIgnoreCase);
    }
    
    private Dictionary<string, List<string>> ParseHeaderDefines(string headerContent)
    {
        var defineGroups = new Dictionary<string, List<string>>();
        
        // Split content into lines and process sequentially to detect groups
        var lines = headerContent.Split('\n');
        var currentGroup = new List<string>();
        var currentGroupKey = "";
        int groupCounter = 0;
        int? lastValue = null; // Track last enum value to detect resets
        
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            
            // Check if this is an empty line or comment - these end the current group
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("/*"))
            {
                if (currentGroup.Count > 0)
                {
                    // Save current group and start fresh
                    var groupKey = $"{currentGroupKey}_{groupCounter}";
                    defineGroups[groupKey] = new List<string>(currentGroup);
                    currentGroup.Clear();
                    currentGroupKey = "";
                    groupCounter++;
                    lastValue = null;
                }
                continue;
            }
            
            // Check if this is a #define line
            var definePattern = @"^#define\s+(WFS_IDC_[\w]+)\s+(.+)";
            var match = Regex.Match(line, definePattern);
            
            if (match.Success)
            {
                var defineName = match.Groups[1].Value.Trim();
                var defineValue = match.Groups[2].Value.Trim();
                
                // Skip if it's a define for offsets, class names, or function codes
                if (defineName.EndsWith("_OFFSET") || defineName.Contains("_CLASS") || 
                    defineName.StartsWith("WFS_INF_") || defineName.StartsWith("WFS_CMD_") ||
                    defineName.StartsWith("WFS_SRVE_") || defineName.StartsWith("WFS_EXEE_") ||
                    defineName.StartsWith("WFS_USRE_") || defineName.StartsWith("WFS_ERR_"))
                {
                    continue;
                }
                
                // Try to extract numeric value
                int? currentValue = null;
                var valuePattern = @"\((-?\d+|0x[0-9A-Fa-f]+)\)";
                var valueMatch = Regex.Match(defineValue, valuePattern);
                if (valueMatch.Success)
                {
                    var valueStr = valueMatch.Groups[1].Value;
                    if (valueStr.StartsWith("0x"))
                    {
                        currentValue = Convert.ToInt32(valueStr, 16);
                    }
                    else
                    {
                        currentValue = int.Parse(valueStr);
                    }
                }
                
                var prefix = GetDefinePrefix(defineName);
                
                // Determine if we should start a new group
                bool shouldStartNewGroup = false;
                
                // Start new group if prefix changed
                if (currentGroupKey != prefix && currentGroup.Count > 0)
                {
                    shouldStartNewGroup = true;
                }
                // Start new group if value reset detected (0 or value < lastValue)
                else if (currentValue.HasValue && lastValue.HasValue)
                {
                    if (currentValue.Value == 0 || currentValue.Value < lastValue.Value)
                    {
                        shouldStartNewGroup = true;
                    }
                }
                
                if (shouldStartNewGroup)
                {
                    // Save previous group
                    var groupKey = $"{currentGroupKey}_{groupCounter}";
                    defineGroups[groupKey] = new List<string>(currentGroup);
                    currentGroup.Clear();
                    groupCounter++;
                    lastValue = null;
                }
                
                currentGroupKey = prefix;
                currentGroup.Add(defineName);
                lastValue = currentValue;
            }
        }
        
        // Save last group if any
        if (currentGroup.Count > 0)
        {
            var groupKey = $"{currentGroupKey}_{groupCounter}";
            defineGroups[groupKey] = new List<string>(currentGroup);
        }
        
        return defineGroups;
    }
    
    private string GetDefinePrefix(string defineName)
    {
        // Extract prefix pattern, e.g., WFS_IDC_DEV* -> WFS_IDC_DEV
        var parts = defineName.Split('_');
        if (parts.Length >= 3)
        {
            return $"{parts[0]}_{parts[1]}_{parts[2]}";
        }
        return defineName;
    }
    
    private bool IsPrimitiveType(string typeName)
    {
        // Strip array notation for checking
        var baseType = System.Text.RegularExpressions.Regex.Replace(typeName, @"\[.*?\]", "");
        
        var primitiveTypes = new[] { 
            "WORD", "DWORD", "BOOL", "BYTE", "CHAR", 
            "SHORT", "USHORT", "LONG", "ULONG",
            "LPSTR", "LPBYTE", "LPVOID", "LPWORD", "LPDWORD",
            "LPSHORT", "LPUSHORT", "LPLONG", "LPULONG"
        };
        
        return primitiveTypes.Contains(baseType, StringComparer.OrdinalIgnoreCase);
    }
    
    private Dictionary<string, Dictionary<string, string>> LoadStructsJson()
    {
        if (!File.Exists(_structsJsonPath))
        {
            return new Dictionary<string, Dictionary<string, string>>();
        }
        
        var json = File.ReadAllText(_structsJsonPath);
        return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json) 
               ?? new Dictionary<string, Dictionary<string, string>>();
    }
    
    private Dictionary<string, EnumInfo> LoadEnumsJson()
    {
        if (!File.Exists(_enumsJsonPath))
        {
            return new Dictionary<string, EnumInfo>();
        }
        
        var json = File.ReadAllText(_enumsJsonPath);
        return JsonSerializer.Deserialize<Dictionary<string, EnumInfo>>(json) 
               ?? new Dictionary<string, EnumInfo>();
    }
    
    /// <summary>
    /// Get all enum values from EnumsManager cache
    /// </summary>
    private HashSet<string> GetAllEnumValues()
    {
        var enumValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        // Access EnumsManager to get all enum definitions
        // Note: We need to create an instance or access static method
        // Since EnumsManager is not static, we'll need to access it differently
        // For now, we'll read from the JSON file which contains all enum values
        
        var jsonEnums = LoadEnumsJson();
        foreach (var enumEntry in jsonEnums)
        {
            var enumInfo = enumEntry.Value;
            if (enumInfo.Values != null)
            {
                foreach (var value in enumInfo.Values)
                {
                    enumValues.Add(value);
                }
            }
        }
        
        return enumValues;
    }
    
    private void CompareStructs(Dictionary<string, HeaderStruct> headerStructs, 
                                 Dictionary<string, Dictionary<string, string>> jsonStructs,
                                 ValidationReport report)
    {
        report.TotalHeaderStructs = headerStructs.Count;
        report.TotalJsonStructs = jsonStructs.Count;
        
        foreach (var headerStruct in headerStructs)
        {
            var structName = headerStruct.Key;
            var convertedName = NameConverter.ConvertStructName(structName);
            
            if (!jsonStructs.ContainsKey(convertedName))
            {
                report.MissingStructs.Add(new MissingItem
                {
                    Name = structName,
                    ConvertedName = convertedName,
                    Details = $"Struct not found in JSON"
                });
            }
            else
            {
                // Compare fields
                var jsonFields = jsonStructs[convertedName];
                foreach (var field in headerStruct.Value.Fields)
                {
                    var fieldName = field.Key;
                    
                    // Try multiple name formats:
                    // 1. Original name (fwDevice)
                    // 2. Converted without enum (Device)
                    // 3. Converted with enum (DeviceType)
                    var convertedFieldNameNoEnum = NameConverter.ConvertFieldNameForJson(fieldName, false);
                    var convertedFieldNameWithEnum = NameConverter.ConvertFieldNameForJson(fieldName, true);
                    
                    // Check if field exists with any format
                    bool foundOriginal = jsonFields.ContainsKey(fieldName);
                    bool foundWithoutEnum = jsonFields.ContainsKey(convertedFieldNameNoEnum);
                    bool foundWithEnum = jsonFields.ContainsKey(convertedFieldNameWithEnum);
                    
                    if (!foundOriginal && !foundWithoutEnum && !foundWithEnum)
                    {
                        report.MissingStructFields.Add(new MissingItem
                        {
                            Name = $"{structName}.{fieldName}",
                            ConvertedName = $"{convertedName}.{convertedFieldNameNoEnum}",
                            Details = $"Field not found in JSON struct (checked: {fieldName}, {convertedFieldNameNoEnum}, {convertedFieldNameWithEnum})"
                        });
                    }
                }
            }
        }
        
        // Check for extra structs in JSON
        foreach (var jsonStruct in jsonStructs)
        {
            var jsonStructName = jsonStruct.Key;
            var found = false;
            
            foreach (var headerStruct in headerStructs)
            {
                var convertedName = NameConverter.ConvertStructName(headerStruct.Key);
                if (convertedName.Equals(jsonStructName, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
            }
            
            if (!found)
            {
                report.ExtraJsonStructs.Add(jsonStructName);
            }
        }
    }
    
    private void CompareEnums(Dictionary<string, List<string>> headerDefines,
                             Dictionary<string, EnumInfo> jsonEnums,
                             ValidationReport report)
    {
        report.TotalHeaderDefineGroups = headerDefines.Count;
        report.TotalJsonEnums = jsonEnums.Count;
        
        // For each enum in JSON, check if its values exist in header
        foreach (var jsonEnum in jsonEnums)
        {
            var enumName = jsonEnum.Key;
            var enumInfo = jsonEnum.Value;
            
            foreach (var value in enumInfo.Values)
            {
                var found = false;
                foreach (var defineGroup in headerDefines.Values)
                {
                    if (defineGroup.Contains(value))
                    {
                        found = true;
                        break;
                    }
                }
                
                if (!found)
                {
                    report.MissingEnumValues.Add(new MissingItem
                    {
                        Name = value,
                        ConvertedName = enumName,
                        Details = $"Enum value not found in header defines"
                    });
                }
            }
        }
    }
    
    private void CompareEnumOrder(Dictionary<string, List<string>> headerDefines,
                                  Dictionary<string, EnumInfo> jsonEnums,
                                  ValidationReport report)
    {
        // For each enum in JSON, find matching header define group and compare order
        foreach (var jsonEnum in jsonEnums)
        {
            var enumName = jsonEnum.Key;
            var jsonValues = jsonEnum.Value.Values;
            
            if (jsonValues == null || jsonValues.Count == 0)
                continue;
            
            // Find the BEST matching header define group (the one with most matches)
            List<string>? matchingHeaderGroup = null;
            int bestMatchCount = 0;
            
            foreach (var defineGroup in headerDefines.Values)
            {
                // Count how many JSON values are in this group
                int matchCount = jsonValues.Count(v => defineGroup.Contains(v));
                
                if (matchCount > bestMatchCount)
                {
                    bestMatchCount = matchCount;
                    matchingHeaderGroup = defineGroup;
                }
            }
            
            if (matchingHeaderGroup == null || bestMatchCount == 0)
                continue;
            
            // Extract only the values that are in both JSON and header
            var headerValuesInJson = matchingHeaderGroup.Where(h => jsonValues.Contains(h)).ToList();
            var jsonValuesInHeader = jsonValues.Where(j => matchingHeaderGroup.Contains(j)).ToList();
            
            // Check for missing values (values in header but not in JSON)
            var missingValues = matchingHeaderGroup.Where(h => !jsonValues.Contains(h)).ToList();
            
            // Only report if there are MISSING values (order doesn't matter for merged enums)
            if (missingValues.Count > 0)
            {
                var issue = new EnumOrderIssue
                {
                    EnumName = enumName,
                    ExpectedOrder = matchingHeaderGroup,
                    ActualOrder = jsonValues.ToList(),
                    MissingValues = missingValues,
                    Details = $"{missingValues.Count} value(s) missing from JSON"
                };
                
                report.OutOfOrderEnums.Add(issue);
            }
        }
    }
    
    /// <summary>
    /// Compare header defines with enum values to find constants not used in any enum
    /// </summary>
    private void CompareConstants(List<string> headerDefines, HashSet<string> enumValues, ValidationReport report)
    {
        foreach (var defineName in headerDefines)
        {
            // Check if this constant is used in any enum
            if (!enumValues.Contains(defineName))
            {
                report.MissingConstants.Add(defineName);
            }
        }
    }
    
    public string GenerateReport(ValidationReport report)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("# Header Validation Report");
        sb.AppendLine();
        sb.AppendLine($"**Generated**: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine();
        
        // Summary
        sb.AppendLine("## Summary");
        sb.AppendLine();
        sb.AppendLine($"- **Header Structs**: {report.TotalHeaderStructs}");
        sb.AppendLine($"- **JSON Structs**: {report.TotalJsonStructs}");
        sb.AppendLine($"- **Missing Structs**: {report.MissingStructs.Count}");
        sb.AppendLine($"- **Missing Struct Fields**: {report.MissingStructFields.Count}");
        sb.AppendLine($"- **Extra JSON Structs**: {report.ExtraJsonStructs.Count}");
        sb.AppendLine();
        sb.AppendLine($"- **Header Define Groups**: {report.TotalHeaderDefineGroups}");
        sb.AppendLine($"- **JSON Enums**: {report.TotalJsonEnums}");
        sb.AppendLine($"- **Missing Enum Values**: {report.MissingEnumValues.Count}");
        sb.AppendLine($"- **Enum Completeness Issues**: {report.OutOfOrderEnums.Count}");
        sb.AppendLine($"- **Missing Constants**: {report.MissingConstants.Count}");
        sb.AppendLine();
        
        var structsMatch = report.MissingStructs.Count == 0 && report.MissingStructFields.Count == 0;
        var enumsMatch = report.MissingEnumValues.Count == 0 && report.OutOfOrderEnums.Count == 0;
        
        sb.AppendLine($"**Structs Status**: {(structsMatch ? "✅ PASS" : "❌ FAIL")}");
        sb.AppendLine($"**Enums Status**: {(enumsMatch ? "✅ PASS" : "⚠️ WARNING")}");
        sb.AppendLine();
        
        // Missing Structs
        if (report.MissingStructs.Count > 0)
        {
            sb.AppendLine("## Missing Structs");
            sb.AppendLine();
            foreach (var item in report.MissingStructs)
            {
                sb.AppendLine($"- **{item.Name}** → `{item.ConvertedName}`: {item.Details}");
            }
            sb.AppendLine();
        }
        
        // Missing Struct Fields
        if (report.MissingStructFields.Count > 0)
        {
            sb.AppendLine("## Missing Struct Fields");
            sb.AppendLine();
            foreach (var item in report.MissingStructFields)
            {
                sb.AppendLine($"- **{item.Name}** → `{item.ConvertedName}`: {item.Details}");
            }
            sb.AppendLine();
        }
        
        // Extra JSON Structs
        if (report.ExtraJsonStructs.Count > 0)
        {
            sb.AppendLine("## Extra JSON Structs (not in header)");
            sb.AppendLine();
            foreach (var item in report.ExtraJsonStructs)
            {
                sb.AppendLine($"- `{item}`");
            }
            sb.AppendLine();
        }
        
        // Missing Enum Values
        if (report.MissingEnumValues.Count > 0)
        {
            sb.AppendLine("## Missing Enum Values");
            sb.AppendLine();
            sb.AppendLine("*Note: Some values might be intentionally excluded (e.g., constants used in expressions)*");
            sb.AppendLine();
            foreach (var item in report.MissingEnumValues)
            {
                sb.AppendLine($"- **{item.Name}** (in enum `{item.ConvertedName}`): {item.Details}");
            }
            sb.AppendLine();
        }
        
        // Enum Completeness Issues
        if (report.OutOfOrderEnums.Count > 0)
        {
            sb.AppendLine("## Enum Completeness Issues");
            sb.AppendLine();
            foreach (var issue in report.OutOfOrderEnums)
            {
                sb.AppendLine($"### Enum: `{issue.EnumName}`");
                sb.AppendLine();
                sb.AppendLine($"**Issue**: {issue.Details}");
                sb.AppendLine();
                
                sb.AppendLine("**Missing values:**");
                foreach (var missing in issue.MissingValues)
                {
                    sb.AppendLine($"- `{missing}`");
                }
                sb.AppendLine();
                
                sb.AppendLine($"**Total values in header**: {issue.ExpectedOrder.Count}");
                sb.AppendLine($"**Total values in JSON**: {issue.ActualOrder.Count}");
                sb.AppendLine();
            }
        }
        
        // Missing Constants (not used in any enum)
        if (report.MissingConstants.Count > 0)
        {
            sb.AppendLine("## Missing Constants");
            sb.AppendLine();
            sb.AppendLine("*Constants defined in header but not used in any enum*");
            sb.AppendLine();
            foreach (var constant in report.MissingConstants.OrderBy(c => c))
            {
                sb.AppendLine($"- `{constant}`");
            }
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
    
    public void InsertMissingStructs(ValidationReport report)
    {
        if (report.MissingStructs.Count == 0)
        {
            Console.WriteLine("No missing structs to insert.");
            return;
        }
        
        Console.WriteLine($"\nInserting {report.MissingStructs.Count} missing structs...");
        
        // Parse header to get struct definitions
        var headerContent = File.ReadAllText(_headerFilePath);
        var headerStructs = ParseHeaderStructs(headerContent);
        
        // Load existing Structs.json
        var jsonStructs = LoadStructsJson();
        
        int insertedCount = 0;
        
        // For each missing struct, add it to JSON
        foreach (var missingItem in report.MissingStructs)
        {
            var originalStructName = missingItem.Name;
            var convertedStructName = missingItem.ConvertedName;
            
            if (headerStructs.ContainsKey(originalStructName))
            {
                var headerStruct = headerStructs[originalStructName];
                
                // Keep original field names (with Hungarian notation prefixes)
                var fields = new Dictionary<string, string>();
                foreach (var field in headerStruct.Fields)
                {
                    var originalFieldName = field.Key;
                    var originalFieldType = field.Value;
                    
                    // Keep field name as-is (original with prefix like wVersion, lpszLogicalName, etc.)
                    fields[originalFieldName] = originalFieldType;
                }
                
                // Add to JSON structs
                jsonStructs[convertedStructName] = fields;
                insertedCount++;
                
                Console.WriteLine($"  ✅ Inserted: {originalStructName} → {convertedStructName} ({fields.Count} fields)");
            }
        }
        
        // Save updated Structs.json
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        
        var json = JsonSerializer.Serialize(jsonStructs, options);
        File.WriteAllText(_structsJsonPath, json);
        
        Console.WriteLine($"\n✅ Successfully inserted {insertedCount} structs into {_structsJsonPath}");
    }
    
    private void CleanupInvalidEnumValues(Dictionary<string, List<string>> headerDefines, Dictionary<string, EnumInfo> jsonEnums)
    {
        // Get all header define values in a flat set for quick lookup
        var allHeaderValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var group in headerDefines.Values)
        {
            foreach (var value in group)
            {
                allHeaderValues.Add(value);
            }
        }
        
        bool enumsJsonModified = false;
        
        // 1. Clean up Enums.json
        foreach (var jsonEnum in jsonEnums.Values)
        {
            var valuesToRemove = new List<string>();
            
            foreach (var value in jsonEnum.Values)
            {
                if (!allHeaderValues.Contains(value))
                {
                    valuesToRemove.Add(value);
                    Console.WriteLine($"  Removing invalid value from enum '{jsonEnum.OriginalName}': {value}");
                }
            }
            
            if (valuesToRemove.Count > 0)
            {
                jsonEnum.Values.RemoveAll(v => valuesToRemove.Contains(v));
                enumsJsonModified = true;
            }
        }
        
        // Save cleaned Enums.json if modified
        if (enumsJsonModified)
        {
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var enumsJson = JsonSerializer.Serialize(jsonEnums, options);
            File.WriteAllText(_enumsJsonPath, enumsJson);
            Console.WriteLine($"  Cleaned Enums.json saved: {_enumsJsonPath}");
        }
        
        // 2. Clean up individual command JSON files
        CleanupCommandJsonFiles(allHeaderValues);
    }
    
    private void CleanupCommandJsonFiles(HashSet<string> validHeaderValues)
    {
        // Get device name from enums path (e.g., "int/json/idc/Enums.json" -> "idc")
        var deviceName = Path.GetFileName(Path.GetDirectoryName(_enumsJsonPath));
        var jsonBaseDir = Path.GetDirectoryName(_enumsJsonPath);
        
        if (string.IsNullOrEmpty(jsonBaseDir) || string.IsNullOrEmpty(deviceName))
            return;
        
        var directories = new[] { "GetInfo", "Execute", "Events" };
        int totalCleaned = 0;
        
        foreach (var dir in directories)
        {
            var dirPath = Path.Combine(jsonBaseDir, dir);
            if (!Directory.Exists(dirPath))
                continue;
            
            var jsonFiles = Directory.GetFiles(dirPath, "*.json");
            foreach (var jsonFile in jsonFiles)
            {
                try
                {
                    var jsonText = File.ReadAllText(jsonFile);
                    var jsonDoc = JsonSerializer.Deserialize<JsonElement>(jsonText);
                    
                    bool modified = false;
                    var cleanedDoc = CleanJsonElement(jsonDoc, validHeaderValues, ref modified);
                    
                    if (modified)
                    {
                        var options = new JsonSerializerOptions 
                        { 
                            WriteIndented = true,
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        };
                        var cleanedJson = JsonSerializer.Serialize(cleanedDoc, options);
                        File.WriteAllText(jsonFile, cleanedJson);
                        totalCleaned++;
                        Console.WriteLine($"  Cleaned: {Path.GetFileName(jsonFile)}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Warning: Could not clean {Path.GetFileName(jsonFile)}: {ex.Message}");
                }
            }
        }
        
        if (totalCleaned > 0)
        {
            Console.WriteLine($"  Total command files cleaned: {totalCleaned}");
        }
    }
    
    private JsonElement CleanJsonElement(JsonElement element, HashSet<string> validValues, ref bool modified)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            var dict = new Dictionary<string, JsonElement>();
            
            foreach (var prop in element.EnumerateObject())
            {
                if (prop.Name == "Values" && prop.Value.ValueKind == JsonValueKind.Array)
                {
                    // This is a Values array - clean it
                    var cleanedArray = new List<JsonElement>();
                    foreach (var item in prop.Value.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String)
                        {
                            var value = item.GetString();
                            if (value != null && validValues.Contains(value))
                            {
                                cleanedArray.Add(item);
                            }
                            else
                            {
                                modified = true;
                            }
                        }
                        else
                        {
                            cleanedArray.Add(item);
                        }
                    }
                    dict[prop.Name] = JsonSerializer.SerializeToElement(cleanedArray);
                }
                else
                {
                    // Recursively clean nested objects/arrays
                    dict[prop.Name] = CleanJsonElement(prop.Value, validValues, ref modified);
                }
            }
            
            return JsonSerializer.SerializeToElement(dict);
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<JsonElement>();
            foreach (var item in element.EnumerateArray())
            {
                list.Add(CleanJsonElement(item, validValues, ref modified));
            }
            return JsonSerializer.SerializeToElement(list);
        }
        
        return element;
    }
}

public class HeaderStruct
{
    public string Name { get; set; } = "";
    public Dictionary<string, string> Fields { get; set; } = new();
}

public class ValidationReport
{
    public int TotalHeaderStructs { get; set; }
    public int TotalJsonStructs { get; set; }
    public int TotalHeaderDefineGroups { get; set; }
    public int TotalJsonEnums { get; set; }
    
    public List<MissingItem> MissingStructs { get; set; } = new();
    public List<MissingItem> MissingStructFields { get; set; } = new();
    public List<string> ExtraJsonStructs { get; set; } = new();
    public List<MissingItem> MissingEnumValues { get; set; } = new();
    public List<EnumOrderIssue> OutOfOrderEnums { get; set; } = new();
    public List<string> MissingConstants { get; set; } = new();
}

public class EnumOrderIssue
{
    public string EnumName { get; set; } = "";
    public List<string> ExpectedOrder { get; set; } = new();
    public List<string> ActualOrder { get; set; } = new();
    public List<string> MissingValues { get; set; } = new();
    public string Details { get; set; } = "";
}

public class MissingItem
{
    public string Name { get; set; } = "";
    public string ConvertedName { get; set; } = "";
    public string Details { get; set; } = "";
}

