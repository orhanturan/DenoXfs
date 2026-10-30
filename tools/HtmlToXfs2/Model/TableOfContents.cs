using System.Collections.Generic;
using System.Linq;

namespace HtmlToXfs2.Model
{
    /// <summary>
    /// Table of Contents'i yöneten sınıf
    /// </summary>
    public class TableOfContents
    {
        private readonly List<TocItem> _items;

        /// <summary>
        /// Sayfa numaralarının HTML'deki pozisyonları (Sayfa No -> HTML Index)
        /// </summary>
        public Dictionary<int, int> PageIndexes { get; set; } = new Dictionary<int, int>();

        public TableOfContents()
        {
            _items = new List<TocItem>();
        }

        /// <summary>
        /// Tüm TOC öğelerini döndürür
        /// </summary>
        public IReadOnlyList<TocItem> Items => _items.AsReadOnly();

        /// <summary>
        /// Yeni bir TOC öğesi ekler
        /// </summary>
        public void AddItem(TocItem item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// Belirli bir seviyedeki öğeleri döndürür
        /// </summary>
        public IEnumerable<TocItem> GetItemsByLevel(int level)
        {
            return _items.Where(item => item.Level == level);
        }

        /// <summary>
        /// Ana başlıkları döndürür (Level 1)
        /// </summary>
        public IEnumerable<TocItem> GetMainHeaders()
        {
            return GetItemsByLevel(1);
        }

        /// <summary>
        /// Alt başlıkları döndürür (Level 2)
        /// </summary>
        public IEnumerable<TocItem> GetSubHeaders()
        {
            return GetItemsByLevel(2);
        }

        /// <summary>
        /// Belirli bir ana başlığın alt başlıklarını döndürür
        /// </summary>
        public IEnumerable<TocItem> GetSubHeadersFor(string mainSectionNumber)
        {
            return _items.Where(item => 
                item.Level == 2 && 
                item.SectionNumber == mainSectionNumber);
        }

        /// <summary>
        /// Sayfa numarasına göre öğeleri döndürür
        /// </summary>
        public IEnumerable<TocItem> GetItemsByPage(int pageNumber)
        {
            return _items.Where(item => item.PageNumber == pageNumber);
        }

        /// <summary>
        /// Başlık metnine göre arama yapar
        /// </summary>
        public IEnumerable<TocItem> SearchByTitle(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return Enumerable.Empty<TocItem>();

            var searchLower = searchText.ToLowerInvariant();
            return _items.Where(item => 
                item.Title.ToLowerInvariant().Contains(searchLower));
        }

        /// <summary>
        /// Belirli bir bölüm numarasındaki öğeyi bulur
        /// </summary>
        public TocItem? FindBySection(string sectionNumber, string? subSectionNumber = null)
        {
            return _items.FirstOrDefault(item => 
                item.SectionNumber == sectionNumber && 
                item.SubSectionNumber == subSectionNumber);
        }

        /// <summary>
        /// TOC'u sayfa numarasına göre sıralar
        /// </summary>
        public void SortByPageNumber()
        {
            _items.Sort((x, y) => x.PageNumber.CompareTo(y.PageNumber));
        }

        /// <summary>
        /// TOC'u bölüm numarasına göre sıralar
        /// </summary>
        public void SortBySection()
        {
            _items.Sort((x, y) =>
            {
                // Önce ana bölüm numarasına göre sırala
                if (int.TryParse(x.SectionNumber, out int xMain) && 
                    int.TryParse(y.SectionNumber, out int yMain))
                {
                    if (xMain != yMain)
                        return xMain.CompareTo(yMain);
                    
                    // Ana bölümler aynıysa alt bölüme göre sırala
                    if (int.TryParse(x.SubSectionNumber, out int xSub) && 
                        int.TryParse(y.SubSectionNumber, out int ySub))
                    {
                        return xSub.CompareTo(ySub);
                    }
                    
                    // Alt bölüm numarası olmayan ana başlık önce gelir
                    if (string.IsNullOrEmpty(x.SubSectionNumber) && !string.IsNullOrEmpty(y.SubSectionNumber))
                        return -1;
                    if (!string.IsNullOrEmpty(x.SubSectionNumber) && string.IsNullOrEmpty(y.SubSectionNumber))
                        return 1;
                }
                
                return 0;
            });
        }

        /// <summary>
        /// Toplam öğe sayısını döndürür
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// TOC'u temizler
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// TOC'un metin temsilini döndürür
        /// </summary>
        public override string ToString()
        {
            return $"Table of Contents ({Count} items)";
        }
    }
}
