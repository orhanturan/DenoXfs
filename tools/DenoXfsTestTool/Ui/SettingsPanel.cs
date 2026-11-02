using DenoXfs.TestTool.Settings;

namespace DenoXfs.TestTool.Ui;

/// <summary>Settings tab: edit the test tool settings, every change is saved.</summary>
public class SettingsPanel : UserControl
{
    public SettingsPanel(ISettingsStore store, TestToolSettings settings)
    {
        Dock = DockStyle.Fill;

        var grid = new PropertyGrid { Dock = DockStyle.Fill, SelectedObject = settings, PropertySort = PropertySort.Categorized, HelpVisible = true };
        grid.PropertyValueChanged += (_, _) => store.Save(settings);
        Controls.Add(grid);
    }
}
