using System.ComponentModel;
using System.Runtime.InteropServices;
using IdcConstants = DenoXfs.Idc.Idc;
using DenoXfs.Idc.Shared;
using DenoXfs.Interop.Convert;

namespace DenoXfs.TestTool.Operations.Idc;

public class ReadRawDataParameters
{
    [DisplayName("Read"), Description("Data to read (lpwReadData). Zero moves a previously ejected card back inside.")]
    public IdcDataSource Read { get; set; } = IdcDataSource.Track2 | IdcDataSource.Chip;
}

/// <summary>One WFSIDCCARDDATA, copied out of the native buffer. Text is the data as ASCII (tracks), Hex the same bytes.</summary>
public sealed record CardDataView(IdcDataSource Source, IdcDataStatus Status, uint Length, string Text, string Hex);

/// <summary>WFS_CMD_IDC_READ_RAW_DATA: input is a WORD of data flags, output a NULL terminated array of WFSIDCCARDDATA pointers.</summary>
public class IdcReadRawDataOperation : OperationDefinition<ReadRawDataParameters>
{
    public override string Name => "IDC Read raw data";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcReadRawData;

    public override string Description => "WFS_CMD_IDC_READ_RAW_DATA: reads tracks and the chip ATR of the card in the reader.";

    protected override Task<RawResponse> SendAsync(ReadRawDataParameters parameters, IOperationInvoker invoker) =>
        NativeInput.WithWordAsync((ushort)parameters.Read, input => invoker.ExecuteAsync(Code, input));

    protected override object? Decode(IntPtr buffer)
    {
        var cards = PointerArray.ToStructs<XfsIdcCardData>(buffer) ?? [];
        return cards
            .Where(card => card.HasValue)
            .Select(card =>
            {
                var data = card!.Value.Data ?? [];
                return new CardDataView(card.Value.DataSource, card.Value.Status, card.Value.DataLength,
                    ObjectDumper.Ascii(data), ObjectDumper.Hex(data));
            })
            .ToList();
    }
}
