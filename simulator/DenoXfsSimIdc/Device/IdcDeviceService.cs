using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Cards;
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
    Task<InsertOutcome> InsertAsync(string address, string cardName, CancellationToken cancellationToken);

    /// <summary>Web page: changes the reader settings of the terminal and stores them.</summary>
    void UpdateSettings(string address, bool autoRemoveAfterEject, int? retainCapacity, int? retainHighThreshold);

    Task<bool> TakeAsync(string address, CancellationToken cancellationToken);

    Task<IdcCard> WaitForCardAsync(string address, IExecuteEvents events, CancellationToken cancellationToken);

    Task<int> EjectAsync(string address, CancellationToken cancellationToken);

    Task<RetainResult> RetainAsync(string address, IExecuteEvents? events, CancellationToken cancellationToken);

    Task ResetCountAsync(string address, CancellationToken cancellationToken);

    Task<int> ResetAsync(string address, ushort action, CancellationToken cancellationToken);
}

public sealed class IdcDeviceService : IIdcDeviceService
{
    private readonly IIdcDeviceRegistry _devices;
    private readonly ICardCatalog _cards;
    private readonly IEventPublisher _events;
    private readonly INativeBufferBuilderFactory _builders;
    private readonly ILogger<IdcDeviceService> _logger;

    public IdcDeviceService(IIdcDeviceRegistry devices, ICardCatalog cards, IEventPublisher events, INativeBufferBuilderFactory builders,
        ILogger<IdcDeviceService> logger)
    {
        _devices = devices;
        _cards = cards;
        _events = events;
        _builders = builders;
        _logger = logger;
    }

    /// <summary>Web page: the user puts a card into the reader. A waiting read continues with it.</summary>
    public async Task<InsertOutcome> InsertAsync(string address, string cardName, CancellationToken cancellationToken)
    {
        var card = await _cards.FindAsync(cardName, cancellationToken);
        if (card is null)
            return InsertOutcome.UnknownCard;

        var inserted = _devices.Get(address).Insert(card);
        _logger.LogInformation("card insert terminal={Terminal} card={Card} inserted={Inserted}", address, card.Name, inserted);
        return inserted ? InsertOutcome.Inserted : InsertOutcome.ReaderNotEmpty;
    }

    public void UpdateSettings(string address, bool autoRemoveAfterEject, int? retainCapacity, int? retainHighThreshold)
    {
        var settings = _devices.Get(address).Settings;
        settings.AutoRemoveAfterEject = autoRemoveAfterEject;
        if (retainCapacity is > 0)
            settings.RetainCapacity = retainCapacity.Value;
        if (retainHighThreshold is > 0)
            settings.RetainHighThreshold = Math.Min(retainHighThreshold.Value, settings.RetainCapacity);
        _devices.Save(address);
        _logger.LogInformation("settings terminal={Terminal} autoRemove={AutoRemove} capacity={Capacity} high={High}",
            address, settings.AutoRemoveAfterEject, settings.RetainCapacity, settings.RetainHighThreshold);
    }

    /// <summary>Web page: the user takes the card from the exit. Sends WFS_SRVE_IDC_MEDIAREMOVED.</summary>
    public async Task<bool> TakeAsync(string address, CancellationToken cancellationToken)
    {
        if (!_devices.Get(address).Take())
            return false;

        _logger.LogInformation("card taken terminal={Terminal}", address);
        await _events.PublishAsync(address, XfsEventKind.Service, IdcConstants.MediaRemovedEvent, null, cancellationToken);
        return true;
    }

    /// <summary>
    /// Waits for a card inside. When there is none: WFS_EXEE_IDC_INSERTCARD, wait for the web page, then
    /// WFS_EXEE_IDC_MEDIAINSERTED.
    /// </summary>
    public async Task<IdcCard> WaitForCardAsync(string address, IExecuteEvents events, CancellationToken cancellationToken)
    {
        var device = _devices.Get(address);
        if (device.CardInside is { } card)
            return card;

        await events.SendAsync(IdcConstants.InsertCardEvent, null, cancellationToken);
        card = await device.WaitForCardAsync(cancellationToken);
        await events.SendAsync(IdcConstants.MediaInsertedEvent, null, cancellationToken);
        return card;
    }

    /// <summary>WFS_CMD_IDC_EJECT_CARD: card to the exit; with auto remove it is taken at once (WFS_SRVE_IDC_MEDIAREMOVED).</summary>
    public async Task<int> EjectAsync(string address, CancellationToken cancellationToken)
    {
        var device = _devices.Get(address);
        if (!device.Eject())
            return IdcConstants.ErrNoMedia;

        if (device.Settings.AutoRemoveAfterEject)
            await TakeAsync(address, cancellationToken);
        return XfsResultCodes.Success;
    }

    /// <summary>
    /// Card into the retain bin. A retain command sends WFS_EXEE_IDC_MEDIARETAINED (when <paramref name="events"/> is
    /// given); every retain that changes the bin state sends WFS_USRE_IDC_RETAINBINTHRESHOLD.
    /// </summary>
    public async Task<RetainResult> RetainAsync(string address, IExecuteEvents? events, CancellationToken cancellationToken)
    {
        var result = _devices.Get(address).Retain();
        _logger.LogInformation("card retain terminal={Terminal} outcome={Outcome} count={Count} bin={Bin}", address, result.Outcome, result.Count, result.BinAfter);
        if (result.Outcome != RetainOutcome.Retained)
            return result;

        _devices.Save(address);

        if (events is not null)
            await events.SendAsync(IdcConstants.MediaRetainedEvent, null, cancellationToken);
        await PublishBinChangeAsync(address, result.BinBefore, result.BinAfter, cancellationToken);
        return result;
    }

    /// <summary>WFS_CMD_IDC_RESET_COUNT: empties the retain bin.</summary>
    public async Task ResetCountAsync(string address, CancellationToken cancellationToken)
    {
        var (before, after) = _devices.Get(address).ResetCount();
        _devices.Save(address);
        await PublishBinChangeAsync(address, before, after, cancellationToken);
    }

    /// <summary>
    /// WFS_CMD_IDC_RESET: with a card in the reader the reset action is applied and WFS_SRVE_IDC_MEDIADETECTED tells
    /// where the card went.
    /// </summary>
    public async Task<int> ResetAsync(string address, ushort action, CancellationToken cancellationToken)
    {
        var device = _devices.Get(address);
        if (device.CardInside is null)
            return XfsResultCodes.Success;

        device.SetChipPower(false);
        ushort position;
        switch (action)
        {
            case IdcConstants.ResetEject:
                await EjectAsync(address, cancellationToken);
                position = IdcConstants.CardEjected;
                break;
            case IdcConstants.ResetRetain:
                var retained = await RetainAsync(address, null, cancellationToken);
                if (retained.Outcome == RetainOutcome.BinFull)
                    return IdcConstants.ErrRetainBinFull;
                position = IdcConstants.CardRetained;
                break;
            default:
                position = IdcConstants.CardReadPosition;
                break;
        }

        await _events.PublishAsync(address, XfsEventKind.Service, IdcConstants.MediaDetectedEvent, Word(position), cancellationToken);
        return XfsResultCodes.Success;
    }

    private async Task PublishBinChangeAsync(string address, RetainBinState before, RetainBinState after, CancellationToken cancellationToken)
    {
        if (before == after)
            return;

        var state = after switch
        {
            RetainBinState.Full => IdcConstants.RetainBinFull,
            RetainBinState.High => IdcConstants.RetainBinHigh,
            _ => IdcConstants.RetainBinOk
        };
        await _events.PublishAsync(address, XfsEventKind.User, IdcConstants.RetainBinThresholdEvent, Word(state), cancellationToken);
    }

    /// <summary>Event payload that is a pointer to a single WORD (lpBuffer points at the value).</summary>
    private ResultBuffer Word(ushort value)
    {
        var builder = _builders.Create();
        builder.WriteUInt16(builder.Reserve(sizeof(ushort)), value);
        return builder.Build();
    }
}
