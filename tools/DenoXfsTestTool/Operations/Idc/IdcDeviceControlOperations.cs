using System.ComponentModel;
using System.Runtime.InteropServices;
using IdcConstants = DenoXfs.Idc.Idc;
using DenoXfs.Idc.Commands;
using DenoXfs.Idc.Shared;

namespace DenoXfs.TestTool.Operations.Idc;

public class SetkeyParameters
{
    [DisplayName("Key (hex)"), Description("lpbKeyValue, spaces allowed; usKeyLen is its length.")]
    public string Key { get; set; } = "01 23 45 67 89 AB CD EF";
}

/// <summary>WFS_CMD_IDC_SETKEY: input WFSIDCSETKEY (pack 1: usKeyLen 0, lpbKeyValue 2).</summary>
public class IdcSetkeyOperation : OperationDefinition<SetkeyParameters>
{
    public override string Name => "IDC Set key";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcSetkey;

    public override string Description => "WFS_CMD_IDC_SETKEY: loads the key of the security module.";

    protected override async Task<RawResponse> SendAsync(SetkeyParameters parameters, IOperationInvoker invoker)
    {
        var key = Convert.FromHexString(parameters.Key.Replace(" ", string.Empty));
        using var scope = new NativeScope();
        var input = scope.Block(6);
        Marshal.WriteInt16(input, 0, (short)key.Length);
        Marshal.WriteIntPtr(input, 2, scope.Bytes(key));
        return await invoker.ExecuteAsync(Code, input);
    }

    protected override object? Decode(IntPtr buffer) => null;
}

public class SetGuidanceLightParameters
{
    [DisplayName("Light"), Description("wGuidLight: index in dwGuidLights (0 = card unit).")]
    public ushort Light { get; set; } = (ushort)IdcConstants.GuidanceCardUnit;

    [DisplayName("Command"), Description("dwCommand: one flash mode plus one colour, or Off.")]
    public IdcGuidLight Command { get; set; } = IdcGuidLight.QuickFlash | IdcGuidLight.Green;
}

/// <summary>WFS_CMD_IDC_SET_GUIDANCE_LIGHT: input WFSIDCSETGUIDLIGHT (pack 1: wGuidLight 0, dwCommand 2).</summary>
public class IdcSetGuidanceLightOperation : OperationDefinition<SetGuidanceLightParameters>
{
    public override string Name => "IDC Guidance light";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcSetGuidanceLight;

    public override string Description => "WFS_CMD_IDC_SET_GUIDANCE_LIGHT: sets the card unit light (shown on the simulator page).";

    protected override async Task<RawResponse> SendAsync(SetGuidanceLightParameters parameters, IOperationInvoker invoker)
    {
        using var scope = new NativeScope();
        var input = scope.Block(6);
        Marshal.WriteInt16(input, 0, (short)parameters.Light);
        Marshal.WriteInt32(input, 2, unchecked((int)(uint)parameters.Command));
        return await invoker.ExecuteAsync(Code, input);
    }

    protected override object? Decode(IntPtr buffer) => null;
}

public class PowerSaveControlParameters
{
    [DisplayName("Max recovery time (s)"), Description("usMaxPowerSaveRecoveryTime.")]
    public ushort MaxRecoveryTime { get; set; } = 5;
}

/// <summary>WFS_CMD_IDC_POWER_SAVE_CONTROL: input WFSIDCPOWERSAVECONTROL.</summary>
public class IdcPowerSaveControlOperation : OperationDefinition<PowerSaveControlParameters>
{
    public override string Name => "IDC Power save";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcPowerSaveControl;

    public override string Description => "WFS_CMD_IDC_POWER_SAVE_CONTROL: sets the power save recovery time (POWER_SAVE_CHANGE follows).";

    protected override Task<RawResponse> SendAsync(PowerSaveControlParameters parameters, IOperationInvoker invoker) =>
        NativeInput.WithWordAsync(parameters.MaxRecoveryTime, input => invoker.ExecuteAsync(Code, input));

    protected override object? Decode(IntPtr buffer) => null;
}

public class ParkCardParameters
{
    [DisplayName("Direction"), Description("wDirection.")]
    public IdcDirectionParkCard Direction { get; set; } = IdcDirectionParkCard.In;

    [DisplayName("Parking station"), Description("usParkingStation.")]
    public ushort ParkingStation { get; set; } = 1;
}

/// <summary>WFS_CMD_IDC_PARK_CARD: input WFSIDCPARKCARD (pack 1: wDirection 0, usParkingStation 2).</summary>
public class IdcParkCardOperation : OperationDefinition<ParkCardParameters>
{
    public override string Name => "IDC Park card";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcParkCard;

    public override string Description => "WFS_CMD_IDC_PARK_CARD: moves a card into or out of a parking station.";

    protected override Task<RawResponse> SendAsync(ParkCardParameters parameters, IOperationInvoker invoker) =>
        NativeInput.WithBlockAsync(4, block =>
        {
            Marshal.WriteInt16(block, 0, (short)parameters.Direction);
            Marshal.WriteInt16(block, 2, (short)parameters.ParkingStation);
        }, input => invoker.ExecuteAsync(Code, input));

    protected override object? Decode(IntPtr buffer) => null;
}

public class SynchronizeParameters
{
    [DisplayName("Command"), Description("dwCommand to synchronize (lpCmdData is NULL).")]
    public uint Command { get; set; } = (uint)IdcConstants.CmdIdcReadRawData;
}

/// <summary>WFS_CMD_IDC_SYNCHRONIZE_COMMAND: input WFSIDCSYNCHRONIZECOMMAND (pack 1: dwCommand 0, lpCmdData 4).</summary>
public class IdcSynchronizeOperation : OperationDefinition<SynchronizeParameters>
{
    public override string Name => "IDC Synchronize";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcSynchronizeCommand;

    public override string Description => "WFS_CMD_IDC_SYNCHRONIZE_COMMAND: not supported by the simulator (COMMANDUNSUPP).";

    protected override Task<RawResponse> SendAsync(SynchronizeParameters parameters, IOperationInvoker invoker) =>
        NativeInput.WithBlockAsync(8, block => Marshal.WriteInt32(block, 0, unchecked((int)parameters.Command)),
            input => invoker.ExecuteAsync(Code, input));

    protected override object? Decode(IntPtr buffer) => null;
}

/// <summary>Contactless commands: sent with NULL input; the simulator does not support contactless.</summary>
public class IdcContactlessOperation : OperationDefinition<NoParameters>
{
    private readonly string _name;
    private readonly string _description;

    private IdcContactlessOperation(string name, uint code, string description)
    {
        _name = name;
        Code = code;
        _description = description;
    }

    public static IdcContactlessOperation Configure() => new("IDC Contactless configure",
        (uint)IdcConstants.CmdIdcEmvclessConfigure, "WFS_CMD_IDC_EMVCLESS_CONFIGURE (NULL input): contactless is not supported.");

    public static IdcContactlessOperation PerformTransaction() => new("IDC Contactless transaction",
        (uint)IdcConstants.CmdIdcEmvclessPerformTransaction, "WFS_CMD_IDC_EMVCLESS_PERFORM_TRANSACTION (NULL input): contactless is not supported.");

    public static IdcContactlessOperation IssuerUpdate() => new("IDC Contactless issuer update",
        (uint)IdcConstants.CmdIdcEmvclessIssuerupdate, "WFS_CMD_IDC_EMVCLESS_ISSUERUPDATE (NULL input): contactless is not supported.");

    public override string Name => _name;

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code { get; }

    public override string Description => _description;

    protected override Task<RawResponse> SendAsync(NoParameters parameters, IOperationInvoker invoker) =>
        invoker.ExecuteAsync(Code, IntPtr.Zero);

    protected override object? Decode(IntPtr buffer) => null;
}
