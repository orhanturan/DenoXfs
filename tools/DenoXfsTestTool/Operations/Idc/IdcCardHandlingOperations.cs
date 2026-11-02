using System.ComponentModel;
using System.Runtime.InteropServices;
using IdcConstants = DenoXfs.Idc.Idc;
using DenoXfs.Idc.Commands;
using DenoXfs.Idc.Infos;
using DenoXfs.Idc.Shared;

namespace DenoXfs.TestTool.Operations.Idc;

public class EjectCardParameters
{
    [DisplayName("Eject position"), Description("wEjectPosition of WFSIDCEJECTCARD.")]
    public IdcEjectPosition EjectPosition { get; set; } = IdcEjectPosition.ExitPosition;
}

/// <summary>WFS_CMD_IDC_EJECT_CARD: input LPWFSIDCEJECTCARD, no output.</summary>
public class IdcEjectCardOperation : OperationDefinition<EjectCardParameters>
{
    public override string Name => "IDC Eject card";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcEjectCard;

    public override string Description => "WFS_CMD_IDC_EJECT_CARD: moves the card to the exit; MEDIAREMOVED follows when it is taken.";

    protected override Task<RawResponse> SendAsync(EjectCardParameters parameters, IOperationInvoker invoker) =>
        NativeInput.WithWordAsync((ushort)parameters.EjectPosition, input => invoker.ExecuteAsync(Code, input));

    protected override object? Decode(IntPtr buffer) => null;
}

/// <summary>WFS_CMD_IDC_RETAIN_CARD: no input, output WFSIDCRETAINCARD.</summary>
public class IdcRetainCardOperation : OperationDefinition<NoParameters>
{
    public override string Name => "IDC Retain card";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcRetainCard;

    public override string Description => "WFS_CMD_IDC_RETAIN_CARD: card into the retain bin (MEDIARETAINED, RETAINBINTHRESHOLD).";

    protected override Task<RawResponse> SendAsync(NoParameters parameters, IOperationInvoker invoker) =>
        invoker.ExecuteAsync(Code, IntPtr.Zero);

    protected override object? Decode(IntPtr buffer) => Marshal.PtrToStructure<XfsIdcRetainCard>(buffer);
}

/// <summary>WFS_CMD_IDC_RESET_COUNT: no input or output.</summary>
public class IdcResetCountOperation : OperationDefinition<NoParameters>
{
    public override string Name => "IDC Reset count";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcResetCount;

    public override string Description => "WFS_CMD_IDC_RESET_COUNT: sets the retained card count to zero.";

    protected override Task<RawResponse> SendAsync(NoParameters parameters, IOperationInvoker invoker) =>
        invoker.ExecuteAsync(Code, IntPtr.Zero);

    protected override object? Decode(IntPtr buffer) => null;
}

public class ResetParameters
{
    [DisplayName("Reset in"), Description("lpwResetIn: what to do with a card in the reader.")]
    public IdcPowerOptionCapability ResetIn { get; set; } = IdcPowerOptionCapability.NoAction;
}

/// <summary>WFS_CMD_IDC_RESET: input LPWORD lpwResetIn, no output.</summary>
public class IdcResetOperation : OperationDefinition<ResetParameters>
{
    public override string Name => "IDC Reset";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcReset;

    public override string Description => "WFS_CMD_IDC_RESET: resets the reader; a card in it is handled as asked (MEDIADETECTED).";

    protected override Task<RawResponse> SendAsync(ResetParameters parameters, IOperationInvoker invoker) =>
        NativeInput.WithWordAsync((ushort)parameters.ResetIn, input => invoker.ExecuteAsync(Code, input));

    protected override object? Decode(IntPtr buffer) => null;
}
