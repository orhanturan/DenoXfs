using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Device;

namespace DenoXfs.Sim.Idc.Commands;

/// <summary>WFS_CMD_IDC_SETKEY: input WFSIDCSETKEY; success when a key is given (WFS_ERR_IDC_INVALIDKEY otherwise).</summary>
public sealed class SetkeyCommand : IExecuteCommand
{
    public uint Command => IdcConstants.SetkeyCommand;

    public Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var length = IdcInput.GetUShort(context.Input, "keyLength") ?? 0;
        var key = IdcInput.GetBytes(context.Input, "key");
        var valid = length > 0 && key is not null && key.Length == length;
        return Task.FromResult(new CommandOutcome(valid ? XfsResultCodes.Success : IdcConstants.ErrInvalidKey, null));
    }
}

/// <summary>WFS_CMD_IDC_SET_GUIDANCE_LIGHT: input WFSIDCSETGUIDLIGHT; only the card unit light exists, the web page shows it.</summary>
public sealed class SetGuidanceLightCommand : IExecuteCommand
{
    /// <summary>What the card unit light can do (dwGuidLights[WFS_IDC_GUIDANCE_CARDUNIT] in the capabilities).</summary>
    public const uint CardUnitCapabilities =
        IdcConstants.GuidanceOff | IdcConstants.GuidanceSlowFlash | IdcConstants.GuidanceMediumFlash | IdcConstants.GuidanceQuickFlash |
        IdcConstants.GuidanceContinuous | IdcConstants.GuidanceRed | IdcConstants.GuidanceGreen | IdcConstants.GuidanceYellow;

    private readonly IIdcDeviceRegistry _devices;

    public SetGuidanceLightCommand(IIdcDeviceRegistry devices)
    {
        _devices = devices;
    }

    public uint Command => IdcConstants.SetGuidanceLightCommand;

    public Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var light = IdcInput.GetUShort(context.Input, "guidLight");
        var command = IdcInput.GetUInt(context.Input, "command") ?? 0;
        if (light is null or > IdcConstants.GuidLightsMax || command == 0)
            return Task.FromResult(new CommandOutcome(XfsResultCodes.InvalidData, null));
        if (light != IdcConstants.GuidanceCardUnit || (command & ~CardUnitCapabilities) != 0)
            return Task.FromResult(new CommandOutcome(XfsResultCodes.UnsupportedData, null));

        _devices.Get(context.RemoteAddress).SetGuidanceLight(light.Value, command);
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, null));
    }
}

/// <summary>WFS_CMD_IDC_POWER_SAVE_CONTROL: input WFSIDCPOWERSAVECONTROL; success, WFS_SRVE_IDC_POWER_SAVE_CHANGE when the time changes.</summary>
public sealed class PowerSaveControlCommand : IExecuteCommand
{
    private readonly IIdcDeviceRegistry _devices;
    private readonly IEventPublisher _events;
    private readonly INativeBufferBuilderFactory _builders;

    public PowerSaveControlCommand(IIdcDeviceRegistry devices, IEventPublisher events, INativeBufferBuilderFactory builders)
    {
        _devices = devices;
        _events = events;
        _builders = builders;
    }

    public uint Command => IdcConstants.PowerSaveControlCommand;

    public async Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var seconds = IdcInput.GetUShort(context.Input, "maxPowerSaveRecoveryTime");
        if (seconds is null)
            return new CommandOutcome(XfsResultCodes.InvalidData, null);

        if (_devices.Get(context.RemoteAddress).SetPowerSaveRecoveryTime(seconds.Value))
        {
            var builder = _builders.Create();
            builder.WriteUInt16(builder.Reserve(sizeof(ushort)), seconds.Value);
            await _events.PublishAsync(context.RemoteAddress, XfsEventKind.Service, IdcConstants.PowerSaveChangeEvent, builder.Build(), cancellationToken);
        }
        return new CommandOutcome(XfsResultCodes.Success, null);
    }
}

/// <summary>WFS_CMD_IDC_PARK_CARD: input WFSIDCPARKCARD; success when the input is there (no parking stations are simulated).</summary>
public sealed class ParkCardCommand : IExecuteCommand
{
    public uint Command => IdcConstants.ParkCardCommand;

    public Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken) =>
        Task.FromResult(new CommandOutcome(
            IdcInput.GetUShort(context.Input, "direction") is null ? XfsResultCodes.InvalidData : XfsResultCodes.Success, null));
}

/// <summary>WFS_CMD_IDC_SYNCHRONIZE_COMMAND: not supported by this reader.</summary>
public sealed class SynchronizeCommand : IExecuteCommand
{
    public uint Command => IdcConstants.SynchronizeCommand;

    public Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken) =>
        Task.FromResult(new CommandOutcome(IdcConstants.ErrCommandUnsupp, null));
}
