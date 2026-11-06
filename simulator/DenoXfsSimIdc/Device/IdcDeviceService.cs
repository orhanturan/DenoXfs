using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Layouts;
using DenoXfs.Sim.Idc.Scenarios;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Idc.Device;

public enum InsertOutcome
{
    Inserted,
    UnknownCard,
    ReaderNotEmpty
}

/// <summary>
/// The card reader's rules: what each action does to the device and which event goes out when. Used by the XFS
/// commands and by the web front end, so the same action always produces the same events.
/// </summary>
public interface IIdcDeviceService
{
    Task<InsertOutcome> InsertAsync(DeviceRef reader, string cardName, CancellationToken cancellationToken);

    /// <summary>Web page: changes the settings of the card reader and stores them.</summary>
    void UpdateSettings(DeviceRef reader, bool autoRemoveAfterEject, int? retainCapacity, int? retainHighThreshold);

    Task<bool> TakeAsync(DeviceRef reader, CancellationToken cancellationToken);

    Task<IdcCard> WaitForCardAsync(DeviceRef reader, IExecuteEvents events, CancellationToken cancellationToken);

    Task<int> EjectAsync(DeviceRef reader, CancellationToken cancellationToken);

    Task<RetainResult> RetainAsync(DeviceRef reader, IExecuteEvents? events, CancellationToken cancellationToken);

    Task ResetCountAsync(DeviceRef reader, CancellationToken cancellationToken);

    Task<int> ResetAsync(DeviceRef reader, ushort action, CancellationToken cancellationToken);
}

public sealed class IdcDeviceService : IIdcDeviceService
{
    private readonly IIdcDeviceRegistry _devices;
    private readonly ICardCatalog _cards;
    private readonly IEventPublisher _events;
    private readonly ILogger<IdcDeviceService> _logger;

    public IdcDeviceService(IIdcDeviceRegistry devices, ICardCatalog cards, IEventPublisher events, ILogger<IdcDeviceService> logger)
    {
        _devices = devices;
        _cards = cards;
        _events = events;
        _logger = logger;
    }

    /// <summary>Web page: the user puts a card into the reader. A waiting read continues with it.</summary>
    public async Task<InsertOutcome> InsertAsync(DeviceRef reader, string cardName, CancellationToken cancellationToken)
    {
        var card = await _cards.FindAsync(cardName, cancellationToken);
        if (card is null)
            return InsertOutcome.UnknownCard;

        var inserted = _devices.Get(reader).Insert(card);
        _logger.LogInformation("card insert reader={Reader} card={Card} inserted={Inserted}", reader, card.Name, inserted);
        return inserted ? InsertOutcome.Inserted : InsertOutcome.ReaderNotEmpty;
    }

    public void UpdateSettings(DeviceRef reader, bool autoRemoveAfterEject, int? retainCapacity, int? retainHighThreshold)
    {
        var settings = _devices.Get(reader).Settings;
        settings.AutoRemoveAfterEject = autoRemoveAfterEject;
        if (retainCapacity is > 0)
            settings.RetainCapacity = retainCapacity.Value;
        if (retainHighThreshold is > 0)
            settings.RetainHighThreshold = Math.Min(retainHighThreshold.Value, settings.RetainCapacity);
        _devices.Save(reader);
        _devices.Get(reader).NotifyChanged();
        _logger.LogInformation("settings reader={Reader} autoRemove={AutoRemove} capacity={Capacity} high={High}",
            reader, settings.AutoRemoveAfterEject, settings.RetainCapacity, settings.RetainHighThreshold);
    }

    /// <summary>Web page: the user takes the card from the exit. Sends WFS_SRVE_IDC_MEDIAREMOVED.</summary>
    public async Task<bool> TakeAsync(DeviceRef reader, CancellationToken cancellationToken)
    {
        if (!_devices.Get(reader).Take())
            return false;

        _logger.LogInformation("card taken reader={Reader}", reader);
        await _events.PublishAsync(reader, IdcProfiles.DeviceClass, XfsEventKind.Service, IdcConstants.MediaRemovedEvent, null, cancellationToken);
        return true;
    }

    /// <summary>
    /// Waits for a card inside. When there is none: WFS_EXEE_IDC_INSERTCARD, wait for the web page, then
    /// WFS_EXEE_IDC_MEDIAINSERTED.
    /// </summary>
    public async Task<IdcCard> WaitForCardAsync(DeviceRef reader, IExecuteEvents events, CancellationToken cancellationToken)
    {
        var device = _devices.Get(reader);
        if (device.CardInside is { } card)
            return card;

        await events.SendAsync(IdcConstants.InsertCardEvent, null, cancellationToken);
        card = await device.WaitForCardAsync(cancellationToken);
        await events.SendAsync(IdcConstants.MediaInsertedEvent, null, cancellationToken);
        return card;
    }

    /// <summary>WFS_CMD_IDC_EJECT_CARD: card to the exit; with auto remove it is taken at once (WFS_SRVE_IDC_MEDIAREMOVED).</summary>
    public async Task<int> EjectAsync(DeviceRef reader, CancellationToken cancellationToken)
    {
        var device = _devices.Get(reader);
        if (!device.Eject())
            return IdcConstants.ErrNoMedia;

        if (device.Settings.AutoRemoveAfterEject)
            await TakeAsync(reader, cancellationToken);
        return XfsResultCodes.Success;
    }

    /// <summary>
    /// Card into the retain bin. A retain command sends WFS_EXEE_IDC_MEDIARETAINED (when <paramref name="events"/> is
    /// given); every retain that changes the bin state sends WFS_USRE_IDC_RETAINBINTHRESHOLD.
    /// </summary>
    public async Task<RetainResult> RetainAsync(DeviceRef reader, IExecuteEvents? events, CancellationToken cancellationToken)
    {
        var result = _devices.Get(reader).Retain();
        _logger.LogInformation("card retain reader={Reader} outcome={Outcome} count={Count} bin={Bin}", reader, result.Outcome, result.Count, result.BinAfter);
        if (result.Outcome != RetainOutcome.Retained)
            return result;

        _devices.Save(reader);

        if (events is not null)
            await events.SendAsync(IdcConstants.MediaRetainedEvent, null, cancellationToken);
        await PublishBinChangeAsync(reader, result.BinBefore, result.BinAfter, cancellationToken);
        return result;
    }

    /// <summary>WFS_CMD_IDC_RESET_COUNT: empties the retain bin.</summary>
    public async Task ResetCountAsync(DeviceRef reader, CancellationToken cancellationToken)
    {
        var (before, after) = _devices.Get(reader).ResetCount();
        _devices.Save(reader);
        await PublishBinChangeAsync(reader, before, after, cancellationToken);
    }

    /// <summary>
    /// WFS_CMD_IDC_RESET: with a card in the reader the reset action is applied and WFS_SRVE_IDC_MEDIADETECTED tells
    /// where the card went.
    /// </summary>
    public async Task<int> ResetAsync(DeviceRef reader, ushort action, CancellationToken cancellationToken)
    {
        var device = _devices.Get(reader);
        if (device.CardInside is null)
            return XfsResultCodes.Success;

        device.SetChipPower(false);
        ushort position;
        switch (action)
        {
            case IdcConstants.ResetEject:
                await EjectAsync(reader, cancellationToken);
                position = IdcConstants.CardEjected;
                break;
            case IdcConstants.ResetRetain:
                var retained = await RetainAsync(reader, null, cancellationToken);
                if (retained.Outcome == RetainOutcome.BinFull)
                    return IdcConstants.ErrRetainBinFull;
                position = IdcConstants.CardRetained;
                break;
            default:
                position = IdcConstants.CardReadPosition;
                break;
        }

        await _events.PublishAsync(reader, IdcProfiles.DeviceClass, XfsEventKind.Service, IdcConstants.MediaDetectedEvent,
            new NativeValue(IdcResults.MediaDetected, position), cancellationToken);
        return XfsResultCodes.Success;
    }

    private async Task PublishBinChangeAsync(DeviceRef reader, RetainBinState before, RetainBinState after, CancellationToken cancellationToken)
    {
        if (before == after)
            return;

        var state = after switch
        {
            RetainBinState.Full => IdcConstants.RetainBinFull,
            RetainBinState.High => IdcConstants.RetainBinHigh,
            _ => IdcConstants.RetainBinOk
        };
        await _events.PublishAsync(reader, IdcProfiles.DeviceClass, XfsEventKind.User, IdcConstants.RetainBinThresholdEvent,
            new NativeValue(IdcResults.RetainBinThreshold, state), cancellationToken);
    }
}
