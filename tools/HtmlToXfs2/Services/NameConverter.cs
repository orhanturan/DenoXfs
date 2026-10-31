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
    /// Property name of a header field: everything before the first uppercase letter is Hungarian notation.
    /// hService → Service, lpvOldStatus → OldStatus, RequestID → RequestID
    /// </summary>
    public static string ToPropertyName(string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
            return "";

        if (char.IsUpper(fieldName[0]))
            return fieldName;

        for (var i = 0; i < fieldName.Length; i++)
        {
            if (char.IsUpper(fieldName[i]))
                return fieldName.Substring(i);
        }

        return char.ToUpper(fieldName[0]) + fieldName.Substring(1);
    }

    /// <summary>#define name as it appears on the constants class: WFS_INF_IDC_STATUS → InfIdcStatus.</summary>
    public static string ConstantName(string defineName, string device)
    {
        return ToPascalCase(EnumMemberNamer.StripFamilyPrefix(defineName, device));
    }

    /// <summary>
    /// Convert a device struct tag: wfs_idc_emvcless_tx_data_output → IdcEmvclessTxDataOutput.
    /// Underscores in the tag mark the word boundaries, so no word dictionary is needed.
    /// </summary>
    public static string DeviceStructName(string tag)
    {
        var parts = tag.Split('_', StringSplitOptions.RemoveEmptyEntries)
            .Where(p => !p.Equals("wfs", StringComparison.OrdinalIgnoreCase));
        return string.Concat(parts.Select(Capitalize));
    }

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

        // Longest match wins. Prefer XFS fragments that appear alone; keep 2-letter tokens rare.
        // APPLICATIONS before APP, CONTACTLESS before CONTACT, INACTIVE/INTELLIGENT before IN.
        var knownWords = new[] {
            "CONTACTLESS", "APPLICATIONS", "APPLICATION", "CONFIRMATION", "INTELLIGENT",
            "PROCESSING", "TRANSPORT", "POWERSAVE", "THRESHOLD", "DISCONNECT", "INTERFACE",
            "TRANSACTION", "SYNCHRONIZE", "CONFIGURED", "INITIATED", "COLLISION", "TERMINAL",
            "COMPLETION", "PREFERENCE", "APPROVE", "REQUEST", "DECLINE",
            "MAGNETIC", "INACTIVE", "REQUIRED", "INSERTED", "REMOVED", "DETECTED", "RETAINED",
            "POSITION", "POTENTIAL", "FIRMWARE", "SOFTWARE", "PROTOCOL", "SECURITY", "SHUTTER",
            "MISSING", "PRESENT", "UNKNOWN", "INVALID", "OFFLINE", "ONLINE", "POWERED",
            "ATTEMPT", "OBTAINED", "ANOTHER", "MULTIPLE", "VERIFIED", "READER", "DEVICE",
            "STATUS", "MODULE", "SECURE", "STRIPE", "TRACK", "WRITE", "READ", "CARD", "CARDS",
            "CHIP", "DATA", "INFO", "ERROR", "ACTION", "CONTACT", "LATCHED", "SWIPE", "MOTOR",
            "POWER", "EJECT", "RETAIN", "PARK", "FORM", "FIELD", "MEDIA", "BUSY", "READY",
            "OPEN", "CHANGE", "CHANGED", "INSERT", "FRAUD", "USER", "COMPOUND", "VENDOR",
            "VERSION", "SERVICE", "RESULT", "STATE", "MODE", "TYPE", "COMMAND", "BALANCE",
            "AMOUNT", "IMAGE", "FRONT", "BACK", "SYNTAX", "FOUND", "PORT", "KEY", "DIP",
            "MOD", "NOT", "SUPP", "UNSUPP", "OK", "INOP", "OFF", "DEV", "UNDEV", "MSG",
            "APP", "DISC", "HW", "VRSN", "API", "XFS", "NO", "TOO", "LONG", "SHORT", "SRC",
            "THEN", "EXIT", "FAIL", "FULL", "HIGH", "BIN", "POS", "SEC", "MAG", "IMG", "AFM",
            "ATR", "CODE", "HOLD", "TRY", "AGAIN", "FLUX", "END", "JAM", "AID", "CVM", "PIN",
            "SIGN", "IDLE", "RETAP", "IN", "ON", "EMVCLESS"
        };

        var upperInput = input.ToUpper();
        var words = new List<string>();
        var remaining = upperInput;
        var ordered = knownWords.OrderByDescending(w => w.Length).ToArray();

        // Match longest known fragment. 1-2 letter matches only stick if the rest
        // is empty or itself starts with a known word (IN+POSITION yes, IN+F no).
        while (remaining.Length > 0)
        {
            var match = ordered.FirstOrDefault(known =>
                remaining.StartsWith(known, StringComparison.OrdinalIgnoreCase)
                && (known.Length >= 3 || RestStartsWithKnownWord(remaining.Substring(known.Length), ordered)));

            if (match == null)
            {
                words.Add(remaining);
                break;
            }

            words.Add(match);
            remaining = remaining.Substring(match.Length);
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

    private static bool RestStartsWithKnownWord(string rest, IReadOnlyList<string> orderedKnownWords)
    {
        if (rest.Length == 0)
            return true;

        return orderedKnownWords.Any(known =>
            known.Length >= 3 && rest.StartsWith(known, StringComparison.OrdinalIgnoreCase));
    }
}


