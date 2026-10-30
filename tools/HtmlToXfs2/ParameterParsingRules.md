# Parameter Parsing Rules

Bu dosya, HTML'den JSON'a parameter parsing için kullanılan kuralları içerir.

## Primitive Type Detection Rules

### Primitive Tipler
Aşağıdaki tipler primitive olarak kabul edilir:
- `WORD`, `SHORT`, `USHORT`, `LONG`, `ULONG`, `DWORD`
- `LPSTR`, `LPBYTE` (string ve byte pointer'ları)
- `LP` prefix'li pointer'lar da primitive sayılır (örn: `LPWORD`, `LPSHORT`, `LPULONG`)

### Primitive Type JSON Format
Primitive tipler için sadece 3 property gerekli:
```json
"InputParam": {
  "Value": "LPWORD lpwReadData",
  "ParamType": "LPWORD", 
  "Name": "lpwReadData"
}
```

## Struct Type Detection Rules

### Struct Tipler
- Primitive olmayan tipler (örn: `LPWFSIDCSTATUS`, `LPWFSIDCCHIPIO`)
- HTML'de parameter'dan sonra `typedef struct` tanımı bulunur
- Struct tip detection: `IsPrimitiveType()` false döndürürse struct'tır

### Struct Type JSON Format
Struct tipler için 5 property gerekli:
```json
"OutputParam": {
  "Value": "LPWFSIDCSTATUS lpStatus",
  "ParamType": "LPWFSIDCSTATUS",
  "Name": "lpStatus", 
  "OriginalStructureName": "wfs_idc_status",
  "Fields": {
    "fwDevice": "WORD",
    "fwMedia": "WORD",
    "usCards": "USHORT"
  }
}
```

## Field Parsing Rules

### Struct Definition Format
```html
typedef struct _struct_name 
{ 
  WORD    fieldName1; 
  ULONG   fieldName2; 
  LPBYTE  fieldName3; 
} STRUCTNAME, *LPSTRUCTNAME;
```

### Field Parsing Rules
1. `_struct_name` → `struct_name` (underscore kaldır)
2. Her field → `"fieldName": "fieldType"` Dictionary formatında
3. Array field'lar → `DWORD dwGuidLights[WFS_IDC_GUIDLIGHTS_SIZE]` → `"dwGuidLights": "DWORD[WFS_IDC_GUIDLIGHTS_SIZE]"`

## Detection Logic

### Primitive Type Detection
```csharp
bool IsPrimitiveType(string paramType)
{
    var primitives = new[] { "WORD", "SHORT", "USHORT", "LONG", "ULONG", "DWORD", "LPSTR", "LPBYTE" };
    var cleanType = paramType.StartsWith("LP") ? paramType.Substring(2) : paramType;
    return primitives.Contains(cleanType);
}
```

### Struct Detection Pattern
```csharp
var structPattern = @"typedef\s+struct\s+_?(\w+)\s*\{([^}]+)\}\s*(\w+)";
```

### Field Extraction Pattern
```csharp
var fieldPattern = @"(\w+(?:\[[\w_]+\])?)\s+(\w+)\s*;";
```

## Örnekler

### Primitive Örnek (WFS_CMD_IDC_READ_RAW_DATA)
**HTML:**
```html
<h3>Input Param</h3>
<p> LPWORD lpwReadData;
```

**JSON:**
```json
"InputParam": {
  "Value": "LPWORD lpwReadData",
  "ParamType": "LPWORD", 
  "Name": "lpwReadData"
}
```

### Struct Örnek (WFS_CMD_IDC_CHIP_IO)
**HTML:**
```html
<h3>Input Param</h3>
<p> LPWFSIDCCHIPIO lpChipIoIn; 
<br/>typedef struct _wfs_idc_chip_io 
<br/> { 
<br/> WORD    wChipProtocol; 
<br/> ULONG   ulChipDataLength; 
<br/> LPBYTE   lpbChipData; 
<br/> } WFSIDCCHIPIO, *LPWFSIDCCHIPIO;
```

**JSON:**
```json
"InputParam": {
  "Value": "LPWFSIDCCHIPIO lpChipIoIn",
  "ParamType": "LPWFSIDCCHIPIO",
  "Name": "lpChipIoIn",
  "StructureName": "wfs_idc_chip_io",
  "Fields": {
    "wChipProtocol": "WORD",
    "ulChipDataLength": "ULONG", 
    "lpbChipData": "LPBYTE"
  }
}
```

