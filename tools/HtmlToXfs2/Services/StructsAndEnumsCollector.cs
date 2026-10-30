using System.Text.Json;
using HtmlToXfs2.Model;

namespace HtmlToXfs2.Services;

public class StructsAndEnumsCollector
{
    private readonly StructsManager _structsManager;
    private readonly EnumsManager _enumsManager;

    public StructsAndEnumsCollector()
    {
        _structsManager = new StructsManager();
        _enumsManager = new EnumsManager();
    }

    public void CollectFromJsonFiles(string deviceName)
    {
        var baseDirectory = Path.Combine("int", "json", deviceName.ToLowerInvariant());
        var directories = new[] { "GetInfo", "Execute", "Events" };
        
        // Capitalize device name for enum naming (e.g., "idc" → "Idc")
        var capitalizedDeviceName = NameConverter.ExtractDeviceFromCommand($"WFS_INF_{deviceName.ToUpper()}_DUMMY");

        Console.WriteLine($"Collecting structs and enums from {deviceName} JSON files...");

        int totalFiles = 0;
        int totalStructs = 0;
        int totalEnums = 0;

        // PASS 1: Collect all enums first
        foreach (var dir in directories)
        {
            var dirPath = Path.Combine(baseDirectory, dir);

            if (!Directory.Exists(dirPath))
                continue;

            var jsonFiles = Directory.GetFiles(dirPath, "*.json");

            foreach (var jsonFile in jsonFiles)
            {
                try
                {
                    var commandName = Path.GetFileNameWithoutExtension(jsonFile);
                    var jsonContent = File.ReadAllText(jsonFile);
                    var command = JsonSerializer.Deserialize<WfsCommand>(jsonContent);

                    if (command == null)
                        continue;

                    // Collect enums from InputParam
                    if (command.InputParam?.Enums != null)
                    {
                        foreach (var enumEntry in command.InputParam.Enums)
                        {
                            var enumInfo = enumEntry.Value;
                            
                            _enumsManager.AddEnum(
                                enumInfo.OriginalName,
                                enumInfo.Type,
                                enumInfo.Values, 
                                commandName, 
                                capitalizedDeviceName);
                            totalEnums++;
                        }
                    }

                    // Collect enums from OutputParam
                    if (command.OutputParam?.Enums != null)
                    {
                        foreach (var enumEntry in command.OutputParam.Enums)
                        {
                            var enumInfo = enumEntry.Value;
                            
                            _enumsManager.AddEnum(
                                enumInfo.OriginalName,
                                enumInfo.Type,
                                enumInfo.Values, 
                                commandName, 
                                capitalizedDeviceName);
                            totalEnums++;
                        }
                    }

                    // Collect enums from AdditionalStructs
                    if (command.AdditionalStructs != null)
                    {
                        foreach (var structEntry in command.AdditionalStructs)
                        {
                            var additionalStruct = structEntry.Value;
                            if (additionalStruct.Enums != null)
                            {
                                foreach (var enumEntry in additionalStruct.Enums)
                                {
                                    var enumInfo = enumEntry.Value;
                                    
                                    _enumsManager.AddEnum(
                                        enumInfo.OriginalName,
                                        enumInfo.Type,
                                        enumInfo.Values, 
                                        commandName, 
                                        capitalizedDeviceName);
                                    totalEnums++;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    Error collecting enums from {Path.GetFileName(jsonFile)}: {ex.Message}");
                }
            }
        }

        Console.WriteLine($"  Enums collected: {totalEnums}");

        // Set enums reference in StructsManager for field type resolution
        _structsManager.SetEnumsReference(_enumsManager.GetAllEnums());

        // PASS 2: Collect all structs (with enum type resolution)
        foreach (var dir in directories)
        {
            var dirPath = Path.Combine(baseDirectory, dir);

            if (!Directory.Exists(dirPath))
            {
                Console.WriteLine($"  Directory not found: {dirPath}");
                continue;
            }

            var jsonFiles = Directory.GetFiles(dirPath, "*.json");
            Console.WriteLine($"  Processing {dir}: {jsonFiles.Length} files");

            foreach (var jsonFile in jsonFiles)
            {
                try
                {
                    var commandName = Path.GetFileNameWithoutExtension(jsonFile);
                    var jsonContent = File.ReadAllText(jsonFile);
                    var command = JsonSerializer.Deserialize<WfsCommand>(jsonContent);

                    if (command == null)
                        continue;

                    totalFiles++;

                    // Process InputParam structs
                    if (command.InputParam != null &&
                        !string.IsNullOrEmpty(command.InputParam.OriginalStructureName) && 
                        command.InputParam.Fields != null && 
                        command.InputParam.Fields.Count > 0)
                    {
                        _structsManager.AddStruct(
                            command.InputParam.OriginalStructureName, 
                            command.InputParam.Fields, 
                            commandName);
                        totalStructs++;
                    }

                    // Process OutputParam structs
                    if (command.OutputParam != null &&
                        !string.IsNullOrEmpty(command.OutputParam.OriginalStructureName) && 
                        command.OutputParam.Fields != null && 
                        command.OutputParam.Fields.Count > 0)
                    {
                        _structsManager.AddStruct(
                            command.OutputParam.OriginalStructureName, 
                            command.OutputParam.Fields, 
                            commandName);
                        totalStructs++;
                    }

                    // Process AdditionalStructs
                    if (command.AdditionalStructs != null)
                    {
                        foreach (var structEntry in command.AdditionalStructs)
                        {
                            var additionalStruct = structEntry.Value;
                            if (!string.IsNullOrEmpty(additionalStruct.OriginalStructureName) && 
                                additionalStruct.Fields != null && 
                                additionalStruct.Fields.Count > 0)
                            {
                                _structsManager.AddStruct(
                                    additionalStruct.OriginalStructureName, 
                                    additionalStruct.Fields, 
                                    commandName);
                                totalStructs++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    Error processing {Path.GetFileName(jsonFile)}: {ex.Message}");
                }
            }
        }

        Console.WriteLine($"  Total files processed: {totalFiles}");
        Console.WriteLine($"  Total structs found: {totalStructs}");
    }

    public void SaveResults(string deviceName)
    {
        Console.WriteLine("\nSaving collected structs and enums...");
        _structsManager.SaveToJson(deviceName);
        _enumsManager.SaveToJson(deviceName);
        
        Console.WriteLine($"\nSummary:");
        Console.WriteLine($"  Unique structs: {_structsManager.GetStructCount()}");
        Console.WriteLine($"  Unique enums: {_enumsManager.GetEnumCount()}");
    }
}
