using System;
using System.IO;

namespace HtmlToXfs2.Services;

/// <summary>
/// Output dosyalarını hedef projeye kopyalar.
/// Environment variable: DENO_SRC (örn: C:\src\orhanturan\DenoXfs)
/// </summary>
public class OutputCopier
{
    private readonly string _baseTargetPath;
    
    public OutputCopier()
    {
        // Get target path from environment variable
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
    /// Device için Constants dosyasını hedef projeye kopyalar.
    /// </summary>
    public void CopyConstants(string deviceName)
    {
        var className = GetClassName(deviceName);
        var sourcePath = Path.Combine("output", deviceName, "Constants", $"{className}.txt");
        var targetPath = GetConstantsTargetPath(deviceName, className);
        
        CopyFile(sourcePath, targetPath, "Constants");
    }
    
    /// <summary>
    /// Device için tüm Struct dosyalarını hedef projeye kopyalar.
    /// </summary>
    public void CopyStructs(string deviceName)
    {
        var sourceDir = Path.Combine("output", deviceName, "Structs");
        var targetDir = GetStructsTargetPath(deviceName);
        
        if (!Directory.Exists(sourceDir))
        {
            Console.WriteLine($"  ⚠️  Structs source directory not found: {sourceDir}");
            return;
        }
        
        var files = Directory.GetFiles(sourceDir, "*.txt");
        Console.WriteLine($"  Copying {files.Length} struct files...");
        
        foreach (var sourceFile in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(sourceFile);
            var targetPath = Path.Combine(targetDir, $"{fileName}.cs");
            CopyFile(sourceFile, targetPath, "Struct");
        }
    }
    
    private string GetClassName(string deviceName)
    {
        return deviceName == "api" ? "Xfs" : Capitalize(deviceName);
    }
    
    private string GetConstantsTargetPath(string deviceName, string className)
    {
        if (deviceName == "api")
        {
            // DenoXfs/src/DenoXfs.Core/Api/Xfs.cs
            return Path.Combine(_baseTargetPath, "src", "DenoXfs.Core", "Api", $"{className}.cs");
        }
        else
        {
            // DenoXfs/src/DenoXfs.{Device}/Api/{Device}.cs
            return Path.Combine(_baseTargetPath, "src", $"DenoXfs.{className}", "Api", $"{className}.cs");
        }
    }
    
    private string GetStructsTargetPath(string deviceName)
    {
        if (deviceName == "api")
        {
            // DenoXfs/src/DenoXfs.Core/Api/
            return Path.Combine(_baseTargetPath, "src", "DenoXfs.Core", "Api");
        }
        else
        {
            // DenoXfs/src/DenoXfs.{Device}/Models/Structs/
            var className = Capitalize(deviceName);
            return Path.Combine(_baseTargetPath, "src", $"DenoXfs.{className}", "Models", "Structs");
        }
    }
    
    private void CopyFile(string sourcePath, string targetPath, string fileType)
    {
        try
        {
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"  ⚠️  {fileType} source not found: {sourcePath}");
                return;
            }
            
            // Create target directory if it doesn't exist
            var targetDir = Path.GetDirectoryName(targetPath);
            if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }
            
            // Copy file
            File.Copy(sourcePath, targetPath, overwrite: true);
            Console.WriteLine($"  ✅ Copied {fileType}: {Path.GetFileName(sourcePath)} → {targetPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ❌ Copy error ({fileType}): {ex.Message}");
        }
    }
    
    private static string Capitalize(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }
}

