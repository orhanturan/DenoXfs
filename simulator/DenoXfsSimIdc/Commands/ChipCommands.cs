using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Idc.Commands;

/// <summary>WFS_CMD_IDC_CHIP_POWER: input LPWORD fwChipPower; cold/warm reset returns the ATR (WFSIDCCHIPPOWEROUT), off returns nothing.</summary>
public sealed class ChipPowerCommand : IExecuteCommand
{
    private readonly IIdcDeviceRegistry _devices;
    private readonly INativeBufferBuilderFactory _builders;

    public ChipPowerCommand(IIdcDeviceRegistry devices, INativeBufferBuilderFactory builders)
    {
        _devices = devices;
        _builders = builders;
    }

    public uint Command => IdcConstants.ChipPowerCommand;

    public Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var power = IdcInput.GetUShort(context.Input, "chipPower");
        if (power is not (IdcConstants.ChipPowerCold or IdcConstants.ChipPowerWarm or IdcConstants.ChipPowerOff))
            return Task.FromResult(new CommandOutcome(XfsResultCodes.InvalidData, null));

        var device = _devices.Get(context.RemoteAddress);
        var card = device.CardInside;
        if (card is null)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrNoMedia, null));
        if (!card.HasChip)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrInvalidMedia, null));

        var on = power != IdcConstants.ChipPowerOff;
        device.SetChipPower(on);
        if (!on)
            return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, null));

        var builder = _builders.Create();
        var root = builder.Reserve(ChipPowerOutLayout.Size);
        builder.WriteUInt32(root + ChipPowerOutLayout.DataLength, (uint)card.Atr!.Length);
        builder.WritePointer(root + ChipPowerOutLayout.DataPointer, builder.AddBytes(card.Atr));
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, builder.Build()));
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
    private readonly INativeBufferBuilderFactory _builders;

    public ChipIoCommand(IIdcDeviceRegistry devices, INativeBufferBuilderFactory builders)
    {
        _devices = devices;
        _builders = builders;
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

        var device = _devices.Get(context.RemoteAddress);
        var card = device.CardInside;
        if (card is null)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrNoMedia, null));
        if (!card.HasChip)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrInvalidMedia, null));
        if (device.Chip != ChipState.Online)
            return Task.FromResult(new CommandOutcome(IdcConstants.ErrAtrNotObtained, null));

        var response = card.RespondTo(apdu);
        var builder = _builders.Create();
        var root = builder.Reserve(ChipIoLayout.Size);
        builder.WriteUInt16(root + ChipIoLayout.Protocol, protocol);
        builder.WriteUInt32(root + ChipIoLayout.DataLength, (uint)response.Length);
        builder.WritePointer(root + ChipIoLayout.DataPointer, builder.AddBytes(response));
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, builder.Build()));
    }
}
