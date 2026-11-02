using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Idc.Commands;

/// <summary>WFS_CMD_IDC_EJECT_CARD: input LPWFSIDCEJECTCARD (may be NULL); this reader only ejects to the exit position.</summary>
public sealed class EjectCardCommand : IExecuteCommand
{
    private readonly IIdcDeviceService _service;

    public EjectCardCommand(IIdcDeviceService service)
    {
        _service = service;
    }

    public uint Command => IdcConstants.EjectCardCommand;

    public async Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var position = IdcInput.GetUShort(context.Input, "ejectPosition") ?? IdcConstants.ExitPosition;
        if (position != IdcConstants.ExitPosition)
            return new CommandOutcome(XfsResultCodes.UnsupportedData, null);

        return new CommandOutcome(await _service.EjectAsync(context.RemoteAddress, cancellationToken), null);
    }
}

/// <summary>WFS_CMD_IDC_RETAIN_CARD: no input, output WFSIDCRETAINCARD (count and where the card was).</summary>
public sealed class RetainCardCommand : IExecuteCommand
{
    private readonly IIdcDeviceService _service;
    private readonly INativeBufferBuilderFactory _builders;

    public RetainCardCommand(IIdcDeviceService service, INativeBufferBuilderFactory builders)
    {
        _service = service;
        _builders = builders;
    }

    public uint Command => IdcConstants.RetainCardCommand;

    public async Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var result = await _service.RetainAsync(context.RemoteAddress, context.Events, cancellationToken);
        switch (result.Outcome)
        {
            case RetainOutcome.NoMedia:
                return new CommandOutcome(IdcConstants.ErrNoMedia, null);
            case RetainOutcome.BinFull:
                return new CommandOutcome(IdcConstants.ErrRetainBinFull, null);
        }

        var builder = _builders.Create();
        var root = builder.Reserve(RetainCardLayout.Size);
        builder.WriteUInt16(root + RetainCardLayout.Count, (ushort)result.Count);
        builder.WriteUInt16(root + RetainCardLayout.Position,
            result.Position == MediaPosition.AtExit ? IdcConstants.MediaEntering : IdcConstants.MediaPresent);
        return new CommandOutcome(XfsResultCodes.Success, builder.Build());
    }
}

/// <summary>WFS_CMD_IDC_RESET_COUNT: no input or output; empties the retain bin.</summary>
public sealed class ResetCountCommand : IExecuteCommand
{
    private readonly IIdcDeviceService _service;

    public ResetCountCommand(IIdcDeviceService service)
    {
        _service = service;
    }

    public uint Command => IdcConstants.ResetCountCommand;

    public async Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        await _service.ResetCountAsync(context.RemoteAddress, cancellationToken);
        return new CommandOutcome(XfsResultCodes.Success, null);
    }
}

/// <summary>WFS_CMD_IDC_RESET: input LPWORD lpwResetIn (NULL = no action), no output.</summary>
public sealed class ResetCommand : IExecuteCommand
{
    private readonly IIdcDeviceService _service;

    public ResetCommand(IIdcDeviceService service)
    {
        _service = service;
    }

    public uint Command => IdcConstants.ResetCommand;

    public async Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var action = IdcInput.GetUShort(context.Input, "resetIn") ?? IdcConstants.ResetNoAction;
        if (action is not (IdcConstants.ResetNoAction or IdcConstants.ResetEject or IdcConstants.ResetRetain))
            return new CommandOutcome(XfsResultCodes.InvalidData, null);

        return new CommandOutcome(await _service.ResetAsync(context.RemoteAddress, action, cancellationToken), null);
    }
}
