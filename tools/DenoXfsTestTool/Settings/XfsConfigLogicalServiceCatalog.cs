using System.Text;
using DenoXfs.Api;
using DenoXfs.TestTool.Operations;
using Microsoft.Extensions.Logging;

namespace DenoXfs.TestTool.Settings;

/// <summary>
/// Reads WFS_CFG_HKEY_USER_DEFAULT_XFS_ROOT\LOGICAL_SERVICES through xfs_conf.dll (XFS API 4.7), the same place
/// the XFS manager looks up a WFSOpen name; the registry is read on every call, so a fresh import shows up at once.
/// </summary>
public class XfsConfigLogicalServiceCatalog : ILogicalServiceCatalog
{
    private const string LogicalServicesKey = "LOGICAL_SERVICES";
    private const int MaxLength = 256;
    private readonly ILogger<XfsConfigLogicalServiceCatalog> _logger;

    public XfsConfigLogicalServiceCatalog(ILogger<XfsConfigLogicalServiceCatalog> logger)
    {
        _logger = logger;
    }

    public IReadOnlyList<LogicalService> All()
    {
        try
        {
            int result = XfsConfigApi.OpenKey(XfsConfigApi.UserDefaultXfsRoot, LogicalServicesKey, out var root);
            if (result != Xfs.Success)
            {
                _logger.LogWarning("logical services cannot be read: WFMOpenKey {Key} result={Result}", LogicalServicesKey, ResultNames.Describe(result));
                return [];
            }

            try
            {
                return Names(root).Select(name => Read(root, name)).OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase).ToList();
            }
            finally
            {
                XfsConfigApi.CloseKey(root);
            }
        }
        catch (Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException or BadImageFormatException)
        {
            _logger.LogWarning(ex, "logical services cannot be read: xfs_conf.dll is not available");
            return [];
        }
    }

    private List<string> Names(IntPtr root)
    {
        var names = new List<string>();
        for (uint index = 0; ; index++)
        {
            var name = new StringBuilder(MaxLength);
            uint length = MaxLength;
            int result = XfsConfigApi.EnumKey(root, index, name, ref length, out _);
            if (result == Xfs.ErrCfgNoMoreItems)
                return names;
            if (result != Xfs.Success)
            {
                _logger.LogWarning("WFMEnumKey {Key} index={Index} result={Result}", LogicalServicesKey, index, ResultNames.Describe(result));
                return names;
            }
            names.Add(name.ToString());
        }
    }

    private static LogicalService Read(IntPtr root, string name)
    {
        if (XfsConfigApi.OpenKey(root, name, out var key) != Xfs.Success)
            return new LogicalService(name, string.Empty, string.Empty);
        try
        {
            return new LogicalService(name, Value(key, "class"), Value(key, "provider"));
        }
        finally
        {
            XfsConfigApi.CloseKey(key);
        }
    }

    private static string Value(IntPtr key, string valueName)
    {
        var data = new StringBuilder(MaxLength);
        uint length = MaxLength;
        return XfsConfigApi.QueryValue(key, valueName, data, ref length) == Xfs.Success ? data.ToString() : string.Empty;
    }
}
