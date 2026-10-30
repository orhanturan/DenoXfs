using System.Text.Json;
using System.Text.RegularExpressions;
using HtmlToXfs2.Model;

namespace HtmlToXfs2.Services;

public class StructsManager
{
    // Key = original name (e.g., "wfs_idc_status")
    // Value = List of struct definitions with same original name
    private readonly Dictionary<string, List<StructDefinition>> _structs = new();
    private Dictionary<string, List<EnumDefinition>>? _enumsReference = null;

    public void SetEnumsReference(Dictionary<string, List<EnumDefinition>> enums)
    {
        _enumsReference = enums;
    }

    public void AddStruct(string originalName, Dictionary<string, string> fields, string commandName)
    {
        if (string.IsNullOrEmpty(originalName) || fields == null || fields.Count == 0)
            return;

        // Convert to renamed name
        var renamedName = NameConverter.ConvertStructName(originalName);
        
        // Process fields: rename field names and resolve enum types
        var processedFields = ProcessFields(fields, originalName, commandName);
        
        // Initialize list if needed
        if (!_structs.ContainsKey(originalName))
        {
            _structs[originalName] = new List<StructDefinition>();
        }

        // Check if a struct with the same renamed name already exists
        var existingDef = _structs[originalName].FirstOrDefault(d => d.RenamedName == renamedName);
        
        if (existingDef != null)
        {
            // Merge fields, preferring enum types over primitives
            existingDef.Fields = MergeStructFields(existingDef.Fields, processedFields, renamedName);
        }
        else
        {
            // Add new struct definition
            var definition = new StructDefinition
            {
                RenamedName = renamedName,
                Fields = processedFields
            };
            _structs[originalName].Add(definition);
        }
    }

    private Dictionary<string, string> ProcessFields(Dictionary<string, string> originalFields, string structName, string commandName)
    {
        var processedFields = new Dictionary<string, string>();

        foreach (var field in originalFields)
        {
            var fieldName = field.Key; // Keep original field name (don't convert for Structs.json)
            var fieldType = field.Value;

            // Try to resolve enum type
            // Note: fieldName might be already converted from JSON, but we keep it as-is for Structs.json
            var resolvedFieldType = ResolveFieldType(fieldName, fieldType, commandName);

            // Keep field name as-is (original) for Structs.json
            processedFields[fieldName] = resolvedFieldType;
        }

        return processedFields;
    }

    private string ResolveFieldType(string originalFieldName, string originalFieldType, string commandName)
    {
        // Check if there's an enum with this original field name
        if (_enumsReference != null && _enumsReference.ContainsKey(originalFieldName))
        {
            var enumDefinitions = _enumsReference[originalFieldName];
            if (enumDefinitions.Count > 0)
            {
                // Select the enum that belongs to the current command context
                var enumDef = enumDefinitions
                    .FirstOrDefault(d => d.Commands.Contains(commandName, StringComparer.OrdinalIgnoreCase))
                    ?? enumDefinitions[0];
                var enumName = enumDef.RenamedName;

                // Preserve array notation from the original field type, e.g.
                // originalFieldType = "DWORD[WFS_IDC_GUIDLIGHTS_SIZE]" -> append "[WFS_IDC_GUIDLIGHTS_SIZE]"
                // originalFieldType = "WORD[ZeroTerminated]"            -> append "[ZeroTerminated]"
                var arrayMatch = Regex.Match(originalFieldType, @"(\[[\w_+\- ]+\])");
                if (arrayMatch.Success)
                    enumName += arrayMatch.Value;

                return enumName;
            }
        }

        // No enum found, return original type (could be converted to C# types later)
        return originalFieldType;
    }

    private Dictionary<string, string> MergeStructFields(Dictionary<string, string> existingFields, Dictionary<string, string> newFields, string structName)
    {
        var mergedFields = new Dictionary<string, string>(existingFields);
        
        foreach (var newField in newFields)
        {
            var fieldName = newField.Key;
            var newType = newField.Value;
            
            if (mergedFields.ContainsKey(fieldName))
            {
                var existingType = mergedFields[fieldName];
                var chosenType = ChooseBetterType(existingType, newType);
                mergedFields[fieldName] = chosenType;
            }
            else
            {
                // New field not in existing struct, add it
                mergedFields[fieldName] = newType;
            }
        }
        
        return mergedFields;
    }

    private string ChooseBetterType(string type1, string type2)
    {
        bool type1IsEnum = IsEnumType(type1);
        bool type2IsEnum = IsEnumType(type2);
        
        // Both are enums or both are primitives - keep first one
        if (type1IsEnum == type2IsEnum)
            return type1;
            
        // Prefer enum over primitive
        return type1IsEnum ? type1 : type2;
    }

    private bool IsEnumType(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            return false;
            
        // Strip array notation for checking
        var baseType = System.Text.RegularExpressions.Regex.Replace(typeName, @"\[.*?\]", "");
        
        // Enum types start with capital letter and are not primitive types
        if (!char.IsUpper(baseType[0]))
            return false;
            
        var primitiveTypes = new[] { 
            "WORD", "DWORD", "BOOL", "BYTE", "CHAR", 
            "SHORT", "USHORT", "LONG", "ULONG",
            "LPSTR", "LPBYTE", "LPVOID", "LPWORD", "LPDWORD",
            "LPSHORT", "LPUSHORT", "LPLONG", "LPULONG"
        };
        
        return !primitiveTypes.Contains(baseType, StringComparer.OrdinalIgnoreCase);
    }

    public void SaveToJson(string deviceName, string outputPath = "")
    {
        if (string.IsNullOrEmpty(outputPath))
        {
            outputPath = $"int/json/{deviceName.ToLower()}/Structs.json";
        }

        var directory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Convert internal dictionary to output dictionary
        // Output key = RenamedName, Value = Fields
        var outputDict = new Dictionary<string, Dictionary<string, string>>();

        foreach (var kvp in _structs)
        {
            var definitions = kvp.Value;

            // After merging, each original struct should have only one definition
            foreach (var def in definitions)
            {
                // Use renamed name as key
                // If there are still duplicates (shouldn't happen after merge), last one wins
                outputDict[def.RenamedName] = def.Fields;
            }
        }

        var json = JsonSerializer.Serialize(outputDict, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(outputPath, json);
        Console.WriteLine($"  Structs saved: {outputPath} ({outputDict.Count} structs)");
    }

    public int GetStructCount() => _structs.Values.Sum(list => list.Count);
    
    public Dictionary<string, List<StructDefinition>> GetAllStructs() => _structs;
}
