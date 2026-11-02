namespace DenoXfs.TestTool.Ui;

/// <summary>Where a work area puts its controls relative to the log.</summary>
public enum WorkAreaLayout
{
    /// <summary>A narrow column left of the log (Settings, Session).</summary>
    Side,

    /// <summary>A low strip above the log (operations); the content says how high through <see cref="IPreferredHeight"/>.</summary>
    Top
}

/// <summary>Content of a <see cref="WorkAreaLayout.Top"/> work area that knows the height it needs.</summary>
public interface IPreferredHeight
{
    int PreferredHeight { get; }
}

/// <summary>
/// Content of a leaf tab: the tab's controls next to or above a slot that receives the shared XFS log while this tab
/// is shown.
/// </summary>
public class WorkArea : UserControl
{
    private const int LeftWidth = 360;
    private const int DefaultTopHeight = 120;
    private readonly SplitContainer _split;
    private bool _sized;

    public WorkArea(Control content, WorkAreaLayout layout = WorkAreaLayout.Side)
    {
        Dock = DockStyle.Fill;
        _split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = layout == WorkAreaLayout.Side ? Orientation.Vertical : Orientation.Horizontal,
            FixedPanel = FixedPanel.Panel1
        };
        content.Dock = DockStyle.Fill;
        _split.Panel1.Controls.Add(content);
        Controls.Add(_split);

        var distance = layout == WorkAreaLayout.Side
            ? LeftWidth
            : (content as IPreferredHeight)?.PreferredHeight ?? DefaultTopHeight;

        // The splitter can only be placed once the control has its real size (hidden tabs start at size 0).
        _split.SizeChanged += (_, _) =>
        {
            var size = layout == WorkAreaLayout.Side ? _split.Width : _split.Height;
            if (_sized || size <= distance + 100)
                return;
            _split.SplitterDistance = distance;
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
