using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Ui;
using DenoXfs.Sim.Idc.Cards;

namespace DenoXfs.Sim.Idc.Device;

public enum MediaPosition
{
    NotPresent,
    Inside,
    AtExit
}

public enum ChipState
{
    NoCard,
    PoweredOff,
    Online
}

public enum RetainBinState
{
    Ok,
    High,
    Full
}

/// <summary>Settings of one card reader, changed on the web page.</summary>
public sealed class IdcDeviceSettings
{
    /// <summary>On: the card is taken right after an eject (WFS_SRVE_IDC_MEDIAREMOVED follows). Off: the user takes it on the web page.</summary>
    public bool AutoRemoveAfterEject { get; set; } = true;

    public int RetainCapacity { get; set; } = 50;

    public int RetainHighThreshold { get; set; } = 40;
}

public sealed record IdcDeviceSnapshot(
    DeviceRef Reader, MediaPosition Media, string? CardName, ChipState Chip, int RetainCount, RetainBinState RetainBin,
    bool WaitingForCard, bool AutoRemoveAfterEject, int RetainCapacity, IReadOnlyList<uint> GuidanceLights, ushort PowerSaveRecoveryTime);

/// <summary>
/// State of one card reader of a terminal (a model device). Only state transitions live here; which XFS result or event follows
/// is decided by <see cref="IdcDeviceService"/>. Thread safe: commands and the web page change it concurrently.
/// </summary>
public sealed class IdcDevice
{
    private readonly object _lock = new();
    private MediaPosition _media = MediaPosition.NotPresent;
    private IdcCard? _card;
    private ChipState _chip = ChipState.NoCard;
    private int _retainCount;
    private readonly uint[] _lights = new uint[IdcConstants.GuidLightsMax + 1];
    private ushort _powerSaveRecoveryTime;
    private TaskCompletionSource<IdcCard>? _waiter;

    public IdcDevice(DeviceRef reader)
    {
        Reader = reader;
        _lights[IdcConstants.GuidanceCardUnit] = IdcConstants.GuidanceOff;
    }

    public DeviceRef Reader { get; }

    public IdcDeviceSettings Settings { get; } = new();

    /// <summary>Raised after every state change (outside the lock), for the web page.</summary>
    public event Action? Changed;

    /// <summary>Raises <see cref="Changed"/>; for changes made from outside, e.g. the settings.</summary>
    public void NotifyChanged() => Changed?.Invoke();

    public IdcDeviceSnapshot Snapshot()
    {
        lock (_lock)
            return new IdcDeviceSnapshot(Reader, _media, _card?.Name, _chip, _retainCount, BinState(_retainCount),
                _waiter is not null, Settings.AutoRemoveAfterEject, Settings.RetainCapacity, _lights.ToArray(), _powerSaveRecoveryTime);
    }

    /// <summary>The card inside the reader, or null (also null when the card waits at the exit).</summary>
    public IdcCard? CardInside
    {
        get
        {
            lock (_lock)
                return _media == MediaPosition.Inside ? _card : null;
        }
    }

    public ChipState Chip
    {
        get
        {
            lock (_lock)
                return _chip;
        }
    }

    /// <summary>Completes when a card is inside: at once when there is one, otherwise when the user inserts one.</summary>
    public Task<IdcCard> WaitForCardAsync(CancellationToken cancellationToken)
    {
        TaskCompletionSource<IdcCard> waiter;
        lock (_lock)
        {
            if (_media == MediaPosition.Inside && _card is not null)
                return Task.FromResult(_card);
            waiter = _waiter ??= new TaskCompletionSource<IdcCard>(TaskCreationOptions.RunContinuationsAsynchronously);
        }
        NotifyChanged();

        var registration = cancellationToken.Register(() =>
        {
            lock (_lock)
            {
                if (ReferenceEquals(_waiter, waiter))
                    _waiter = null;
            }
            waiter.TrySetCanceled(cancellationToken);
            NotifyChanged();
        });
        waiter.Task.ContinueWith(_ => registration.Dispose(), TaskScheduler.Default);
        return waiter.Task;
    }

    /// <summary>True when the reader was empty and the card is now inside.</summary>
    public bool Insert(IdcCard card)
    {
        TaskCompletionSource<IdcCard>? waiter;
        lock (_lock)
        {
            if (_media != MediaPosition.NotPresent)
                return false;
            _media = MediaPosition.Inside;
            _card = card;
            _chip = card.HasChip ? ChipState.PoweredOff : ChipState.NoCard;
            waiter = _waiter;
            _waiter = null;
        }
        waiter?.TrySetResult(card);
        NotifyChanged();
        return true;
    }

    /// <summary>True when a card at the exit was taken.</summary>
    public bool Take()
    {
        lock (_lock)
        {
            if (_media != MediaPosition.AtExit)
                return false;
            _media = MediaPosition.NotPresent;
            _card = null;
            _chip = ChipState.NoCard;
        }
        NotifyChanged();
        return true;
    }

    /// <summary>True when a card inside moved to the exit.</summary>
    public bool Eject()
    {
        lock (_lock)
        {
            if (_media != MediaPosition.Inside)
                return false;
            _media = MediaPosition.AtExit;
            _chip = ChipState.NoCard;
        }
        NotifyChanged();
        return true;
    }

    /// <summary>Moves the card (inside or at the exit) into the retain bin.</summary>
    public RetainResult Retain()
    {
        var result = RetainLocked();
        if (result.Outcome == RetainOutcome.Retained)
            NotifyChanged();
        return result;
    }

    private RetainResult RetainLocked()
    {
        lock (_lock)
        {
            var before = BinState(_retainCount);
            if (_media == MediaPosition.NotPresent)
                return new RetainResult(RetainOutcome.NoMedia, _retainCount, MediaPosition.NotPresent, before, before);
            if (before == RetainBinState.Full)
                return new RetainResult(RetainOutcome.BinFull, _retainCount, _media, before, before);

            var position = _media;
            _retainCount++;
            _media = MediaPosition.NotPresent;
            _card = null;
            _chip = ChipState.NoCard;
            return new RetainResult(RetainOutcome.Retained, _retainCount, position, before, BinState(_retainCount));
        }
    }

    /// <summary>Empties the retain bin; returns the bin state before and after.</summary>
    public (RetainBinState Before, RetainBinState After) ResetCount()
    {
        RetainBinState before;
        lock (_lock)
        {
            before = BinState(_retainCount);
            _retainCount = 0;
        }
        NotifyChanged();
        return (before, RetainBinState.Ok);
    }

    /// <summary>Powers the chip of the card inside on or off; false when there is no chip card inside.</summary>
    public bool SetChipPower(bool on)
    {
        lock (_lock)
        {
            if (_media != MediaPosition.Inside || _card is not { HasChip: true })
                return false;
            _chip = on ? ChipState.Online : ChipState.PoweredOff;
        }
        NotifyChanged();
        return true;
    }

    /// <summary>Takes over the settings and retain count stored for the terminal.</summary>
    public void Restore(IdcDeviceState state)
    {
        lock (_lock)
        {
            Settings.AutoRemoveAfterEject = state.AutoRemoveAfterEject;
            Settings.RetainCapacity = state.RetainCapacity;
            Settings.RetainHighThreshold = state.RetainHighThreshold;
            _retainCount = state.RetainCount;
        }
    }

    /// <summary>The part of the state that is stored for the terminal.</summary>
    public IdcDeviceState PersistentState()
    {
        lock (_lock)
            return new IdcDeviceState(Settings.AutoRemoveAfterEject, Settings.RetainCapacity, Settings.RetainHighThreshold, _retainCount);
    }

    /// <summary>Sets one dwGuidLights entry (WFS_IDC_GUIDANCE_* flags).</summary>
    public void SetGuidanceLight(int index, uint command)
    {
        lock (_lock)
            _lights[index] = command;
        NotifyChanged();
    }

    /// <summary>Sets the power save recovery time; true when it changed.</summary>
    public bool SetPowerSaveRecoveryTime(ushort seconds)
    {
        lock (_lock)
        {
            if (_powerSaveRecoveryTime == seconds)
                return false;
            _powerSaveRecoveryTime = seconds;
        }
        NotifyChanged();
        return true;
    }

    private RetainBinState BinState(int count) =>
        count >= Settings.RetainCapacity ? RetainBinState.Full
        : count >= Settings.RetainHighThreshold ? RetainBinState.High
        : RetainBinState.Ok;
}

public enum RetainOutcome
{
    Retained,
    NoMedia,
    BinFull
}

public sealed record RetainResult(RetainOutcome Outcome, int Count, MediaPosition Position, RetainBinState BinBefore, RetainBinState BinAfter);

/// <summary>Every card reader of every terminal, created on first use with the state stored for it.</summary>
public interface IIdcDeviceRegistry
{
    IdcDevice Get(DeviceRef reader);

    IReadOnlyList<IdcDevice> All { get; }

    /// <summary>Stores the settings and retain count of the card reader (after they changed).</summary>
    void Save(DeviceRef reader);
}

public sealed class IdcDeviceRegistry : IIdcDeviceRegistry
{
    private readonly System.Collections.Concurrent.ConcurrentDictionary<DeviceRef, Lazy<IdcDevice>> _devices = new();
    private readonly IIdcDeviceStateStore _store;
    private readonly ITerminalNotifier _notifier;

    public IdcDeviceRegistry(IIdcDeviceStateStore store, ITerminalNotifier notifier)
    {
        _store = store;
        _notifier = notifier;
    }

    public IdcDevice Get(DeviceRef reader) => _devices.GetOrAdd(reader, r => new Lazy<IdcDevice>(() => Create(r))).Value;

    public IReadOnlyList<IdcDevice> All =>
        _devices.Values.Select(d => d.Value).OrderBy(d => d.Reader.TerminalId, StringComparer.Ordinal).ThenBy(d => d.Reader.DeviceId).ToList();

    public void Save(DeviceRef reader) => _store.Save(reader, Get(reader).PersistentState());

    private IdcDevice Create(DeviceRef reader)
    {
        var device = new IdcDevice(reader);
        if (_store.Load(reader) is { } state)
            device.Restore(state);
        device.Changed += () => _notifier.Notify(reader.TerminalId);
        return device;
    }
}
