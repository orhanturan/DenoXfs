using System.Text;
using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services;

/// <summary>Renders planned types and the header constants as C# source text.</summary>
public static class PlanWriter
{
    public static string WriteEnum(TypePlan plan, PlannedEnum planned)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"namespace {plan.Namespace(planned.Group)};");
        sb.AppendLine();

        if (planned.IsFlags)
            sb.AppendLine("[Flags]");

        sb.AppendLine($"public enum {planned.Name} : {planned.BaseType}");
        sb.AppendLine("{");
        for (var i = 0; i < planned.Members.Count; i++)
        {
            var member = planned.Members[i];
            sb.AppendLine($"    {member.Name} = {member.Value}{(i < planned.Members.Count - 1 ? "," : "")}");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    public static string WriteConstants(TypePlan plan, HeaderModel model, ConstantResolver resolver)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"namespace DenoXfs.{plan.Library};");
        sb.AppendLine();
        sb.AppendLine($"public static class {plan.ConstantsClass}");
        sb.AppendLine("{");

        var written = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var define in model.Defines)
        {
            var value = resolver.Resolve(define.Name);
            if (value == null)
                continue;

            var name = NameConverter.ConstantName(define.Name, model.Device);
            if (name.Length == 0 || !written.Add(name))
                continue;

            var (type, text) = FormatConstant(value);
            sb.AppendLine($"    public const {type} {name} = {text};");
        }

        if (plan.Device == "api")
        {
            sb.AppendLine();
            sb.AppendLine("    // Platform: private window message used by the message pump, not part of the XFS spec");
            sb.AppendLine("    public const uint WmXfsProcessRequest = 0x8001u;");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    private static (string type, string text) FormatConstant(ConstValue value)
    {
        if (value.Text != null)
            return ("string", $"\"{value.Text}\"");

        var number = value.Number!.Value;
        if (value.IsChar)
            return ("char", $"'{(char)number}'");

        if (number < 0 || number <= int.MaxValue && !value.IsHex && !value.UsesWmUser)
            return ("int", number.ToString());

        return ("uint", value.IsHex ? $"0x{number:x4}" : number.ToString());
    }

    public static string WriteStruct(TypePlan plan, PlannedStruct planned, ConstantResolver resolver)
    {
        if (planned.IsResult)
            return WriteResultStruct(plan.Namespace(planned.Group));

        var ns = plan.Namespace(planned.Group);
        var rows = planned.Fields.Select(f => (field: f, parsed: ParseField(f, planned, resolver))).ToList();

        var sb = new StringBuilder();
        sb.AppendLine("using System.Runtime.InteropServices;");
        if (rows.Any(r => r.parsed.Getter.Length > 0))
            sb.AppendLine("using DenoXfs.Interop.Convert;");
        if (rows.Any(r => r.parsed.Getter.Contains(".ToStructSafe", StringComparison.Ordinal)))
            sb.AppendLine("using DenoXfs.Interop.Extensions;");

        foreach (var other in rows.SelectMany(r => r.field.Namespaces).Where(n => n != ns).Distinct().OrderBy(n => n, StringComparer.Ordinal))
            sb.AppendLine($"using {other};");

        sb.AppendLine();
        sb.AppendLine($"namespace {ns};");
        sb.AppendLine();
        sb.AppendLine("[StructLayout(LayoutKind.Sequential, Pack = 1)]");
        sb.AppendLine($"public struct {planned.Name}");
        sb.AppendLine("{");

        foreach (var (field, parsed) in rows)
        {
            if (parsed.MarshalAs != null)
                sb.AppendLine($"    {parsed.MarshalAs}");

            sb.AppendLine($"    private {parsed.PrivateType} {PrivateName(field.Name)};");
        }

        sb.AppendLine();

        foreach (var (field, parsed) in rows)
        {
            var privateName = PrivateName(field.Name);
            sb.AppendLine($"    public {parsed.PublicType} {NameConverter.ToPropertyName(field.Name)}");
            sb.AppendLine("    {");
            sb.AppendLine(parsed.Getter.Length > 0 ? $"        get => {parsed.Getter};" : $"        get => {privateName};");
            sb.AppendLine(parsed.Setter.Length > 0 ? $"        set => {parsed.Setter};" : $"        set => {privateName} = value;");
            sb.AppendLine("    }");
            sb.AppendLine();
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    private sealed class Parsed
    {
        public string PrivateType = "";
        public string PublicType = "";
        public string? MarshalAs;
        public string Getter = "";
        public string Setter = "";
    }

    private static Parsed ParseField(PlannedField field, PlannedStruct owner, ConstantResolver resolver)
    {
        var name = PrivateName(field.Name);
        var type = field.Type;

        var dynamic = Regex.Match(type, @"^\*(\w+)\[(\w+)\]$");
        if (dynamic.Success)
            return ParseDynamicBytes(name, dynamic.Groups[2].Value, owner);

        if (type.StartsWith('@'))
        {
            var target = type.Substring(1);
            return new Parsed
            {
                PrivateType = "IntPtr",
                PublicType = $"{target}?",
                Getter = $"{name}.ToStructSafe<{target}>()",
                Setter = $"StructureConvert.ToIntPtr(ref {name}, value)"
            };
        }

        if (type.StartsWith('#'))
            return ParseEnum(name, type.Substring(1), resolver);

        var array = Regex.Match(type, @"^(\w+)\[(.+)\]$");
        if (array.Success)
        {
            var size = resolver.Evaluate(array.Groups[2].Value)?.Number
                       ?? throw new InvalidOperationException($"Cannot resolve array size '{array.Groups[2].Value}' of {owner.Name}.{field.Name}");
            var element = ConvertBaseType(array.Groups[1].Value);
            return new Parsed
            {
                PrivateType = $"{element}[]",
                PublicType = $"{element}[]",
                MarshalAs = $"[MarshalAs(UnmanagedType.ByValArray, SizeConst = {size})]"
            };
        }

        return ParseScalar(name, type);
    }

    private static Parsed ParseEnum(string name, string spec, ConstantResolver resolver)
    {
        var zero = Regex.Match(spec, @"^(\w+)\[ZeroTerminated\]$");
        if (zero.Success)
        {
            var enumName = zero.Groups[1].Value;
            return new Parsed
            {
                PrivateType = "IntPtr",
                PublicType = $"{enumName}?[]?",
                Getter = $"IntPtrConvert.ToZeroTerminatedArray<{enumName}>({name})",
                Setter = $"ZeroTerminatedArrayConvert.ToIntPtr(ref {name}, value)"
            };
        }

        var fixedArray = Regex.Match(spec, @"^(\w+)\[(.+)\]$");
        if (fixedArray.Success)
        {
            var size = resolver.Evaluate(fixedArray.Groups[2].Value)?.Number
                       ?? throw new InvalidOperationException($"Cannot resolve array size '{fixedArray.Groups[2].Value}'");
            var enumName = fixedArray.Groups[1].Value;
            return new Parsed
            {
                PrivateType = $"{enumName}[]",
                PublicType = $"{enumName}[]",
                MarshalAs = $"[MarshalAs(UnmanagedType.ByValArray, SizeConst = {size})]"
            };
        }

        return new Parsed { PrivateType = spec, PublicType = spec };
    }

    private static Parsed ParseDynamicBytes(string name, string countField, PlannedStruct owner)
    {
        var count = owner.Fields.FirstOrDefault(f => f.Name == countField)
                    ?? throw new InvalidOperationException($"Length field {countField} not found in {owner.Name}");
        var countName = PrivateName(count.Name);

        return new Parsed
        {
            PrivateType = "IntPtr",
            PublicType = "byte[]?",
            Getter = $"IntPtrConvert.ToByteArray({name}, (int){countName})",
            Setter = $"DynamicArrayConvert.ToIntPtr(ref {name}, value, out {countName})"
        };
    }

    private static Parsed ParseScalar(string name, string type)
    {
        var result = new Parsed { PrivateType = "IntPtr" };

        switch (type)
        {
            case "LPSTR":
                result.PublicType = "string?";
                result.Getter = $"IntPtrConvert.ToString({name})";
                result.Setter = $"StringConvert.ToIntPtr(ref {name}, value)";
                break;
            case "LPWSTR":
                result.PublicType = "string?";
                result.Getter = $"IntPtrConvert.ToWString({name})";
                result.Setter = $"WStringConvert.ToIntPtr(ref {name}, value)";
                break;
            case "LPBYTE":
                result.PublicType = "byte[]?";
                result.Getter = $"IntPtrConvert.ToByteArray({name})";
                result.Setter = $"ByteArrayConvert.ToIntPtr(ref {name}, value)";
                break;
            case "LPDWORD":
                result.PublicType = "uint[]?";
                result.Getter = $"IntPtrConvert.ToUInt32Array({name})";
                result.Setter = $"UInt32ArrayConvert.ToIntPtr(ref {name}, value)";
                break;
            default:
                if (type.StartsWith("LP", StringComparison.Ordinal))
                {
                    result.PublicType = "IntPtr";
                    break;
                }

                var csharp = ConvertBaseType(type);
                result.PrivateType = csharp;
                result.PublicType = csharp;
                if (type.Equals("BOOL", StringComparison.OrdinalIgnoreCase))
                    result.MarshalAs = "[MarshalAs(UnmanagedType.Bool)]";
                break;
        }

        return result;
    }

    private static string ConvertBaseType(string type)
    {
        return type.ToUpperInvariant() switch
        {
            "WORD" or "USHORT" or "HSERVICE" => "ushort",
            "SHORT" => "short",
            "DWORD" or "ULONG" or "UINT" or "REQUESTID" => "uint",
            "LONG" or "INT" or "HRESULT" => "int",
            "BOOL" => "bool",
            "CHAR" or "BYTE" or "UCHAR" => "byte",
            "SYSTEMTIME" => "SystemTime",
            "HANDLE" or "HWND" or "LPARAM" or "WPARAM" => "IntPtr",
            _ => type
        };
    }

    private static string PrivateName(string fieldName)
    {
        return fieldName.Length > 0 && char.IsUpper(fieldName[0])
            ? char.ToLowerInvariant(fieldName[0]) + fieldName.Substring(1)
            : fieldName;
    }

    // WFSRESULT contains a union, so it needs explicit offsets and is not derived from the header fields.
    private static string WriteResultStruct(string ns)
    {
        return $$"""
            using System.Runtime.InteropServices;

            namespace {{ns}};

            [StructLayout(LayoutKind.Explicit, Pack = 1)]
            public struct XfsResult
            {
                [FieldOffset(0)]
                private uint requestID;

                [FieldOffset(4)]
                private ushort hService;

                [FieldOffset(6)]
                private SystemTime tsTimestamp;

                [FieldOffset(22)]
                private int hResult;

                // commandCode and eventID share the same memory location
                [FieldOffset(26)]
                private uint dwCommandCode;

                [FieldOffset(26)]
                private uint dwEventID;

                [FieldOffset(30)]
                private IntPtr lpBuffer;

                public uint RequestID
                {
                    get => requestID;
                    set => requestID = value;
                }

                public ushort Service
                {
                    get => hService;
                    set => hService = value;
                }

                public SystemTime Timestamp
                {
                    get => tsTimestamp;
                    set => tsTimestamp = value;
                }

                public int Result
                {
                    get => hResult;
                    set => hResult = value;
                }

                public uint CommandCode
                {
                    get => dwCommandCode;
                    set => dwCommandCode = value;
                }

                public uint EventID
                {
                    get => dwEventID;
                    set => dwEventID = value;
                }

                public IntPtr Buffer
                {
                    get => lpBuffer;
                    set => lpBuffer = value;
                }
            }

            """;
    }
}
