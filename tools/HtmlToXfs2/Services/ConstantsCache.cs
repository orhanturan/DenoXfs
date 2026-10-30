using HtmlToXfs2.Model;

namespace HtmlToXfs2.Services;

/// <summary>
/// Static cache for header file constants, accessible from anywhere in the application.
/// Key: Device name (e.g., "idc", "ptr")
/// Value: Dictionary of constant names to their ConstantInfo objects
/// </summary>
public static class ConstantsCache
{
    private static readonly Dictionary<string, Dictionary<string, ConstantInfo>> _cache = new();
    
    /// <summary>
    /// Get constants for a specific device
    /// </summary>
    public static Dictionary<string, ConstantInfo>? GetConstants(string deviceName)
    {
        if (_cache.TryGetValue(deviceName.ToLower(), out var constants))
        {
            return constants;
        }
        return null;
    }
    
    /// <summary>
    /// Set constants for a specific device
    /// </summary>
    public static void SetConstants(string deviceName, Dictionary<string, ConstantInfo> constants)
    {
        _cache[deviceName.ToLower()] = constants;
    }
    
    /// <summary>
    /// Get a specific constant value
    /// </summary>
    public static string? GetConstant(string deviceName, string constantName)
    {
        var constants = GetConstants(deviceName);
        if (constants != null && constants.TryGetValue(constantName, out var constantInfo))
        {
            return constantInfo.Value;
        }
        return null;
    }
    
    /// <summary>
    /// Get a specific constant info
    /// </summary>
    public static ConstantInfo? GetConstantInfo(string deviceName, string constantName)
    {
        var constants = GetConstants(deviceName);
        if (constants != null && constants.TryGetValue(constantName, out var constantInfo))
        {
            return constantInfo;
        }
        return null;
    }
    
    /// <summary>
    /// Clear all cached constants
    /// </summary>
    public static void Clear()
    {
        _cache.Clear();
    }
}

