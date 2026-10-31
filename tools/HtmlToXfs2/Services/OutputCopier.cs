namespace HtmlToXfs2.Services;

/// <summary>
/// Copies generated output/{device}/ files into the matching library under src/.
/// Environment variable: DENO_SRC (e.g. C:\src\orhanturan\DenoXfs)
/// </summary>
public class OutputCopier
{
    private readonly string _baseTargetPath;

    public OutputCopier()
    {
        var envPath = Environment.GetEnvironmentVariable("DENO_SRC");
        if (string.IsNullOrEmpty(envPath))
        {
            throw new InvalidOperationException(
                "DENO_SRC environment variable is not set. " +
                "Please set it to the DenoXfs project path (e.g., C:\\src\\orhanturan\\DenoXfs)");
        }

        _baseTargetPath = envPath;
    }

    /// <summary>
    /// Constants go to the library root, every group folder (Infos, Commands, ...) is replaced as a whole.
    /// </summary>
    public void Copy(string deviceName)
    {
        var sourceRoot = Path.Combine("output", deviceName);
        if (!Directory.Exists(sourceRoot))
        {
            Console.WriteLine($"  Output not found: {sourceRoot}");
            return;
        }

        var targetRoot = Path.Combine(_baseTargetPath, "src", LibraryFolder(deviceName));
        if (!Directory.Exists(targetRoot))
            throw new DirectoryNotFoundException($"Library folder not found: {targetRoot}");

        foreach (var file in Directory.GetFiles(sourceRoot, "*.txt"))
            CopyFile(file, targetRoot);

        foreach (var groupDir in Directory.GetDirectories(sourceRoot))
        {
            var name = Path.GetFileName(groupDir);
            var copied = ReplaceTree(groupDir, Path.Combine(targetRoot, name));
            Console.WriteLine($"  {name}: {copied} file(s)");
        }
    }

    /// <summary>
    /// Replaces generated files in a group folder, including command subfolders.
    /// Does not touch the library root, so hand-written files such as XfsApi.cs stay.
    /// </summary>
    private static int ReplaceTree(string sourceDir, string targetDir)
    {
        Directory.CreateDirectory(targetDir);

        foreach (var stale in Directory.GetFiles(targetDir, "*.cs"))
            File.Delete(stale);

        var copied = 0;
        foreach (var file in Directory.GetFiles(sourceDir, "*.txt"))
        {
            CopyFile(file, targetDir);
            copied++;
        }

        var sourceSubs = Directory.GetDirectories(sourceDir).Select(Path.GetFileName).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var existing in Directory.GetDirectories(targetDir))
        {
            if (!sourceSubs.Contains(Path.GetFileName(existing)))
                Directory.Delete(existing, recursive: true);
        }

        foreach (var sub in Directory.GetDirectories(sourceDir))
            copied += ReplaceTree(sub, Path.Combine(targetDir, Path.GetFileName(sub)));

        return copied;
    }

    private static string LibraryFolder(string deviceName)
    {
        return deviceName == "api"
            ? "DenoXfsApi"
            : "DenoXfs" + char.ToUpperInvariant(deviceName[0]) + deviceName.Substring(1).ToLowerInvariant();
    }

    private static void CopyFile(string sourceFile, string targetDir)
    {
        var target = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(sourceFile) + ".cs");
        File.Copy(sourceFile, target, overwrite: true);
    }
}
