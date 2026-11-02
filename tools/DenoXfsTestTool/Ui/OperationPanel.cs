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
        var pages = new List<OperationPage>();
        // In the order of the spec's codes: Status, Capabilities, ... / ReadTrack, WriteTrack, EjectCard, ...
        foreach (var operation in operations.Where(o => o.Kind == kind).OrderBy(o => o.Code).ThenBy(o => o.Name))
        {
            var operationPage = new OperationPage(operation, runner);
            var page = new TabPage(operation.Name) { ToolTipText = operation.Description };
            page.Controls.Add(new WorkArea(operationPage));
            tabs.TabPages.Add(page);
            pages.Add(operationPage);
        }

        // Operations without input run as soon as their tab is clicked, also when it is already selected.
        tabs.MouseClick += async (_, e) =>
        {
            if (e.Button != MouseButtons.Left)
                return;
            for (var i = 0; i < tabs.TabCount; i++)
            {
                if (tabs.GetTabRect(i).Contains(e.Location) && !pages[i].HasParameters)
                    await pages[i].RunIfIdleAsync();
            }
        };

        Controls.Add(tabs);
    }
}
