using DenoXfs.TestTool.Operations;

namespace DenoXfs.TestTool.Ui;

/// <summary>One operation: description, parameters and the Run button. Input and output go to the XFS log.</summary>
public class OperationPage : UserControl
{
    private readonly IOperationDefinition _operation;
    private readonly IOperationRunner _runner;
    private readonly PropertyGrid _parameters = new() { Dock = DockStyle.Fill, HelpVisible = false, ToolbarVisible = false };
    private readonly Button _run = new() { Text = "Run", Dock = DockStyle.Top, Height = 30 };
    private readonly Button _cancel = new() { Text = "Cancel", Dock = DockStyle.Top, Height = 26, Enabled = false };
    private OperationRun? _running;
    private readonly object _parameterObject;

    public OperationPage(IOperationDefinition operation, IOperationRunner runner)
    {
        _operation = operation;
        _runner = runner;
        _parameterObject = operation.CreateParameters();
        Dock = DockStyle.Fill;

        // Narrow column: Run always at the top, then the description, then the parameters.
        var description = new Label
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            MaximumSize = new Size(340, 0),
            Padding = new Padding(4),
            Text = $"{operation.Description}  (code {operation.Code})"
        };
        _parameters.SelectedObject = _parameterObject;

        // Docking takes the last added control first: Run ends up on top, Cancel right under it.
        Controls.Add(_parameters);
        Controls.Add(description);
        Controls.Add(_cancel);
        Controls.Add(_run);

        _run.Click += async (_, _) => await RunAsync();
        _cancel.Click += (_, _) =>
        {
            _cancel.Enabled = false;
            _running?.Cancel();
        };
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
