using System.Text.Json;

namespace HtmlToXfs2.Services;

/// <summary>
/// Header-driven generation of constants, enums and structs for one device.
/// Output goes to output/{device}/ as {Group}/{Type}.txt plus the constants file in the root.
/// </summary>
public sealed class TypeGenerator
{
    private static readonly Dictionary<string, (HeaderModel model, TypePlan plan)> ApiCache = new();

    private readonly string _device;
    private readonly string _version;

    public TypeGenerator(string device, string version)
    {
        _device = device.ToLowerInvariant();
        _version = version;
    }

    public void Run()
    {
        var overrides = NamingOverrides.Load("naming-overrides.json");
        var (model, plan) = _device == "api" ? GetApi(_version, overrides) : Build(_device, _version, overrides);
        var (apiModel, _) = _device == "api" ? (model, plan) : GetApi(_version, overrides);
        var resolver = _device == "api" ? new ConstantResolver(model) : new ConstantResolver(model, apiModel);

        var outputDir = Path.Combine("output", _device);
        if (Directory.Exists(outputDir))
            Directory.Delete(outputDir, recursive: true);

        Directory.CreateDirectory(outputDir);

        File.WriteAllText(Path.Combine(outputDir, $"{plan.ConstantsClass}.txt"), PlanWriter.WriteConstants(plan, model, resolver));

        foreach (var planned in plan.Enums)
            WriteType(outputDir, planned.Folder, planned.Name, PlanWriter.WriteEnum(plan, planned));

        foreach (var planned in plan.Structs)
            WriteType(outputDir, planned.Folder, planned.Name, PlanWriter.WriteStruct(plan, planned, resolver));

        WritePlacement(plan);
        Console.WriteLine($"  {plan.Structs.Count} struct(s), {plan.Enums.Count} enum(s) written to {outputDir}");
    }

    private static void WriteType(string outputDir, string folder, string name, string content)
    {
        var dir = string.IsNullOrEmpty(folder) ? outputDir : Path.Combine(outputDir, folder);
        Directory.CreateDirectory(dir);
        File.WriteAllText(Path.Combine(dir, $"{name}.txt"), content);
    }

    private void WritePlacement(TypePlan plan)
    {
        var entries = plan.Structs
            .Select(s => new { kind = "struct", name = s.Name, group = s.Group, folder = s.Folder, reason = s.Reason, owners = (List<string>?)null })
            .Concat(plan.Enums.Select(e => new { kind = "enum", name = e.Name, group = e.Group, folder = e.Folder, reason = e.Reason, owners = (List<string>?)e.Owners }))
            .OrderBy(e => e.group, StringComparer.Ordinal)
            .ThenBy(e => e.name, StringComparer.Ordinal);

        var dir = Path.Combine("int", "json", _device);
        Directory.CreateDirectory(dir);
        var options = new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
        File.WriteAllText(Path.Combine(dir, "Placement.json"), JsonSerializer.Serialize(entries, options));
    }

    /// <summary>Plans the types of one device the same way <see cref="Run"/> does; paths are relative to the tool folder.</summary>
    public static (HeaderModel model, TypePlan plan, ConstantResolver resolver) BuildPlan(string device, string version)
    {
        device = device.ToLowerInvariant();
        var overrides = NamingOverrides.Load("naming-overrides.json");
        var (model, plan) = device == "api" ? GetApi(version, overrides) : Build(device, version, overrides);
        var resolver = device == "api" ? new ConstantResolver(model) : new ConstantResolver(model, GetApi(version, overrides).model);
        return (model, plan, resolver);
    }

    private static (HeaderModel model, TypePlan plan) GetApi(string version, NamingOverrides overrides)
    {
        if (!ApiCache.TryGetValue(version, out var cached))
            ApiCache[version] = cached = Build("api", version, overrides);

        return cached;
    }

    private static (HeaderModel model, TypePlan plan) Build(string device, string version, NamingOverrides overrides)
    {
        var model = HeaderParser.ParseFile(HeaderPath(device), device);
        if (device == "api")
            return (model, TypePlanner.Plan(model, null, null, Array.Empty<SectionDoc>(), overrides));

        var (apiModel, apiPlan) = GetApi(version, overrides);
        var sections = Path.Combine("pdf", version, "sections", "Xfs" + NameConverter.CapitalizeFirstOnly(device));
        var docs = SectionUsage.Load(sections);
        if (docs.Count == 0)
            Console.WriteLine($"  Warning: no spec sections found in {sections}; every struct is treated as header-only");

        return (model, TypePlanner.Plan(model, apiModel, apiPlan, docs, overrides));
    }

    private static string HeaderPath(string device)
    {
        return Path.Combine("INCLUDE", device == "api" ? "XFSAPI.H" : $"XFS{device.ToUpperInvariant()}.H");
    }
}
