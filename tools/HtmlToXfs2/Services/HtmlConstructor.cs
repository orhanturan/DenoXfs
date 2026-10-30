using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlToXfs2.Model;

namespace HtmlToXfs2.Services
{
    /// <summary>
    /// HTML içeriğini parse ederek Table of Contents kullanarak organize edilmiş dosyalar oluşturur
    /// </summary>
    public class HtmlConstructor
    {
        private readonly string _htmlContent;
        private readonly TableOfContents _tableOfContents;
        private readonly string _outputBasePath;

        public HtmlConstructor(string htmlContent, TableOfContents tableOfContents, string outputBasePath = "output")
        {
            _htmlContent = htmlContent ?? throw new ArgumentNullException(nameof(htmlContent));
            _tableOfContents = tableOfContents ?? throw new ArgumentNullException(nameof(tableOfContents));
            _outputBasePath = outputBasePath;
        }

        /// <summary>
        /// Dosya adından device ismini çıkarır (örn: XfsIdc.html -> idc)
        /// </summary>
        public static string ExtractDeviceName(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            
            // Xfs prefix'ini kaldır ve lowercase'e çevir
            if (fileName.StartsWith("Xfs", StringComparison.OrdinalIgnoreCase))
            {
                return fileName.Substring(3).ToLowerInvariant();
            }
            
            return fileName.ToLowerInvariant();
        }

        /// <summary>
        /// Device adından header'ları çıkarır ve kaydeder (Yeni Index-based yaklaşım)
        /// </summary>
        public void ExtractHeaders(string deviceName)
        {
            var level1Headers = _tableOfContents.Items.Where(x => x.Level == 1 && x.HtmlStartIndex > 0).ToList();
            var level2Headers = _tableOfContents.Items.Where(x => x.Level == 2 && x.HtmlStartIndex > 0).ToList();
            
            if (!level1Headers.Any() && !level2Headers.Any())
            {
                Console.WriteLine("Geçerli pozisyona sahip header bulunamadı.");
                return;
            }

            var level1Path = Path.Combine("int", "parts", deviceName, "level1");
            var level2Path = Path.Combine("int", "parts", deviceName, "level2");
            Directory.CreateDirectory(level1Path);
            Directory.CreateDirectory(level2Path);

            Console.WriteLine($"Level 1 Headers: {level1Headers.Count}, Level 2 Headers: {level2Headers.Count}");

            // Level 1 headers
            foreach (var header in level1Headers)
            {
                try
                {
                    var content = ExtractContentByIndex(header);
                    if (!string.IsNullOrEmpty(content))
                    {
                        SaveHeaderFile(header, content, level1Path, deviceName);
                        Console.WriteLine($"✓ Level 1 kaydedildi: {header.Title}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Level 1 içerik boş: {header.Title}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata - Level 1 {header.Title}: {ex.Message}");
                }
            }

            // Level 2 headers
            foreach (var header in level2Headers)
            {
                try
                {
                    var content = ExtractContentByIndex(header);
                    if (!string.IsNullOrEmpty(content))
                    {
                        SaveHeaderFile(header, content, level2Path, deviceName);
                        Console.WriteLine($"✓ Level 2 kaydedildi: {header.Title}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Level 2 içerik boş: {header.Title}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata - Level 2 {header.Title}: {ex.Message}");
                }
            }

            Console.WriteLine($"Headers '{level1Path}' ve '{level2Path}' dizinlerine kaydedildi.");
        }

        /// <summary>
        /// Index pozisyonlarını kullanarak içerik çıkarır (Basit ve güvenilir)
        /// </summary>
        private string ExtractContentByIndex(TocItem header)
        {
            if (header.HtmlStartIndex == -1)
            {
                Console.WriteLine($"Geçersiz başlangıç pozisyonu: {header.Title}");
                return string.Empty;
            }
            
            var startIndex = header.HtmlStartIndex;
            var endIndex = header.HtmlEndIndex > startIndex ? header.HtmlEndIndex : _htmlContent.Length;
            
            Console.WriteLine($"İçerik çıkarılıyor: {header.Title} [{startIndex} - {endIndex}]");
            
            try
            {
                var content = _htmlContent.Substring(startIndex, endIndex - startIndex).Trim();
                Console.WriteLine($"Çıkarılan içerik uzunluğu: {content.Length} karakter");
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Substring hatası: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Header dosyasını kaydeder
        /// </summary>
        private void SaveHeaderFile(TocItem header, string content, string outputDirectory, string deviceName)
        {
            var fileName = CreateHeaderFileName(header);
            var filePath = Path.Combine(outputDirectory, fileName);
            
            var htmlDocument = CreateSimpleHtmlDocument(header, content, deviceName);
            File.WriteAllText(filePath, htmlDocument);
        }

        /// <summary>
        /// Tüm GetInfo komutlarını çıkarır ve infGetInfo dizininde organize eder
        /// </summary>
        public void ExtractGetInfoCommands()
        {
            var getInfoCommands = FindGetInfoCommands();
            
            if (!getInfoCommands.Any())
            {
                Console.WriteLine("GetInfo komutları bulunamadı.");
                return;
            }

            var infGetInfoPath = Path.Combine(_outputBasePath, "infGetInfo");
            Directory.CreateDirectory(infGetInfoPath);

            Console.WriteLine($"Toplam {getInfoCommands.Count} GetInfo komutu bulundu.");

            foreach (var command in getInfoCommands)
            {
                try
                {
                    ExtractAndSaveCommand(command, infGetInfoPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata - {command.Title}: {ex.Message}");
                }
            }

            Console.WriteLine($"GetInfo komutları {infGetInfoPath} dizinine kaydedildi.");
        }

        /// <summary>
        /// Belirtilen seviyedeki başlıkları bulur
        /// </summary>
        private List<TocItem> FindHeadersByLevel(int level)
        {
            var headers = new List<TocItem>();

            foreach (var item in _tableOfContents.Items)
            {
                if (item.Level == level && !string.IsNullOrEmpty(item.SectionNumber))
                {
                    headers.Add(item);
                }
            }

            // Bölüm numarasına göre sırala
            headers.Sort((x, y) => 
            {
                if (int.TryParse(x.SectionNumber, out int xNum) && 
                    int.TryParse(y.SectionNumber, out int yNum))
                    return xNum.CompareTo(yNum);
                return string.Compare(x.SectionNumber, y.SectionNumber, StringComparison.Ordinal);
            });

            return headers;
        }

        /// <summary>
        /// Belirli bir header'ı çıkarır ve dosyaya kaydeder
        /// </summary>
        private void ExtractAndSaveHeader(TocItem header, string outputDirectory, string deviceName)
        {
            var sectionContent = ExtractSectionContent(header);
            
            if (string.IsNullOrEmpty(sectionContent))
            {
                Console.WriteLine($"İçerik bulunamadı: {header.Title}");
                return;
            }

            // Dosya adını oluştur
            var fileName = CreateHeaderFileName(header);
            var filePath = Path.Combine(outputDirectory, fileName);

            // Basit HTML dosyası oluştur
            var htmlDocument = CreateSimpleHtmlDocument(header, sectionContent, deviceName);
            
            File.WriteAllText(filePath, htmlDocument);
            Console.WriteLine($"Oluşturuldu: {fileName} (Level {header.Level})");
        }

        /// <summary>
        /// Header için dosya adı oluşturur
        /// </summary>
        private string CreateHeaderFileName(TocItem header)
        {
            var title = CleanTitleForSearch(header.Title ?? ""); // Nokta ve sayfa numaralarını temizle
            title = SanitizeFileName(title);
            
            if (header.Level == 1)
            {
                // Level 1: 4_Introduction.html
                return string.IsNullOrEmpty(header.SectionNumber) 
                    ? $"{title}.html" 
                    : $"{header.SectionNumber}_{title}.html";
            }
            else if (header.Level == 2)
            {
                // Level 2: 4_1_WFS_INF_IDC_STATUS.html
                var fullSection = $"{header.SectionNumber}_{header.SubSectionNumber}";
                return string.IsNullOrEmpty(header.SectionNumber) 
                    ? $"{title}.html" 
                    : $"{fullSection}_{title}.html";
            }
            else
            {
                // Diğer level'lar için fallback
                return string.IsNullOrEmpty(header.SectionNumber) 
                    ? $"{title}.html" 
                    : $"{header.SectionNumber}_{title}.html";
            }
        }

        /// <summary>
        /// Header içeriğini HTML'den çıkarır
        /// </summary>
        private string ExtractSectionContent(TocItem header)
        {
            try
            {
                // Gerçek bölüm başlığını HTML'de bul (Table of Contents değil, gerçek içerik)
                var sectionNumber = header.SectionNumber ?? "";
                var title = CleanTitleForSearch(header.Title ?? "");
                
                // Gerçek bölüm başlığı formatları (page-break sonrası)
                var headerPatterns = new[]
                {
                    $@"<p><b>{Regex.Escape(sectionNumber)}\.\s*{Regex.Escape(title)}\s*<br/></b></p>",
                    $@"<b>{Regex.Escape(sectionNumber)}\.\s*{Regex.Escape(title)}\s*<br/></b>",
                    $@"<b>{Regex.Escape(sectionNumber)}\.\s*{Regex.Escape(title)}\s*</b>"
                };

                Match? match = null;
                int matchIndex = -1;
                
                foreach (var pattern in headerPatterns)
                {
                    var matches = Regex.Matches(_htmlContent, pattern, RegexOptions.IgnoreCase);
                    
                    // Table of Contents'ten sonraki eşleşmeyi bul
                    foreach (Match m in matches)
                    {
                        // Table of Contents bölümünden sonra olan eşleşmeyi al
                        if (m.Index > 1000) // Table of Contents genellikle dosyanın başında
                        {
                            match = m;
                            matchIndex = m.Index;
                            break;
                        }
                    }
                    
                    if (match != null && match.Success)
                        break;
                }

                if (match == null || !match.Success)
                {
                    Console.WriteLine($"Gerçek başlık bulunamadı: {sectionNumber}. {title}");
                    return string.Empty;
                }

                // Sonraki Level 1 başlığı bul
                var nextHeader = FindNextLevel1Header(header);
                var endPosition = nextHeader != null ? 
                    FindRealHeaderInHtml(nextHeader) : _htmlContent.Length;

                if (endPosition == -1 || endPosition <= matchIndex)
                    endPosition = _htmlContent.Length;

                // İçeriği çıkar
                var content = _htmlContent.Substring(matchIndex, endPosition - matchIndex);
                
                return content.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"İçerik çıkarma hatası - {header.Title}: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Başlık metnini arama için temizler
        /// </summary>
        private string CleanTitleForSearch(string title)
        {
            // Table of Contents'teki nokta ve sayfa numaralarını kaldır
            title = Regex.Replace(title, @"\.+\s*\d+$", "").Trim();
            return title;
        }

        /// <summary>
        /// Gerçek başlığı HTML'de bulur (Table of Contents değil)
        /// </summary>
        private int FindRealHeaderInHtml(TocItem header)
        {
            var sectionNumber = header.SectionNumber ?? "";
            var title = CleanTitleForSearch(header.Title ?? "");
            
            var headerPatterns = new[]
            {
                $@"<p><b>{Regex.Escape(sectionNumber)}\.\s*{Regex.Escape(title)}\s*<br/></b></p>",
                $@"<b>{Regex.Escape(sectionNumber)}\.\s*{Regex.Escape(title)}\s*<br/></b>",
                $@"<b>{Regex.Escape(sectionNumber)}\.\s*{Regex.Escape(title)}\s*</b>"
            };

            foreach (var pattern in headerPatterns)
            {
                var matches = Regex.Matches(_htmlContent, pattern, RegexOptions.IgnoreCase);
                
                // Table of Contents'ten sonraki eşleşmeyi bul
                foreach (Match match in matches)
                {
                    if (match.Index > 1000) // Table of Contents'ten sonra
                        return match.Index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Sonraki Level 1 başlığı bulur
        /// </summary>
        private TocItem FindNextLevel1Header(TocItem currentHeader)
        {
            var level1Headers = FindHeadersByLevel(1);
            
            // Mevcut header'ın indeksini bul
            var currentIndex = -1;
            for (int i = 0; i < level1Headers.Count; i++)
            {
                if (level1Headers[i].Title.Equals(currentHeader.Title, StringComparison.OrdinalIgnoreCase) &&
                    level1Headers[i].SectionNumber == currentHeader.SectionNumber)
                {
                    currentIndex = i;
                    break;
                }
            }

            // Sonraki header'ı döndür
            if (currentIndex >= 0 && currentIndex < level1Headers.Count - 1)
                return level1Headers[currentIndex + 1];
            
            return null;
        }

        /// <summary>
        /// Belirtilen header'ı HTML'de bulur
        /// </summary>
        private int FindHeaderInHtml(TocItem header)
        {
            var sectionNumber = header.SectionNumber ?? "";
            var title = header.Title ?? "";
            
            var headerPatterns = new[]
            {
                $@"<b>\s*{Regex.Escape(sectionNumber)}\s*\.\s*{Regex.Escape(title)}\s*</b>",
                $@"<b>\s*{Regex.Escape(sectionNumber)}\s*</b>\s*<b>\s*{Regex.Escape(title)}\s*</b>",
                $@"<b>\s*{Regex.Escape(title)}\s*</b>"
            };

            foreach (var pattern in headerPatterns)
            {
                var match = Regex.Match(_htmlContent, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                    return match.Index;
            }

            return -1;
        }

        /// <summary>
        /// Header'ın HTML'deki başlangıç pozisyonunu bulur
        /// </summary>
        private int FindHeaderStartPosition(TocItem header)
        {
            var searchPatterns = new[]
            {
                $@"<b>{Regex.Escape(header.GetFullSectionNumber())}\.\s*{Regex.Escape(header.Title)}</b>",
                $@"<b>{Regex.Escape(header.GetFullSectionNumber())}\.</b>\s*<b>{Regex.Escape(header.Title)}</b>",
                $@"<p><b>{Regex.Escape(header.GetFullSectionNumber())}\s*<br/></b></p>",
                $@"<b>{Regex.Escape(header.Title)}</b>"
            };

            foreach (var pattern in searchPatterns)
            {
                var match = Regex.Match(_htmlContent, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    // Başlığın bulunduğu div'in başını bul
                    var divStart = _htmlContent.LastIndexOf("<div", match.Index);
                    if (divStart != -1)
                    {
                        var pageBreakDiv = _htmlContent.LastIndexOf("page-break-before:always", divStart + 100);
                        if (pageBreakDiv > divStart - 200 && pageBreakDiv < match.Index)
                        {
                            var actualDivStart = _htmlContent.LastIndexOf("<div", pageBreakDiv);
                            return actualDivStart != -1 ? actualDivStart : divStart;
                        }
                    }
                    return divStart != -1 ? divStart : match.Index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Sonraki header'ın pozisyonunu bulur
        /// </summary>
        private int FindNextHeaderPosition(TocItem currentHeader, int startPosition)
        {
            // Mevcut header'dan sonraki header'ları bul
            var nextHeaders = _tableOfContents.Items
                .Where(item => item.Level <= currentHeader.Level && 
                              !string.IsNullOrEmpty(item.SectionNumber) &&
                              !item.Title.Equals(currentHeader.Title, StringComparison.OrdinalIgnoreCase))
                .OrderBy(item => item.PageNumber)
                .ToList();

            foreach (var nextHeader in nextHeaders)
            {
                var nextPosition = FindHeaderStartPosition(nextHeader);
                if (nextPosition > startPosition)
                    return nextPosition;
            }

            return -1;
        }

        /// <summary>
        /// Header içeriğini temizler ve formatlar
        /// </summary>
        private string CleanAndFormatHeaderContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            // İçeriği temizle ama daha az agresif
            content = Regex.Replace(content, @"<div style=""page-break-before:always[^""]*""[^>]*>", "", RegexOptions.IgnoreCase);
            
            // Çoklu boşlukları temizle
            content = Regex.Replace(content, @"\s+", " ");
            
            // Paragraf etiketlerini düzenle
            content = content.Replace("<p>", "<p>\n  ");
            content = content.Replace("</p>", "\n</p>");
            
            return content.Trim();
        }

        /// <summary>
        /// Basit HTML dokümanı oluşturur
        /// </summary>
        private string CreateSimpleHtmlDocument(TocItem header, string content, string deviceName)
        {
            // Sadece content'i döndür, HTML wrapper'ları yok
            return content;
        }

        /// <summary>
        /// Table of Contents'ten GetInfo komutlarını bulur
        /// </summary>
        private List<TocItem> FindGetInfoCommands()
        {
            var getInfoCommands = new List<TocItem>();

            // WFS_INF_ ile başlayan komutları bul
            foreach (var item in _tableOfContents.Items)
            {
                if (IsGetInfoCommand(item.Title))
                {
                    getInfoCommands.Add(item);
                }
            }

            // Sayfa numarasına göre sırala
            getInfoCommands.Sort((x, y) => x.PageNumber.CompareTo(y.PageNumber));

            return getInfoCommands;
        }

        /// <summary>
        /// Bir başlığın GetInfo komutu olup olmadığını kontrol eder
        /// </summary>
        private bool IsGetInfoCommand(string title)
        {
            if (string.IsNullOrEmpty(title))
                return false;

            // WFS_INF_ ile başlayan komutları ara
            return title.StartsWith("WFS_INF_", StringComparison.OrdinalIgnoreCase) ||
                   title.Contains("WFS_INF_") ||
                   title.Contains("Info Commands") ||
                   (title.Contains("WFS") && title.Contains("INF"));
        }

        /// <summary>
        /// Belirli bir komutu çıkarır ve dosyaya kaydeder
        /// </summary>
        private void ExtractAndSaveCommand(TocItem command, string outputDirectory)
        {
            var commandContent = ExtractCommandContent(command);
            
            if (string.IsNullOrEmpty(commandContent))
            {
                Console.WriteLine($"İçerik bulunamadı: {command.Title}");
                return;
            }

            // Dosya adını oluştur
            var fileName = SanitizeFileName(command.Title) + ".html";
            var filePath = Path.Combine(outputDirectory, fileName);

            // HTML dosyası oluştur
            var htmlDocument = CreateHtmlDocument(command, commandContent);
            
            File.WriteAllText(filePath, htmlDocument);
            Console.WriteLine($"Oluşturuldu: {fileName} (Sayfa {command.PageNumber})");
        }

        /// <summary>
        /// Belirli bir komutun içeriğini HTML'den çıkarır
        /// </summary>
        private string ExtractCommandContent(TocItem command)
        {
            try
            {
                // Komutun başlangıç pozisyonunu bul
                var startPosition = FindCommandStartPosition(command);
                if (startPosition == -1)
                {
                    Console.WriteLine($"Başlangıç bulunamadı: {command.Title}");
                    return string.Empty;
                }

                // Sonraki komutun başlangıç pozisyonunu bul
                var endPosition = FindNextCommandPosition(command, startPosition);
                if (endPosition == -1)
                    endPosition = _htmlContent.Length;

                // İçeriği çıkar
                var content = _htmlContent.Substring(startPosition, endPosition - startPosition);
                
                // İçeriği temizle ve düzenle
                return CleanAndFormatContent(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"İçerik çıkarma hatası - {command.Title}: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Komutun HTML'deki başlangıç pozisyonunu bulur
        /// </summary>
        private int FindCommandStartPosition(TocItem command)
        {
            var searchPatterns = new[]
            {
                $@"<b>{Regex.Escape(command.Title)}</b>",
                $@"<b>{Regex.Escape(command.GetFullSectionNumber())}\s*</b>\s*<b>{Regex.Escape(command.Title)}</b>",
                Regex.Escape(command.Title)
            };

            foreach (var pattern in searchPatterns)
            {
                var match = Regex.Match(_htmlContent, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    // Başlığın bulunduğu satırın başını bul
                    var lineStart = _htmlContent.LastIndexOf("<p>", match.Index);
                    return lineStart != -1 ? lineStart : match.Index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Sonraki komutun pozisyonunu bulur
        /// </summary>
        private int FindNextCommandPosition(TocItem currentCommand, int startPosition)
        {
            // Mevcut komuttan sonraki komutları bul
            var nextCommands = _tableOfContents.Items
                .Where(item => item.PageNumber > currentCommand.PageNumber || 
                              (item.PageNumber == currentCommand.PageNumber && 
                               item.Level <= currentCommand.Level &&
                               !item.Title.Equals(currentCommand.Title, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(item => item.PageNumber)
                .ToList();

            foreach (var nextCommand in nextCommands)
            {
                var nextPosition = FindCommandStartPosition(nextCommand);
                if (nextPosition > startPosition)
                    return nextPosition;
            }

            // Sonraki sayfa break'ini bul
            var pageBreakPattern = @"<div style=""page-break-before:always";
            var pageBreakMatch = Regex.Match(_htmlContent.Substring(startPosition + 1000), pageBreakPattern);
            if (pageBreakMatch.Success)
            {
                return startPosition + 1000 + pageBreakMatch.Index;
            }

            return -1;
        }

        /// <summary>
        /// İçeriği temizler ve formatlar
        /// </summary>
        private string CleanAndFormatContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            // Gereksiz div'leri ve page-break'leri kaldır
            content = Regex.Replace(content, @"<div style=""page-break-[^""]*""[^>]*>", "", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"</div>\s*</div>", "", RegexOptions.IgnoreCase);
            
            // Çoklu boşlukları temizle
            content = Regex.Replace(content, @"\s+", " ");
            content = Regex.Replace(content, @">\s+<", "><");
            
            // Paragraf etiketlerini düzenle
            content = content.Replace("<p>", "<p>\n  ");
            content = content.Replace("</p>", "\n</p>");
            
            return content.Trim();
        }

        /// <summary>
        /// Dosya adını güvenli hale getirir
        /// </summary>
        private string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "unnamed";

            // Güvenli olmayan karakterleri kaldır
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            
            // Fazla uzun isimleri kısalt
            if (sanitized.Length > 100)
                sanitized = sanitized.Substring(0, 100);

            return sanitized.Trim();
        }

        /// <summary>
        /// Komut için tam HTML dokümanı oluşturur
        /// </summary>
        private string CreateHtmlDocument(TocItem command, string content)
        {
            var sectionInfo = string.IsNullOrEmpty(command.GetFullSectionNumber()) 
                ? "" 
                : $"{command.GetFullSectionNumber()} - ";

            var html = $@"<!DOCTYPE html>
<html lang=""tr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>{sectionInfo}{command.Title}</title>
    <link rel=""stylesheet"" href=""../int/xfs-styles.css"">
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }}
        .header {{
            background-color: #f4f4f4;
            padding: 20px;
            border-radius: 5px;
            margin-bottom: 20px;
        }}
        .page-info {{
            color: #666;
            font-size: 0.9em;
        }}
        .content {{
            background-color: white;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        }}
    </style>
</head>
<body>
    <div class=""header"">
        <h1>{sectionInfo}{command.Title}</h1>
        <div class=""page-info"">
            Sayfa: {command.PageNumber} | 
            Seviye: {command.Level} | 
            Anchor: {command.AnchorId ?? "N/A"}
        </div>
    </div>
    
    <div class=""content"">
        {content}
    </div>
    
    <footer style=""margin-top: 40px; text-align: center; color: #666; font-size: 0.8em;"">
        <p>HtmlToXfs2 tarafından oluşturuldu - {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
    </footer>
</body>
</html>";

            return html;
        }

        /// <summary>
        /// Tüm Info Commands bölümünü çıkarır
        /// </summary>
        public void ExtractInfoCommandsSection()
        {
            var infoCommandsSection = FindInfoCommandsSection();
            if (string.IsNullOrEmpty(infoCommandsSection))
            {
                Console.WriteLine("Info Commands bölümü bulunamadı.");
                return;
            }

            var outputPath = Path.Combine(_outputBasePath, "InfoCommands.html");
            Directory.CreateDirectory(_outputBasePath);

            var htmlDocument = $@"<!DOCTYPE html>
<html lang=""tr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Info Commands - XFS IDC</title>
    <link rel=""stylesheet"" href=""int/xfs-styles.css"">
</head>
<body>
    <h1>Info Commands</h1>
    {infoCommandsSection}
</body>
</html>";

            File.WriteAllText(outputPath, htmlDocument);
            Console.WriteLine($"Info Commands bölümü kaydedildi: {outputPath}");
        }

        /// <summary>
        /// HTML'den Info Commands bölümünü bulur
        /// </summary>
        private string FindInfoCommandsSection()
        {
            var pattern = @"<b>4\.\s*Info Commands</b>.*?(?=<b>\d+\.|$)";
            var match = Regex.Match(_htmlContent, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            
            return match.Success ? match.Value : string.Empty;
        }

        /// <summary>
        /// İstatistikleri döndürür
        /// </summary>
        public void PrintStatistics()
        {
            var getInfoCommands = FindGetInfoCommands();
            
            Console.WriteLine($"\n=== HtmlConstructor İstatistikleri ===");
            Console.WriteLine($"Toplam TOC öğesi: {_tableOfContents.Count}");
            Console.WriteLine($"GetInfo komutları: {getInfoCommands.Count}");
            Console.WriteLine($"Çıktı dizini: {_outputBasePath}");
            
            Console.WriteLine("\nBulunan GetInfo komutları:");
            foreach (var cmd in getInfoCommands.Take(10))
            {
                Console.WriteLine($"  - {cmd.Title} (Sayfa {cmd.PageNumber})");
            }
            
            if (getInfoCommands.Count > 10)
                Console.WriteLine($"  ... ve {getInfoCommands.Count - 10} tane daha");
        }
    }
}

