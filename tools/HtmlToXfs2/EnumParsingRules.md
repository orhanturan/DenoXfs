# Enum Parsing Kuralları

## Genel Bakış
WFS dokumentasyonunda struct field'ları için enum değerleri parse edilir ve JSON çıktısına eklenir.

## Enum Tespit Kuralları

### 1. Field Tipi Kontrolü
Field'ın tipi **primitive integer** olmalıdır:
- `WORD`
- `SHORT`
- `USHORT`
- `LONG`
- `ULONG`
- `DWORD`

**Not**: `LPSTR`, `LPBYTE`, `LPWORD` gibi pointer tipler enum'a sahip olamaz.

### 2. "Value Meaning" Başlığı
Field açıklamasında **"Value Meaning"** metni bulunmalıdır. Bu, field'ın enum değerlere sahip olduğunun göstergesidir.

### 3. Field Açıklama Formatı
HTML'de field açıklamaları şu formatlarda bulunur:
- `<p><i>fieldName</i>`
- `<br/><i>fieldName</i>`

## Enum Değer Formatı

### Satır Başı Kuralı
Enum değerleri **mutlaka satır başında** olmalıdır:
- `<br/>WFS_...`
- `<p>WFS_...`

### Değer Pattern'i
- `WFS_` ile başlamalı
- Büyük harf, rakam ve underscore içerebilir
- Örnek: `WFS_IDC_DEVONLINE`, `WFS_IDC_GUIDANCE_NOT_AVAILABLE`

## Field Sınırları

Her field'ın enum değerleri sadece o field'a ait bölümden alınır:
- **Başlangıç**: `<p><i>fieldName</i>` veya `<br/><i>fieldName</i>`
- **Bitiş**: Sonraki `<p><i>` veya `<br/><i>` tag'i (yani sonraki field başlangıcı)

**Önemli**: Bir sonraki field'ın enum'larını almamak için field sınırları doğru belirlenmeli.

## Örnek HTML Yapısı

```html
<p><i>fwDevice</i>
<br/>Specifies the state of the ID card device as one of the following flags:
<br/></p>

<p>Value Meaning
<br/>WFS_IDC_DEVONLINE The device is present, powered on and online
<br/></p>

<p>WFS_IDC_DEVOFFLINE The device is offline
<br/></p>

<p>WFS_IDC_DEVPOWEROFF The device is powered off
<br/></p>

<p><i>fwMedia</i>
<br/>Specifies the state of the ID card unit...
```

Bu örnekte:
- `fwDevice` field'ı için: `WFS_IDC_DEVONLINE`, `WFS_IDC_DEVOFFLINE`, `WFS_IDC_DEVPOWEROFF`
- `fwMedia` field'ı için: Ayrı olarak parse edilir

## Hedef JSON Yapısı

```json
{
  "OutputParam": {
    "OriginalStructureName": "wfs_idc_status",
    "Fields": {
      "fwDevice": "WORD",
      "fwMedia": "WORD",
      "lpszExtra": "LPSTR"
    },
    "Enums": {
      "fwDevice": [
        "WFS_IDC_DEVONLINE",
        "WFS_IDC_DEVOFFLINE",
        "WFS_IDC_DEVPOWEROFF"
      ],
      "fwMedia": [
        "WFS_IDC_MEDIAPRESENT",
        "WFS_IDC_MEDIANOTPRESENT"
      ]
      // lpszExtra için enum yok (LPSTR tipi)
    }
  }
}
```

## İşlem Akışı

1. **Field listesini al** (örn: `fwDevice`, `fwMedia`, ...)
2. **Her field için**:
   - Field tipini kontrol et (primitive integer mi?)
   - HTML'de field açıklamasını bul (`<i>fieldName</i>`)
   - Field'a ait bölümü belirle (başlangıç-bitiş index)
   - "Value Meaning" var mı kontrol et
   - Varsa, `WFS_` ile başlayan satır başı değerlerini topla
   - Enum listesine ekle
3. **Sonuç**: `Dictionary<string, List<string>>`


