namespace DenoXfs.TestTool.Ui;

/// <summary>
/// Content of a leaf tab: the tab's controls in a narrow left column and a slot on the right that receives the
/// shared XFS log while this tab is shown.
/// </summary>
public class WorkArea : UserControl
{
    private const int LeftWidth = 360;
    private readonly SplitContainer _split = new() { Dock = DockStyle.Fill, Orientation = Orientation.Vertical, FixedPanel = FixedPanel.Panel1 };
    private bool _sized;

    public WorkArea(Control left)
    {
        Dock = DockStyle.Fill;
        left.Dock = DockStyle.Fill;
        _split.Panel1.Controls.Add(left);
        Controls.Add(_split);

        // The splitter can only be placed once the control has its real width (hidden tabs start at size 0).
        _split.SizeChanged += (_, _) =>
        {
            if (_sized || _split.Width <= LeftWidth + 100)
                return;
            _split.SplitterDistance = LeftWidth;
            _sized = true;
        };
    }

    public Control LogSlot => _split.Panel2;
}

/// <summary>Moves the single XFS log panel into the work area of the tab the user is looking at.</summary>
public class SharedLogPlacement
{
    private readonly Control _root;
    private readonly Control _log;

    public SharedLogPlacement(Control root, Control log)
    {
        _root = root;
        _log = log;
        _log.Dock = DockStyle.Fill;
        foreach (var tabs in TabControls(root))
            tabs.SelectedIndexChanged += (_, _) => Place();
        Place();
    }

    public void Place()
    {
        var area = ActiveWorkArea(_root);
        if (area == null || _log.Parent == area.LogSlot)
            return;
        area.LogSlot.Controls.Add(_log);
    }

    private static WorkArea? ActiveWorkArea(Control control) => control switch
    {
        WorkArea area => area,
        TabControl tabs => tabs.SelectedTab == null ? null : ActiveWorkArea(tabs.SelectedTab),
        _ => control.Controls.Cast<Control>().Select(ActiveWorkArea).FirstOrDefault(a => a != null)
    };

    private static IEnumerable<TabControl> TabControls(Control control)
    {
        if (control is TabControl tabs)
            yield return tabs;
        foreach (Control child in control.Controls)
            foreach (var nested in TabControls(child))
                yield return nested;
    }
}
