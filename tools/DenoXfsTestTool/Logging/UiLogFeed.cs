namespace DenoXfs.TestTool.Logging;

/// <summary>Entries of the XFS log on screen, written by <see cref="IXfsTrafficLog"/>. An entry may span several lines.</summary>
public interface IUiLogFeed
{
    event Action<string>? LineWritten;

    void Write(string line);
}

public class UiLogFeed : IUiLogFeed
{
    public event Action<string>? LineWritten;

    public void Write(string line) => LineWritten?.Invoke(line);
}
