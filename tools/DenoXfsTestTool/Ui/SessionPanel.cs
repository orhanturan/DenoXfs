using DenoXfs.TestTool.Session;

namespace DenoXfs.TestTool.Ui;

/// <summary>Session tab: open and close the session, clear the log.</summary>
public class SessionPanel : UserControl
{
    private readonly ISessionController _session;
    private readonly Button _open = new() { Text = "Open session", AutoSize = true };
    private readonly Button _close = new() { Text = "Close session", AutoSize = true };
    private readonly Button _clearLog = new() { Text = "Clear log", AutoSize = true };
    private readonly Label _status = new() { AutoSize = true, Padding = new Padding(8, 6, 0, 0) };

    public SessionPanel(ISessionController session, XfsLogPanel log)
    {
        _session = session;
        Dock = DockStyle.Fill;

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(6) };
        buttons.Controls.AddRange([_open, _close, _clearLog, _status]);
        Controls.Add(buttons);

        _clearLog.Click += (_, _) => log.Clear();

        _open.Click += async (_, _) => await RunAsync(_session.OpenAsync);
        _close.Click += async (_, _) => await RunAsync(_session.CloseAsync);
        _session.StateChanged += () => BeginInvoke(Refresh_);
        Refresh_();
    }

    private async Task RunAsync(Func<Task<SessionResult>> action)
    {
        _open.Enabled = _close.Enabled = false;
        var result = await action();
        _status.Text = result.Message;
        Refresh_();
    }

    private void Refresh_()
    {
        var state = _session.State;
        _open.Enabled = state == SessionState.Closed;
        _close.Enabled = state == SessionState.Open;
        if (string.IsNullOrEmpty(_status.Text) || state is SessionState.Opening or SessionState.Closing)
            _status.Text = state.ToString();
    }
}
