namespace HtmlToXfs2.Model
{
    /// <summary>
    /// Table of Contents'teki bir başlık öğesini temsil eder
    /// </summary>
    public class TocItem
    {
        /// <summary>
        /// Başlığın metni (örn: "WFS_INF_IDC_STATUS")
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// Başlığın sayfa numarası
        /// </summary>
        public int PageNumber { get; set; }
        
        /// <summary>
        /// Başlığın seviyesi (1 = ana başlık, 2 = alt başlık, vb.)
        /// </summary>
        public int Level { get; set; }
        
        /// <summary>
        /// Ana başlık numarası (örn: "4" for "4.1" başlığı)
        /// </summary>
        public string? SectionNumber { get; set; }
        
        /// <summary>
        /// Alt başlık numarası (örn: "1" for "4.1" başlığı, null ise ana başlık)
        /// </summary>
        public string? SubSectionNumber { get; set; }
        
        /// <summary>
        /// HTML'deki orijinal içerik anchor olarak kullanılabilir
        /// </summary>
        public string? AnchorId { get; set; }
        
        /// <summary>
        /// Başlığın HTML'deki başlangıç pozisyonu
        /// </summary>
        public int HtmlStartIndex { get; set; } = -1;
        
        /// <summary>
        /// İçeriğin HTML'deki bitiş pozisyonu
        /// </summary>
        public int HtmlEndIndex { get; set; } = -1;
        
        /// <summary>
        /// Sayfa başlangıç pozisyonu (opsiyonel)
        /// </summary>
        public int PageStartIndex { get; set; } = -1;

        public TocItem(string title, int pageNumber, int level)
        {
            Title = title;
            PageNumber = pageNumber;
            Level = level;
        }

        /// <summary>
        /// Başlığın tam numarasını döndürür (örn: "4.1")
        /// </summary>
        public string GetFullSectionNumber()
        {
            if (string.IsNullOrEmpty(SectionNumber))
                return string.Empty;
            
            if (string.IsNullOrEmpty(SubSectionNumber))
                return SectionNumber;
            
            return $"{SectionNumber}.{SubSectionNumber}";
        }

        public override string ToString()
        {
            var sectionNum = GetFullSectionNumber();
            var prefix = string.IsNullOrEmpty(sectionNum) ? "" : $"{sectionNum} ";
            return $"{prefix}{Title} (Sayfa {PageNumber})";
        }
    }
}
