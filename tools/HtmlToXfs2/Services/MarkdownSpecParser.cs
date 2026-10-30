using System.Text.RegularExpressions;
using HtmlToXfs2.Model;

namespace HtmlToXfs2.Services;

public class MarkdownSpecParser
{
    private static readonly Regex CommandHeadingRegex = new(
        @"(?im)^\s*(?:\d+(?:\.\d+)?\s+)?(?<command>WFS_[A-Z0-9_]+)\s*$",
        RegexOptions.Compiled);

    private static readonly Regex StructRegex = new(
        @"(?is)typedef\s+struct\s+_?(?<structName>\w+)\s*\{(?<body>.*?)\}\s*(?<alias>\w+)\s*(?:,\s*\*?(?<pointer>\w+)\s*)?;",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex FieldRowRegex = new(
        @"(?im)^\s*\|\s*(?<type>[^|]+?)\s*\|\s*(?:[^|]+?)\s*\|\s*(?<field>[^|]+?)\s*\|\s*$",
        RegexOptions.Compiled);

    public List<WfsCommand> ParseDevice(string markdownPath, string deviceName)
    {
        if (string.IsNullOrWhiteSpace(markdownPath) || !File.Exists(markdownPath))
        {
            return new List<WfsCommand>();
        }

        var content = File.ReadAllText(markdownPath);
        var matches = CommandHeadingRegex.Matches(content);
        var commands = new List<WfsCommand>();

        for (var i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            var commandName = match.Groups["command"].Value;
            var startIndex = match.Index;
            var endIndex = i < matches.Count - 1 ? matches[i + 1].Index : content.Length;
            var block = content.Substring(startIndex, endIndex - startIndex);

            if (string.IsNullOrWhiteSpace(commandName) || !commandName.StartsWith("WFS_", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            commands.Add(ParseCommand(commandName, block, deviceName));
        }

        return commands;
    }

    private static WfsCommand ParseCommand(string commandName, string block, string deviceName)
    {
        var description = ExtractDescription(block);
        var inputParam = ParseInputParam(block, commandName, deviceName);
        var outputParam = ParseOutputParam(block, commandName, deviceName);

        return new WfsCommand
        {
            CommandName = commandName,
            Description = description,
            Comments = string.Empty,
            InputParam = inputParam,
            OutputParam = outputParam,
            AdditionalStructs = null
        };
    }

    private static string ExtractDescription(string block)
    {
        var match = Regex.Match(block, @"(?is)Description\s*(?:\r?\n|\s+)(.*?)(?=\s*Input Param\s+|\s*Output Param\s+|\s*Event Param\s+|\s*typedef struct)", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            return NormalizeWhitespace(match.Groups[1].Value);
        }

        return string.Empty;
    }

    private static InputParam? ParseInputParam(string block, string commandName, string deviceName)
    {
        return ParseNamedParam(block, "Input Param", commandName, deviceName, isInput: true);
    }

    private static OutputParam? ParseOutputParam(string block, string commandName, string deviceName)
    {
        var outputMatch = Regex.Match(block, @"(?is)Output Param\s+(?<value>.*?)(?=(?:\s*\r?\n\s*(?:Input Param|Event Param|typedef struct|WFS_[A-Z0-9_]+))|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (!outputMatch.Success)
        {
            return null;
        }

        var rawValue = outputMatch.Groups["value"].Value.Trim();
        var normalizedValue = NormalizeWhitespace(rawValue);

        if (normalizedValue.Equals("None.", StringComparison.OrdinalIgnoreCase) || normalizedValue.Equals("None", StringComparison.OrdinalIgnoreCase))
        {
            return new OutputParam
            {
                Value = normalizedValue,
                ParamType = "None",
                Name = "None",
                Fields = new Dictionary<string, string>(),
                Enums = new Dictionary<string, EnumInfo>()
            };
        }

        var structMatch = StructRegex.Match(block);
        if (!structMatch.Success)
        {
            return new OutputParam
            {
                Value = normalizedValue,
                ParamType = string.Empty,
                Name = string.Empty,
                Fields = new Dictionary<string, string>(),
                Enums = new Dictionary<string, EnumInfo>()
            };
        }

        var structName = structMatch.Groups["structName"].Value;
        var fields = ParseStructFields(structMatch.Groups["body"].Value);
        var enumMap = ExtractEnumMap(block, fields, commandName, deviceName);

        return new OutputParam
        {
            Value = normalizedValue,
            OriginalParamType = rawValue.Contains(" ") ? rawValue.Split(' ', 2)[0] : rawValue,
            ParamType = string.Empty,
            OriginalName = rawValue.Contains(" ") ? rawValue.Split(' ', 2)[1].TrimEnd(';') : string.Empty,
            Name = string.Empty,
            OriginalStructureName = structName,
            StructureName = NameConverter.ConvertStructName(structName),
            Fields = fields,
            Enums = enumMap
        };
    }

    private static InputParam? ParseNamedParam(string block, string label, string commandName, string deviceName, bool isInput)
    {
        var labelPattern = $@"{Regex.Escape(label)}\s+(?<value>.*?)(?=(?:\s*\r?\n\s*(?:Output Param|Input Param|Event Param|typedef struct|WFS_[A-Z0-9_]+))|$)";
        var match = Regex.Match(block, labelPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (!match.Success)
        {
            return null;
        }

        var rawValue = match.Groups["value"].Value.Trim();
        var normalizedValue = NormalizeWhitespace(rawValue);

        if (normalizedValue.Equals("None.", StringComparison.OrdinalIgnoreCase) || normalizedValue.Equals("None", StringComparison.OrdinalIgnoreCase))
        {
            return new InputParam
            {
                Value = normalizedValue,
                ParamType = "None",
                Name = "None",
                Fields = new Dictionary<string, string>(),
                Enums = new Dictionary<string, EnumInfo>()
            };
        }

        var structMatch = StructRegex.Match(block);
        if (!structMatch.Success)
        {
            return new InputParam
            {
                Value = normalizedValue,
                ParamType = string.Empty,
                Name = string.Empty,
                Fields = new Dictionary<string, string>(),
                Enums = new Dictionary<string, EnumInfo>()
            };
        }

        var structName = structMatch.Groups["structName"].Value;
        var fields = ParseStructFields(structMatch.Groups["body"].Value);
        var enumMap = ExtractEnumMap(block, fields, commandName, deviceName);

        return new InputParam
        {
            Value = normalizedValue,
            OriginalParamType = rawValue.Contains(" ") ? rawValue.Split(' ', 2)[0] : rawValue,
            ParamType = string.Empty,
            OriginalName = rawValue.Contains(" ") ? rawValue.Split(' ', 2)[1].TrimEnd(';') : string.Empty,
            Name = string.Empty,
            OriginalStructureName = structName,
            StructureName = NameConverter.ConvertStructName(structName),
            Fields = fields,
            Enums = enumMap
        };
    }

    private static Dictionary<string, string> ParseStructFields(string body)
    {
        var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var rows = FieldRowRegex.Matches(body);

        foreach (Match row in rows)
        {
            var type = row.Groups["type"].Value.Trim();
            var field = row.Groups["field"].Value.Trim();

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(field))
            {
                continue;
            }

            var cleanField = field.Trim();
            cleanField = cleanField.TrimEnd(';');
            cleanField = cleanField.Trim();
            if (cleanField.Contains(" "))
            {
                cleanField = cleanField.Split(' ', 2)[0].Trim();
            }

            var cleanType = type.Trim();
            if (cleanType.Contains("|"))
            {
                cleanType = cleanType.Split('|')[0].Trim();
            }

            if (cleanType.All(char.IsDigit) || cleanType.Contains("-"))
            {
                continue;
            }

            if (cleanField.Contains("["))
            {
                var namePart = cleanField.Split('[', 2)[0].Trim();
                fields[namePart] = cleanType;
            }
            else
            {
                fields[cleanField] = cleanType;
            }
        }

        return fields;
    }

    private static Dictionary<string, EnumInfo> ExtractEnumMap(string block, Dictionary<string, string> fields, string commandName, string deviceName)
    {
        var result = new Dictionary<string, EnumInfo>(StringComparer.OrdinalIgnoreCase);
        if (fields.Count == 0)
        {
            return result;
        }

        var orderedFields = fields.Keys.ToList();
        for (var i = 0; i < orderedFields.Count; i++)
        {
            var fieldName = orderedFields[i];
            var fieldType = fields[fieldName];
            var values = ExtractEnumValues(block, fieldName, orderedFields, i);
            if (values.Count == 0)
            {
                continue;
            }

            var enumName = NameConverter.ConvertEnumName(fieldName, commandName, deviceName);
            result[enumName] = new EnumInfo
            {
                Name = enumName,
                OriginalName = fieldName,
                Type = fieldType,
                Commands = new List<string> { commandName },
                Values = values
            };
        }

        return result;
    }

    private static List<string> ExtractEnumValues(string block, string fieldName, List<string> orderedFields, int index)
    {
        var matchIndex = block.IndexOf(fieldName, StringComparison.OrdinalIgnoreCase);
        if (matchIndex < 0)
        {
            return new List<string>();
        }

        var nextBoundary = block.Length;
        for (var i = index + 1; i < orderedFields.Count; i++)
        {
            var nextField = orderedFields[i];
            var nextIndex = block.IndexOf(nextField, matchIndex + fieldName.Length, StringComparison.OrdinalIgnoreCase);
            if (nextIndex > 0)
            {
                nextBoundary = Math.Min(nextBoundary, nextIndex);
            }
        }

        var segment = block.Substring(matchIndex, nextBoundary - matchIndex);
        var valueMeaningIndex = segment.IndexOf("Value Meaning", StringComparison.OrdinalIgnoreCase);
        if (valueMeaningIndex < 0)
        {
            return new List<string>();
        }

        var candidateText = segment.Substring(valueMeaningIndex);
        var values = new List<string>();

        foreach (Match candidate in Regex.Matches(candidateText, @"(?im)^(?:\s*\|\s*|\s*\.\s*|\s*)?(?<value>WFS_[A-Z0-9_]+)\b.*$"))
        {
            var value = NormalizeEnumValue(candidate.Groups["value"].Value);
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            if (IsCommandOrNonEnumConstant(value))
            {
                continue;
            }

            values.Add(value);
        }

        return values
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string NormalizeEnumValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var trimmed = value.Trim();
        trimmed = trimmed.TrimStart('(', '[', '{', '|', '.');
        trimmed = trimmed.TrimEnd(',', '.', ';', ':', ')', ']', '}');
        return trimmed.Trim();
    }

    private static bool IsCommandOrNonEnumConstant(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        var upper = value.ToUpperInvariant();

        return upper.StartsWith("WFS_CMD_", StringComparison.OrdinalIgnoreCase)
            || upper.StartsWith("WFS_INF_", StringComparison.OrdinalIgnoreCase)
            || upper.StartsWith("WFS_EVT_", StringComparison.OrdinalIgnoreCase)
            || upper.StartsWith("WFS_SRV_", StringComparison.OrdinalIgnoreCase)
            || upper.StartsWith("WFS_EXEE_", StringComparison.OrdinalIgnoreCase)
            || upper.StartsWith("WFS_SRVE_", StringComparison.OrdinalIgnoreCase)
            || upper.StartsWith("WFS_USRE_", StringComparison.OrdinalIgnoreCase)
            || upper.Contains("GUIDLIGHTS_MAX");
    }

    private static string NormalizeWhitespace(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        return Regex.Replace(input, @"\s+", " ").Trim();
    }
}
