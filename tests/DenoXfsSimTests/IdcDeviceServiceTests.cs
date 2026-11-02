using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Device;

namespace DenoXfs.Sim.Tests;

public class IdcDeviceServiceTests
{
    private const string Address = "10.0.0.5";
    private readonly IdcTestRig _rig = new();

    [Fact]
    public async Task Eject_with_auto_remove_takes_the_card_and_sends_media_removed()
    {
        _rig.Insert(Address, "Chip only");

        Assert.Equal(XfsResultCodes.Success, await _rig.Service.EjectAsync(Address, CancellationToken.None));

        Assert.Equal(MediaPosition.NotPresent, _rig.Devices.Get(Address).Snapshot().Media);
        Assert.Equal((XfsEventKind.Service, IdcConstants.MediaRemovedEvent), Single());
    }

    [Fact]
    public async Task Eject_without_auto_remove_waits_for_take_on_the_web_page()
    {
        _rig.Devices.Get(Address).Settings.AutoRemoveAfterEject = false;
        _rig.Insert(Address, "Chip only");

        await _rig.Service.EjectAsync(Address, CancellationToken.None);
        Assert.Equal(MediaPosition.AtExit, _rig.Devices.Get(Address).Snapshot().Media);
        Assert.Empty(_rig.Publisher.Published);

        Assert.True(await _rig.Service.TakeAsync(Address, CancellationToken.None));
        Assert.Equal((XfsEventKind.Service, IdcConstants.MediaRemovedEvent), Single());
    }

    [Fact]
    public async Task Eject_without_card_is_no_media()
    {
        Assert.Equal(IdcConstants.ErrNoMedia, await _rig.Service.EjectAsync(Address, CancellationToken.None));
    }

    [Fact]
    public async Task Retain_sends_media_retained_and_a_threshold_event_only_when_the_bin_state_changes()
    {
        var settings = _rig.Devices.Get(Address).Settings;
        settings.RetainHighThreshold = 2;
        settings.RetainCapacity = 3;

        for (var i = 0; i < 3; i++)
        {
            _rig.Insert(Address, "Chip only");
            Assert.Equal(RetainOutcome.Retained, (await _rig.Service.RetainAsync(Address, _rig.ExecuteEvents, CancellationToken.None)).Outcome);
        }

        Assert.All(_rig.ExecuteEventIds, id => Assert.Equal(IdcConstants.MediaRetainedEvent, id));
        Assert.Equal(3, _rig.ExecuteEventIds.Length);
        var thresholds = _rig.Publisher.Published.Where(e => e.EventId == IdcConstants.RetainBinThresholdEvent).ToList();
        Assert.Equal(new[] { IdcConstants.RetainBinHigh, IdcConstants.RetainBinFull }, thresholds.Select(e => Word(e.Payload!)));

        _rig.Insert(Address, "Chip only");
        Assert.Equal(RetainOutcome.BinFull, (await _rig.Service.RetainAsync(Address, _rig.ExecuteEvents, CancellationToken.None)).Outcome);

        await _rig.Service.ResetCountAsync(Address, CancellationToken.None);
        Assert.Equal(IdcConstants.RetainBinOk, Word(_rig.Publisher.Published[^1].Payload!));
    }

    [Fact]
    public async Task Reset_with_eject_sends_media_detected_with_the_card_position()
    {
        _rig.Insert(Address, "Chip only");

        await _rig.Service.ResetAsync(Address, IdcConstants.ResetEject, CancellationToken.None);

        var detected = _rig.Publisher.Published.Single(e => e.EventId == IdcConstants.MediaDetectedEvent);
        Assert.Equal(IdcConstants.CardEjected, Word(detected.Payload!));
    }

    [Fact]
    public async Task Retain_count_and_settings_are_stored_and_restored_for_the_terminal()
    {
        _rig.Service.UpdateSettings(Address, autoRemoveAfterEject: false, retainCapacity: 20, retainHighThreshold: 15);
        _rig.Insert(Address, "Chip only");
        await _rig.Service.RetainAsync(Address, null, CancellationToken.None);

        Assert.Equal(new IdcDeviceState(false, 20, 15, 1), _rig.Store.States[Address]);

        // A new registry (host restart) restores the stored state.
        var restored = new IdcDeviceRegistry(_rig.Store).Get(Address);
        Assert.Equal(1, restored.Snapshot().RetainCount);
        Assert.False(restored.Settings.AutoRemoveAfterEject);
    }

    [Fact]
    public void Insert_into_a_reader_that_holds_a_card_is_refused()
    {
        _rig.Insert(Address, "Chip only");

        Assert.Equal(InsertOutcome.ReaderNotEmpty, _rig.Service.InsertAsync(Address, "Chip only", CancellationToken.None).Result);
        Assert.Equal(InsertOutcome.UnknownCard, _rig.Service.InsertAsync("other", "no such card", CancellationToken.None).Result);
    }

    private (XfsEventKind, uint) Single()
    {
        var published = Assert.Single(_rig.Publisher.Published);
        return (published.Kind, published.EventId);
    }

    private static ushort Word(ResultBuffer payload) => BitConverter.ToUInt16(Convert.FromBase64String(payload.Data));
}
