using DenoXfs.TestTool.Operations;

namespace DenoXfs.TestTool.Ui;

/// <summary>
/// One operation as a low strip above the log: Run and Cancel on the left over the full strip height, the parameters
/// (in columns, see <see cref="ParameterColumns"/>) with the description under them on the right. Input and output go
/// to the XFS log.
/// </summary>
public class OperationPage : UserControl, IPreferredHeight
{
    private const int ButtonColumnWidth = 110;
    private readonly IOperationDefinition _operation;
    private readonly IOperationRunner _runner;
    private const int MinStripHeight = 66;
    private readonly Button _run = new() { Text = "▶ Run", Dock = DockStyle.Fill, Margin = new Padding(2) };
    private readonly Button _cancel = new() { Text = "Cancel", Dock = DockStyle.Fill, Margin = new Padding(2), Enabled = false };
    private OperationRun? _running;
    private readonly object _parameterObject;

    public OperationPage(IOperationDefinition operation, IOperationRunner runner)
    {
        _operation = operation;
        _runner = runner;
        _parameterObject = operation.CreateParameters();
        HasParameters = _parameterObject.GetType().GetProperties().Length > 0;
        FlagsEnumEditor.RegisterFor(_parameterObject.GetType());
        Dock = DockStyle.Fill;
        _run.Font = new Font(_run.Font, FontStyle.Bold);

        var text = $"{operation.Description}  (code {operation.Code})";
        // Under the parameters, so the first parameter row lines up with the top of Run.
        var description = new Label
        {
            Dock = DockStyle.Bottom,
            AutoEllipsis = true,
            Height = Font.Height + 6,
            Padding = new Padding(4, 2, 4, 0),
            ForeColor = SystemColors.GrayText,
            Text = text
        };
        new ToolTip().SetToolTip(description, text);

        var right = new Panel { Dock = DockStyle.Fill };
        var rows = 0;
        if (HasParameters)
        {
            var columns = new ParameterColumns(_parameterObject);
            rows = columns.ParameterRows;
            right.Controls.Add(columns);
        }
        right.Controls.Add(description);

        // Run and Cancel span the whole strip (parameters + description): Run takes the larger part.
        var buttons = new TableLayoutPanel { Dock = DockStyle.Left, Width = ButtonColumnWidth, ColumnCount = 1, RowCount = 2 };
        buttons.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
        buttons.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
        buttons.Controls.Add(_run, 0, 0);
        buttons.Controls.Add(_cancel, 0, 1);

        Controls.Add(right);
        Controls.Add(buttons);

        var parametersHeight = description.Height + (rows > 0 ? rows * (Font.Height + 5) + 8 : 0);
        PreferredHeight = Math.Max(MinStripHeight, parametersHeight);

        _run.Click += async (_, _) => await RunAsync();
        _cancel.Click += (_, _) =>
        {
            _cancel.Enabled = false;
            _running?.Cancel();
        };
    }

    /// <summary>False for operations without input (e.g. Status, Capabilities): their tab runs them on click.</summary>
    public bool HasParameters { get; }

    public int PreferredHeight { get; }

    /// <summary>Runs the operation unless it is already running.</summary>
    public async Task RunIfIdleAsync()
    {
        if (_run.Enabled)
            await RunAsync();
    }

    private async Task RunAsync()
    {
        _run.Enabled = false;
        _cancel.Enabled = true;
        try
        {
            _running = _runner.Start(_operation, _parameterObject);
            await _running.Completion;
        }
        finally
        {
            _running = null;
            _run.Enabled = true;
            _cancel.Enabled = false;
        }
    }
}
