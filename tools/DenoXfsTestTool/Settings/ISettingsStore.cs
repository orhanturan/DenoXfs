namespace DenoXfs.TestTool.Settings;

/// <summary>Loads and saves the settings; the application keeps one shared <see cref="TestToolSettings"/> instance.</summary>
public interface ISettingsStore
{
    TestToolSettings Load();

    void Save(TestToolSettings settings);
}
