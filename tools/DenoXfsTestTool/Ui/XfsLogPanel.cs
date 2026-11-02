using DenoXfs.TestTool.Logging;

namespace DenoXfs.TestTool.Ui;

/// <summary>The XFS log below the tabs: commands with input and output, events and session steps (see <see cref="IXfsTrafficLog"/>).</summary>
public class XfsLogPanel : UserControl
{
    private const int MaxEntries = 5000;
    private readonly TextBox _text = new() { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Both, WordWrap = false, Font = new Font("Consolas", 9F) };
    private readonly Queue<string> _entries = new();

    public XfsLogPanel(IUiLogFeed feed)
    {
        Dock = DockStyle.Fill;
        Controls.Add(_text);

        feed.LineWritten += entry =>
        {
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(() => Append(entry));
        };
    }

    public void Clear()
    {
        _entries.Clear();
        _text.Clear();
    }

    private void Append(string entry)
    {
        _entries.Enqueue(entry);
        while (_entries.Count > MaxEntries)
            _entries.Dequeue();

        _text.AppendText(entry + Environment.NewLine);
        if (_entries.Count == MaxEntries)
            _text.Text = string.Join(Environment.NewLine, _entries) + Environment.NewLine;
    }
}
