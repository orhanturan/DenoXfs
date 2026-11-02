using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace DenoXfs.TestTool.Settings;

/// <summary>Stores the settings as JSON under %LocalAppData%\DenoXfsTestTool\settings.json.</summary>
public class JsonSettingsStore : ISettingsStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _path;
    private readonly ILogger<JsonSettingsStore> _logger;

    public JsonSettingsStore(ILogger<JsonSettingsStore> logger)
        : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DenoXfsTestTool", "settings.json"), logger)
    {
    }

    public JsonSettingsStore(string path, ILogger<JsonSettingsStore> logger)
    {
        _path = path;
        _logger = logger;
    }

    public TestToolSettings Load()
    {
        try
        {
            if (File.Exists(_path))
                return JsonSerializer.Deserialize<TestToolSettings>(File.ReadAllText(_path), Options) ?? new TestToolSettings();
        }
        catch (Exception ex) when (ex is IOException or JsonException)
        {
            _logger.LogWarning(ex, "settings could not be read path={Path}, defaults are used", _path);
        }

        return new TestToolSettings();
    }

    public void Save(TestToolSettings settings)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            File.WriteAllText(_path, JsonSerializer.Serialize(settings, Options));
        }
        catch (IOException ex)
        {
            _logger.LogWarning(ex, "settings could not be saved path={Path}", _path);
        }
    }
}
