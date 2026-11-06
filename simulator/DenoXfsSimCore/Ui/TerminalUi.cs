using System.Collections.Concurrent;
using System.Net;
using DenoXfs.Sim.Core.Names;

namespace DenoXfs.Sim.Core.Ui;

/// <summary>
/// Tells the web front end that something of a terminal changed (device state, sessions, traffic, selection). The key
/// is an IP (traffic, selection) or a Terminal Id (device state, which belongs to the terminal, not to one of its IPs);
/// a page listens to both of its terminal.
/// </summary>
public interface ITerminalNotifier
{
    event Action<string>? Changed;

    void Notify(string addressOrTerminalId);
}

public sealed class TerminalNotifier : ITerminalNotifier
{
    public event Action<string>? Changed;

    public void Notify(string addressOrTerminalId) => Changed?.Invoke(addressOrTerminalId);
}

/// <summary>A terminal is identified by the IP of its DLL connections; the web page of a user is the terminal of the browser's IP.</summary>
public static class TerminalAddress
{
    /// <summary>IPv4 mapped addresses as IPv4 and the IPv6 loopback as 127.0.0.1, so the DLL and the browser on one machine match.</summary>
    public static string Normalize(IPAddress? address)
    {
        if (address is null)
            return "unknown";
        if (address.IsIPv4MappedToIPv6)
            address = address.MapToIPv4();
        if (IPAddress.IPv6Loopback.Equals(address))
            address = IPAddress.Loopback;
        return address.ToString();
    }
}

public enum TrafficDirection
{
    Request,
    Completion,
    Event,
    Session
}

public sealed record TrafficEntry(DateTime Time, TrafficDirection Direction, string Text, bool IsError);

/// <summary>The last XFS messages of each terminal for the live log of the web page.</summary>
public interface ITrafficFeed
{
    void Add(string address, TrafficEntry entry);

    IReadOnlyList<TrafficEntry> Recent(string address);
}

public sealed class TrafficFeed : ITrafficFeed
{
    public const int Capacity = 300;
    private readonly ConcurrentDictionary<string, LinkedList<TrafficEntry>> _entries = new();
    private readonly ITerminalNotifier _notifier;

    public TrafficFeed(ITerminalNotifier notifier)
    {
        _notifier = notifier;
    }

    public void Add(string address, TrafficEntry entry)
    {
        var list = _entries.GetOrAdd(address, _ => new LinkedList<TrafficEntry>());
        lock (list)
        {
            list.AddLast(entry);
            if (list.Count > Capacity)
                list.RemoveFirst();
        }
        _notifier.Notify(address);
    }

    public IReadOnlyList<TrafficEntry> Recent(string address)
    {
        if (!_entries.TryGetValue(address, out var list))
            return [];
        lock (list)
            return list.ToList();
    }
}

/// <summary>Writes requests, completions, events and session changes into the feed with their spec names.</summary>
public interface ITrafficRecorder
{
    void Request(string address, uint reqId, XfsCodeKind kind, uint code);

    void Completion(string address, uint reqId, XfsCodeKind kind, uint code, int hResult);

    void Event(string address, uint reqId, string eventClass, uint eventId);

    void Session(string address, string text, bool isError = false);
}

public sealed class TrafficRecorder : ITrafficRecorder
{
    private readonly ITrafficFeed _feed;
    private readonly IXfsCodeNames _names;

    public TrafficRecorder(ITrafficFeed feed, IXfsCodeNames names)
    {
        _feed = feed;
        _names = names;
    }

    public void Request(string address, uint reqId, XfsCodeKind kind, uint code) =>
        Add(address, TrafficDirection.Request, $"#{reqId} {_names.Describe(kind, (int)code)}", false);

    public void Completion(string address, uint reqId, XfsCodeKind kind, uint code, int hResult) =>
        Add(address, TrafficDirection.Completion,
            $"#{reqId} {_names.NameOf(kind, (int)code) ?? code.ToString()} → {_names.NameOf(XfsCodeKind.Result, hResult) ?? hResult.ToString()}", hResult != 0);

    public void Event(string address, uint reqId, string eventClass, uint eventId) =>
        Add(address, TrafficDirection.Event, $"{(reqId == 0 ? string.Empty : $"#{reqId} ")}{eventClass} {_names.Describe(XfsCodeKind.Event, (int)eventId)}", false);

    public void Session(string address, string text, bool isError = false) => Add(address, TrafficDirection.Session, text, isError);

    private void Add(string address, TrafficDirection direction, string text, bool isError) =>
        _feed.Add(address, new TrafficEntry(DateTime.Now, direction, text, isError));
}

/// <summary>Where a device sits on the ATM picture.</summary>
public enum AtmArea
{
    Screen,
    CardReader,
    PinPad,
    ReceiptPrinter,
    CashDispenser,
    Deposit
}

/// <summary>How a device area looks right now.</summary>
public sealed record DeviceVisual(
    bool NeedsAttention, bool Fault, string StatusText, string? LightColor = null, string? LightFlash = null, bool MediaInside = false,
    bool MediaAtExit = false, string? ScreenHint = null);

/// <summary>
/// A device class module's part of the terminal page: its ATM area and the panel (a component with an <c>Address</c>
/// parameter) that opens when the area is clicked or when the device needs the user (e.g. a read waits for a card).
/// </summary>
public interface IDeviceUiModule
{
    string DeviceClass { get; }

    AtmArea Area { get; }

    string Title { get; }

    Type PanelComponent { get; }

    Task<DeviceVisual> VisualAsync(string address, CancellationToken cancellationToken);
}

/// <summary>The web page redraws a terminal after it switched scenario or device profile.</summary>
public sealed class NotifyingSelectionListener : Scenarios.IProfileSelectionListener
{
    private readonly ITerminalNotifier _notifier;

    public NotifyingSelectionListener(ITerminalNotifier notifier)
    {
        _notifier = notifier;
    }

    public Task OnSelectionChangedAsync(string address, Scenarios.ProfileSelection before, Scenarios.ProfileSelection after, CancellationToken cancellationToken)
    {
        _notifier.Notify(address);
        return Task.CompletedTask;
    }
}
