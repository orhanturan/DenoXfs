using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services
{
    public class WfsHtmlFormatter
    {
        public void ProcessLevel2Files(string level2Directory, string deviceName)
        {
            Console.WriteLine($"WFS HTML Formatter başlatılıyor: {level2Directory}");
            
            if (!Directory.Exists(level2Directory))
            {
                Console.WriteLine($"HATA: Level 2 dizini bulunamadı: {level2Directory}");
                return;
            }

            var wfsFiles = FindWfsFiles(level2Directory);
            Console.WriteLine($"{wfsFiles.Count} adet WFS_ dosyası bulundu.");

            if (wfsFiles.Count == 0)
            {
                Console.WriteLine("İşlenecek WFS_ dosyası bulunamadı.");
                return;
            }

            foreach (var file in wfsFiles)
            {
                try
                {
                    Console.WriteLine($"İşleniyor: {file.Name}");
                    
                    var formattedContent = FormatHtmlContent(file.Content);
                    var wrappedContent = WrapWithHtml(formattedContent);
                    var commandName = ExtractCommandName(file.Name);
                    var targetDirectory = DetermineTargetDirectory(commandName, deviceName);
                    
                    SaveFormattedFile(commandName, wrappedContent, targetDirectory);
                    
                    Console.WriteLine($"  -> {commandName}.html olarak {targetDirectory} dizinine kaydedildi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"HATA: {file.Name} işlenirken hata oluştu: {ex.Message}");
                }
            }
            
            Console.WriteLine("WFS HTML Formatter tamamlandı.\n");
        }

        private List<WfsFileInfo> FindWfsFiles(string level2Directory)
        {
            var wfsFiles = new List<WfsFileInfo>();
            
            var htmlFiles = Directory.GetFiles(level2Directory, "*.html");
            
            foreach (var filePath in htmlFiles)
            {
                var fileName = Path.GetFileName(filePath);
                
                // WFS_ ile başlayan dosyaları bul
                if (fileName.Contains("WFS_", StringComparison.OrdinalIgnoreCase))
                {
                    var content = File.ReadAllText(filePath);
                    wfsFiles.Add(new WfsFileInfo
                    {
                        Name = fileName,
                        Path = filePath,
                        Content = content
                    });
                }
            }
            
            return wfsFiles.OrderBy(f => f.Name).ToList();
        }

        private string ExtractCommandName(string fileName)
        {
            // "4_1_WFS_INF_IDC_STATUS.html" -> "WFS_INF_IDC_STATUS"
            var match = Regex.Match(fileName, @"WFS_[A-Z_]+", RegexOptions.IgnoreCase);
            return match.Success ? match.Value.ToUpperInvariant() : Path.GetFileNameWithoutExtension(fileName);
        }

        private string FormatHtmlContent(string htmlContent)
        {
            // Sadece formatlama - içerik temizleme yok
            var lines = htmlContent.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            var formattedLines = new List<string>();
            
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    formattedLines.Add($"    {trimmedLine}"); // 4 boşluk indent
                }
                else if (string.IsNullOrWhiteSpace(line) && formattedLines.Count > 0)
                {
                    // Boş satırları koru ama sadece içerik varsa
                    formattedLines.Add("");
                }
            }
            
            return string.Join(Environment.NewLine, formattedLines);
        }

        private string WrapWithHtml(string formattedContent)
        {
            return $@"<!DOCTYPE html>
<html>
<head>
</head>
<body>
{formattedContent}
</body>
</html>";
        }

        private string DetermineTargetDirectory(string commandName, string deviceName)
        {
            var baseDir = Path.Combine("int", "html_unformatted", deviceName.ToLowerInvariant());
            
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

        private void SaveFormattedFile(string commandName, string wrappedContent, string targetDirectory)
        {
            // Hedef dizini oluştur
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
                Console.WriteLine($"  Dizin oluşturuldu: {targetDirectory}");
            }
            
            var fileName = $"{commandName}.html";
            var filePath = Path.Combine(targetDirectory, fileName);
            
            File.WriteAllText(filePath, wrappedContent);
        }

        private class WfsFileInfo
        {
            public string Name { get; set; } = string.Empty;
            public string Path { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
        }
    }
}
