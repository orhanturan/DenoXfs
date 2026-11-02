# DenoXfs

CEN/XFS (eXtensions for Financial Services) için C# kütüphaneleri ve uygulamaları üretmeyi amaçlayan proje. ATM, kart okuyucu gibi finansal cihazların XFS API'sini .NET'ten kullanılabilir hale getirir.

C kodunu elle çevirmek yerine, XFS C header dosyalarından (`XFSAPI.H`, `XFSIDC.H` vb.) ve spesifikasyon metninden C# tipleri üretilir. Sabitler ve struct yerleşimlerinde header, spesifikasyon metninden önceliklidir.

## Üretim hattı

Şu an spesifikasyon sürümü 3.40.

```
Spec PDF -> markdown (markitdown) -> komut/struct başına JSON
         -> C# struct ve enum'lar (tools/HtmlToXfs2)
         -> cihaz kütüphaneleri ve middleware
         -> üretilen dönüşüm testleri (tools/XfsTestGen)
```

## Projeler ve görevleri

### `src/` - kütüphaneler

| Proje | Görevi |
|---|---|
| `DenoXfsInterop` (`DenoXfs.Interop`) | Elle yazılmış marshalling yardımcıları. Bağımlılığı yok. |
| `DenoXfsApi` (`DenoXfs.Api`) | `XfsApi` P/Invoke fonksiyonları ve `XFSAPI.H`'den üretilen ortak tipler (`XfsResult`, `XfsServiceInfo`, `XfsVersion` vb.). |
| `DenoXfsCore` (`DenoXfs.Core`) | Session/servis yönetimi, mesaj döngüsü (message pump), komut builder'ları (Open, GetInfo, Execute) ve DI desteği (`AddDenoXfsCore`). Api ve Interop'a bağlıdır. |
| `DenoXfsIdc` (`DenoXfs.Idc`) | Kimlik/banka kartı okuyucu (IDC) cihaz sınıfı için üretilmiş tipler: komutlar, infolar, eventler, durum ve yetenek yapıları. Diğer cihaz sınıfları için şablondur. |

### `tools/` - üreticiler

| Proje | Görevi |
|---|---|
| `HtmlToXfs2` | Ana üretici CLI. `INCLUDE/*.H` header'larından tipleri ve sabitleri, spec markdown'ından kullanım bilgisini okuyup Api ve cihaz kütüphanelerine C# kodu üretir. Sürüm `--version` ile verilir (varsayılan 340). |
| `XfsTestGen` | HtmlToXfs2'nin tip planından struct dönüşüm testlerini üretir: C++ native tarafı ve C# xunit tarafı. |

### `tests/` - testler

| Proje | Görevi |
|---|---|
| `HtmlToXfs2.Tests` | Üreticinin testleri (ör. `MarkdownSpecParser`, tip planlama). |
| `DenoXfsTestNative` | Dönüşüm testleri için C++ native taraf. |
| `DenoXfsApiTests` | Api tiplerinin testleri. |
| `DenoXfsIdcTests` | IDC tiplerinin testleri (üretilmiş). |
| `DenoXfsInteropTests` | Interop yardımcılarının testleri (elle yazılmış). |

## Kurallar

- Hedef çerçeve `net9.0-windows`; `Nullable` ve `ImplicitUsings` açık.
- Üretilmiş C# dosyaları elle düzeltilmez, üreticideki hata düzeltilir.
- Spec sürümü koda gömülmez, `--version` parametresinden gelir.
- Ayrıntılı kurallar için `.github/copilot-instructions.md` ve `.github/instructions/` altındaki dosyalara bakın.

## XFS sözlüğü

- `WFS` önekli fonksiyonları uygulama çağırır (API). `WFP` önekliler SPI'dır, Manager sağlayıcıları çağırır. `WFM` önekliler destek ve konfigürasyon fonksiyonlarıdır.
- `hService`: `WFSOpen` ile alınan oturum tanıtıcısı. `hApp`: uygulama kimliği.
- `WFSRESULT`: sonuç tamponu. `WFSFreeResult` ile serbest bırakılmalıdır.
- Komut önekleri: `WFS_INF_` GetInfo, `WFS_CMD_` Execute, `WFS_EXEE_`/`WFS_SRVE_`/`WFS_USRE_`/`WFS_SYSE_` eventler.
- Cihaz sınıfları: IDC, CDM, CIM, PIN, PTR, CHK, DEP, TTU, SIU, VDM, CAM, ALM, CEU, CRD, BCR, IPM, BIO.
