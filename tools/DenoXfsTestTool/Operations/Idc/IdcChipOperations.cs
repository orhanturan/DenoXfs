using System.ComponentModel;
using System.Runtime.InteropServices;
using IdcConstants = DenoXfs.Idc.Idc;
using DenoXfs.Idc.Commands;
using DenoXfs.Idc.Shared;

namespace DenoXfs.TestTool.Operations.Idc;

/// <summary>Values of lpwChipPower (one at a time).</summary>
public enum ChipPowerAction : ushort
{
    Cold = 0x0002,
    Warm = 0x0004,
    Off = 0x0008
}

/// <summary>Values of wChipProtocol for a contact chip.</summary>
public enum ChipProtocol : ushort
{
    T0 = 0x0001,
    T1 = 0x0002
}

public class ChipPowerParameters
{
    [DisplayName("Chip power"), Description("lpwChipPower: cold or warm reset returns the ATR, off powers the chip down.")]
    public ChipPowerAction ChipPower { get; set; } = ChipPowerAction.Cold;
}

/// <summary>WFS_CMD_IDC_CHIP_POWER: input LPWORD lpwChipPower, output WFSIDCCHIPPOWEROUT (ATR).</summary>
public class IdcChipPowerOperation : OperationDefinition<ChipPowerParameters>
{
    public override string Name => "IDC Chip power";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcChipPower;

    public override string Description => "WFS_CMD_IDC_CHIP_POWER: powers the chip of the card inside.";

    protected override Task<RawResponse> SendAsync(ChipPowerParameters parameters, IOperationInvoker invoker) =>
        NativeInput.WithWordAsync((ushort)parameters.ChipPower, input => invoker.ExecuteAsync(Code, input));

    protected override object? Decode(IntPtr buffer) => Marshal.PtrToStructure<XfsIdcChipPowerOut>(buffer);
}

public class ChipIoParameters
{
    [DisplayName("Protocol"), Description("wChipProtocol.")]
    public ChipProtocol Protocol { get; set; } = ChipProtocol.T0;

    [DisplayName("Command APDU (hex)"), Description("lpbChipData, spaces allowed. Default: SELECT PSE 1PAY.SYS.DDF01.")]
    public string Apdu { get; set; } = "00 A4 04 00 0E 31 50 41 59 2E 53 59 53 2E 44 44 46 30 31 00";
}

/// <summary>WFS_CMD_IDC_CHIP_IO: input and output WFSIDCCHIPIO (pack 1: protocol at 0, length at 2, data pointer at 6).</summary>
public class IdcChipIoOperation : OperationDefinition<ChipIoParameters>
{
    private const int ChipIoSize = 2 + 4 + 4;

    public override string Name => "IDC Chip IO";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcChipIo;

    public override string Description => "WFS_CMD_IDC_CHIP_IO: sends an APDU to the powered chip and returns its response.";

    protected override async Task<RawResponse> SendAsync(ChipIoParameters parameters, IOperationInvoker invoker)
    {
        var apdu = Convert.FromHexString(parameters.Apdu.Replace(" ", string.Empty));
        var data = Marshal.AllocHGlobal(Math.Max(apdu.Length, 1));
        try
        {
            Marshal.Copy(apdu, 0, data, apdu.Length);
            return await NativeInput.WithBlockAsync(ChipIoSize, block =>
            {
                Marshal.WriteInt16(block, 0, unchecked((short)(ushort)parameters.Protocol));
                Marshal.WriteInt32(block, 2, apdu.Length);
                Marshal.WriteIntPtr(block, 6, data);
            }, input => invoker.ExecuteAsync(Code, input));
        }
        finally
        {
            Marshal.FreeHGlobal(data);
        }
    }

    protected override object? Decode(IntPtr buffer) => Marshal.PtrToStructure<XfsIdcChipIo>(buffer);
}
