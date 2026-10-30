using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services;

public static class NameConverter
{
    // Hungarian notation prefixes to remove (order matters - longer prefixes first)
    private static readonly string[] HungarianPrefixes = 
    {
        "lpsz", "lpwsz", "lpv", "lpw", "lpdw", "lpul", "lpus", "lpb", "lpc", "lpp", "lp",
        "dw", "fw", "ul", "us", "sz", "ts", "h", "b", "c", "w"
    };

    /// <summary>
    /// Convert struct name: wfs_idc_status → XfsIdcStatus
    /// </summary>
    public static string ConvertStructName(string originalName)
    {
        if (string.IsNullOrEmpty(originalName))
            return "";

        // wfs → Xfs
        if (originalName.StartsWith("wfs_", StringComparison.OrdinalIgnoreCase))
        {
            originalName = "xfs_" + originalName.Substring(4);
        }
        else if (originalName.StartsWith("wfs", StringComparison.OrdinalIgnoreCase))
        {
            originalName = "xfs" + originalName.Substring(3);
        }

        // Use ToPascalCase for intelligent word splitting
        // xfs_undevmsg → XfsUndevMsg, xfs_hwerror → XfsHwError
        return ToPascalCase(originalName);
    }

    /// <summary>
    /// Convert enum name: fwDevice → IdcDeviceStatus
    /// Format: {Device}{FieldWithoutPrefix}{Suffix}
    /// </summary>
    public static string ConvertEnumName(string originalFieldName, string commandName, string deviceName)
    {
        if (string.IsNullOrEmpty(originalFieldName))
            return "";

        // Remove Hungarian notation prefix (already starts with capital letter)
        var fieldWithoutPrefix = RemoveHungarianPrefix(originalFieldName);
        
        // Get suffix based on command name
        var suffix = GetEnumSuffix(commandName);
        
        // Format: {Device}{Field}{Suffix}
        // deviceName is already in correct format (Idc, Cdm, etc.)
        return $"{deviceName}{fieldWithoutPrefix}{suffix}";
    }

    /// <summary>
    /// Remove Hungarian notation prefix from field name
    /// fwDevice → Device, lpszExtra → Extra
    /// Already converted names (starting with uppercase) are not modified.
    /// </summary>
    public static string RemoveHungarianPrefix(string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
            return "";

        // If field name starts with uppercase, it's already converted - don't modify
        if (char.IsUpper(fieldName[0]))
            return fieldName;

        var lowerField = fieldName.ToLower();
        
        // Try to match longest prefix first
        foreach (var prefix in HungarianPrefixes.OrderByDescending(p => p.Length))
        {
            if (lowerField.StartsWith(prefix))
            {
                // Check if next character is uppercase or underscore (to avoid false matches)
                var afterPrefix = fieldName.Substring(prefix.Length);
                if (afterPrefix.Length > 0 && (char.IsUpper(afterPrefix[0]) || afterPrefix[0] == '_'))
                {
                    return afterPrefix.TrimStart('_');
                }
            }
        }
        
        return fieldName;
    }

    /// <summary>
    /// Get enum suffix based on command name
    /// _STATUS → Status, _CAPABILITIES → Capabilities, other → ""
    /// </summary>
    public static string GetEnumSuffix(string commandName)
    {
        if (string.IsNullOrEmpty(commandName))
            return "";

        var upperCommand = commandName.ToUpper();
        
        if (upperCommand.EndsWith("_STATUS"))
            return "Status";
        
        if (upperCommand.EndsWith("_CAPABILITIES"))
            return "Capabilities";
        
        return "";
    }

    /// <summary>
    /// Capitalize first letter of a string (for struct names only - keeps rest lowercase)
    /// </summary>
    private static string Capitalize(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";
        
        if (text.Length == 1)
            return text.ToUpper();
        
        return char.ToUpper(text[0]) + text.Substring(1).ToLower();
    }

    /// <summary>
    /// Capitalize first letter only (preserves camelCase)
    /// </summary>
    public static string CapitalizeFirstOnly(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";
        
        if (text.Length == 1)
            return text.ToUpper();
        
        return char.ToUpper(text[0]) + text.Substring(1);
    }

    /// <summary>
    /// Convert field name: fwPowerOnOption → PowerOnOption (preserves camelCase)
    /// </summary>
    public static string ConvertFieldName(string originalFieldName)
    {
        if (string.IsNullOrEmpty(originalFieldName))
            return "";

        // Remove Hungarian notation prefix (already starts with capital letter)
        return RemoveHungarianPrefix(originalFieldName);
    }

    /// <summary>
    /// Convert field name for JSON output with special handling for reserved keywords
    /// wClass → DeviceClass, fwType → DeviceType (if has enum), bCompound → Compound
    /// </summary>
    public static string ConvertFieldNameForJson(string originalFieldName, bool hasEnum)
    {
        if (string.IsNullOrEmpty(originalFieldName))
            return "";

        // Special case: wClass always becomes DeviceClass
        if (originalFieldName.Equals("wClass", StringComparison.OrdinalIgnoreCase))
            return "DeviceClass";

        // Remove Hungarian notation prefix
        var converted = RemoveHungarianPrefix(originalFieldName);

        // Special case: if result is "Type" and field has enum, change to DeviceType
        if (converted == "Type" && hasEnum)
            return "DeviceType";

        return converted;
    }

    /// <summary>
    /// Extract device name from command name
    /// WFS_INF_IDC_STATUS → Idc
    /// </summary>
    public static string ExtractDeviceFromCommand(string commandName)
    {
        if (string.IsNullOrEmpty(commandName))
            return "";

        // Pattern: WFS_{CMD|INF|SRVE|EXEE}_{DEVICE}_...
        var match = Regex.Match(commandName, @"WFS_(?:CMD|INF|SRVE|EXEE|USRE)_([A-Z]+)_", RegexOptions.IgnoreCase);
        
        if (match.Success)
        {
            var deviceName = match.Groups[1].Value;
            // IDC → Idc, CDM → Cdm
            return CapitalizeFirstOnly(deviceName.ToLower());
        }
        
        return "";
    }

    /// <summary>
    /// Convert UPPER_CASE or concatenated words to PascalCase with intelligent word splitting
    /// Examples: DEVONLINE → DevOnline, DEVPOWEROFF → DevPowerOff, MEDIA_PRESENT → MediaPresent
    /// </summary>
    public static string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        // Önce underscore ile ayrılmış parçaları işle
        var parts = input.Split('_', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length > 1)
        {
            var result = new StringBuilder();
            foreach (var part in parts)
            {
                // Her parça için de kelime bazlı ayrıştırma yap
                result.Append(SplitWordsAndConvertToPascalCase(part));
            }
            return result.ToString();
        }

        // Underscore yoksa, kelime bazlı ayrıştırma yap
        return SplitWordsAndConvertToPascalCase(input);
    }

    private static string ConvertWordToPascalCase(string word)
    {
        if (string.IsNullOrEmpty(word))
            return "";

        if (word.Length == 1)
            return word.ToUpper();

        return char.ToUpper(word[0]) + word.Substring(1).ToLower();
    }

    private static string SplitWordsAndConvertToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        // Bilinen kelime pattern'leri (uzun olandan kısa olana doğru sıralı)
        // NOT: HWERROR, USERERROR gibi birleşik kelimeler yerine HW, USER, ERROR ayrı ayrı kullanılıyor
        var knownWords = new[] { 
            "POTENTIAL", "FRAUD", "ATTEMPT", "DEVICE", "POSITION", "UNKNOWN", "NOTSUPP",
            "POWERED", "OFFLINE", "ONLINE", "NODEVICE", "USER",
            "STRIPE", "READ", "WRITE", "CARD", "CHIP", "TRACK", "DATA", "INFO", 
            "STATUS", "ERROR", "ACTION", "MAGNETIC", "CONTACT", "DIP", "SWIPE",
            "MOTOR", "POWER", "EJECT", "RETAIN", "PARK", "FORM", "FIELD", "SECURE",
            "MEDIA", "PRESENT", "BUSY", "MODULE", "MOD", "READY", "NOT", "SUPP", "OPEN",
            "INSERTED", "REMOVED", "DETECTED", "RETAINED", "THRESHOLD", "INVALID", "EMVCLESS",
            "POWERSAVE", "CHANGED", "CHANGE", "INSERT", "OK", "INOP", "OFF", "DEV",
            "UNDEV", "MSG", "APP", "DISCONNECT", "DISC", "HW", "VRSN", "API", "VERSION", "SERVICE",
            "TRANSACTION", "STATE", "COMPOUND", "VENDOR", "MODE", "INTERFACE", "FIRMWARE",
            "SOFTWARE", "RESULT", "XFS"
        };

        var upperInput = input.ToUpper();
        var words = new List<string>();
        var remaining = upperInput;

        // Baştan başlayarak bilinen kelimeleri bul
        while (remaining.Length > 0)
        {
            bool found = false;
            foreach (var knownWord in knownWords.OrderByDescending(w => w.Length))
            {
                if (remaining.StartsWith(knownWord, StringComparison.OrdinalIgnoreCase))
                {
                    words.Add(knownWord);
                    remaining = remaining.Substring(knownWord.Length);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                // Bilinen kelime bulunamadı, kalan kısmı tek kelime olarak al
                if (remaining.Length > 0)
                {
                    words.Add(remaining);
                }
                break;
            }
        }

        // Kelimeleri PascalCase'e çevir ve birleştir
        var result = new StringBuilder();
        foreach (var word in words)
        {
            result.Append(ConvertWordToPascalCase(word));
        }

        var finalResult = result.ToString();
        
        // Son "Ed" düzeltmesi: CardEjectEd → CardEjected
        // Eğer sonuç "Ed" ile bitiyorsa ve önceki karakter büyük harf ise, "ed" yap
        if (finalResult.Length >= 3 && finalResult.EndsWith("Ed"))
        {
            var beforeEd = finalResult[finalResult.Length - 3];
            if (char.IsLower(beforeEd))
            {
                finalResult = finalResult.Substring(0, finalResult.Length - 2) + "ed";
            }
        }

        return finalResult;
    }
}


