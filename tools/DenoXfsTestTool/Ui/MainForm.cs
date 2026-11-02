using DenoXfs.TestTool.Operations;
using DenoXfs.TestTool.Session;

namespace DenoXfs.TestTool.Ui;

public class MainForm : Form
{
    public MainForm(SettingsPanel settingsPanel, SessionPanel sessionPanel, IEnumerable<IOperationDefinition> operations, IOperationRunner runner,
        XfsLogPanel logPanel, ISessionController session)
    {
        Text = "DenoXfs Test Tool (x86)";
        Width = 1100;
        Height = 720;
        StartPosition = FormStartPosition.CenterScreen;

        var status = new StatusStrip();
        var state = new ToolStripStatusLabel("Closed");
        status.Items.Add(state);
        session.StateChanged += () => BeginInvoke(() =>
            state.Text = session.State == SessionState.Open ? $"Open (hService {session.HService})" : session.State.ToString());

        var tabs = new TabControl { Dock = DockStyle.Fill };
        var definitions = operations.ToList();
        tabs.TabPages.Add(Page("Settings", new WorkArea(settingsPanel)));
        tabs.TabPages.Add(Page("Session", new WorkArea(sessionPanel)));
        tabs.TabPages.Add(Page("Info", new OperationPanel(definitions, OperationKind.Info, runner)));
        tabs.TabPages.Add(Page("Commands", new OperationPanel(definitions, OperationKind.Execute, runner)));

        // Tabs span the whole window; below them each tab has its inputs on the left and the one XFS log on the right.
        // The log panel moves with the selected tab, so its content never changes when the user switches tabs.
        Controls.Add(tabs);
        Controls.Add(status);
        var placement = new SharedLogPlacement(tabs, logPanel);
        Load += (_, _) => placement.Place();
    }

    private static TabPage Page(string title, Control content)
    {
        var page = new TabPage(title);
        page.Controls.Add(content);
        return page;
    }
}
