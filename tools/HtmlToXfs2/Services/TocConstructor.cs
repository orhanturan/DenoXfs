using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using HtmlToXfs2.Model;

namespace HtmlToXfs2.Services
{
    /// <summary>
    /// HTML içeriğinden Table of Contents oluşturur
    /// </summary>
    public class TocConstructor
    {
        /// <summary>
        /// HTML içeriğinden Table of Contents oluşturur
        /// </summary>
        public TableOfContents CreateTableOfContents(string htmlContent)
        {
            var toc = new TableOfContents();
            
            // TOC bölümünü bul
            var tocSection = ExtractTocSection(htmlContent);
            if (string.IsNullOrEmpty(tocSection))
            {
                Console.WriteLine("Table of Contents bölümü bulunamadı, tüm HTML'i tarayacak...");
                tocSection = htmlContent;
            }

            // TOC öğelerini parse et
            var tocItems = ParseTocItems(tocSection);
            
            foreach (var item in tocItems)
            {
                toc.AddItem(item);
            }

            // Her TOC item için HTML'deki gerçek pozisyonunu bul
            Console.WriteLine("\n=== HTML POZİSYONLARI BULUNUYOR ===");
            
            // Önce sayfa pozisyonlarını bul
            FindPagePositions(htmlContent, toc);
            
            // Referansı ayarla
            _currentToc = toc;
            
            foreach (var item in toc.Items)
            {
                item.HtmlStartIndex = FindRealHeaderPosition(htmlContent, item);
                Console.WriteLine($"{item.GetFullSectionNumber()} {item.Title} [{item.HtmlStartIndex}]");
            }
            
            // Bitiş pozisyonlarını sıralı olarak hesapla
            CalculateEndPositions(htmlContent, toc.Items);

            return toc;
        }
        
        /// <summary>
        /// Header'ların bitiş pozisyonlarını sıralı olarak hesaplar
        /// </summary>
        private void CalculateEndPositions(string htmlContent, IReadOnlyList<TocItem> items)
        {
            // Başlangıç pozisyonuna göre sırala
            var sortedItems = items.Where(x => x.HtmlStartIndex > 0)
                                  .OrderBy(x => x.HtmlStartIndex)
                                  .ToList();
            
            for (int i = 0; i < sortedItems.Count; i++)
            {
                var currentItem = sortedItems[i];
                
                if (i < sortedItems.Count - 1)
                {
                    var nextItem = sortedItems[i + 1];
                    
                    // GÜVENLİK KONTROLÜ: Sonraki header'ın başlangıcı mevcut header'dan küçük veya eşitse
                    if (nextItem.HtmlStartIndex <= currentItem.HtmlStartIndex)
                    {
                        // Minimum içerik bırak (500 karakter)
                        currentItem.HtmlEndIndex = currentItem.HtmlStartIndex + 500;
                        Console.WriteLine($"  UYARI: {currentItem.Title} - güvenlik kontrolü aktif! Start: {currentItem.HtmlStartIndex}, End: {currentItem.HtmlEndIndex}");
                    }
                    else
                    {
                        // Normal durum: sonraki header'ın başlangıcını bitiş olarak kullan
                        currentItem.HtmlEndIndex = nextItem.HtmlStartIndex;
                        Console.WriteLine($"  Start: {currentItem.HtmlStartIndex}, End: {currentItem.HtmlEndIndex}");
                    }
                }
                else
                {
                    // Son item için dosya sonunu kullan
                    currentItem.HtmlEndIndex = htmlContent.Length;
                    Console.WriteLine($"  Start: {currentItem.HtmlStartIndex}, End: {currentItem.HtmlEndIndex}");
                }
            }
        }

        /// <summary>
        /// HTML içeriğinden Table of Contents bölümünü çıkarır
        /// </summary>
        private string ExtractTocSection(string htmlContent)
        {
            var tocStartPattern = @"<b>Table of Contents</b>";
            var tocMatch = Regex.Match(htmlContent, tocStartPattern, RegexOptions.IgnoreCase);
            
            if (!tocMatch.Success)
                return string.Empty;

            var startIndex = tocMatch.Index;
            
            // TOC bölümünün bitişini bul
            var endPatterns = new[]
            {
                @"<b>European Foreword</b>",
                @"<b>\d+\.\s*Introduction</b>",
                @"<div style=""page-break-before:always""[^>]*><div><p><b>\d+"
            };

            var endIndex = htmlContent.Length;
            foreach (var pattern in endPatterns)
            {
                var searchStart = Math.Min(startIndex + 2000, htmlContent.Length);
                if (searchStart < htmlContent.Length)
                {
                    var endMatch = Regex.Match(htmlContent.Substring(searchStart), pattern, RegexOptions.IgnoreCase);
                    if (endMatch.Success)
                    {
                        endIndex = searchStart + endMatch.Index;
                        break;
                    }
                }
            }

            return htmlContent.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// TOC bölümünden öğeleri parse eder
        /// </summary>
        private List<TocItem> ParseTocItems(string tocSection)
        {
            var items = new List<TocItem>();
            
            // Ana başlık patterns
            var mainHeaderPattern = @"<b>(\d+)\.\s*</b>\s*<b>([^<]+)</b>[^<]*<br[^>]*>";
            var mainHeaderMatches = Regex.Matches(tocSection, mainHeaderPattern, RegexOptions.IgnoreCase);
            
            foreach (Match match in mainHeaderMatches)
            {
                var sectionNumber = match.Groups[1].Value;
                var title = CleanTitle(match.Groups[2].Value);
                var pageNumber = ExtractPageNumberFromContext(tocSection, match.Index);
                
                var item = new TocItem(title, pageNumber, 1)
                {
                    SectionNumber = sectionNumber,
                    AnchorId = GenerateAnchorId(sectionNumber, title)
                };
                
                items.Add(item);
            }

            // Alt başlık patterns  
            var subHeaderPattern = @"<b>(\d+)\.(\d+)\s*</b>\s*<b>([^<]+)</b>";
            var subHeaderMatches = Regex.Matches(tocSection, subHeaderPattern, RegexOptions.IgnoreCase);
            
            foreach (Match match in subHeaderMatches)
            {
                var mainSection = match.Groups[1].Value;
                var subSection = match.Groups[2].Value;
                var title = CleanTitle(match.Groups[3].Value);
                var pageNumber = ExtractPageNumberFromContext(tocSection, match.Index);
                
                var item = new TocItem(title, pageNumber, 2)
                {
                    SectionNumber = mainSection,
                    SubSectionNumber = subSection,
                    AnchorId = GenerateAnchorId($"{mainSection}.{subSection}", title)
                };
                
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Verilen pozisyondan sayfa numarasını çıkarır
        /// </summary>
        private int ExtractPageNumberFromContext(string content, int position)
        {
            // Mevcut pozisyondan sonraki 500 karakter içinde sayfa numarası ara
            var searchLength = Math.Min(500, content.Length - position);
            var searchArea = content.Substring(position, searchLength);
            
            var pagePattern = @"(\d+)\s*<br[^>]*>";
            var pageMatch = Regex.Match(searchArea, pagePattern);
            
            if (pageMatch.Success && int.TryParse(pageMatch.Groups[1].Value, out int pageNum))
                return pageNum;

            return 0;
        }

        /// <summary>
        /// Başlık metnini temizler
        /// </summary>
        private string CleanTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return string.Empty;

            // HTML tag'lerini kaldır
            title = Regex.Replace(title, @"<[^>]+>", "");
            
            // HTML entity'lerini decode et
            title = title.Replace("&amp;", "&")
                        .Replace("&lt;", "<")
                        .Replace("&gt;", ">")
                        .Replace("&quot;", "\"");
            
            // Gereksiz boşlukları temizle
            title = Regex.Replace(title, @"\s+", " ").Trim();
            
            return title;
        }

        /// <summary>
        /// Anchor ID oluşturur
        /// </summary>
        private string GenerateAnchorId(string sectionNumber, string title)
        {
            var anchor = string.IsNullOrEmpty(sectionNumber) ? title : $"{sectionNumber}-{title}";
            
            // Güvenli anchor ID oluştur
            anchor = Regex.Replace(anchor, @"[^\w\-\.]", "-");
            anchor = Regex.Replace(anchor, @"-+", "-");
            anchor = anchor.Trim('-');
            
            return anchor.ToLowerInvariant();
        }

        /// <summary>
        /// Bir header'ın HTML'deki gerçek pozisyonunu bulur (PageIndex kullanarak)
        /// </summary>
        private int FindRealHeaderPosition(string htmlContent, TocItem item)
        {
            var sectionNumber = item.SectionNumber ?? "";
            var title = CleanTitleForSearch(item.Title ?? "");
            
            // Önce item'ın sayfa numarasına yakın bir aralıkta ara
            var searchStartIndex = GetSearchStartIndex(item);
            var searchEndIndex = GetSearchEndIndex(item, htmlContent.Length);
            
            Console.WriteLine($"Arama aralığı [{searchStartIndex} - {searchEndIndex}] (Sayfa {item.PageNumber})");
            
            // Level'a göre farklı pattern'ler
            var patterns = item.Level == 1 ? GetLevel1Patterns(sectionNumber, title) 
                                           : GetLevel2Patterns(sectionNumber, title);
            
            foreach (var pattern in patterns)
            {
                // Sadece belirlenen aralıkta ara
                var searchContent = searchEndIndex > searchStartIndex ? 
                    htmlContent.Substring(searchStartIndex, searchEndIndex - searchStartIndex) : 
                    htmlContent.Substring(searchStartIndex);
                    
                var matches = Regex.Matches(searchContent, pattern, RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    var actualIndex = searchStartIndex + match.Index;
                    
                    // Context'i göster
                    var contextStart = Math.Max(0, actualIndex - 30);
                    var contextLength = Math.Min(100, htmlContent.Length - contextStart);
                    var context = htmlContent.Substring(contextStart, contextLength);
                    Console.WriteLine($"  Pattern eşleşme: {actualIndex} - {context.Replace("\n", " ").Replace("\r", "")}");
                    
                    return actualIndex;
                }
            }
            
            return -1; // Bulunamadı
        }

        /// <summary>
        /// PageIndex'leri kullanarak arama başlangıç pozisyonunu belirler
        /// </summary>
        private int GetSearchStartIndex(TocItem item)
        {
            // Item'ın sayfa numarasından önceki sayfadan başla
            var targetPage = Math.Max(1, item.PageNumber - 1);
            
            // PageIndexes'te bu sayfa var mı?
            if (_currentToc?.PageIndexes.ContainsKey(targetPage) == true)
            {
                return _currentToc.PageIndexes[targetPage];
            }
            
            // Yoksa en yakın önceki sayfayı bul
            var nearestPage = _currentToc?.PageIndexes.Keys
                .Where(p => p <= targetPage)
                .OrderByDescending(p => p)
                .FirstOrDefault();
                
            if (nearestPage > 0 && _currentToc?.PageIndexes.ContainsKey(nearestPage.Value) == true)
            {
                return _currentToc.PageIndexes[nearestPage.Value];
            }
            
            return 1000; // Default: TOC'ten sonra
        }

        /// <summary>
        /// PageIndex'leri kullanarak arama bitiş pozisyonunu belirler
        /// </summary>
        private int GetSearchEndIndex(TocItem item, int htmlLength)
        {
            // Item'ın sayfa numarasından sonraki sayfaya kadar ara
            var targetPage = item.PageNumber + 2; // 2 sayfa sonrasına kadar
            
            // PageIndexes'te bu sayfa var mı?
            if (_currentToc?.PageIndexes.ContainsKey(targetPage) == true)
            {
                return _currentToc.PageIndexes[targetPage];
            }
            
            // Yoksa en yakın sonraki sayfayı bul
            var nearestPage = _currentToc?.PageIndexes.Keys
                .Where(p => p >= targetPage)
                .OrderBy(p => p)
                .FirstOrDefault();
                
            if (nearestPage > 0 && _currentToc?.PageIndexes.ContainsKey(nearestPage.Value) == true)
            {
                return _currentToc.PageIndexes[nearestPage.Value];
            }
            
            return htmlLength; // Default: dosya sonu
        }

        // Geçici referans için TableOfContents'i sakla
        private TableOfContents? _currentToc;

        /// <summary>
        /// Bir sonraki header'ın pozisyonunu bulur (bitiş pozisyonu için)
        /// </summary>
        private int FindNextHeaderPosition(string htmlContent, TocItem currentItem, IReadOnlyList<TocItem> allItems)
        {
            // Aynı level veya daha üst level'daki bir sonraki header'ı bul
            var nextItem = FindNextRelevantHeader(currentItem, allItems);
            
            if (nextItem != null)
            {
                var nextPosition = FindRealHeaderPosition(htmlContent, nextItem);
                if (nextPosition > 0)
                {
                    // KRITIK: Bir komutun bitiş indexi sonraki komutun başlangıcından büyük olamaz
                    // Eğer current item'ın başlangıcı sonraki item'ın başlangıcından büyükse, 
                    // bu durumda current item'ın başlangıcını kullan
                    if (currentItem.HtmlStartIndex >= 0 && nextPosition <= currentItem.HtmlStartIndex)
                    {
                        Console.WriteLine($"  UYARI: Sonraki header pozisyonu ({nextPosition}) mevcut header'dan ({currentItem.HtmlStartIndex}) küçük!");
                        return currentItem.HtmlStartIndex + 100; // Minimum bir içerik bırak
                    }
                    return nextPosition;
                }
            }
            
            return htmlContent.Length; // Dosya sonu
        }

        /// <summary>
        /// Bir sonraki ilgili header'ı bulur
        /// </summary>
        private TocItem? FindNextRelevantHeader(TocItem currentItem, IReadOnlyList<TocItem> allItems)
        {
            var currentIndex = allItems.ToList().IndexOf(currentItem);
            if (currentIndex == -1 || currentIndex >= allItems.Count - 1)
                return null;

            // Level 1 için: bir sonraki Level 1
            if (currentItem.Level == 1)
            {
                return allItems.Skip(currentIndex + 1)
                              .FirstOrDefault(x => x.Level == 1);
            }
            
            // Level 2 için: bir sonraki Level 2 veya Level 1
            return allItems.Skip(currentIndex + 1)
                          .FirstOrDefault(x => x.Level <= currentItem.Level);
        }

        /// <summary>
        /// Level 1 başlık pattern'lerini döndürür
        /// </summary>
        private string[] GetLevel1Patterns(string sectionNumber, string title)
        {
            return new[]
            {
                $@"<p><b>{Regex.Escape(sectionNumber)}\.\s*{Regex.Escape(title)}\s*<br/></b></p>",
                $@"<b>{Regex.Escape(sectionNumber)}\.\s*{Regex.Escape(title)}\s*<br/></b>",
                $@"<b>{Regex.Escape(sectionNumber)}\.\s*{Regex.Escape(title)}\s*</b>"
            };
        }

        /// <summary>
        /// Level 2 başlık pattern'lerini döndürür
        /// </summary>
        private string[] GetLevel2Patterns(string sectionNumber, string title)
        {
            // TocItem'dan SubSectionNumber'ı al
            var tocItem = _currentToc?.Items.FirstOrDefault(x => 
                x.SectionNumber == sectionNumber && 
                x.Title.Contains(title) && 
                x.Level == 2);
            
            if (tocItem?.SubSectionNumber != null)
            {
                var fullSection = $"{sectionNumber}.{tocItem.SubSectionNumber}";
                
                return new[]
                {
                    $@"<p><b>{Regex.Escape(fullSection)}\s+{Regex.Escape(title)}\s*<br/>[^<]*</b>",
                    $@"<p><b>{Regex.Escape(fullSection)}\s+{Regex.Escape(title)}\s*<br/>",
                    $@"<p><b>{Regex.Escape(fullSection)}\s+[^<]*<br/>"
                };
            }
            
            // Fallback - sadece title ile ara
            return new[]
            {
                $@"<p><b>[^<]*{Regex.Escape(title)}[^<]*<br/>"
            };
        }

        /// <summary>
        /// Başlık metnini arama için temizler
        /// </summary>
        private string CleanTitleForSearch(string title)
        {
            // Nokta ve sayfa numaralarını kaldır
            title = Regex.Replace(title, @"\.+\s*\d+$", "").Trim();
            
            // Gereksiz boşlukları temizle
            title = Regex.Replace(title, @"\s+", " ").Trim();
            
            return title;
        }

        /// <summary>
        /// HTML'deki tüm sayfa pozisyonlarını bulur ve kaydeder
        /// </summary>
        private void FindPagePositions(string htmlContent, TableOfContents toc)
        {
            Console.WriteLine("\n=== SAYFA POZİSYONLARI BULUNUYOR ===");
            
            // Sayfa numarası pattern'leri - HTML'deki gerçek sayfa işaretlerini ara
            var pagePatterns = new[]
            {
                // Page break ile sayfa numarası
                @"<div style=""page-break-before:always""[^>]*>.*?(\d+)\s*</div>",
                
                // Sayfa numarası tek başına
                @"<div[^>]*>\s*(\d+)\s*</div>",
                
                // Sayfa footer'ında sayfa numarası
                @"<p[^>]*>\s*(\d+)\s*</p>(?=.*?<div[^>]*page-break)",
                
                // Basit sayfa numarası pattern'i
                @"(?:^|\n)\s*(\d+)\s*(?=\n|$)"
            };

            var foundPages = new HashSet<int>();
            
            foreach (var pattern in pagePatterns)
            {
                var matches = Regex.Matches(htmlContent, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                
                foreach (Match match in matches)
                {
                    if (int.TryParse(match.Groups[1].Value, out int pageNumber))
                    {
                        // Sayfa numarası mantıklı aralıkta mı? (1-999)
                        if (pageNumber >= 1 && pageNumber <= 999)
                        {
                            if (!foundPages.Contains(pageNumber))
                            {
                                toc.PageIndexes[pageNumber] = match.Index;
                                foundPages.Add(pageNumber);
                                
                                // Context'i göster
                                var contextStart = Math.Max(0, match.Index - 50);
                                var contextLength = Math.Min(150, htmlContent.Length - contextStart);
                                var context = htmlContent.Substring(contextStart, contextLength);
                                
                                Console.WriteLine($"Sayfa {pageNumber}: Index {match.Index}");
                                Console.WriteLine($"  Context: {context.Replace("\n", " ").Replace("\r", "")}");
                            }
                        }
                    }
                }
            }
            
            Console.WriteLine($"Toplam {toc.PageIndexes.Count} sayfa pozisyonu bulundu.");
            
            // Sayfa numaralarını sırala ve göster
            var sortedPages = toc.PageIndexes.OrderBy(x => x.Key).ToList();
            Console.WriteLine("Bulunan sayfalar:");
            foreach (var page in sortedPages.Take(10)) // İlk 10 sayfa
            {
                Console.WriteLine($"  Sayfa {page.Key}: Index {page.Value}");
            }
            if (sortedPages.Count > 10)
            {
                Console.WriteLine($"  ... ve {sortedPages.Count - 10} sayfa daha");
            }
        }
    }
}

