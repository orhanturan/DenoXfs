using System.Text.Json;
using System.Text.RegularExpressions;
using HtmlToXfs2.Model;

namespace HtmlToXfs2.Services;

public class WfsJsonConstructor
{
    private readonly string _htmlFilePath;
    private readonly string _htmlContent;
    private readonly string _deviceName;
    private readonly EnumsManager? _enumsManager;
    private Dictionary<string, KeywordSection> _keywordSections = new();
    private Dictionary<string, StructDefinitionInfo> _structCache = new();
    private Dictionary<string, AdditionalStruct> _additionalStructs = new();

    private class KeywordSection
    {
        public string KeywordName { get; set; } = "";
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string HtmlContent { get; set; } = "";
        public string RawContent { get; set; } = "";
    }

    private class StructDefinitionInfo
    {
        public string StructDefinition { get; set; } = "";
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string StructNameLower { get; set; } = "";
        public string StructNameUpper { get; set; } = "";
    }

    public WfsJsonConstructor(string htmlFilePath, EnumsManager? enumsManager = null)
    {
        _htmlFilePath = htmlFilePath;
        _htmlContent = File.ReadAllText(htmlFilePath);
        _enumsManager = enumsManager;
        
        // Extract device name from file path (e.g., "int/html/idc/GetInfo/..." -> "idc")
        _deviceName = ExtractDeviceNameFromPath(htmlFilePath);
        
        ParseKeywordSections();
        FindAllStructDefinitions();
    }

    private static string ExtractDeviceNameFromPath(string filePath)
    {
        // Path format: int/html/{deviceName}/...
        var parts = filePath.Replace('\\', '/').Split('/');
        for (int i = 0; i < parts.Length - 1; i++)
        {
            if (parts[i].Equals("html", StringComparison.OrdinalIgnoreCase) && i + 1 < parts.Length)
            {
                return parts[i + 1].ToLowerInvariant();
            }
        }
        return "idc"; // Default fallback
    }

    private void ParseKeywordSections()
    {
        // Tüm <h3> tag'lerini bul
        var h3Pattern = @"<h3>([^<]+)</h3>";
        var matches = Regex.Matches(_htmlContent, h3Pattern);
        
        if (matches.Count == 0)
            return;
        
        for (int i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            var keywordName = match.Groups[1].Value.Trim();
            var startIndex = match.Index + match.Length; // <h3>keyword</h3> sonrası
            
            // Sonraki <h3> tag'ine kadar veya dosya sonuna kadar
            var endIndex = (i < matches.Count - 1) 
                ? matches[i + 1].Index 
                : _htmlContent.Length;
            
            // HTML content'i çıkar
            var htmlContent = _htmlContent.Substring(startIndex, endIndex - startIndex);
            
            // Raw content oluştur (HTML tag'lerini temizle)
            var rawContent = StripHtmlTags(htmlContent);
            
            var section = new KeywordSection
            {
                KeywordName = keywordName,
                StartIndex = startIndex,
                EndIndex = endIndex,
                HtmlContent = htmlContent,
                RawContent = rawContent
            };
            
            _keywordSections[keywordName] = section;
        }
    }

    private void FindAllStructDefinitions()
    {
        // HTML'de tüm typedef struct tanımlarını bul
        // Pattern: typedef struct _struct_name { ... } STRUCTNAME, *LPSTRUCTNAME;
        var pattern = @"typedef\s+struct\s+_?(\w+)\s*\{";
        var matches = Regex.Matches(_htmlContent, pattern, RegexOptions.IgnoreCase);
        
        if (matches.Count == 0)
            return;
        
        for (int i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            var structNameLower = match.Groups[1].Value;
            var startIndex = match.Index;
            
            // End index: bir sonraki typedef struct başlangıcı veya HTML sonu
            var endIndex = (i < matches.Count - 1) 
                ? matches[i + 1].Index 
                : _htmlContent.Length;
            
            // Struct definition'ı extract et (startIndex'ten endIndex'e kadar)
            var structDefinition = _htmlContent.Substring(startIndex, endIndex - startIndex);
            
            // Struct name'i uppercase versiyonunu bul (typedef struct _name { ... } NAME pattern)
            // Pattern: } NAME, veya } NAME; (hem comma hem semicolon olabilir)
            var structNameUpperPattern = @"\}\s*(\w+)\s*[,;]";
            var nameMatch = Regex.Match(structDefinition, structNameUpperPattern, RegexOptions.Singleline);
            var structNameUpper = nameMatch.Success ? nameMatch.Groups[1].Value : "";
            
            // Struct definition'ı normalize et: <br/> tag'lerini space'e çevir
            var normalizedDefinition = structDefinition.Replace("<br/>", " ").Replace("<br />", " ");
            
            var structInfo = new StructDefinitionInfo
            {
                StructDefinition = normalizedDefinition,
                StartIndex = startIndex,
                EndIndex = endIndex,
                StructNameLower = structNameLower,
                StructNameUpper = structNameUpper
            };
            
            // Cache'e ekle (hem lowercase hem uppercase name ile)
            if (!string.IsNullOrEmpty(structNameLower))
            {
                _structCache[structNameLower.ToLower()] = structInfo;
            }
            if (!string.IsNullOrEmpty(structNameUpper))
            {
                _structCache[structNameUpper.ToUpper()] = structInfo;
            }
        }
    }

    private string StripHtmlTags(string html)
    {
        if (string.IsNullOrEmpty(html))
            return "";
        
        // HTML tag'lerini kaldır
        var withoutTags = Regex.Replace(html, @"<[^>]+>", " ");
        
        // Fazla boşlukları temizle
        withoutTags = Regex.Replace(withoutTags, @"\s+", " ");
        
        return withoutTags.Trim();
    }

    public string GenerateJson()
    {
        var command = CreateWfsCommand();
        return JsonSerializer.Serialize(command, new JsonSerializerOptions { WriteIndented = true });
    }

    public void GenerateAndSaveJson(string deviceName)
    {
        var command = CreateWfsCommand();
        var json = JsonSerializer.Serialize(command, new JsonSerializerOptions { WriteIndented = true });
        
        // Komut türüne göre dizin belirle
        var targetDirectory = DetermineJsonDirectory(command.CommandName, deviceName);
        
        // Hedef dizini oluştur
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }
        
        // JSON dosyasını kaydet
        var jsonFileName = $"{command.CommandName}.json";
        var jsonFilePath = Path.Combine(targetDirectory, jsonFileName);
        
        File.WriteAllText(jsonFilePath, json);
        Console.WriteLine($"  JSON kaydedildi: {jsonFilePath}");
    }

    /// <summary>
    /// Collects enums from this HTML file and adds them to EnumsManager.
    /// This should be called in Pass 1 before JSON generation.
    /// </summary>
    public void CollectEnumsOnly()
    {
        if (_enumsManager == null)
            return;

        var commandName = ExtractCommandName();
        var deviceName = NameConverter.ExtractDeviceFromCommand(commandName);

        // Collect enums from Input Param
        if (_keywordSections.ContainsKey("Input Param"))
        {
            var inputSection = _keywordSections["Input Param"];
            var trimmedContent = inputSection.RawContent.Trim();
            
            if (!trimmedContent.Equals("None", StringComparison.OrdinalIgnoreCase) && 
                !trimmedContent.Equals("None.", StringComparison.OrdinalIgnoreCase))
            {
                CollectEnumsFromSection(inputSection.RawContent, inputSection.HtmlContent, commandName, deviceName);
            }
        }

        // Collect enums from Output Param or Event Param
        KeywordSection? outputSection = null;
        if (_keywordSections.ContainsKey("Output Param"))
        {
            outputSection = _keywordSections["Output Param"];
        }
        else if (_keywordSections.ContainsKey("Event Param"))
        {
            outputSection = _keywordSections["Event Param"];
        }

        if (outputSection != null)
        {
            var trimmedContent = outputSection.RawContent.Trim();
            
            if (!trimmedContent.Equals("None", StringComparison.OrdinalIgnoreCase) && 
                !trimmedContent.Equals("None.", StringComparison.OrdinalIgnoreCase))
            {
                CollectEnumsFromSection(outputSection.RawContent, outputSection.HtmlContent, commandName, deviceName);
            }
        }
    }

    /// <summary>
    /// Collects enums from a parameter section (Input/Output/Event Param)
    /// </summary>
    private void CollectEnumsFromSection(string paramText, string htmlContent, string commandName, string deviceName)
    {
        if (string.IsNullOrEmpty(paramText))
            return;

        // Parse parameter type and name
        var firstStatement = paramText;
        var semicolonIndex = paramText.IndexOf(';');
        if (semicolonIndex > 0)
        {
            firstStatement = paramText.Substring(0, semicolonIndex + 1).Trim();
        }

        var paramLine = firstStatement.Trim();
        var pattern = @"^(\w+)\s+(\**)(\w+)\s*;?";
        var match = Regex.Match(paramLine, pattern);

        if (!match.Success)
            return;

        var paramType = match.Groups[1].Value.Trim();
        var paramName = match.Groups[3].Value.Trim();

        if (IsPrimitiveType(paramType))
        {
            // Primitive Integer Pointer: Check for enum on the parameter itself
            if (IsIntegerPointerType(paramType))
            {
                var paramEnums = ExtractEnumValues(htmlContent, paramName, paramType, null);
                if (paramEnums.Count > 0)
                {
                    _enumsManager?.AddEnum(paramName, paramType, paramEnums, commandName, deviceName);
                }
            }
        }
        else
        {
            // Non-Primitive: Parse struct and collect field enums
            string structDefinition = "";
            if (semicolonIndex > 0 && semicolonIndex < paramText.Length - 1)
            {
                structDefinition = paramText.Substring(semicolonIndex + 1).Trim();
            }

            if (string.IsNullOrEmpty(structDefinition))
            {
                // Try to find struct definition in cache
                var structNameLower = paramType.ToLower();
                if (structNameLower.StartsWith("lp"))
                    structNameLower = structNameLower.Substring(2);

                foreach (var kvp in _structCache)
                {
                    if (kvp.Value.StructNameLower.Equals(structNameLower, StringComparison.OrdinalIgnoreCase) ||
                        kvp.Value.StructNameUpper.Equals(paramType, StringComparison.OrdinalIgnoreCase) ||
                        kvp.Value.StructNameUpper.Equals(paramType.Substring(2), StringComparison.OrdinalIgnoreCase))
                    {
                        structDefinition = kvp.Value.StructDefinition;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(structDefinition))
            {
                // Extract fields and collect enums
                var originalFields = ExtractStructFieldsRecursive(structDefinition, htmlContent, "");
                var fieldsList = originalFields.ToList();
                
                for (int i = 0; i < fieldsList.Count; i++)
                {
                    var fieldName = fieldsList[i].Key;
                    var fieldType = fieldsList[i].Value;
                    
                    // Get next field for boundary detection
                    string? nextFieldInStruct = null;
                    if (i + 1 < fieldsList.Count)
                    {
                        var nextField = fieldsList[i + 1].Key;
                        nextFieldInStruct = nextField.Contains('.') 
                            ? nextField.Substring(nextField.LastIndexOf('.') + 1)
                            : nextField;
                    }
                    
                    var actualFieldName = fieldName.Contains('.') 
                        ? fieldName.Substring(fieldName.LastIndexOf('.') + 1)
                        : fieldName;

                    var enumValues = ExtractEnumValues(htmlContent, actualFieldName, fieldType, nextFieldInStruct);
                    if (enumValues.Count > 0)
                    {
                        var baseType = Regex.Replace(fieldType, @"\[.*?\]", "");
                        _enumsManager?.AddEnum(actualFieldName, baseType, enumValues, commandName, deviceName);
                    }
                }

                // Also collect enums from additional structs in this section
                var additionalStructDefs = FindAllTypedefStructsInSection(htmlContent);
                var mainStructName = ExtractStructName(structDefinition);
                
                foreach (var additionalDef in additionalStructDefs)
                {
                    var additionalStructName = ExtractStructName(additionalDef);
                    if (!string.IsNullOrEmpty(additionalStructName) && 
                        !additionalStructName.Equals(mainStructName, StringComparison.OrdinalIgnoreCase))
                    {
                        var additionalFields = ExtractStructFieldsSimple(additionalDef);
                        var additionalFieldsList = additionalFields.ToList();
                        
                        for (int i = 0; i < additionalFieldsList.Count; i++)
                        {
                            var field = additionalFieldsList[i];
                            string? nextFieldInStruct = i + 1 < additionalFieldsList.Count 
                                ? additionalFieldsList[i + 1].Key 
                                : null;
                            
                            var enumValues = ExtractEnumValues(htmlContent, field.Key, field.Value, nextFieldInStruct);
                            if (enumValues.Count > 0)
                            {
                                var baseType = Regex.Replace(field.Value, @"\[.*?\]", "");
                                _enumsManager?.AddEnum(field.Key, baseType, enumValues, commandName, deviceName);
                            }
                        }
                    }
                }
            }
        }
    }

    private WfsCommand CreateWfsCommand()
    {
        // Clear additional structs for each new command
        _additionalStructs.Clear();
        
        var command = new WfsCommand();
        command.CommandName = ExtractCommandName();
        
        // Description
        if (_keywordSections.ContainsKey("Description"))
        {
            command.Description = ExtractFirstSentence(_keywordSections["Description"].RawContent);
        }
        
        // Comments
        if (_keywordSections.ContainsKey("Comments"))
        {
            command.Comments = ExtractFirstSentence(_keywordSections["Comments"].RawContent);
        }
        
        // Input Param - None kontrolü
        if (_keywordSections.ContainsKey("Input Param"))
        {
            var inputSection = _keywordSections["Input Param"];
            var trimmedContent = inputSection.RawContent.Trim();
            
            if (trimmedContent.Equals("None", StringComparison.OrdinalIgnoreCase) || 
                trimmedContent.Equals("None.", StringComparison.OrdinalIgnoreCase))
            {
                command.InputParam = null;
            }
            else
            {
                command.InputParam = ParseParameter<InputParam>(inputSection.RawContent, inputSection.HtmlContent, command.CommandName);
            }
        }
        
        // Output Param - None kontrolü (Events için "Event Param" kullanılır)
        KeywordSection? outputSection = null;
        if (_keywordSections.ContainsKey("Output Param"))
        {
            outputSection = _keywordSections["Output Param"];
        }
        else if (_keywordSections.ContainsKey("Event Param"))
        {
            // Events use "Event Param" instead of "Output Param"
            outputSection = _keywordSections["Event Param"];
        }
        
        if (outputSection != null)
        {
            var trimmedContent = outputSection.RawContent.Trim();
            
            if (trimmedContent.Equals("None", StringComparison.OrdinalIgnoreCase) || 
                trimmedContent.Equals("None.", StringComparison.OrdinalIgnoreCase))
            {
                command.OutputParam = null;
            }
            else
            {
                command.OutputParam = ParseParameter<OutputParam>(outputSection.RawContent, outputSection.HtmlContent, command.CommandName);
            }
        }
        
        // AdditionalStructs
        if (_additionalStructs.Count > 0)
        {
            command.AdditionalStructs = _additionalStructs;
        }
        
        return command;
    }


    private T ParseParameter<T>(string paramText, string htmlContent, string commandName) where T : new()
    {
        var param = new T();
        
        if (string.IsNullOrEmpty(paramText))
            return param;
        
        // İlk cümleyi veya noktalı virgüle kadar olan kısmı al
        var firstStatement = paramText;
        var semicolonIndex = paramText.IndexOf(';');
        if (semicolonIndex > 0)
        {
            firstStatement = paramText.Substring(0, semicolonIndex + 1).Trim();
        }
        
        var paramLine = firstStatement.Trim();
        
        // "LPWORD lpwReadData;" veya "LPWFSIDCCARDDATA *lppCardData;" formatından parse et
        var pattern = @"^(\w+)\s+(\**)(\w+)\s*;?";
        var match = Regex.Match(paramLine, pattern);
        
        if (match.Success)
        {
            var paramType = match.Groups[1].Value.Trim();
            var pointers = match.Groups[2].Value.Trim(); // *, **, vb.
            var paramName = match.Groups[3].Value.Trim();
            
            // Reflection kullanarak temel property'leri set et
            var valueProperty = typeof(T).GetProperty("Value");
            var originalParamTypeProperty = typeof(T).GetProperty("OriginalParamType");
            var paramTypeProperty = typeof(T).GetProperty("ParamType");
            var originalNameProperty = typeof(T).GetProperty("OriginalName");
            var nameProperty = typeof(T).GetProperty("Name");
            
            valueProperty?.SetValue(param, paramLine);
            originalParamTypeProperty?.SetValue(param, paramType);
            paramTypeProperty?.SetValue(param, paramType); // Will be updated later
            originalNameProperty?.SetValue(param, paramName);
            nameProperty?.SetValue(param, paramName); // Will be updated later
            
            // Primitive type kontrolü
            if (!IsPrimitiveType(paramType))
            {
                // Non-Primitive: Struct parsing + field enum parsing
                if (semicolonIndex > 0 && semicolonIndex < paramText.Length - 1)
                {
                    var structDefinition = paramText.Substring(semicolonIndex + 1).Trim();
                    if (!string.IsNullOrEmpty(structDefinition))
                    {
                        // Parse main struct (eski çalışan mantık)
                        ParseStructDefinition(param, structDefinition, htmlContent);
                        
                        // Check for additional structs in htmlContent (only if more than 1 typedef struct exists)
                        if (!string.IsNullOrEmpty(htmlContent))
                        {
                            var allStructDefs = FindAllTypedefStructsInSection(htmlContent);
                            
                            // If more than 1 struct found, add remaining as AdditionalStructs
                            if (allStructDefs.Count > 1)
                            {
                                for (int structIdx = 1; structIdx < allStructDefs.Count; structIdx++)
                                {
                                    var additionalStructDef = allStructDefs[structIdx];
                                    ParseAndAddAdditionalStruct(additionalStructDef, htmlContent, commandName);
                                }
                            }
                        }
                    }
                }
                
                // Enum parsing - struct'ın fields'larını kullan
                var fieldsProperty = typeof(T).GetProperty("Fields");
                var originalFields = fieldsProperty?.GetValue(param) as Dictionary<string, string>;
                
                if (originalFields != null && originalFields.Count > 0)
                {
                    var enums = ParseEnums(htmlContent, originalFields, commandName);
                    
                    // For Structs.json, we need original field names with enum type resolution
                    // Create a dictionary with original field names but resolved enum types
                    var fieldsForStructs = new Dictionary<string, string>();
                    var convertedEnums = new Dictionary<string, EnumInfo>();
                    var deviceName = NameConverter.ExtractDeviceFromCommand(commandName);
                    
                    foreach (var field in originalFields)
                    {
                        var originalFieldName = field.Key;
                        var originalFieldType = field.Value;
                        
                        // Check if this field has an enum
                        var hasEnum = enums.ContainsKey(originalFieldName);
                        
                        // Convert field type (resolve enum types)
                        string resolvedFieldType;
                        if (hasEnum)
                        {
                            // Get the actual field name (last part if nested)
                            var actualFieldName = originalFieldName.Contains('.') 
                                ? originalFieldName.Substring(originalFieldName.LastIndexOf('.') + 1)
                                : originalFieldName;
                            
                            // Get the converted enum name
                            var enumName = NameConverter.ConvertEnumName(actualFieldName, commandName, deviceName);
                            
                            // Get final enum name (may differ if collision occurred)
                            var finalEnumName = _enumsManager?.GetFinalEnumName(actualFieldName, commandName, deviceName) ?? enumName;
                            
                            resolvedFieldType = enumName;
                            
                            // Preserve array notation if present in original type
                            var arrayMatch = Regex.Match(originalFieldType, @"(\[[\w_]+\])");
                            if (arrayMatch.Success)
                            {
                                resolvedFieldType += arrayMatch.Value;
                            }
                            
                            // Update the Name property to final name
                            var enumInfo = enums[originalFieldName];
                            enumInfo.Name = finalEnumName;
                            
                            // Add to converted enums dictionary with converted name as key
                            convertedEnums[enumName] = enumInfo;
                        }
                        else
                        {
                            resolvedFieldType = originalFieldType;
                        }
                        
                        // Use original field name (not converted) for Structs.json
                        fieldsForStructs[originalFieldName] = resolvedFieldType;
                    }
                    
                    // Set fields with original names but resolved enum types (for Structs.json)
                    fieldsProperty?.SetValue(param, fieldsForStructs);
                    
                    if (convertedEnums.Count > 0)
                    {
                        var enumsProperty = typeof(T).GetProperty("Enums");
                        enumsProperty?.SetValue(param, convertedEnums);
                    }
                }
            }
            else if (IsIntegerPointerType(paramType))
            {
                // Primitive Integer Pointer: Get enum from EnumsManager (was collected in Pass 1)
                var deviceName = NameConverter.ExtractDeviceFromCommand(commandName);
                var enumDef = _enumsManager?.GetEnumDefinition(paramName, commandName);
                
                if (enumDef != null && enumDef.Values.Count > 0)
                {
                    var enumName = NameConverter.ConvertEnumName(paramName, commandName, deviceName);
                    var finalEnumName = !string.IsNullOrEmpty(enumDef.FinalName) ? enumDef.FinalName : enumDef.RenamedName;
                    
                    var enumInfo = new EnumInfo 
                    { 
                        Name = finalEnumName,
                        OriginalName = paramName,
                        Type = paramType,
                        Commands = new List<string> { commandName },
                        Values = enumDef.Values 
                    };
                    
                    var enumsProperty = typeof(T).GetProperty("Enums");
                    var enumDict = new Dictionary<string, EnumInfo>
                    {
                        { enumName, enumInfo }
                    };
                    enumsProperty?.SetValue(param, enumDict);
                }
            }
            
            // After all parsing is done, update ParamType and Name with converted values
            var originalParamType = originalParamTypeProperty?.GetValue(param) as string;
            var originalName = originalNameProperty?.GetValue(param) as string;
            
            if (!string.IsNullOrEmpty(originalParamType))
            {
                // Determine new ParamType
                string newParamType = originalParamType; // Default: keep original
                
                // Check if struct exists
                var structNameProperty = typeof(T).GetProperty("StructureName");
                var structName = structNameProperty?.GetValue(param) as string;
                if (!string.IsNullOrEmpty(structName))
                {
                    newParamType = structName;
                }
                else
                {
                    // Check if primitive with enum
                    var enumsProperty = typeof(T).GetProperty("Enums");
                    var enums = enumsProperty?.GetValue(param) as Dictionary<string, EnumInfo>;
                    if (enums != null && enums.Count > 0)
                    {
                        // Use first enum's key (which is the converted enum name)
                        newParamType = enums.First().Key;
                    }
                }
                
                paramTypeProperty?.SetValue(param, newParamType);
            }
            
            if (!string.IsNullOrEmpty(originalName))
            {
                // Determine new Name using same rules as field names
                // Check if this parameter has an enum (for special handling like Type -> DeviceType)
                var enumsProperty = typeof(T).GetProperty("Enums");
                var enums = enumsProperty?.GetValue(param) as Dictionary<string, EnumInfo>;
                var hasEnum = enums != null && enums.Count > 0;
                
                var convertedName = NameConverter.ConvertFieldNameForJson(originalName, hasEnum);
                nameProperty?.SetValue(param, convertedName);
            }
        }
        
        return param;
    }

    private bool IsPrimitiveType(string paramType)
    {
        var primitives = new[] { "WORD", "SHORT", "USHORT", "LONG", "ULONG", "DWORD", "LPSTR", "LPBYTE" };
        var cleanType = paramType.StartsWith("LP") ? paramType.Substring(2) : paramType;
        return primitives.Contains(cleanType, StringComparer.OrdinalIgnoreCase);
    }

    private bool IsIntegerPointerType(string paramType)
    {
        // Integer pointer tipleri için enum parsing yapılacak
        var integerPointers = new[] { "LPWORD", "LPSHORT", "LPUSHORT", "LPLONG", "LPULONG", "LPDWORD" };
        return integerPointers.Contains(paramType, StringComparer.OrdinalIgnoreCase);
    }

    private string DetermineJsonDirectory(string commandName, string deviceName)
    {
        var baseDir = Path.Combine("int", "json", deviceName.ToLowerInvariant());
        
        if (commandName.StartsWith("WFS_INF_", StringComparison.OrdinalIgnoreCase))
        {
            return Path.Combine(baseDir, "GetInfo");
        }
        else if (commandName.StartsWith("WFS_CMD_", StringComparison.OrdinalIgnoreCase))
        {
            return Path.Combine(baseDir, "Execute");
        }
        else
        {
            // WFS_SRVE_, WFS_EXEE_, WFS_USRE_ ve diğer tüm WFS_
            return Path.Combine(baseDir, "Events");
        }
    }

    private string ExtractCommandName()
    {
        // Dosya isminden komut adını al (örn: WFS_INF_IDC_STATUS.html → WFS_INF_IDC_STATUS)
        var fileName = Path.GetFileNameWithoutExtension(_htmlFilePath);
        return fileName;
    }

    private string ExtractFirstSentence(string fullText)
    {
        if (string.IsNullOrEmpty(fullText))
            return "";
            
        // İlk cümleyi al (. karakterine kadar)
        var firstSentenceEnd = fullText.IndexOf('.');
        if (firstSentenceEnd > 0)
        {
            return fullText.Substring(0, firstSentenceEnd + 1).Trim();
        }
        
        return fullText;
    }

    /// <summary>
    /// Find all typedef struct definitions in a section of text.
    /// Returns a list of struct definition strings.
    /// </summary>
    private List<string> FindAllTypedefStructsInSection(string sectionContent)
    {
        var structDefs = new List<string>();
        
        // First normalize HTML: replace <br/> tags with spaces for easier pattern matching
        var normalizedContent = sectionContent.Replace("<br/>", " ").Replace("<br />", " ");
        
        // Pattern to find typedef struct start (more flexible - allows whitespace/newlines between struct and {)
        var pattern = @"typedef\s+struct\s+_?(\w+)\s*\{";
        var matches = Regex.Matches(normalizedContent, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        
        if (matches.Count == 0)
            return structDefs;
        
        for (int i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            var startIndex = match.Index;
            
            // Find the end of this struct definition (closing brace + struct name)
            // End is either the start of the next typedef struct or end of section
            var endIndex = (i < matches.Count - 1) 
                ? matches[i + 1].Index 
                : normalizedContent.Length;
            
            // Extract struct definition
            var structDef = normalizedContent.Substring(startIndex, endIndex - startIndex);
            
            // Clean up extra whitespace
            structDef = Regex.Replace(structDef, @"\s+", " ").Trim();
            
            structDefs.Add(structDef);
        }
        
        return structDefs;
    }

    /// <summary>
    /// Parse an additional struct definition and add it to _additionalStructs
    /// </summary>
    private void ParseAndAddAdditionalStruct(string structDefinition, string htmlContent, string commandName)
    {
        if (string.IsNullOrEmpty(structDefinition))
            return;
        
        var structName = ExtractStructName(structDefinition);
        if (string.IsNullOrEmpty(structName))
            return;
        
        var convertedStructName = NameConverter.ConvertStructName(structName);
        
        // Skip if already added
        if (_additionalStructs.ContainsKey(convertedStructName))
            return;
        
        // Extract fields from struct definition
        var fields = ExtractStructFieldsSimple(structDefinition);
        
        // Parse enums for these fields
        var enums = ParseEnums(htmlContent, fields, commandName);
        
        // For Structs.json, we need original field names with enum type resolution
        var fieldsForStructs = new Dictionary<string, string>();
        var convertedEnums = new Dictionary<string, EnumInfo>();
        var deviceName = NameConverter.ExtractDeviceFromCommand(commandName);
        
        foreach (var field in fields)
        {
            var originalFieldName = field.Key;
            var originalFieldType = field.Value;
            
            var hasEnum = enums.ContainsKey(originalFieldName);
            
            // Resolve field type (enum types)
            string resolvedFieldType;
            if (hasEnum)
            {
                var enumName = NameConverter.ConvertEnumName(originalFieldName, commandName, deviceName);
                // Get final enum name (may differ if collision occurred)
                var finalEnumName = _enumsManager?.GetFinalEnumName(originalFieldName, commandName, deviceName) ?? enumName;
                resolvedFieldType = enumName;
                // Update the Name property to final name
                var enumInfo = enums[originalFieldName];
                enumInfo.Name = finalEnumName;
                convertedEnums[enumName] = enumInfo;
            }
            else
            {
                resolvedFieldType = originalFieldType;
            }
            
            // Use original field name (not converted) for Structs.json
            fieldsForStructs[originalFieldName] = resolvedFieldType;
        }
        
        var additionalStruct = new AdditionalStruct
        {
            OriginalStructureName = structName,
            StructureName = convertedStructName,
            // Use original field names with resolved enum types for Structs.json
            Fields = fieldsForStructs.Count > 0 ? fieldsForStructs : null,
            Enums = convertedEnums.Count > 0 ? convertedEnums : null
        };
        
        _additionalStructs[convertedStructName] = additionalStruct;
    }

    /// <summary>
    /// Extract struct fields from a struct definition (simple version without nested struct handling)
    /// </summary>
    private Dictionary<string, string> ExtractStructFieldsSimple(string structDefinition)
    {
        var fields = new Dictionary<string, string>();
        
        // Find content between { and }
        var bracePattern = @"\{([^}]+)\}";
        var braceMatch = Regex.Match(structDefinition, bracePattern, RegexOptions.Singleline);
        
        if (!braceMatch.Success)
            return fields;
        
        var fieldsSection = braceMatch.Groups[1].Value;
        
        // Parse each field: TYPE fieldName; or TYPE fieldName[SIZE];
        var fieldPattern = @"(\w+)\s+(\w+)(\[[\w_]+\])?\s*;";
        var fieldMatches = Regex.Matches(fieldsSection, fieldPattern);
        
        // Keep track of previous field for dynamic array size detection
        string? previousFieldName = null;
        
        foreach (Match fieldMatch in fieldMatches)
        {
            var fieldType = fieldMatch.Groups[1].Value.Trim();
            var fieldName = fieldMatch.Groups[2].Value.Trim();
            var arrayNotation = fieldMatch.Groups[3].Value.Trim();
            
            if (!string.IsNullOrEmpty(fieldType) && !string.IsNullOrEmpty(fieldName))
            {
                if (!string.IsNullOrEmpty(arrayNotation))
                {
                    // Resolve WFS_ constants in array notation to their actual values
                    arrayNotation = ResolveArraySizeConstant(arrayNotation, _deviceName);
                    fieldType += arrayNotation;
                }
                // Check for dynamic array size (LP field with previous Len/Length/Count field)
                // For simple struct extraction (e.g., AdditionalStructs), don't require "array" keyword
                else if (IsDynamicArrayField(fieldType, fieldName, previousFieldName, "", false))
                {
                    // Convert LP prefix to * (e.g., LPBYTE -> *BYTE)
                    var baseType = fieldType.Substring(2); // Remove "LP"
                    fieldType = $"*{baseType}[{previousFieldName}]";
                }
                
                fields[fieldName] = fieldType;
                
                // Update previous field name for next iteration
                previousFieldName = fieldName;
            }
        }
        
        return fields;
    }

    private void ParseStructDefinition<T>(T param, string structDefinition, string htmlContent)
    {
        if (string.IsNullOrEmpty(structDefinition))
            return;

        // Struct name'i çıkar
        var structName = ExtractStructName(structDefinition);
        if (!string.IsNullOrEmpty(structName))
        {
            var structNameProperty = typeof(T).GetProperty("OriginalStructureName");
            structNameProperty?.SetValue(param, structName);
            
            // Convert to PascalCase and set StructureName
            var convertedName = NameConverter.ConvertStructName(structName);
            var convertedNameProperty = typeof(T).GetProperty("StructureName");
            convertedNameProperty?.SetValue(param, convertedName);
        }

        // Fields'ları çıkar
        var fields = ExtractStructFields(structDefinition, htmlContent);
        if (fields.Count > 0)
        {
            var fieldsProperty = typeof(T).GetProperty("Fields");
            fieldsProperty?.SetValue(param, fields);
        }
    }

    private string ExtractStructName(string structDefinition)
    {
        // typedef struct _struct_name { ... } STRUCTNAME, *LPSTRUCTNAME;
        var pattern = @"typedef\s+struct\s+_?(\w+)";
        var match = Regex.Match(structDefinition, pattern, RegexOptions.IgnoreCase);
        
        if (match.Success)
        {
            var structName = match.Groups[1].Value;
            // _wfs_idc_status → wfs_idc_status (underscore kaldır)
            return structName.StartsWith("_") ? structName.Substring(1) : structName;
        }
        
        return "";
    }

    private Dictionary<string, string> ExtractStructFields(string structDefinition, string htmlContent)
    {
        return ExtractStructFieldsRecursive(structDefinition, htmlContent, "");
    }
    
    private Dictionary<string, string> ExtractStructFieldsRecursive(string structDefinition, string htmlContent, string parentFieldPrefix)
    {
        var fields = new Dictionary<string, string>();
        
        // { ile } arasındaki kısmı bul
        var bracePattern = @"\{([^}]+)\}";
        var braceMatch = Regex.Match(structDefinition, bracePattern, RegexOptions.Singleline);
        
        if (!braceMatch.Success)
            return fields;
            
        var fieldsSection = braceMatch.Groups[1].Value;
        
        // Her field'ı parse et: TYPE fieldName; veya TYPE fieldName[SIZE];
        // Array desteği: DWORD dwGuidLights[WFS_IDC_GUIDLIGHTS_SIZE];
        var fieldPattern = @"(\w+)\s+(\w+)(\[[\w_]+\])?\s*;";
        var fieldMatches = Regex.Matches(fieldsSection, fieldPattern);
        
        // Keep track of previous field for dynamic array size detection
        string? previousFieldName = null;
        
        foreach (Match fieldMatch in fieldMatches)
        {
            var fieldType = fieldMatch.Groups[1].Value.Trim();
            var fieldName = fieldMatch.Groups[2].Value.Trim();
            var arrayNotation = fieldMatch.Groups[3].Value.Trim();
            
            if (!string.IsNullOrEmpty(fieldType) && !string.IsNullOrEmpty(fieldName))
            {
                // Preserve array notation from struct definition
                if (!string.IsNullOrEmpty(arrayNotation))
                {
                    // Resolve WFS_ constants in array notation to their actual values
                    arrayNotation = ResolveArraySizeConstant(arrayNotation, _deviceName);
                    fieldType += arrayNotation;
                }
                
                // Check if field description indicates zero-terminated array/list
                var fieldDescription = ExtractFieldDescription(htmlContent, fieldName);
                if (IsZeroTerminatedField(fieldDescription))
                {
                    fieldType += "[ZeroTerminated]";
                }
                // Check for dynamic array size (LP field with previous Len/Length/Count field)
                else if (IsDynamicArrayField(fieldType, fieldName, previousFieldName, fieldDescription, true))
                {
                    // Convert LP prefix to * (e.g., LPBYTE -> *BYTE)
                    var baseType = fieldType.Substring(2); // Remove "LP"
                    fieldType = $"*{baseType}[{previousFieldName}]";
                }
                
                // Build full field name with prefix for nested structs
                var fullFieldName = string.IsNullOrEmpty(parentFieldPrefix) ? fieldName : $"{parentFieldPrefix}.{fieldName}";
                
                // Check if this field is a nested struct pointer (LP prefix + struct name pattern)
                if (fieldType.StartsWith("LP", StringComparison.OrdinalIgnoreCase) && 
                    fieldType.Length > 2 && 
                    char.IsUpper(fieldType[2]))
                {
                    // Nested struct pointer - just add as regular field (AdditionalStructs handled separately)
                    fields[fullFieldName] = fieldType;
                }
                else
                {
                    // Regular field (not a nested struct)
                    fields[fullFieldName] = fieldType;
                }
                
                // Update previous field name for next iteration
                previousFieldName = fieldName;
            }
        }
        
        return fields;
    }

    /// <summary>
    /// Checks if a field is a dynamic array (LP pointer with previous Len/Length/Count field)
    /// </summary>
    /// <param name="fieldType">Current field type</param>
    /// <param name="fieldName">Current field name</param>
    /// <param name="previousFieldName">Previous field name (may be null)</param>
    /// <param name="fieldDescription">Field description from HTML</param>
    /// <param name="requireArrayKeyword">If true, requires "array" keyword in description (for HTML-parsed structs)</param>
    private bool IsDynamicArrayField(string fieldType, string fieldName, string? previousFieldName, string fieldDescription, bool requireArrayKeyword)
    {
        // Must be LP pointer type
        if (!fieldType.StartsWith("LP", StringComparison.OrdinalIgnoreCase))
            return false;

        // Previous field must exist and end with Len, Length, or Count
        if (string.IsNullOrEmpty(previousFieldName))
            return false;

        var hasSizePrefix = previousFieldName.EndsWith("Len", StringComparison.OrdinalIgnoreCase) ||
                           previousFieldName.EndsWith("Length", StringComparison.OrdinalIgnoreCase) ||
                           previousFieldName.EndsWith("Count", StringComparison.OrdinalIgnoreCase);

        if (!hasSizePrefix)
            return false;

        // For HTML-parsed structs, require "array" keyword in description
        if (requireArrayKeyword)
        {
            if (string.IsNullOrEmpty(fieldDescription))
                return false;

            var lowerDesc = fieldDescription.ToLower();
            
            // Must contain "array"
            if (!lowerDesc.Contains("array"))
                return false;

            // Must NOT be fixed, zero-terminated, or null-terminated
            if (lowerDesc.Contains("fixed") ||
                lowerDesc.Contains("zero terminated") ||
                lowerDesc.Contains("zero-terminated") ||
                lowerDesc.Contains("null terminated") ||
                lowerDesc.Contains("null-terminated"))
                return false;
        }

        return true;
    }

    private string FindNestedStructDefinition(string structPointerType, string htmlContent)
    {
        if (string.IsNullOrEmpty(structPointerType) || !structPointerType.StartsWith("LP", StringComparison.OrdinalIgnoreCase))
            return "";
        
        // Remove LP prefix: LPWFSIDCEMVCLESSUI -> WFSIDCEMVCLESSUI
        var structName = structPointerType.Substring(2);
        
        // Cache'de ara (hem uppercase hem lowercase)
        if (_structCache.ContainsKey(structName.ToUpper()))
        {
            return _structCache[structName.ToUpper()].StructDefinition;
        }
        
        // Convert struct name to lowercase with underscores: WFSIDCEMVCLESSUI -> wfs_idc_emvcless_ui
        var structNameWithUnderscores = Regex.Replace(structName, @"([a-z\d])([A-Z])", "$1_$2").ToLower();
        
        if (_structCache.ContainsKey(structNameWithUnderscores))
        {
            return _structCache[structNameWithUnderscores].StructDefinition;
        }
        return "";
    }
    
    private string ExtractFieldDescription(string htmlContent, string fieldName)
    {
        if (string.IsNullOrEmpty(htmlContent) || string.IsNullOrEmpty(fieldName))
            return "";
        
        // <p><i>fieldName</i> veya <br/><i>fieldName</i> pattern'ini bul
        var fieldPattern1 = $@"<p><i>{Regex.Escape(fieldName)}</i>";
        var fieldPattern2 = $@"<br/><i>{Regex.Escape(fieldName)}</i>";
        
        var match = Regex.Match(htmlContent, fieldPattern1);
        if (!match.Success)
            match = Regex.Match(htmlContent, fieldPattern2);
        
        if (!match.Success)
            return "";
        
        // Field'ın başlangıç indexi
        var fieldStartIndex = match.Index + match.Length;
        
        // Sonraki field'ın başlangıcını bul (<br/><i> veya <p><i>)
        var nextFieldPattern = @"(?:<br/>|<p>)<i>";
        var nextFieldMatch = Regex.Match(htmlContent.Substring(fieldStartIndex), nextFieldPattern);
        
        // Field'ın bitiş indexini belirle (sonraki field veya htmlContent sonu)
        var fieldEndIndex = nextFieldMatch.Success 
            ? fieldStartIndex + nextFieldMatch.Index 
            : htmlContent.Length;
        
        // Sadece bu field'a ait section'ı al
        var fieldSection = htmlContent.Substring(fieldStartIndex, fieldEndIndex - fieldStartIndex);
        
        // HTML tag'lerini temizle ve döndür
        return StripHtmlTags(fieldSection);
    }

    private bool IsZeroTerminatedField(string fieldDescription)
    {
        if (string.IsNullOrEmpty(fieldDescription))
            return false;
        
        var lowerDesc = fieldDescription.ToLower();
        
        // Check for "zero terminated" or "null terminated" or "null-terminated"
        var hasTerminatedKeyword = lowerDesc.Contains("zero terminated") || 
                                   lowerDesc.Contains("null terminated") || 
                                   lowerDesc.Contains("null-terminated");
        
        // Check for "array" or "list"
        var hasArrayOrList = lowerDesc.Contains("array") || lowerDesc.Contains("list");
        
        return hasTerminatedKeyword && hasArrayOrList;
    }

    /// <summary>
    /// Resolves WFS_ constants in array notation to their actual values from ConstantsCache.
    /// Example: "[WFS_IDC_GUIDLIGHTS_SIZE]" -> "[32]"
    /// </summary>
    private string ResolveArraySizeConstant(string arrayNotation, string deviceName)
    {
        if (string.IsNullOrEmpty(arrayNotation))
            return arrayNotation;

        // Pattern to match [CONSTANT_NAME] where CONSTANT_NAME starts with WFS_
        var pattern = @"\[(WFS_[A-Z_0-9]+)\]";
        var match = Regex.Match(arrayNotation, pattern);
        
        if (match.Success)
        {
            var constantName = match.Groups[1].Value;
            var constantInfo = ConstantsCache.GetConstantInfo(deviceName, constantName);
            
            if (constantInfo != null && !string.IsNullOrEmpty(constantInfo.Value))
            {
                // Replace constant name with its value
                return $"[{constantInfo.Value}]";
            }
        }
        
        return arrayNotation;
    }

    private Dictionary<string, EnumInfo> ParseEnums(string htmlContent, Dictionary<string, string> fields, string commandName)
    {
        var enums = new Dictionary<string, EnumInfo>();
        var deviceName = NameConverter.ExtractDeviceFromCommand(commandName);
        
        var fieldsList = fields.ToList();
        for (int i = 0; i < fieldsList.Count; i++)
        {
            var fieldName = fieldsList[i].Key;
            var fieldType = fieldsList[i].Value;
            
            // For nested fields (with dot notation), extract the actual field name for HTML search
            // Example: "lpClessUIOutcome.wValueQualifier" -> "wValueQualifier"
            var actualFieldName = fieldName;
            if (fieldName.Contains('.'))
            {
                actualFieldName = fieldName.Substring(fieldName.LastIndexOf('.') + 1);
            }
            
            // Get enum from EnumsManager (was collected in Pass 1)
            var enumDef = _enumsManager?.GetEnumDefinition(actualFieldName, commandName);
            if (enumDef != null && enumDef.Values.Count > 0)
            {
                // Strip array notation from type for enum definition
                var baseType = Regex.Replace(fieldType, @"\[.*?\]", "");
                
                // Get final enum name
                var finalEnumName = !string.IsNullOrEmpty(enumDef.FinalName) ? enumDef.FinalName : enumDef.RenamedName;
                
                enums[fieldName] = new EnumInfo
                {
                    Name = finalEnumName,
                    OriginalName = actualFieldName,
                    Type = baseType,
                    Commands = new List<string> { commandName },
                    Values = enumDef.Values
                };
            }
        }
        
        return enums;
    }

    private List<string> ExtractEnumValues(string htmlContent, string fieldName, string fieldType, string? nextFieldInStruct)
    {
        var values = new List<string>();
        
        // Primitive integer tipleri ve pointer'ları için enum ara
        var primitiveIntTypes = new[] { "WORD", "SHORT", "USHORT", "LONG", "ULONG", "DWORD",
                                        "LPWORD", "LPSHORT", "LPUSHORT", "LPLONG", "LPULONG", "LPDWORD" };
        
        // Strip array notation and [ZeroTerminated] for type checking
        var baseType = Regex.Replace(fieldType, @"\[.*?\]", "");
        
        if (!primitiveIntTypes.Contains(baseType, StringComparer.OrdinalIgnoreCase))
            return values;
        
        // <p><i>fieldName</i> veya <br/><i>fieldName</i> pattern'ini bul
        // Array fields için [...] opsiyonel olarak destekle (örn: dwGuidLights [...])
        // Match anything (including newlines) between field name and </i>
        // Find ALL occurrences and pick the one with "Value Meaning" section
        var fieldPattern = $@"(?:<p>|<br/>)<i>{Regex.Escape(fieldName)}.*?</i>";
        var matches = Regex.Matches(htmlContent, fieldPattern, RegexOptions.Singleline);
        
        if (matches.Count == 0)
            return values;
        
        Match? bestMatch = null;
        int bestFieldEndIndex = -1;
        
        // Try each occurrence and find one that has "Value Meaning"
        foreach (Match candidateMatch in matches)
        {
            var candidateStartIndex = candidateMatch.Index + candidateMatch.Length;
            int candidateEndIndex = htmlContent.Length;
            
            if (!string.IsNullOrEmpty(nextFieldInStruct))
            {
                var nextFieldPattern = $@"(?:<br/>|<p>)<i>{Regex.Escape(nextFieldInStruct)}";
                var nextFieldMatch = Regex.Match(htmlContent.Substring(candidateStartIndex), nextFieldPattern, RegexOptions.Singleline);
                
                if (nextFieldMatch.Success)
                {
                    candidateEndIndex = candidateStartIndex + nextFieldMatch.Index;
                }
            }
            
            var candidateSection = htmlContent.Substring(candidateStartIndex, candidateEndIndex - candidateStartIndex);
            if (candidateSection.Contains("Value Meaning", StringComparison.OrdinalIgnoreCase))
            {
                bestMatch = candidateMatch;
                bestFieldEndIndex = candidateEndIndex;
                break; // Found a good match
            }
        }
        
        if (bestMatch == null)
            return values;
        
        var match = bestMatch;
        var fieldEndIndex = bestFieldEndIndex;
        
        // Field'ın başlangıç indexi
        var fieldStartIndex = match.Index + match.Length;
        
        // Sadece bu field'a ait section'ı al (2000 karakter sınırı YOK, tüm field)
        var searchSection = htmlContent.Substring(fieldStartIndex, fieldEndIndex - fieldStartIndex);
        
        // "Value Meaning" kontrolü
        if (!searchSection.Contains("Value Meaning", StringComparison.OrdinalIgnoreCase))
            return values;
        
        // FIRST PASS: Collect initial values using ONLY Pattern 1 to determine common prefix
        var initialValues = new List<string>();
        
        // Pattern 1: Enum value with description on same line (at least 10 chars)
        // Support values starting with dot (e.g., ".WFS_IDC_RETAINNOTSUPP")
        var pattern1 = @"(?:<br/>|<p>)(\.?)(WFS_[A-Z_0-9]+)(?!\s*<br/>|\s*</p>)\s+[^\,\)].{10,}";
        var matches1 = Regex.Matches(searchSection, pattern1);
        foreach (Match m1 in matches1)
        {
            var enumValue = m1.Groups[2].Value; // Group 2 is the actual enum value (dot is group 1)
            if (!initialValues.Contains(enumValue))
                initialValues.Add(enumValue);
        }
        
        // Remove WFS_[DeviceName]_NOTSUPP values before finding common prefix
        // These generic values would incorrectly influence the common prefix calculation
        // Example: WFS_IDC_NOTSUPP should be excluded when finding prefix for WFS_IDC_CLESS_* values
        var notsuppPattern = @"^WFS_[A-Z]+_NOTSUPP$";
        initialValues.RemoveAll(v => Regex.IsMatch(v, notsuppPattern));
        
        // Determine common prefix for Pattern 3
        string commonPrefix = string.Empty;
        if (initialValues.Count > 0)
        {
            commonPrefix = FindLongestCommonPrefix(initialValues);
        }
        
        // SECOND PASS: Process line-by-line to preserve order
        var lines = searchSection.Split('\n');
        var skippedValues = new List<string>();
        
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            
            // Skip line if it doesn't contain "WFS_" (optimization)
            if (!line.Contains("WFS_"))
                continue;
            
            // Try Pattern 1: Value + description on same line
            var match1 = Regex.Match(line, pattern1);
            if (match1.Success)
            {
                var enumValue = match1.Groups[2].Value; // Group 2 is the actual enum value (dot is group 1)
                if (!values.Contains(enumValue))
                    values.Add(enumValue);
                continue;
            }
            
            // Try Pattern 2: Value alone on line (no description), next line is <br/></p>
            // Only accept if common prefix with initialValues > 9 chars
            if (i + 1 < lines.Length)
            {
                var nextLine = lines[i + 1].Trim();
                
                if (nextLine == "<br/></p>" || nextLine == "</p>")
                {
                    // Match value without description
                    var pattern2 = @"(?:<br/>|<p>)(\.?)(WFS_[A-Z_0-9]+)\s*$";
                    var match2 = Regex.Match(line, pattern2);
                    
                    if (match2.Success)
                    {
                        var enumValue = match2.Groups[2].Value;
                        // Check if this value shares a common prefix > 9 chars with commonPrefix
                        var sharedPrefix = FindLongestCommonPrefix(new List<string> { enumValue, commonPrefix });
                        if (sharedPrefix.Length > 9)
                        {
                            if (!values.Contains(enumValue))
                                values.Add(enumValue);
                            continue;
                        }
                    }
                }
            }
            
            // Try Pattern 3: Value with common prefix (if prefix > 9 chars)
            // Support values starting with dot
            if (commonPrefix.Length > 9)
            {
                // Build pattern to match any value starting with common prefix
                var escapedPrefix = Regex.Escape(commonPrefix);
                var pattern3 = $@"(?:<br/>|<p>)(\.?)({escapedPrefix}[A-Z_0-9]+)";
                var match3 = Regex.Match(line, pattern3);
                if (match3.Success)
                {
                    var enumValue = match3.Groups[2].Value; // Group 2 is the actual enum value (dot is group 1)
                    if (!values.Contains(enumValue))
                        values.Add(enumValue);
                    continue;
                }
            }
            
            // DIAGNOSTIC: Check if line has WFS_ value that was skipped
            // Support values starting with dot
            var diagnosticMatch = Regex.Match(line, @"(?:<br/>|<p>)(\.?)(WFS_[A-Z_0-9]+)");
            if (diagnosticMatch.Success)
            {
                var potentialValue = diagnosticMatch.Groups[2].Value; // Group 2 is the actual enum value (dot is group 1)
                if (!values.Contains(potentialValue) && !skippedValues.Contains(potentialValue))
                {
                    skippedValues.Add(potentialValue);
                }
            }
        }
        
        // Log skipped values for debugging
        if (skippedValues.Count > 0)
        {
            LogSkippedEnumValues(fieldName, skippedValues, searchSection, commonPrefix);
        }
        
        return values;
    }
    
    private static void LogSkippedEnumValues(string fieldName, List<string> skippedValues, string searchSection, string commonPrefix)
    {
        try
        {
            var logDirectory = "log";
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            
            var logPath = Path.Combine(logDirectory, "skipped-enum-values.log");
            
            using (var writer = new StreamWriter(logPath, append: true))
            {
                writer.WriteLine($"Field: {fieldName}");
                writer.WriteLine($"CommonPrefix: {commonPrefix} (len={commonPrefix.Length})");
                writer.WriteLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine($"Skipped values: {skippedValues.Count}");
                writer.WriteLine();
                
                foreach (var skippedValue in skippedValues)
                {
                    writer.WriteLine($"Skipped: {skippedValue}");
                    
                    // Find the line containing this value
                    var lines = searchSection.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(skippedValue))
                        {
                            writer.WriteLine($"  Current line: {lines[i].Trim()}");
                            if (i + 1 < lines.Length)
                            {
                                writer.WriteLine($"  Next line: {lines[i + 1].Trim()}");
                            }
                            break;
                        }
                    }
                    writer.WriteLine();
                }
                
                writer.WriteLine("---");
                writer.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not log skipped enum values: {ex.Message}");
        }
    }
    
    private static string FindLongestCommonPrefix(List<string> values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;
        
        if (values.Count == 1)
            return values[0];
        
        // Start with the first value as the potential prefix
        var prefix = values[0];
        
        // Compare with all other values
        for (int i = 1; i < values.Count; i++)
        {
            // Reduce prefix until it matches the start of current value
            while (!values[i].StartsWith(prefix))
            {
                if (prefix.Length == 0)
                    return string.Empty;
                
                // Remove last character from prefix
                prefix = prefix.Substring(0, prefix.Length - 1);
            }
        }
        
        return prefix;
    }
}

