using DenoXfs.TestTool.Operations;

namespace DenoXfs.TestTool.Ui;

/// <summary>
/// The command panel shared by the Info and Commands tabs: one tab per operation along the top.
/// It knows no device; every operation comes from an <see cref="IOperationDefinition"/>.
/// </summary>
public class OperationPanel : UserControl
{
    public OperationPanel(IEnumerable<IOperationDefinition> operations, OperationKind kind, IOperationRunner runner)
    {
        Dock = DockStyle.Fill;

        // Multiline: when there are many operations the tabs wrap into rows instead of scrolling.
        var tabs = new TabControl { Dock = DockStyle.Fill, ShowToolTips = true, Multiline = true };
        foreach (var operation in operations.Where(o => o.Kind == kind).OrderBy(o => o.Name))
        {
            var page = new TabPage(operation.Name) { ToolTipText = operation.Description };
            page.Controls.Add(new WorkArea(new OperationPage(operation, runner)));
            tabs.TabPages.Add(page);
        }

        Controls.Add(tabs);
    }
}
