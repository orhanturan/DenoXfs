using System.Text;
using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services;

public sealed record HeaderDefine(string Name, string RawValue, int Order);

public sealed record HeaderField(string CType, string Name, string? ArraySize, bool IsDoublePointer);

public sealed record HeaderTypedef(string Tag, string TypedefName, string PointerTypedefName, IReadOnlyList<HeaderField> Fields, bool HasUnion);

public sealed record FieldRef(string TypedefName, string Field);

/// <summary>A run of #defines introduced by one header comment ("values of WFSIDCSTATUS.fwMedia, ...").</summary>
public sealed record HeaderValueGroup(string Comment, IReadOnlyList<FieldRef> Owners, IReadOnlyList<string> DefineNames);

public sealed class HeaderModel
{
    public HeaderModel(string device)
    {
        Device = device;
    }

    public string Device { get; }
    public List<HeaderDefine> Defines { get; } = new();
    public Dictionary<string, HeaderDefine> DefineByName { get; } = new(StringComparer.Ordinal);
    public List<HeaderTypedef> Structs { get; } = new();
    public List<HeaderValueGroup> Groups { get; } = new();
}

/// <summary>Reads #defines, comment-delimited value groups and typedef structs from an XFS C header.</summary>
public static class HeaderParser
{
    private static readonly Regex TokenRegex = new(
        @"(?<c>/\*.*?\*/)|(?<d>^[ \t]*#[ \t]*define[ \t]+(?<n>\w+)[ \t]*(?<v>[^\r\n]*))",
        RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex CommentRegex = new(@"/\*.*?\*/", RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex StructRegex = new(
        @"typedef\s+struct\s+_?(?<tag>\w+)\s*\{(?<body>.*?)\}\s*(?<name>\w+)\s*,\s*\*\s*(?<ptr>\w+)\s*;",
        RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex FieldRegex = new(
        @"^(?<type>[A-Za-z_]\w*)\s*(?<star>\*)?\s*\*?\s*(?<name>[A-Za-z_]\w*)\s*(?:\[(?<size>[^\]]+)\])?$",
        RegexOptions.Compiled);

    private static readonly Regex OwnerRegex = new(@"\b(?<t>WFS[A-Z0-9]+)\s*\.\s*(?<f>\w+)", RegexOptions.Compiled);

    private static readonly Regex ValuesCommentRegex = new(@"\bvalues?\s+of\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static HeaderModel ParseFile(string path, string device)
    {
        return Parse(File.ReadAllText(path), device);
    }

    public static HeaderModel Parse(string text, string device)
    {
        var model = new HeaderModel(device);
        ParseDefinesAndGroups(text, model);
        ParseStructs(text, model);
        return model;
    }

    private static void ParseDefinesAndGroups(string text, HeaderModel model)
    {
        string? pendingComment = null;
        List<string>? currentGroup = null;
        var groupComments = new List<(string comment, List<string> names)>();
        var order = 0;

        foreach (Match m in TokenRegex.Matches(text))
        {
            if (m.Groups["c"].Success)
            {
                var comment = NormalizeComment(m.Groups["c"].Value);
                if (comment.Length == 0 || comment.StartsWith("Note", StringComparison.OrdinalIgnoreCase))
                    continue;

                pendingComment = comment;
                currentGroup = null;
                continue;
            }

            var name = m.Groups["n"].Value;
            var value = CommentRegex.Replace(m.Groups["v"].Value, "").Trim();
            if (value.Length == 0)
                continue;

            var define = new HeaderDefine(name, value, order++);
            model.Defines.Add(define);
            model.DefineByName.TryAdd(name, define);

            if (currentGroup == null)
            {
                currentGroup = new List<string>();
                groupComments.Add((pendingComment ?? "", currentGroup));
            }

            currentGroup.Add(name);
        }

        foreach (var (comment, names) in groupComments)
        {
            var owners = new List<FieldRef>();
            if (ValuesCommentRegex.IsMatch(comment))
            {
                foreach (Match o in OwnerRegex.Matches(comment))
                {
                    var fieldRef = new FieldRef(o.Groups["t"].Value, o.Groups["f"].Value);
                    if (!owners.Contains(fieldRef))
                        owners.Add(fieldRef);
                }
            }

            model.Groups.Add(new HeaderValueGroup(comment, owners, names));
        }
    }

    private static void ParseStructs(string text, HeaderModel model)
    {
        var clean = CommentRegex.Replace(text, "");

        foreach (Match m in StructRegex.Matches(clean))
        {
            var body = m.Groups["body"].Value;
            var tag = m.Groups["tag"].Value;
            var hasUnion = body.Contains("union", StringComparison.Ordinal);
            var fields = new List<HeaderField>();

            if (!hasUnion)
            {
                foreach (var piece in body.Split(';'))
                {
                    var line = Regex.Replace(piece, @"\s+", " ").Trim();
                    if (line.Length == 0)
                        continue;

                    var f = FieldRegex.Match(line);
                    if (!f.Success)
                        throw new InvalidOperationException($"Cannot parse field '{line}' in struct {tag}");

                    var size = f.Groups["size"].Success ? f.Groups["size"].Value.Trim() : null;
                    fields.Add(new HeaderField(f.Groups["type"].Value, f.Groups["name"].Value, size, f.Groups["star"].Success));
                }
            }

            model.Structs.Add(new HeaderTypedef(tag, m.Groups["name"].Value, m.Groups["ptr"].Value, fields, hasUnion));
        }
    }

    private static string NormalizeComment(string raw)
    {
        var inner = raw.Substring(2, raw.Length - 4);
        inner = Regex.Replace(inner, @"\*+", " ");
        return Regex.Replace(inner, @"\s+", " ").Trim();
    }
}

public sealed record ConstValue(long? Number, string? Text, bool IsHex, bool IsChar, bool UsesWmUser);

/// <summary>Evaluates #define values (numbers, characters, strings, references and +,-,* expressions).</summary>
public sealed class ConstantResolver
{
    private const long WmUser = 0x400;
    private readonly IReadOnlyList<HeaderModel> _models;

    public ConstantResolver(params HeaderModel[] models)
    {
        _models = models;
    }

    public ConstValue? Resolve(string name)
    {
        return ResolveName(name, new HashSet<string>(StringComparer.Ordinal));
    }

    public ConstValue? Evaluate(string expression)
    {
        return EvaluateText(expression, new HashSet<string>(StringComparer.Ordinal));
    }

    private ConstValue? ResolveName(string name, HashSet<string> visiting)
    {
        if (name == "WM_USER")
            return new ConstValue(WmUser, null, false, false, true);

        if (!visiting.Add(name))
            return null;

        try
        {
            foreach (var model in _models)
            {
                if (model.DefineByName.TryGetValue(name, out var define))
                    return EvaluateText(define.RawValue, visiting);
            }

            return null;
        }
        finally
        {
            visiting.Remove(name);
        }
    }

    private ConstValue? EvaluateText(string raw, HashSet<string> visiting)
    {
        var trimmed = raw.Trim();

        if (trimmed.Length >= 2 && trimmed[0] == '"' && trimmed[^1] == '"')
            return new ConstValue(null, trimmed.Substring(1, trimmed.Length - 2), false, false, false);

        if (trimmed.Length == 3 && trimmed[0] == '\'' && trimmed[2] == '\'')
            return new ConstValue(trimmed[1], null, false, true, false);

        var parser = new ExpressionParser(trimmed, name => ResolveName(name, visiting));
        var result = parser.Parse();
        if (result == null)
            return null;

        var unwrapped = trimmed;
        while (unwrapped.StartsWith('(') && unwrapped.EndsWith(')'))
            unwrapped = unwrapped.Substring(1, unwrapped.Length - 2).Trim();

        var isHex = Regex.IsMatch(unwrapped, @"^0[xX][0-9a-fA-F]+[uUlL]*$");
        return result with { IsHex = isHex };
    }

    private sealed class ExpressionParser
    {
        private readonly string _text;
        private readonly Func<string, ConstValue?> _resolve;
        private int _pos;
        private bool _wm;
        private bool _failed;

        public ExpressionParser(string text, Func<string, ConstValue?> resolve)
        {
            _text = text;
            _resolve = resolve;
        }

        public ConstValue? Parse()
        {
            var value = ParseExpression();
            SkipSpaces();
            if (_failed || value == null || _pos != _text.Length)
                return null;

            return new ConstValue(value, null, false, false, _wm);
        }

        private long? ParseExpression()
        {
            var left = ParseTerm();
            while (left != null)
            {
                SkipSpaces();
                if (Peek('+')) { _pos++; var r = ParseTerm(); left = r == null ? null : left + r; }
                else if (Peek('-')) { _pos++; var r = ParseTerm(); left = r == null ? null : left - r; }
                else break;
            }

            return left;
        }

        private long? ParseTerm()
        {
            var left = ParseUnary();
            while (left != null)
            {
                SkipSpaces();
                if (Peek('*')) { _pos++; var r = ParseUnary(); left = r == null ? null : left * r; }
                else break;
            }

            return left;
        }

        private long? ParseUnary()
        {
            SkipSpaces();
            if (Peek('-')) { _pos++; var v = ParseUnary(); return v == null ? null : -v; }
            return ParsePrimary();
        }

        private long? ParsePrimary()
        {
            SkipSpaces();
            if (_pos >= _text.Length) { _failed = true; return null; }

            if (Peek('('))
            {
                _pos++;
                var inner = ParseExpression();
                SkipSpaces();
                if (!Peek(')')) { _failed = true; return null; }
                _pos++;
                return inner;
            }

            var number = Regex.Match(_text.Substring(_pos), @"^(0[xX][0-9a-fA-F]+|\d+)[uUlL]*");
            if (number.Success)
            {
                _pos += number.Length;
                var digits = Regex.Match(number.Value, @"^(0[xX][0-9a-fA-F]+|\d+)").Value;
                return digits.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                    ? System.Convert.ToInt64(digits, 16)
                    : long.Parse(digits);
            }

            var ident = Regex.Match(_text.Substring(_pos), @"^[A-Za-z_]\w*");
            if (ident.Success)
            {
                _pos += ident.Length;
                var resolved = _resolve(ident.Value);
                if (resolved?.Number == null) { _failed = true; return null; }
                _wm |= resolved.UsesWmUser;
                return resolved.Number;
            }

            _failed = true;
            return null;
        }

        private bool Peek(char c) => _pos < _text.Length && _text[_pos] == c;

        private void SkipSpaces()
        {
            while (_pos < _text.Length && char.IsWhiteSpace(_text[_pos]))
                _pos++;
        }
    }
}
