using System.Text.Json.Nodes;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Idc.Commands;

/// <summary>WFS_CMD_IDC_CHIP_POWER: input LPWORD fwChipPower; cold/warm reset returns the ATR (WFSIDCCHIPPOWEROUT), off returns nothing.</summary>
public sealed class ChipPowerCommand : IExecuteCommand
{
    private readonly IIdcDeviceRegistry _devices;

    public ChipPowerCommand(IIdcDeviceRegistry devices)
    {
        _devices = devices;
    }

    public uint Command => IdcConstants.ChipPowerCommand;

    public Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var power = IdcInput.GetUShort(context.Input, "chipPower");
        if (power is not (IdcConstants.ChipPowerCold or IdcConstants.ChipPowerWarm or IdcConstants.ChipPowerOff))
            return Task.FromResult(new CommandOutcome(XfsResultCodes.InvalidData, null));

        var device = _devices.Get(context.Device);
        var card = device.CardInside;
        if (card is null)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrNoMedia, null));
        if (!card.HasChip)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrInvalidMedia, null));

        var on = power != IdcConstants.ChipPowerOff;
        device.SetChipPower(on);
        if (!on)
            return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, null));

        var output = new JsonObject { ["lpbChipData"] = NativeJson.Hex(card.Atr!) };
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, new NativeValue(IdcResults.ChipPowerOut, output)));
    }
}

/// <summary>
/// WFS_CMD_IDC_CHIP_IO: input and output WFSIDCCHIPIO. The response comes from the card's stored APDU/response pairs.
/// The chip must be powered (READ_RAW_DATA with chip or CHIP_POWER).
/// </summary>
public sealed class ChipIoCommand : IExecuteCommand
{
    private const ushort SupportedProtocols = IdcConstants.ChipT0 | IdcConstants.ChipT1;
    private readonly IIdcDeviceRegistry _devices;

    public ChipIoCommand(IIdcDeviceRegistry devices)
    {
        _devices = devices;
    }

    public uint Command => IdcConstants.ChipIoCommand;

    public Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var protocol = IdcInput.GetUShort(context.Input, "chipProtocol") ?? 0;
        var apdu = IdcInput.GetBytes(context.Input, "chipData");
        if (apdu is not { Length: > 0 })
            return Task.FromResult(new CommandOutcome(XfsResultCodes.InvalidData, null));
        if (protocol == 0 || (protocol & ~SupportedProtocols) != 0)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrProtocolNotSupp, null));

        var device = _devices.Get(context.Device);
        var card = device.CardInside;
        if (card is null)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrNoMedia, null));
        if (!card.HasChip)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrInvalidMedia, null));
        if (device.Chip != ChipState.Online)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrAtrNotObtained, null));

        var response = card.RespondTo(apdu);
        var output = new JsonObject { ["wChipProtocol"] = protocol, ["lpbChipData"] = NativeJson.Hex(response) };
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, new NativeValue(IdcResults.ChipIo, output)));
    }
}
