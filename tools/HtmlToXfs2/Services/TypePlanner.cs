using System.Text.RegularExpressions;

namespace HtmlToXfs2.Services;

public sealed record PlannedMember(string Define, string Name, string Value);

public sealed class PlannedEnum
{
    public string Name { get; set; } = "";
    /// <summary>Namespace segment: Infos, Commands, Events, Shared, Common, Errors, or empty for the library root.</summary>
    public string Group { get; set; } = "";
    /// <summary>Path under the device output folder. May be Infos/Status. Empty writes to the library root.</summary>
    public string Folder { get; set; } = "";
    public string Reason { get; set; } = "";
    public string BaseType { get; set; } = "ushort";
    public bool IsFlags { get; set; }
    public List<PlannedMember> Members { get; } = new();
    public List<string> Owners { get; } = new();
}

public sealed class PlannedField
{
    public string Name { get; set; } = "";

    /// <summary>
    /// C type, or a marker form: #Enum, #Enum[N], #Enum[ZeroTerminated], @Struct, *BYTE[countField], CType[N].
    /// </summary>
    public string Type { get; set; } = "";

    public HashSet<string> Namespaces { get; } = new();
}

public sealed class PlannedStruct
{
    public string Name { get; set; } = "";
    public string Tag { get; set; } = "";
    public string TypedefName { get; set; } = "";
    public string Group { get; set; } = "";
    /// <summary>Path under the device output folder. May be Infos/Status.</summary>
    public string Folder { get; set; } = "";
    public string Reason { get; set; } = "";
    public bool IsResult { get; set; }
    public List<PlannedField> Fields { get; } = new();
    public HeaderTypedef Source { get; set; } = null!;
}

public sealed class TypePlan
{
    public TypePlan(string device)
    {
        Device = device;
        Library = device == "api" ? "Api" : NameConverter.CapitalizeFirstOnly(device);
        ConstantsClass = device == "api" ? "Xfs" : Library;
    }

    public string Device { get; }
    public string Library { get; }
    public string ConstantsClass { get; }
    public List<PlannedStruct> Structs { get; } = new();
    public List<PlannedEnum> Enums { get; } = new();

    public string Namespace(string group) =>
        string.IsNullOrEmpty(group) ? $"DenoXfs.{Library}" : $"DenoXfs.{Library}.{group}";
}

/// <summary>
/// Derives final type names, folder groups and field types from a parsed XFS header,
/// the spec sections (for usage counts) and the naming overrides.
/// </summary>
public static class TypePlanner
{
    private static readonly Dictionary<string, string> IntegralBase = new(StringComparer.OrdinalIgnoreCase)
    {
        ["WORD"] = "ushort", ["USHORT"] = "ushort", ["SHORT"] = "short",
        ["DWORD"] = "uint", ["ULONG"] = "uint", ["LONG"] = "int", ["UCHAR"] = "byte"
    };

    private static readonly Dictionary<string, string> SectionGroups = new()
    {
        ["info_command"] = "Infos", ["execute_command"] = "Commands", ["event"] = "Events"
    };

    public static TypePlan Plan(
        HeaderModel model,
        HeaderModel? apiModel,
        TypePlan? apiPlan,
        IReadOnlyList<SectionDoc> docs,
        NamingOverrides overrides)
    {
        var plan = new TypePlan(model.Device);
        var isApi = model.Device == "api";
        var resolver = apiModel == null || isApi ? new ConstantResolver(model) : new ConstantResolver(model, apiModel);

        PlanStructs(plan, model, docs, overrides);
        var bindings = PlanEnums(plan, model, apiModel, apiPlan, resolver, overrides);
        PlanErrors(plan, model, resolver, overrides);
        PlanCommandIds(plan, model);
        PlanFields(plan, model, apiPlan, bindings);
        CheckUniqueNames(plan, apiPlan);
        return plan;
    }

    private static void PlanStructs(TypePlan plan, HeaderModel model, IReadOnlyList<SectionDoc> docs, NamingOverrides overrides)
    {
        var isApi = model.Device == "api";
        foreach (var header in model.Structs)
        {
            var name = overrides.StructNames.TryGetValue(header.Tag, out var renamed)
                ? renamed
                : isApi ? NameConverter.ConvertStructName(header.Tag) : "Xfs" + NameConverter.DeviceStructName(header.Tag);

            var (group, folder, reason) = isApi ? ("Common", "Common", "Api type") : ClassifyStruct(header, docs, model.Device);
            plan.Structs.Add(new PlannedStruct
            {
                Name = name,
                Tag = header.Tag,
                TypedefName = header.TypedefName,
                Group = group,
                Folder = folder,
                Reason = reason,
                IsResult = header.HasUnion,
                Source = header
            });
        }
    }

    private static (string group, string folder, string reason) ClassifyStruct(HeaderTypedef header, IReadOnlyList<SectionDoc> docs, string device)
    {
        var sections = SectionUsage.SectionsMentioning(docs, header);
        if (sections.Count == 0)
            return ("Shared", "Shared", "defined only in the C header");

        if (sections.Count > 1)
            return ("Shared", "Shared", $"used in {sections.Count} sections: {string.Join(", ", sections.Select(s => s.File))}");

        var section = sections[0];
        if (!SectionGroups.TryGetValue(section.Type, out var group))
            return ("Shared", "Shared", $"only in {section.File} ({section.Type})");

        var command = SectionUsage.CommandFolder(section.Command, device);
        var folder = string.IsNullOrEmpty(command) ? group : $"{group}/{command}";
        return (group, folder, $"only in {section.File}");
    }

    private sealed class OwnerInfo
    {
        public OwnerInfo(PlannedStruct owner, HeaderField field)
        {
            Struct = owner;
            Field = field;
        }

        public PlannedStruct Struct { get; }
        public HeaderField Field { get; }
        public List<string> Members { get; } = new();
    }

    private sealed class Candidate
    {
        public string DefaultName { get; set; } = "";
        public string? FinalName { get; set; }
        public List<OwnerInfo> Owners { get; } = new();
        public List<string> Members { get; set; } = new();
    }

    /// <summary>Maps "StructName.fieldName" to the enum type bound to that field and the namespace that type lives in.</summary>
    private static Dictionary<string, (string enumName, string ns)> PlanEnums(
        TypePlan plan,
        HeaderModel model,
        HeaderModel? apiModel,
        TypePlan? apiPlan,
        ConstantResolver resolver,
        NamingOverrides overrides)
    {
        var owners = CollectOwners(plan, model, resolver, overrides);
        var candidates = BuildCandidates(owners, model, resolver, overrides);
        var bindings = new Dictionary<string, (string, string)>();

        foreach (var candidate in candidates)
        {
            var baseType = CandidateBaseType(candidate);
            var alias = FindApiAlias(plan, model, apiModel, apiPlan, candidate, baseType);
            if (alias != null)
            {
                foreach (var owner in candidate.Owners)
                    bindings[$"{owner.Struct.Name}.{owner.Field.Name}"] = (alias.Name, apiPlan!.Namespace(alias.Group));

                continue;
            }

            var planned = BuildEnum(plan, model, resolver, overrides, candidate, baseType);
            plan.Enums.Add(planned);
            foreach (var owner in candidate.Owners)
                bindings[$"{owner.Struct.Name}.{owner.Field.Name}"] = (planned.Name, plan.Namespace(planned.Group));
        }

        return bindings;
    }

    private static Dictionary<string, OwnerInfo> CollectOwners(
        TypePlan plan, HeaderModel model, ConstantResolver resolver, NamingOverrides overrides)
    {
        var owners = new Dictionary<string, OwnerInfo>();

        foreach (var group in model.Groups.Where(g => g.Owners.Count > 0))
        {
            var values = group.DefineNames.Select(resolver.Resolve).ToList();
            if (values.Any(v => v?.Number == null || v.IsChar || v.UsesWmUser))
                continue;

            foreach (var reference in group.Owners)
            {
                var target = ResolveOwner(plan, reference, overrides);
                if (target == null)
                {
                    Console.WriteLine($"  Warning: no field found for '{reference.TypedefName}.{reference.Field}' ({Shorten(group.Comment)})");
                    continue;
                }

                var (owner, field) = target.Value;
                if (!IsBindable(field))
                    continue;

                var key = $"{owner.Name}.{field.Name}";
                if (!owners.TryGetValue(key, out var info))
                    owners[key] = info = new OwnerInfo(owner, field);

                foreach (var name in group.DefineNames.Where(n => !info.Members.Contains(n)))
                    info.Members.Add(name);
            }
        }

        return owners;
    }

    private static List<Candidate> BuildCandidates(
        Dictionary<string, OwnerInfo> owners, HeaderModel model, ConstantResolver resolver, NamingOverrides overrides)
    {
        int OrderOf(string define) => model.DefineByName[define].Order;

        var bySignature = new Dictionary<string, Candidate>();
        var candidates = new List<Candidate>();

        foreach (var info in owners.Values)
        {
            info.Members.Sort((a, b) => OrderOf(a).CompareTo(OrderOf(b)));
            var signature = string.Join("|", info.Members);
            if (!bySignature.TryGetValue(signature, out var candidate))
            {
                candidate = new Candidate { Members = new List<string>(info.Members) };
                bySignature[signature] = candidate;
                candidates.Add(candidate);
            }

            candidate.Owners.Add(info);
        }

        foreach (var candidate in candidates)
            candidate.DefaultName = DefaultEnumName(candidate, model, resolver);

        foreach (var (finalName, defaults) in overrides.EnumMerges)
        {
            var parts = candidates.Where(c => defaults.Contains(c.DefaultName)).ToList();
            if (parts.Count > 0 && parts.Count != defaults.Count)
                Console.WriteLine($"  Warning: enum merge '{finalName}' matched {parts.Count} of {defaults.Count} ({string.Join(", ", defaults)})");

            if (parts.Count == 0)
                continue;

            var target = parts[0];
            foreach (var other in parts.Skip(1))
            {
                target.Owners.AddRange(other.Owners);
                target.Members = target.Members.Union(other.Members).OrderBy(OrderOf).ToList();
                candidates.Remove(other);
            }

            target.FinalName = finalName;
        }

        foreach (var candidate in candidates)
        {
            candidate.FinalName ??= overrides.EnumNames.TryGetValue(candidate.DefaultName, out var renamed)
                ? renamed
                : candidate.DefaultName;
        }

        return candidates;
    }

    private static string CandidateBaseType(Candidate candidate)
    {
        var types = candidate.Owners.Select(o => IntegralBase[ElementCType(o.Field)]).Distinct().ToList();
        if (types.Count > 1)
        {
            var owners = string.Join(", ", candidate.Owners.Select(o => $"{o.Struct.Name}.{o.Field.Name}:{o.Field.CType}"));
            throw new InvalidOperationException($"Enum '{candidate.FinalName}' mixes underlying types ({owners})");
        }

        return types[0];
    }

    private static PlannedEnum? FindApiAlias(
        TypePlan plan, HeaderModel model, HeaderModel? apiModel, TypePlan? apiPlan, Candidate candidate, string baseType)
    {
        if (plan.Device == "api" || apiModel == null || apiPlan == null)
            return null;

        var targets = candidate.Members.Select(m => model.DefineByName[m].RawValue.Trim()).ToList();
        if (targets.Any(t => !Regex.IsMatch(t, @"^[A-Za-z_]\w*$") || !apiModel.DefineByName.ContainsKey(t)))
            return null;

        // An alias must keep the field width, otherwise the struct layout would change.
        return apiPlan.Enums.FirstOrDefault(e => e.BaseType == baseType && targets.All(t => e.Members.Any(m => m.Define == t)));
    }

    private static PlannedEnum BuildEnum(
        TypePlan plan, HeaderModel model, ConstantResolver resolver, NamingOverrides overrides, Candidate candidate, string baseType)
    {
        var folders = candidate.Owners.Select(o => o.Struct.Folder).Distinct().ToList();
        string folder, group, reason;
        if (plan.Device == "api")
        {
            (folder, group, reason) = ("Common", "Common", "Api type");
        }
        else if (folders.Count == 1 && folders[0] != "Shared")
        {
            folder = folders[0];
            group = folder.Split('/')[0];
            reason = $"only used by {string.Join(", ", candidate.Owners.Select(o => o.Struct.Name))}";
        }
        else
        {
            (folder, group, reason) = ("Shared", "Shared", $"used by {string.Join(", ", candidate.Owners.Select(o => $"{o.Struct.Name}.{o.Field.Name}"))}");
        }

        var planned = new PlannedEnum { Name = candidate.FinalName!, Group = group, Folder = folder, Reason = reason, BaseType = baseType };
        planned.Owners.AddRange(candidate.Owners.Select(o => $"{o.Struct.Name}.{o.Field.Name}"));

        var prefix = EnumMemberNamer.CommonPrefix(candidate.Members);
        foreach (var define in candidate.Members)
        {
            var value = resolver.Resolve(define)!;
            planned.IsFlags |= value.IsHex;
            var memberName = overrides.MemberNames.TryGetValue(define, out var renamed)
                ? renamed
                : EnumMemberNamer.MemberName(define, prefix, model.Device);

            planned.Members.Add(new PlannedMember(define, memberName, FormatEnumValue(value, baseType, planned.Name, define)));
        }

        var duplicate = planned.Members.GroupBy(m => m.Name).FirstOrDefault(g => g.Count() > 1);
        if (duplicate != null)
            throw new InvalidOperationException($"Enum '{planned.Name}' has duplicate member '{duplicate.Key}'; add a memberNames override.");

        return planned;
    }

    private static void PlanErrors(TypePlan plan, HeaderModel model, ConstantResolver resolver, NamingOverrides overrides)
    {
        var isApi = model.Device == "api";
        var prefix = isApi ? "WFS_ERR_" : $"WFS_ERR_{model.Device.ToUpperInvariant()}_";

        var errors = model.Defines
            .Where(d => d.Name.StartsWith(prefix, StringComparison.Ordinal))
            .Select(d => (d, v: resolver.Resolve(d.Name)))
            .Where(x => x.v?.Number < 0)
            .ToList();

        if (errors.Count == 0)
            return;

        var planned = new PlannedEnum
        {
            Name = isApi ? "XfsError" : plan.Library + "Error",
            Group = "Errors",
            Folder = "Errors",
            Reason = $"{prefix}* values",
            BaseType = "int"
        };

        foreach (var (define, value) in errors)
        {
            if (planned.Members.Any(m => m.Define == define.Name))
                continue;

            var memberName = overrides.MemberNames.TryGetValue(define.Name, out var renamed)
                ? renamed
                : NameConverter.ToPascalCase(define.Name.Substring(prefix.Length));
            planned.Members.Add(new PlannedMember(define.Name, memberName, value!.Number!.Value.ToString()));
        }

        plan.Enums.Add(planned);
    }

    private static void PlanFields(
        TypePlan plan,
        HeaderModel model,
        TypePlan? apiPlan,
        Dictionary<string, (string enumName, string ns)> bindings)
    {
        var pointerMap = new Dictionary<string, (PlannedStruct s, TypePlan owner)>(StringComparer.Ordinal);
        foreach (var s in plan.Structs)
            pointerMap[s.Source.PointerTypedefName] = (s, plan);

        if (apiPlan != null)
        {
            foreach (var s in apiPlan.Structs)
                pointerMap.TryAdd(s.Source.PointerTypedefName, (s, apiPlan));
        }

        foreach (var s in plan.Structs.Where(x => !x.IsResult))
        {
            var fields = s.Source.Fields;
            for (var i = 0; i < fields.Count; i++)
            {
                var header = fields[i];
                var field = new PlannedField { Name = header.Name };

                if (header.IsDoublePointer)
                {
                    field.Type = "LPVOID";
                }
                else if (bindings.TryGetValue($"{s.Name}.{header.Name}", out var bound))
                {
                    field.Namespaces.Add(bound.ns);
                    field.Type = header.ArraySize != null
                        ? $"#{bound.enumName}[{header.ArraySize}]"
                        : header.CType.StartsWith("LP", StringComparison.OrdinalIgnoreCase)
                            ? $"#{bound.enumName}[ZeroTerminated]"
                            : $"#{bound.enumName}";
                }
                else if (header.CType == "LPBYTE" && i > 0 && IsLengthField(fields[i - 1]))
                {
                    field.Type = $"*BYTE[{fields[i - 1].Name}]";
                }
                else if (pointerMap.TryGetValue(header.CType, out var target))
                {
                    field.Type = $"@{target.s.Name}";
                    field.Namespaces.Add(target.owner.Namespace(target.s.Group));
                }
                else if (header.CType.StartsWith("LPWFS", StringComparison.Ordinal))
                {
                    Console.WriteLine($"  Warning: {s.Name}.{header.Name} points to unknown struct {header.CType}; using IntPtr");
                    field.Type = "LPVOID";
                }
                else
                {
                    field.Type = header.ArraySize != null ? $"{header.CType}[{header.ArraySize}]" : header.CType;
                }

                s.Fields.Add(field);
            }
        }
    }

    private static void CheckUniqueNames(TypePlan plan, TypePlan? apiPlan)
    {
        var names = plan.Structs.Select(s => s.Name).Concat(plan.Enums.Select(e => e.Name)).ToList();
        var clashes = names.GroupBy(n => n).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

        if (apiPlan != null && plan.Device != "api")
        {
            var apiNames = apiPlan.Structs.Select(s => s.Name).Concat(apiPlan.Enums.Select(e => e.Name)).ToHashSet();
            clashes.AddRange(names.Where(apiNames.Contains));
        }

        if (clashes.Count > 0)
        {
            throw new InvalidOperationException(
                $"Type name clash in {plan.Library}: {string.Join(", ", clashes.Distinct())}. Add enumNames/structNames overrides.");
        }
    }

    private static (PlannedStruct owner, HeaderField field)? ResolveOwner(TypePlan plan, FieldRef reference, NamingOverrides overrides)
    {
        var typedef = reference.TypedefName;
        var fieldName = reference.Field;

        if (overrides.OwnerRemap.TryGetValue($"{typedef}.{fieldName}", out var remap))
        {
            var parts = remap.Split('.');
            typedef = parts[0];
            fieldName = parts[1];
        }

        var owner = plan.Structs.FirstOrDefault(s => s.TypedefName == typedef);
        if (owner == null)
            return null;

        var field = owner.Source.Fields.FirstOrDefault(f => f.Name == fieldName)
                    ?? owner.Source.Fields.FirstOrDefault(f => NameConverter.ToPropertyName(f.Name) == NameConverter.ToPropertyName(fieldName));

        return field == null ? null : (owner, field);
    }

    private static bool IsBindable(HeaderField field)
    {
        return !field.IsDoublePointer && IntegralBase.ContainsKey(ElementCType(field));
    }

    private static string ElementCType(HeaderField field)
    {
        var type = field.CType;
        return type.StartsWith("LP", StringComparison.OrdinalIgnoreCase) && IntegralBase.ContainsKey(type.Substring(2))
            ? type.Substring(2)
            : type;
    }

    private static bool IsLengthField(HeaderField field)
    {
        return IntegralBase.ContainsKey(field.CType)
               && Regex.IsMatch(field.Name, @"(Len|Length)$", RegexOptions.IgnoreCase);
    }

    private static string DefaultEnumName(Candidate candidate, HeaderModel model, ConstantResolver resolver)
    {
        var first = candidate.Owners[0];
        if (model.Device == "api")
            return StructShortName(first.Struct.Name, model.Device) + NameConverter.ToPropertyName(first.Field.Name);

        var library = NameConverter.CapitalizeFirstOnly(model.Device);
        var property = candidate.Owners
            .Select(o => NameConverter.ToPropertyName(o.Field.Name))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(n => n, StringComparer.Ordinal)
            .First();

        var roles = candidate.Owners
            .Select(o => StructShortName(o.Struct.Name, model.Device))
            .Distinct(StringComparer.Ordinal)
            .ToList();

        if (roles.Count != 1)
            return library + property;

        var role = roles[0];
        if (role == "Status")
            return library + property + "Status";

        if (role == "Caps")
        {
            var flags = candidate.Members.Any(m => resolver.Resolve(m)?.IsHex == true);
            return library + property + (flags ? "Capabilities" : "Capability");
        }

        return library + property + role;
    }

    private static void PlanCommandIds(TypePlan plan, HeaderModel model)
    {
        if (plan.Device == "api")
            return;

        var device = model.Device.ToUpperInvariant();
        AddCommandIdEnum(plan, model, "InfoId", $"WFS_INF_{device}_");
        AddCommandIdEnum(plan, model, "CommandId", $"WFS_CMD_{device}_");
        AddCommandIdEnum(plan, model, "EventId", $"WFS_EXEE_{device}_", $"WFS_SRVE_{device}_", $"WFS_USRE_{device}_");
    }

    private static void AddCommandIdEnum(TypePlan plan, HeaderModel model, string kind, params string[] prefixes)
    {
        var defines = model.Defines.Where(d => prefixes.Any(p => d.Name.StartsWith(p, StringComparison.Ordinal))).ToList();
        if (defines.Count == 0)
            return;

        var rows = defines.Select(define =>
        {
            var shortName = CommandMemberName(define.Name, model.Device);
            var category = define.Name.Split('_')[1];
            var categoryName = NameConverter.CapitalizeFirstOnly(category.ToLowerInvariant());
            var constant = NameConverter.ConstantName(define.Name, model.Device);
            return (define, shortName, categoryName, constant);
        }).ToList();

        var duplicated = rows.GroupBy(r => r.shortName).Where(g => g.Count() > 1).Select(g => g.Key).ToHashSet();
        var planned = new PlannedEnum
        {
            Name = plan.Library + kind,
            Group = "",
            Folder = "",
            Reason = string.Join(", ", prefixes),
            BaseType = "int"
        };

        foreach (var row in rows)
        {
            var member = duplicated.Contains(row.shortName) ? row.categoryName + row.shortName : row.shortName;
            planned.Members.Add(new PlannedMember(row.define.Name, member, $"{plan.ConstantsClass}.{row.constant}"));
        }

        var duplicate = planned.Members.GroupBy(m => m.Name).FirstOrDefault(g => g.Count() > 1);
        if (duplicate != null)
            throw new InvalidOperationException($"Enum '{planned.Name}' has duplicate member '{duplicate.Key}'.");

        plan.Enums.Add(planned);
    }

    private static string CommandMemberName(string define, string device)
    {
        var name = define;
        if (name.StartsWith("WFS_", StringComparison.OrdinalIgnoreCase))
            name = name.Substring(4);

        var split = name.IndexOf('_');
        if (split >= 0)
            name = name.Substring(split + 1);

        var devicePrefix = device.ToUpperInvariant() + "_";
        if (name.StartsWith(devicePrefix, StringComparison.OrdinalIgnoreCase))
            name = name.Substring(devicePrefix.Length);

        return NameConverter.ToPascalCase(name);
    }

    private static string StructShortName(string structName, string device)
    {
        var prefix = device == "api" ? "Xfs" : "Xfs" + NameConverter.CapitalizeFirstOnly(device);
        return structName.StartsWith(prefix, StringComparison.Ordinal) ? structName.Substring(prefix.Length) : structName;
    }

    private static string FormatEnumValue(ConstValue value, string baseType, string enumName, string define)
    {
        var number = value.Number!.Value;
        var fits = baseType switch
        {
            "ushort" => number is >= 0 and <= ushort.MaxValue,
            "short" => number is >= short.MinValue and <= short.MaxValue,
            "uint" => number is >= 0 and <= uint.MaxValue,
            "int" => number is >= int.MinValue and <= int.MaxValue,
            "byte" => number is >= 0 and <= byte.MaxValue,
            _ => true
        };

        if (!fits)
            throw new InvalidOperationException($"{define}={number} does not fit {baseType} of enum {enumName}");

        return value.IsHex ? $"0x{number:x4}" : number.ToString();
    }

    private static string Shorten(string text) => text.Length <= 60 ? text : text.Substring(0, 60) + "...";
}
