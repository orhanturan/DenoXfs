using System.Text.Json;
using HtmlToXfs2.Model;

namespace HtmlToXfs2.Services;

public class EnumsManager
{
    // Key = original field name (e.g., "fwDevice")
    // Value = List of enum definitions with same original name
    private readonly Dictionary<string, List<EnumDefinition>> _enums = new();

    public void AddEnum(string originalFieldName, string fieldType, List<string> values, string commandName, string deviceName)
    {
        if (string.IsNullOrEmpty(originalFieldName) || values == null || values.Count == 0)
            return;

        // Convert to renamed name
        var renamedName = NameConverter.ConvertEnumName(originalFieldName, commandName, deviceName);

        // Find ALL existing enums across all original field names that have the same RENAMED name
        EnumDefinition? existingWithSameName = null;
        
        foreach (var kvp in _enums)
        {
            foreach (var def in kvp.Value)
            {
                if (def.RenamedName.Equals(renamedName, StringComparison.OrdinalIgnoreCase))
                {
                    existingWithSameName = def;
                    break;
                }
            }
            if (existingWithSameName != null) break;
        }

        // If found an enum with same renamed name, check for merge
        if (existingWithSameName != null)
        {
            var commonValues = existingWithSameName.Values.Intersect(values, StringComparer.OrdinalIgnoreCase).Count();
            
            if (commonValues > 0)
            {
                // MERGE: Union of values and add command
                var mergedValues = existingWithSameName.Values.Union(values, StringComparer.OrdinalIgnoreCase).ToList();
                existingWithSameName.Values = mergedValues;
                
                // Add command if not already present
                if (!existingWithSameName.Commands.Contains(commandName, StringComparer.OrdinalIgnoreCase))
                {
                    existingWithSameName.Commands.Add(commandName);
                }
                
                Console.WriteLine($"  Enum merged: {originalFieldName} → {renamedName} (common: {commonValues}, total: {mergedValues.Count})");
                return;
            }
            else
            {
                // Same renamed name but NO common values → add command suffix to make unique
                var commandSuffix = ExtractCommandSuffix(commandName);
                renamedName = InsertSuffixIntoEnumName(renamedName, commandSuffix, deviceName);
                Console.WriteLine($"  Enum name collision (no common values): {originalFieldName} → {renamedName}");
            }
        }

        // Add as new enum
        if (!_enums.ContainsKey(originalFieldName))
        {
            _enums[originalFieldName] = new List<EnumDefinition>();
        }

        var definition = new EnumDefinition
        {
            RenamedName = renamedName,
            FinalName = renamedName, // Will be updated in SaveToJson if collision occurs
            Type = GetBaseTypeForEnum(fieldType),
            Commands = new List<string> { commandName },
            Values = new List<string>(values)
        };
        
        _enums[originalFieldName].Add(definition);
        Console.WriteLine($"  Enum added: {originalFieldName} → {renamedName} ({values.Count} values)");
    }

    public void SaveToJson(string deviceName, string outputPath = "")
    {
        if (string.IsNullOrEmpty(outputPath))
        {
            outputPath = $"int/json/{deviceName.ToLower()}/Enums.json";
        }

        var directory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Convert internal dictionary to output dictionary
        // Output key = RenamedName, Value = { OriginalName, Name, Type, Values }
        var outputDict = new Dictionary<string, object>();
        var usedNames = new HashSet<string>();

        foreach (var kvp in _enums)
        {
            var originalFieldName = kvp.Key;
            var definitions = kvp.Value;

            for (int i = 0; i < definitions.Count; i++)
            {
                var def = definitions[i];
                var outputKey = def.RenamedName;

                // Handle renamed name collisions
                if (usedNames.Contains(outputKey))
                {
                    int suffix = 2;
                    while (usedNames.Contains($"{outputKey}_{suffix}"))
                    {
                        suffix++;
                    }
                    outputKey = $"{outputKey}_{suffix}";
                }

                // Update FinalName with the actual output key (may differ from RenamedName due to collisions)
                def.FinalName = outputKey;

                usedNames.Add(outputKey);
                outputDict[outputKey] = new
                {
                    Name = outputKey, // Final name (same as key, may differ from RenamedName if collision occurred)
                    OriginalName = originalFieldName,
                    Type = def.Type,
                    Commands = def.Commands,
                    Values = def.Values
                };
            }
        }

        var json = JsonSerializer.Serialize(outputDict, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(outputPath, json);
        Console.WriteLine($"  Enums saved: {outputPath} ({outputDict.Count} enums)");
        
        // Update Constants cache with enum types (don't save to file - that's done by ConstantsExporter)
        UpdateConstantsCacheWithEnumTypes(deviceName);
    }
    
    private void UpdateConstantsCacheWithEnumTypes(string deviceName)
    {
        // Get constants from cache
        var constants = ConstantsCache.GetConstants(deviceName);
        if (constants == null)
        {
            return; // No constants in cache
        }
        
        int updateCount = 0;
        
        // For each enum in _enums, check if its values match any constants
        // If a constant is an enum value, its DataType should be the enum's Type
        foreach (var enumDef in _enums.Values.SelectMany(list => list))
        {
            var enumType = enumDef.Type;
            
            if (string.IsNullOrEmpty(enumType))
                continue;
            
            foreach (var enumValue in enumDef.Values)
            {
                if (constants.ContainsKey(enumValue))
                {
                    // Only set DataType if it's empty (don't override existing types)
                    if (string.IsNullOrEmpty(constants[enumValue].DataType))
                    {
                        constants[enumValue].DataType = enumType;
                        updateCount++;
                    }
                }
            }
        }
        
        if (updateCount > 0)
        {
            Console.WriteLine($"  Updated {updateCount} constant DataTypes from enums");
        }
    }

    public int GetEnumCount() => _enums.Values.Sum(list => list.Count);
    
    public Dictionary<string, List<EnumDefinition>> GetAllEnums() => _enums;
    
    /// <summary>
    /// Get the final enum name for a given field. Returns the renamed name (with collision handling) if found,
    /// otherwise returns the converted enum name.
    /// </summary>
    public string GetFinalEnumName(string originalFieldName, string commandName, string deviceName)
    {
        if (!_enums.ContainsKey(originalFieldName))
        {
            // Not found, return converted name
            return NameConverter.ConvertEnumName(originalFieldName, commandName, deviceName);
        }
        
        // Find the enum definition that matches this command
        var definitions = _enums[originalFieldName];
        foreach (var def in definitions)
        {
            if (def.Commands.Contains(commandName, StringComparer.OrdinalIgnoreCase))
            {
                // Return FinalName if set, otherwise RenamedName
                return !string.IsNullOrEmpty(def.FinalName) ? def.FinalName : def.RenamedName;
            }
        }
        
        // If no match found, return the first definition's name
        if (definitions.Count > 0)
        {
            return !string.IsNullOrEmpty(definitions[0].FinalName) ? definitions[0].FinalName : definitions[0].RenamedName;
        }
        
        // Fallback to converted name
        return NameConverter.ConvertEnumName(originalFieldName, commandName, deviceName);
    }

    /// <summary>
    /// Get enum definition for a given field and command. Returns null if not found.
    /// </summary>
    public EnumDefinition? GetEnumDefinition(string originalFieldName, string commandName)
    {
        if (!_enums.ContainsKey(originalFieldName))
            return null;
        
        var definitions = _enums[originalFieldName];
        foreach (var def in definitions)
        {
            if (def.Commands.Contains(commandName, StringComparer.OrdinalIgnoreCase))
            {
                return def;
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// Converts pointer types (LPWORD, LPDWORD, etc.) to base types (WORD, DWORD, etc.)
    /// for enum Type determination. Enum values are not pointers, they are value types.
    /// </summary>
    private string GetBaseTypeForEnum(string fieldType)
    {
        if (string.IsNullOrEmpty(fieldType))
            return fieldType;
        
        // Remove array notation first
        var baseType = System.Text.RegularExpressions.Regex.Replace(fieldType, @"\[.*?\]", "");
        
        // If starts with LP, remove it to get base type
        if (baseType.StartsWith("LP", StringComparison.OrdinalIgnoreCase))
        {
            return baseType.Substring(2); // Remove "LP" prefix
        }
        
        return baseType;
    }
    
    /// <summary>
    /// Extracts the last part of a command name and converts to CamelCase.
    /// Example: WFS_SRVE_IDC_CARDACTION → CardAction
    /// </summary>
    private string ExtractCommandSuffix(string commandName)
    {
        if (string.IsNullOrEmpty(commandName))
            return "";
        
        // Split by underscore and get the last part
        var parts = commandName.Split('_');
        if (parts.Length == 0)
            return "";
        
        var lastPart = parts[parts.Length - 1];
        
        // Convert to CamelCase (first letter uppercase, rest lowercase)
        if (string.IsNullOrEmpty(lastPart))
            return "";
        
        return char.ToUpper(lastPart[0]) + lastPart.Substring(1).ToLower();
    }
    
    /// <summary>
    /// Inserts a suffix into an enum name after the device prefix.
    /// Example: IdcAction + CardAction → IdcCardActionAction
    /// </summary>
    private string InsertSuffixIntoEnumName(string enumName, string suffix, string deviceName)
    {
        if (string.IsNullOrEmpty(enumName) || string.IsNullOrEmpty(suffix))
            return enumName;
        
        // Device name is like "Idc" - find where it ends in the enum name
        if (enumName.StartsWith(deviceName, StringComparison.OrdinalIgnoreCase))
        {
            // Insert suffix after device prefix
            // IdcAction → Idc + CardAction + Action = IdcCardActionAction
            var afterDevice = enumName.Substring(deviceName.Length);
            return deviceName + suffix + afterDevice;
        }
        
        // If no device prefix found, just prepend suffix
        return suffix + enumName;
    }
}



