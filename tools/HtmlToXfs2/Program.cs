using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using HtmlToXfs2.Model;
using HtmlToXfs2.Services;

namespace HtmlToXfs2
{
    internal class Program
    {
        private const string DefaultSpecVersion = "340";
        private static string _specVersion = DefaultSpecVersion;

        static void Main(string[] args)
        {
            var versionIndex = Array.IndexOf(args, "--version");
            if (versionIndex >= 0 && versionIndex + 1 < args.Length)
            {
                _specVersion = args[versionIndex + 1];
                args = args.Where((_, i) => i != versionIndex && i != versionIndex + 1).ToArray();
            }
            Console.WriteLine($"Spec version: {_specVersion}");

            // Clean up log files at the start
            CleanupLogFiles();
            
            // Quick markdown parse for prototype validation
            if (args.Length > 0 && args[0] == "--parse-md")
            {
                var markdownPath = args.Length > 1 ? args[1] : Path.Combine("pdf", _specVersion, "md_output", "XfsIdc.md");
                var deviceName = args.Length > 2 ? args[2] : "idc";
                ParseMarkdownPrototype(markdownPath, deviceName);
                return;
            }

            // Check for --validate flag
            if (args.Length > 0 && args[0] == "--validate")
            {
                RunValidation();
                return;
            }
            
            // Check for --insert-missing flag
            if (args.Length > 0 && args[0] == "--insert-missing")
            {
                RunInsertMissing();
                return;
            }
            
            // Check for --copy-output flag (only copy, no processing)
            if (args.Contains("--copy-output"))
            {
                RunCopyOutput();
                return;
            }
            
            Console.WriteLine("=== HTML Table of Contents Extractor ===\n");

            try
            {
                // Process devices in order: api first, then idc
                var devices = new[] { "api", "idc" };
                
                foreach (var device in devices)
                {
                    ProcessDevice(device);
                }

                Console.WriteLine($"\n=== TÜM İŞLEMLER BAŞARIYLA TAMAMLANDI ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                Console.WriteLine($"Detay: {ex}");
            }

        }

        private static void ParseMarkdownPrototype(string markdownPath, string deviceName)
        {
            if (!File.Exists(markdownPath))
            {
                Console.WriteLine($"Markdown file not found: {markdownPath}");
                return;
            }

            var parser = new MarkdownSpecParser();
            var commands = parser.ParseDevice(markdownPath, deviceName);

            Console.WriteLine($"Parsed {commands.Count} commands from {markdownPath}");

            var command = commands.FirstOrDefault(x => x.CommandName == "WFS_INF_IDC_STATUS") ?? commands.FirstOrDefault();
            if (command == null)
            {
                Console.WriteLine("No commands found.");
                return;
            }

            var json = JsonSerializer.Serialize(command, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);
        }

        private static bool TryProcessMarkdownDevice(string deviceName)
        {
            var markdownPath = Path.Combine("pdf", _specVersion, "md_output", $"Xfs{char.ToUpper(deviceName[0])}{deviceName.Substring(1).ToLowerInvariant()}.md");

            if (!File.Exists(markdownPath))
            {
                return false;
            }

            Console.WriteLine($"Markdown source found: {markdownPath}");

            var parser = new MarkdownSpecParser();
            var commands = parser.ParseDevice(markdownPath, deviceName);

            if (commands.Count == 0)
            {
                Console.WriteLine($"No commands parsed from markdown for {deviceName}.");
                return false;
            }

            var baseDirectory = Path.Combine("int", "json", deviceName.ToLowerInvariant());
            Directory.CreateDirectory(baseDirectory);

            foreach (var command in commands)
            {
                var commandDir = GetMdCommandDirectory(command.CommandName, deviceName);
                Directory.CreateDirectory(commandDir);

                var jsonPath = Path.Combine(commandDir, $"{command.CommandName}.json");
                var json = JsonSerializer.Serialize(command, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(jsonPath, json);
            }

            var collector = new StructsAndEnumsCollector();
            collector.CollectFromJsonFiles(deviceName);
            collector.SaveResults(deviceName);

            // Match the legacy pipeline: enrich header enums/structs and generate C# artifacts.
            Console.WriteLine("\n=== HEADER ENUM EXTRACTION İŞLEMİ ===");
            ExtractHeaderEnums(deviceName);

            Console.WriteLine("\n=== EKSİK STRUCT'LARI EKLEME İŞLEMİ ===");
            InsertMissingStructsForDevice(deviceName);

            Console.WriteLine("\n=== C# CONSTANTS EXPORT İŞLEMİ ===");
            var constantsExporter = new ConstantsExporter(deviceName);
            constantsExporter.Export();

            Console.WriteLine("\n=== C# ENUM EXPORT İŞLEMİ ===");
            var enumExporter = new EnumExporter(deviceName);
            enumExporter.Export();

            Console.WriteLine("\n=== C# STRUCT EXPORT İŞLEMİ ===");
            var structExporter = new StructExporter(deviceName);
            structExporter.Export();

            Console.WriteLine("\n=== DOĞRULAMA İŞLEMİ ===");
            RunValidationForDevice(deviceName);

            Console.WriteLine($"\n=== {deviceName.ToUpper()} MARKDOWN-FIRST SOURCE COMPLETED ===");
            Console.WriteLine($"JSON dosyaları '{baseDirectory}' dizinine kaydedildi.");
            Console.WriteLine($"Structs ve Enums '{Path.Combine("int", "json", deviceName.ToLowerInvariant(), "Structs.json")}', '{Path.Combine("int", "json", deviceName.ToLowerInvariant(), "Enums.json")}' dosyalarına kaydedildi.");
            return true;
        }

        private static string GetMdCommandDirectory(string commandName, string deviceName)
        {
            var normalized = commandName.ToUpperInvariant();

            if (normalized.Contains("WFS_INF_"))
            {
                return Path.Combine("int", "json", deviceName.ToLowerInvariant(), "GetInfo");
            }

            if (normalized.Contains("WFS_EVT_"))
            {
                return Path.Combine("int", "json", deviceName.ToLowerInvariant(), "Events");
            }

            return Path.Combine("int", "json", deviceName.ToLowerInvariant(), "Execute");
        }

        private static void ProcessDevice(string deviceName)
        {
            Console.WriteLine($"\n{'=',-60}");
            Console.WriteLine($"=== PROCESSING DEVICE: {deviceName.ToUpper()} ===");
            Console.WriteLine($"{'=',-60}\n");

            if (TryProcessMarkdownDevice(deviceName))
            {
                Console.WriteLine($"\n=== {deviceName.ToUpper()} MARKDOWN-FIRST SOURCE COMPLETED ===");
                return;
            }

            // HTML dosya yolunu belirle
            var inputPath = Path.Combine("input", $"Xfs{char.ToUpper(deviceName[0])}{deviceName.Substring(1).ToLower()}.html");
            
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Hata: HTML dosyası bulunamadı: {inputPath}");
                return;
            }

            Console.WriteLine($"HTML dosyası okunuyor: {inputPath}");

            // HTML dosyasını oku
            var htmlContent = File.ReadAllText(inputPath);
            Console.WriteLine($"HTML içeriği okundu ({htmlContent.Length:N0} karakter)");

            // Table of Contents'i oluştur
            var tocConstructor = new TocConstructor();
            var tableOfContents = tocConstructor.CreateTableOfContents(htmlContent);
            Console.WriteLine($"Table of Contents oluşturuldu. Toplam {tableOfContents.Count} öğe bulundu.\n");

            // Table of Contents'i listele
            Console.WriteLine("=== BULUNAN BAŞLIKLAR ===");
            foreach (var item in tableOfContents.Items)
            {
                var sectionNum = item.GetFullSectionNumber();
                var prefix = string.IsNullOrEmpty(sectionNum) ? "" : $"{sectionNum} ";
                Console.WriteLine($"  {prefix}{item.Title} (Sayfa {item.PageNumber}, Level {item.Level})");
            }

            // Table of Contents'i dosyaya kaydet
            var outputPath = Path.Combine("output", $"TableOfContents-{deviceName}.txt");
            Directory.CreateDirectory("output");
            
            using (var writer = new StreamWriter(outputPath))
            {
                writer.WriteLine($"=== TABLE OF CONTENTS - {deviceName.ToUpper()} ===");
                writer.WriteLine($"Toplam {tableOfContents.Count} başlık bulundu.\n");
                
                foreach (var item in tableOfContents.Items)
                {
                    var sectionNum = item.GetFullSectionNumber();
                    var prefix = string.IsNullOrEmpty(sectionNum) ? "" : $"{sectionNum} ";
                    writer.WriteLine($"{prefix}{item.Title} (Sayfa {item.PageNumber}, Level {item.Level})");
                }
            }

            Console.WriteLine($"Table of Contents '{outputPath}' dosyasına kaydedildi.\n");

            Console.WriteLine($"Device: {deviceName.ToUpperInvariant()}\n");

            // HtmlConstructor ile header'ları çıkar
            Console.WriteLine("=== HEADER ÇIKARMA İŞLEMİ ===");
            var htmlConstructor = new HtmlConstructor(htmlContent, tableOfContents);
            htmlConstructor.ExtractHeaders(deviceName);

            // WFS HTML Formatter ile Level 2 dosyalarını işle
            Console.WriteLine("\n=== WFS HTML FORMATTER İŞLEMİ ===");
            var level2Directory = Path.Combine("int", "parts", deviceName, "level2");
            var wfsFormatter = new WfsHtmlFormatter();
            wfsFormatter.ProcessLevel2Files(level2Directory, deviceName);

            // HTML Changer ile oluşturulan HTML dosyalarını düzenle
            Console.WriteLine("\n=== HTML CHANGER İŞLEMİ ===");
            ProcessHtmlFiles(deviceName);

            // NOT: Constants export işlemi JSON oluşturulduktan sonra yapılacak

            // JSON oluşturma
            Console.WriteLine("\n=== JSON OLUŞTURMA İŞLEMİ ===");
            ProcessJsonFiles(deviceName);

            // Struct ve Enum toplama
            Console.WriteLine("\n=== STRUCT VE ENUM TOPLAMA İŞLEMİ ===");
            var collector = new StructsAndEnumsCollector();
            collector.CollectFromJsonFiles(deviceName);
            collector.SaveResults(deviceName);

            // Header Enum Extraction - Extract enums from header file sections
            // Bu adım SaveResults'tan SONRA çalışmalı çünkü SaveResults Enums.json'u üzerine yazıyor
            Console.WriteLine("\n=== HEADER ENUM EXTRACTION İŞLEMİ ===");
            ExtractHeaderEnums(deviceName);

            // Insert missing structs from header file
            Console.WriteLine("\n=== EKSİK STRUCT'LARI EKLEME İŞLEMİ ===");
            InsertMissingStructsForDevice(deviceName);

            // C# Constants Export - Constants.json'dan C# dosyası oluştur
            Console.WriteLine("\n=== C# CONSTANTS EXPORT İŞLEMİ ===");
            var constantsExporter = new ConstantsExporter(deviceName);
            constantsExporter.Export();

            // C# Enum Export - Enums.json'dan C# enum dosyaları oluştur
            Console.WriteLine("\n=== C# ENUM EXPORT İŞLEMİ ===");
            var enumExporter = new EnumExporter(deviceName);
            enumExporter.Export();

            // C# Struct Export - Structs.json'dan C# struct dosyaları oluştur
            Console.WriteLine("\n=== C# STRUCT EXPORT İŞLEMİ ===");
            var structExporter = new StructExporter(deviceName);
            structExporter.Export();

            // Validation - always at the end
            Console.WriteLine("\n=== DOĞRULAMA İŞLEMİ ===");
            RunValidationForDevice(deviceName);

            Console.WriteLine($"\n=== {deviceName.ToUpper()} BAŞARIYLA TAMAMLANDI ===");
            Console.WriteLine($"Level 1 Headers 'int/parts/{deviceName}/level1' dizinine kaydedildi.");
            Console.WriteLine($"Level 2 Headers 'int/parts/{deviceName}/level2' dizinine kaydedildi.");
            Console.WriteLine($"WFS Commands (ham) 'int/html_unformatted/{deviceName}/GetInfo', 'int/html_unformatted/{deviceName}/Execute', 'int/html_unformatted/{deviceName}/Events' dizinlerine kaydedildi.");
            Console.WriteLine($"WFS Commands (formatlanmış) 'int/html/{deviceName}/GetInfo', 'int/html/{deviceName}/Execute', 'int/html/{deviceName}/Events' dizinlerine kaydedildi.");
            Console.WriteLine($"JSON dosyaları 'int/json/{deviceName}/GetInfo', 'int/json/{deviceName}/Execute', 'int/json/{deviceName}/Events' dizinlerine kaydedildi.");
            Console.WriteLine($"Structs ve Enums 'int/json/{deviceName}/Structs.json', 'int/json/{deviceName}/Enums.json' dosyalarına kaydedildi.");
        }

        private static void ProcessHtmlFiles(string deviceName)
        {
            var sourceBaseDirectory = Path.Combine("int", "html_unformatted", deviceName.ToLowerInvariant());
            var targetBaseDirectory = Path.Combine("int", "html", deviceName.ToLowerInvariant());
            var directories = new[] { "GetInfo", "Execute", "Events" };

            int totalProcessed = 0;
            
            foreach (var dir in directories)
            {
                var sourceDirPath = Path.Combine(sourceBaseDirectory, dir);
                var targetDirPath = Path.Combine(targetBaseDirectory, dir);
                
                if (!Directory.Exists(sourceDirPath))
                {
                    Console.WriteLine($"  Kaynak dizin bulunamadı: {sourceDirPath}");
                    continue;
                }

                var htmlFiles = Directory.GetFiles(sourceDirPath, "*.html");
                Console.WriteLine($"  {dir} dizininde {htmlFiles.Length} HTML dosyası bulundu.");

                foreach (var sourceFile in htmlFiles)
                {
                    try
                    {
                        var fileName = Path.GetFileName(sourceFile);
                        var targetFile = Path.Combine(targetDirPath, fileName);
                        
                        var htmlChanger = new HtmlChanger(sourceFile, targetFile, deviceName);
                        htmlChanger.ProcessHtmlFile();
                        totalProcessed++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    Hata: {Path.GetFileName(sourceFile)} - {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"  Toplam {totalProcessed} HTML dosyası işlendi.");
        }
        
        /// <summary>
        /// JSON test fonksiyonu - bir HTML dosyasından JSON oluşturur
        /// </summary>
        private static void TestJsonGeneration()
        {
            Console.WriteLine("\n=== JSON OLUŞTURMA TESTİ ===");
            
            // Test için bir HTML dosyası seç
            var testFile = Path.Combine("int", "html", "idc", "GetInfo", "WFS_INF_IDC_STATUS.html");
            
            if (File.Exists(testFile))
            {
                try
                {
                    var jsonConstructor = new WfsJsonConstructor(testFile);
                    var json = jsonConstructor.GenerateJson();
                    
                    Console.WriteLine($"Test dosyası: {testFile}");
                    Console.WriteLine("Oluşturulan JSON:");
                    Console.WriteLine(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"JSON oluşturma hatası: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Test dosyası bulunamadı: {testFile}");
            }
        }

        /// <summary>
        /// Tüm HTML dosyalarından JSON oluşturur (iki aşamalı)
        /// Pass 1: Enum'ları topla (collision handling burada olur)
        /// Pass 2: JSON'ları oluştur (GetFinalEnumName doğru name döndürür)
        /// </summary>
        private static void ProcessJsonFiles(string deviceName)
        {
            var sourceBaseDirectory = Path.Combine("int", "html", deviceName.ToLowerInvariant());
            var directories = new[] { "GetInfo", "Execute", "Events" };

            // Create shared EnumsManager for both passes
            var enumsManager = new EnumsManager();

            // PASS 1: Collect all enums first (collision handling happens here)
            Console.WriteLine("  Pass 1: Enum'lar toplanıyor...");
            int enumsCollected = 0;
            
            foreach (var dir in directories)
            {
                var sourceDirPath = Path.Combine(sourceBaseDirectory, dir);
                
                if (!Directory.Exists(sourceDirPath))
                    continue;

                var htmlFiles = Directory.GetFiles(sourceDirPath, "*.html");

                foreach (var htmlFile in htmlFiles)
                {
                    try
                    {
                        var jsonConstructor = new WfsJsonConstructor(htmlFile, enumsManager);
                        jsonConstructor.CollectEnumsOnly();
                        enumsCollected++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    Hata (enum): {Path.GetFileName(htmlFile)} - {ex.Message}");
                    }
                }
            }
            Console.WriteLine($"    {enumsCollected} dosyadan enum toplandı.");

            // PASS 2: Generate JSON files (GetFinalEnumName now returns correct names)
            Console.WriteLine("  Pass 2: JSON dosyaları oluşturuluyor...");
            int totalProcessed = 0;
            
            foreach (var dir in directories)
            {
                var sourceDirPath = Path.Combine(sourceBaseDirectory, dir);
                
                if (!Directory.Exists(sourceDirPath))
                {
                    Console.WriteLine($"  Kaynak dizin bulunamadı: {sourceDirPath}");
                    continue;
                }

                var htmlFiles = Directory.GetFiles(sourceDirPath, "*.html");
                Console.WriteLine($"  {dir} dizininde {htmlFiles.Length} HTML dosyası bulundu.");

                foreach (var htmlFile in htmlFiles)
                {
                    try
                    {
                        var jsonConstructor = new WfsJsonConstructor(htmlFile, enumsManager);
                        jsonConstructor.GenerateAndSaveJson(deviceName);
                        totalProcessed++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"    Hata: {Path.GetFileName(htmlFile)} - {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"  Toplam {totalProcessed} JSON dosyası oluşturuldu.");
        }
        
        private static void CleanupLogFiles()
        {
            try
            {
                var logDirectory = "log";
                if (Directory.Exists(logDirectory))
                {
                    // Clean up log files at startup
                    var logFiles = new[] { "skipped-enum-values.log" };
                    foreach (var logFile in logFiles)
                    {
                        var logPath = Path.Combine(logDirectory, logFile);
                        if (File.Exists(logPath))
                        {
                            File.Delete(logPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Silently ignore cleanup errors
                Console.WriteLine($"Warning: Could not clean up log files: {ex.Message}");
            }
        }
        
        private static void RunCopyOutput()
        {
            Console.WriteLine("=== COPY OUTPUT FILES ===\n");
            
            var devices = new[] { "api", "idc" };
            var outputCopier = new OutputCopier();
            
            foreach (var device in devices)
            {
                Console.WriteLine($"\nCopying files for {device.ToUpper()}...");
                
                Console.WriteLine("  [Constants]");
                outputCopier.CopyConstants(device);
                
                Console.WriteLine("  [Structs]");
                outputCopier.CopyStructs(device);
            }
            
            Console.WriteLine("\n=== COPY COMPLETED ===");
        }
        
        private static void RunValidation()
        {
            Console.WriteLine("=== HEADER VALIDATION ===\n");
            RunValidationForDevice("idc");
        }
        
        private static void RunValidationForDevice(string deviceName)
        {
            try
            {
                // For "api" device, use XFSAPI.H, otherwise use XFS{DEVICE}.H
                var headerFileName = deviceName.ToLower() == "api" 
                    ? "XFSAPI.H" 
                    : $"XFS{deviceName.ToUpper()}.H";
                var headerPath = Path.Combine("INCLUDE", headerFileName);
                var structsPath = Path.Combine("int", "json", deviceName, "Structs.json");
                var enumsPath = Path.Combine("int", "json", deviceName, "Enums.json");
                
                if (!File.Exists(headerPath))
                {
                    Console.WriteLine($"  Header file not found: {headerPath}, skipping validation...");
                    return;
                }
                
                if (!File.Exists(structsPath))
                {
                    Console.WriteLine($"  Structs JSON not found: {structsPath}, skipping validation...");
                    return;
                }
                
                if (!File.Exists(enumsPath))
                {
                    Console.WriteLine($"  Enums JSON not found: {enumsPath}, skipping validation...");
                    return;
                }
                
                Console.WriteLine($"  Device: {deviceName.ToUpper()}");
                Console.WriteLine($"  Header: {headerPath}");
                Console.WriteLine($"  Structs: {structsPath}");
                Console.WriteLine($"  Enums: {enumsPath}");
                Console.WriteLine();
                
                var validator = new HeaderValidator(headerPath, structsPath, enumsPath);
                var report = validator.Validate();
                var reportText = validator.GenerateReport(report);
                
                // Save report to log directory with device name
                var logDirectory = "log";
                Directory.CreateDirectory(logDirectory);
                var reportPath = Path.Combine(logDirectory, $"validation-report-{deviceName}.md");
                File.WriteAllText(reportPath, reportText);
                
                Console.WriteLine(reportText);
                Console.WriteLine($"  ✅ Report saved to: {reportPath}");
                
                // Summary
                var structsMatch = report.MissingStructs.Count == 0 && report.MissingStructFields.Count == 0;
                var enumsMatch = report.MissingEnumValues.Count == 0;
                
                if (structsMatch && enumsMatch)
                {
                    Console.WriteLine("\n  🎉 All validation checks passed!");
                }
                else if (structsMatch)
                {
                    Console.WriteLine("\n  ✅ Structs validation passed");
                    Console.WriteLine("  ⚠️  Some enum values missing (may be intentional)");
                }
                else
                {
                    Console.WriteLine("\n  ❌ Validation found issues - check report for details");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Warning: Validation error: {ex.Message}");
            }
        }
        
        private static void RunInsertMissing()
        {
            Console.WriteLine("=== INSERT MISSING STRUCTS ===\n");
            
            try
            {
                // Validate for IDC device
                var deviceName = "idc";
                var headerPath = Path.Combine("INCLUDE", $"XFS{deviceName.ToUpper()}.H");
                var structsPath = Path.Combine("int", "json", deviceName, "Structs.json");
                var enumsPath = Path.Combine("int", "json", deviceName, "Enums.json");
                
                if (!File.Exists(headerPath))
                {
                    Console.WriteLine($"Error: Header file not found: {headerPath}");
                    return;
                }
                
                if (!File.Exists(structsPath))
                {
                    Console.WriteLine($"Error: Structs JSON not found: {structsPath}");
                    return;
                }
                
                Console.WriteLine($"Device: {deviceName.ToUpper()}");
                Console.WriteLine($"Header: {headerPath}");
                Console.WriteLine($"Structs: {structsPath}");
                Console.WriteLine();
                
                // First, run validation to find missing structs
                Console.WriteLine("Running validation to identify missing structs...");
                var validator = new HeaderValidator(headerPath, structsPath, enumsPath);
                var report = validator.Validate();
                
                if (report.MissingStructs.Count == 0)
                {
                    Console.WriteLine("\n✅ No missing structs found. All structs from header are already in JSON.");
                    return;
                }
                
                Console.WriteLine($"\nFound {report.MissingStructs.Count} missing struct(s):");
                foreach (var item in report.MissingStructs)
                {
                    Console.WriteLine($"  - {item.Name} → {item.ConvertedName}");
                }
                
                Console.WriteLine("\nDo you want to insert these structs? (y/n): ");
                var response = Console.ReadLine();
                
                if (response?.ToLower() != "y")
                {
                    Console.WriteLine("Operation cancelled.");
                    return;
                }
                
                // Insert missing structs
                validator.InsertMissingStructs(report);
                
                Console.WriteLine("\n🎉 Done! Run 'dotnet run --validate' to verify.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert error: {ex.Message}");
                Console.WriteLine($"Details: {ex}");
            }
        }
        
        private static void ExtractHeaderEnums(string deviceName)
        {
            try
            {
                var headerFileName = deviceName.ToLower() == "api" 
                    ? "XFSAPI.H" 
                    : $"XFS{deviceName.ToUpper()}.H";
                var headerPath = Path.Combine("INCLUDE", headerFileName);
                var outputPath = Path.Combine("int", "json", deviceName.ToLower(), "HeaderEnums.json");
                
                var extractor = new HeaderEnumExtractor();
                
                if (deviceName.ToLower() == "api")
                {
                    extractor.AddRule("Error codes", "XfsErrorCode", "HRESULT", "WFS_SUCCESS", "WFS_ERR_");
                    extractor.AddRule("Messages", "XfsMessage", "DWORD");
                    extractor.AddRule("Event Classes", "XfsEventClasses", "DWORD");
                    extractor.AddRule("System Event IDs", "XfsEventId", "DWORD");
                    extractor.AddRule("XFS Trace Level", "XfsTraceLevels", "DWORD");
                    extractor.AddRule("XFS Error Actions", "XfsErrorActions", "DWORD");
                }
                else
                {
                    var deviceUpper = deviceName.ToUpper();
                    var deviceCapitalized = char.ToUpper(deviceName[0]) + deviceName.Substring(1).ToLower();
                    
                    extractor.AddRule($"{deviceUpper} Info Commands", $"{deviceCapitalized}InfoCommandId", "DWORD");
                    extractor.AddRule($"{deviceUpper} Execute Commands", $"{deviceCapitalized}ExecuteCommandId", "DWORD");
                    extractor.AddRule($"{deviceUpper} Messages", $"{deviceCapitalized}EventId", "DWORD");
                }
                
                extractor.ExtractAndSave(headerPath, outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Warning: Header enum extraction error: {ex.Message}");
            }
        }
        
        private static void InsertMissingStructsForDevice(string deviceName)
        {
            try
            {
                // For "api" device, use XFSAPI.H, otherwise use XFS{DEVICE}.H
                var headerFileName = deviceName.ToLower() == "api" 
                    ? "XFSAPI.H" 
                    : $"XFS{deviceName.ToUpper()}.H";
                var headerPath = Path.Combine("INCLUDE", headerFileName);
                var structsPath = Path.Combine("int", "json", deviceName, "Structs.json");
                var enumsPath = Path.Combine("int", "json", deviceName, "Enums.json");
                
                if (!File.Exists(headerPath))
                {
                    Console.WriteLine($"  Header file not found: {headerPath}, skipping...");
                    return;
                }
                
                if (!File.Exists(structsPath))
                {
                    Console.WriteLine($"  Structs JSON not found: {structsPath}, skipping...");
                    return;
                }
                
                // Run validation to find missing structs
                var validator = new HeaderValidator(headerPath, structsPath, enumsPath);
                var report = validator.Validate();
                
                if (report.MissingStructs.Count == 0)
                {
                    Console.WriteLine("  ✅ No missing structs found. All structs from header are already in JSON.");
                    return;
                }
                
                Console.WriteLine($"  Found {report.MissingStructs.Count} missing struct(s) from header file.");
                
                // Insert missing structs automatically
                validator.InsertMissingStructs(report);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Warning: Could not insert missing structs: {ex.Message}");
            }
        }
        
    }
}
