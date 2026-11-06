using System.ComponentModel;
using DenoXfs.TestTool.Settings;

namespace DenoXfs.TestTool.Ui;

/// <summary>Settings tab: edit the test tool settings, every change is saved. The logical name is picked from the registry.</summary>
public class SettingsPanel : UserControl
{
    public SettingsPanel(ISettingsStore store, TestToolSettings settings, ILogicalServiceCatalog logicalServices)
    {
        Dock = DockStyle.Fill;

        var logicalName = new LogicalNameConverter(logicalServices);
        var properties = TypeDescriptor.GetProperties(settings).Cast<PropertyDescriptor>()
            .Select(p => p.Name == nameof(TestToolSettings.LogicalName) ? new ConvertedProperty(p, logicalName) : p);

        var grid = new PropertyGrid
        {
            Dock = DockStyle.Fill,
            SelectedObject = new PropertySubset(settings, properties),
            PropertySort = PropertySort.Categorized,
            HelpVisible = true
        };
        grid.PropertyValueChanged += (_, _) => store.Save(settings);
        Controls.Add(grid);
    }
}
