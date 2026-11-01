using HtmlToXfs2.Services;

namespace XfsTestGen;

/// <summary>
/// Generates struct conversion tests from the same type plan HtmlToXfs2 uses for the C# libraries:
/// a C++ side (tests/DenoXfsTestNative/Generated) built from the real XFS headers and xunit tests per library.
/// </summary>
internal static class Program
{
    private static int Main(string[] args)
    {
        var version = "340";
        var devices = new List<string> { "api", "idc" };
        string? root = null;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--version": version = args[++i]; break;
                case "--devices": devices = args[++i].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(); break;
                case "--root": root = args[++i]; break;
                default:
                    Console.Error.WriteLine($"Unknown argument {args[i]}. Usage: XfsTestGen [--version 340] [--devices api,idc] [--root <repo>]");
                    return 1;
            }
        }

        root ??= FindRoot();
        if (root == null)
        {
            Console.Error.WriteLine("DenoXfs.slnx not found; pass --root <repo>.");
            return 1;
        }

        Environment.CurrentDirectory = Path.Combine(root, "tools", "HtmlToXfs2");

        var nativeDir = Path.Combine(root, "tests", "DenoXfsTestNative", "Generated");
        if (Directory.Exists(nativeDir))
            Directory.Delete(nativeDir, recursive: true);

        Directory.CreateDirectory(nativeDir);
        File.WriteAllText(Path.Combine(nativeDir, "Common.g.h"), CppWriter.CommonHeader);

        foreach (var device in devices)
            Generate(root, nativeDir, device, version);

        return 0;
    }

    private static void Generate(string root, string nativeDir, string device, string version)
    {
        var (_, plan, resolver) = TypeGenerator.BuildPlan(device, version);
        var testProject = $"DenoXfs{plan.Library}Tests";
        var testDir = Path.Combine(root, "tests", testProject, "Generated");
        if (Directory.Exists(testDir))
            Directory.Delete(testDir, recursive: true);

        Directory.CreateDirectory(testDir);

        var supported = plan.Structs.Where(Classifier.IsSupported).ToList();
        var known = supported.Select(s => s.Name).ToHashSet(StringComparer.Ordinal);
        var models = supported.Select(s => Classifier.Build(plan, s, resolver, known)).ToList();

        var usings = plan.Structs.Select(s => plan.Namespace(s.Group))
            .Concat(plan.Enums.Select(e => plan.Namespace(e.Group)))
            .Append(plan.Namespace(""))
            .Distinct()
            .OrderBy(n => n, StringComparer.Ordinal)
            .ToList();

        var lib = plan.Library;
        File.WriteAllText(Path.Combine(nativeDir, $"{lib}.g.h"), CppWriter.Header(device, models));
        File.WriteAllText(Path.Combine(nativeDir, $"{lib}.g.cpp"), CppWriter.Source(device, models, [lib.ToLowerInvariant()]));
        File.WriteAllText(Path.Combine(testDir, "NativeMethods.g.cs"), CsWriter.NativeMethods(testProject, models));

        foreach (var model in models)
        {
            var dir = string.IsNullOrEmpty(model.Planned.Folder) ? testDir : Path.Combine(testDir, model.Planned.Folder);
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, $"{model.Planned.Name}Tests.g.cs"), CsWriter.Tests(testProject, usings, model));
        }

        Console.WriteLine($"{device}: {models.Count} struct(s) covered, tests in tests/{testProject}/Generated");
        foreach (var skipped in plan.Structs.Except(supported))
            Console.WriteLine($"  skipped struct {skipped.Name} ({(skipped.IsResult ? "WFSRESULT union" : "union or unmapped fields")})");

        foreach (var model in models)
        {
            var left = model.Fields.Where(f => f.Kind == Kind.Skip).Select(f => f.CName).ToList();
            if (left.Count > 0)
                Console.WriteLine($"  {model.Planned.Name}: not covered fields {string.Join(", ", left)}");
        }
    }

    private static string? FindRoot()
    {
        foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            for (var dir = new DirectoryInfo(start); dir != null; dir = dir.Parent)
                if (File.Exists(Path.Combine(dir.FullName, "DenoXfs.slnx")))
                    return dir.FullName;
        }

        return null;
    }
}
