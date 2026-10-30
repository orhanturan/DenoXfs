using System;
using System.IO;
using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services
{
    public class HtmlChanger
    {
        private readonly string _sourceFilePath;
        private readonly string _targetFilePath;
        private readonly string _deviceName;
        private string _htmlContent;

        public HtmlChanger(string sourceFilePath, string targetFilePath, string deviceName)
        {
            _sourceFilePath = sourceFilePath ?? throw new ArgumentNullException(nameof(sourceFilePath));
            _targetFilePath = targetFilePath ?? throw new ArgumentNullException(nameof(targetFilePath));
            _deviceName = deviceName ?? throw new ArgumentNullException(nameof(deviceName));
            
            if (!File.Exists(_sourceFilePath))
            {
                throw new FileNotFoundException($"Kaynak HTML dosyası bulunamadı: {_sourceFilePath}");
            }
            
            _htmlContent = File.ReadAllText(_sourceFilePath);
        }

        public void ProcessHtmlFile()
        {
            Console.WriteLine($"HTML dosyası işleniyor: {Path.GetFileName(_sourceFilePath)}");
            
            try
            {
                AddCssReference();
                RemoveCwaReferences();
                RemovePageNumbers();
                RemovePageBreaks();
                ConvertKeywordsToH3();
                FormatLineBreaks();
                SaveProcessedHtml();
                
                Console.WriteLine($"  ✓ İşlem tamamlandı: {Path.GetFileName(_targetFilePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Hata oluştu: {ex.Message}");
                throw;
            }
        }

        private void AddCssReference()
        {
            // <head> bölümüne CSS referansı ekle - 4 seviye yukarıdaki CSS dosyasını referans ver
            var cssLink = "<link rel=\"stylesheet\" href=\"../../../xfs-styles.css\">";
            
            // Eğer zaten CSS referansı varsa ekleme
            if (_htmlContent.Contains("xfs-styles.css"))
            {
                return;
            }
            
            // <head> tag'inin kapanışından önce CSS referansını ekle
            var headClosePattern = @"</head>";
            if (Regex.IsMatch(_htmlContent, headClosePattern, RegexOptions.IgnoreCase))
            {
                _htmlContent = Regex.Replace(_htmlContent, headClosePattern, 
                    $"    {cssLink}\n</head>", RegexOptions.IgnoreCase);
            }
            else
            {
                // Eğer </head> yoksa <head> tag'ından sonra ekle
                var headOpenPattern = @"<head>";
                _htmlContent = Regex.Replace(_htmlContent, headOpenPattern, 
                    $"<head>\n    {cssLink}", RegexOptions.IgnoreCase);
            }
        }

        private void RemoveCwaReferences()
        {
            // CWA referanslarını kaldır: <p><b>CWA 16926-4:2020 (E)</b> <br/></p>
            var cwaPattern = @"<p><b>CWA 16926-4:2020 \(E\)</b> <br/></p>";
            _htmlContent = Regex.Replace(_htmlContent, cwaPattern, "", RegexOptions.IgnoreCase);
            
            // Satır sonlarını da temizle
            _htmlContent = Regex.Replace(_htmlContent, @"\n\s*\n\s*\n", "\n\n");
        }

        private void RemovePageNumbers()
        {
            // Sayfa numaralarını kaldır: <p><b>14 <br/></b></p>
            var pageNumberPattern = @"<p><b>\d+ <br/></b></p>";
            _htmlContent = Regex.Replace(_htmlContent, pageNumberPattern, "", RegexOptions.IgnoreCase);
            
            // Satır sonlarını da temizle
            _htmlContent = Regex.Replace(_htmlContent, @"\n\s*\n\s*\n", "\n\n");
        }

        private void RemovePageBreaks()
        {
            // Page-break div'lerini kaldır
            var pageBreakPattern = @"<div style=""page-break-before:always; page-break-after:always""><div>";
            _htmlContent = Regex.Replace(_htmlContent, pageBreakPattern, "", RegexOptions.IgnoreCase);
            
            // Kapanmayan div'leri de kaldır  
            var closingDivPattern = @"</div></div>";
            _htmlContent = Regex.Replace(_htmlContent, closingDivPattern, "", RegexOptions.IgnoreCase);
            
            // Gereksiz boş satırları temizle
            _htmlContent = Regex.Replace(_htmlContent, @"\n\s*\n\s*\n", "\n\n");
        }

        private void ConvertKeywordsToH3()
        {
            var keywords = new[]
            {
                "Description", "Input Param", "Output Param", 
                "Error Codes", "Events", "Comments", "Event Param"
            };

            foreach (var keyword in keywords)
            {
                // Pattern 1: <b>KEYWORD formatı (başlangıç)
                var pattern1 = $@"<b>{Regex.Escape(keyword)}";
                var replacement1 = $"<b><h3>{keyword}</h3>";
                _htmlContent = Regex.Replace(_htmlContent, pattern1, replacement1, RegexOptions.None, TimeSpan.FromSeconds(1));
                
                // Pattern 2: <br/>KEYWORD formatı (br/ ile)
                var pattern2 = $@"<br/>{Regex.Escape(keyword)}";
                var replacement2 = $"<h3>{keyword}</h3>";
                _htmlContent = Regex.Replace(_htmlContent, pattern2, replacement2, RegexOptions.None, TimeSpan.FromSeconds(1));
                
                // Pattern 3: <br>KEYWORD formatı (br açılış ile)
                var pattern3 = $@"<br>{Regex.Escape(keyword)}";
                var replacement3 = $"<h3>{keyword}</h3>";
                _htmlContent = Regex.Replace(_htmlContent, pattern3, replacement3, RegexOptions.None, TimeSpan.FromSeconds(1));
            }
        }

        private void FormatLineBreaks()
        {
            // <br/> tag'lerinden önce CR LF ekle
            _htmlContent = Regex.Replace(_htmlContent, @"<br/>", "\n<br/>", RegexOptions.IgnoreCase);
            
            // <br> tag'lerinden önce CR LF ekle
            _htmlContent = Regex.Replace(_htmlContent, @"<br>", "\n<br>", RegexOptions.IgnoreCase);
        }

        private void SaveProcessedHtml()
        {
            // Hedef dizini oluştur
            var targetDirectory = Path.GetDirectoryName(_targetFilePath);
            if (!string.IsNullOrEmpty(targetDirectory) && !Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
                Console.WriteLine($"  Dizin oluşturuldu: {targetDirectory}");
            }
            
            // İşlenmiş HTML'i hedef dosyaya kaydet
            File.WriteAllText(_targetFilePath, _htmlContent);
        }
    }
}
