using HtmlToXfs2.Services;

namespace XfsTestGen;

/// <summary>How a struct field is filled and compared by the generated C++ and C# test code.</summary>
public enum Kind
{
    /// <summary>Not covered (raw IntPtr, SYSTEMTIME, unknown pointers): left zero on both sides.</summary>
    Skip,
    Number,
    Bool,
    NumberArray,
    /// <summary>Fixed array of a type the generator does not fill: allocated but left zero.</summary>
    OpaqueArray,
    String,
    WString,
    /// <summary>LPBYTE ended by a 0 byte.</summary>
    BytesTerminated,
    /// <summary>LPBYTE with the preceding length field.</summary>
    BytesCounted,
    /// <summary>LPWORD / LPDWORD ended by a 0 element.</summary>
    PrimitiveList,
    /// <summary>Enum list ended by a 0 element (LPWORD / LPDWORD behind an enum).</summary>
    EnumList,
    /// <summary>Pointer to one struct.</summary>
    Nested,
    /// <summary>Pointer to a null-terminated array of struct pointers.</summary>
    StructList
}

public sealed class FieldModel
{
    public int Index { get; init; }
    public string CName { get; init; } = "";
    public string Property { get; init; } = "";
    public string Private { get; init; } = "";
    public Kind Kind { get; set; }

    /// <summary>C# element or scalar type (primitive or enum name).</summary>
    public string CsType { get; init; } = "";
    public bool IsEnum { get; init; }
    public int ArraySize { get; init; }

    /// <summary>For counted bytes: C name / property / C# type of the length field.</summary>
    public FieldModel? Length { get; set; }

    /// <summary>For Nested / StructList: the C# name of the target struct.</summary>
    public string Target { get; init; } = "";

    /// <summary>True when another field (counted bytes) sets this length field.</summary>
    public bool IsConsumedLength { get; set; }

    /// <summary>Items used by list kinds, as C# literals ("1, 2, 3").</summary>
    public string[] ListItems { get; set; } = [];
}

public sealed class StructModel
{
    public required PlannedStruct Planned { get; init; }
    public required TypePlan Plan { get; init; }
    public required List<FieldModel> Fields { get; init; }
}

public static class Classifier
{
    private static readonly HashSet<string> NumericCs = ["ushort", "short", "uint", "int", "byte"];

    /// <summary>Structs the generator cannot describe from the header alone (unions, WFSRESULT).</summary>
    public static bool IsSupported(PlannedStruct s) => !s.IsResult && !s.Source.HasUnion && s.Fields.Count == s.Source.Fields.Count;

    public static StructModel Build(TypePlan plan, PlannedStruct s, ConstantResolver resolver, ISet<string> knownStructs)
    {
        var fields = new List<FieldModel>();
        for (var i = 0; i < s.Fields.Count; i++)
            fields.Add(Classify(i, s.Fields[i], s.Source.Fields[i], resolver, knownStructs));

        for (var i = 0; i < fields.Count; i++)
        {
            if (fields[i].Kind != Kind.BytesCounted)
                continue;

            var length = fields[i - 1];
            if (length.Kind != Kind.Number)
            {
                fields[i].Kind = Kind.Skip;
                continue;
            }

            fields[i].Length = length;
            length.IsConsumedLength = true;
        }

        return new StructModel { Planned = s, Plan = plan, Fields = fields };
    }

    private static FieldModel Classify(int index, PlannedField planned, HeaderField header, ConstantResolver resolver, ISet<string> knownStructs)
    {
        FieldModel Make(Kind kind, string csType = "", bool isEnum = false, int size = 0, string target = "", string[]? items = null) => new()
        {
            Index = index,
            CName = header.Name,
            Property = NameConverter.ToPropertyName(planned.Name),
            Private = PlanWriter.PrivateName(planned.Name),
            Kind = kind,
            CsType = csType,
            IsEnum = isEnum,
            ArraySize = size,
            Target = target,
            ListItems = items ?? []
        };

        var type = planned.Type;

        if (type.StartsWith("[@", StringComparison.Ordinal))
        {
            var target = type.Substring(2, type.Length - 3);
            return knownStructs.Contains(target) ? Make(Kind.StructList, target: target) : Make(Kind.Skip);
        }

        if (type.StartsWith('@'))
        {
            var target = type.Substring(1);
            return knownStructs.Contains(target) ? Make(Kind.Nested, target: target) : Make(Kind.Skip);
        }

        if (type.StartsWith('*'))
            return Make(Kind.BytesCounted, "byte");

        if (type.StartsWith('#'))
        {
            var spec = type.Substring(1);
            if (spec.EndsWith("[ZeroTerminated]", StringComparison.Ordinal))
                return Make(Kind.EnumList, spec[..spec.IndexOf('[')], isEnum: true);

            var bracket = spec.IndexOf('[');
            if (bracket >= 0)
                return Make(Kind.NumberArray, spec[..bracket], isEnum: true, size: ArraySize(spec[(bracket + 1)..^1], resolver));

            return Make(Kind.Number, spec, isEnum: true);
        }

        var cs = PlanWriter.ConvertBaseType(header.CType);

        if (header.ArraySize != null)
        {
            var size = ArraySize(header.ArraySize, resolver);
            return NumericCs.Contains(cs) ? Make(Kind.NumberArray, cs, size: size) : Make(Kind.OpaqueArray, cs, size: size);
        }

        switch (header.CType)
        {
            case "LPSTR": return Make(Kind.String);
            case "LPWSTR": return Make(Kind.WString);
            case "LPBYTE": return Make(Kind.BytesTerminated, "byte");
            case "LPWORD": return Make(Kind.PrimitiveList, "ushort", items: ["1", "2", "3"]);
            case "LPDWORD": return Make(Kind.PrimitiveList, "uint", items: ["10", "20", "30"]);
        }

        if (cs == "bool")
            return Make(Kind.Bool, "bool");

        return NumericCs.Contains(cs) ? Make(Kind.Number, cs) : Make(Kind.Skip);
    }

    private static int ArraySize(string expression, ConstantResolver resolver)
    {
        return (int)(resolver.Evaluate(expression)?.Number
                     ?? throw new InvalidOperationException($"Cannot resolve array size '{expression}'"));
    }
}
