using System.Text;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Idc.Commands;

/// <summary>
/// WFS_CMD_IDC_READ_RAW_DATA. Waits for a card when the reader is empty (see <see cref="IIdcDeviceService.WaitForCardAsync"/>).
/// Output is a NULL terminated array of pointers to WFSIDCCARDDATA, one per requested data source: tracks as their
/// bytes, chip as ATR (the chip is then powered), absent data as DATAMISSING, sources this reader lacks as DATASRCNOTSUPP.
/// </summary>
public sealed class ReadRawDataCommand : IExecuteCommand
{
    // Requested bits that are data sources; FLUXINACTIVE and TRACK_WM are options, not sources.
    private static readonly ushort[] Sources =
    {
        IdcConstants.Track1, IdcConstants.Track2, IdcConstants.Track3, IdcConstants.Chip, IdcConstants.Security,
        IdcConstants.MemoryChip, IdcConstants.FrontTrack1, IdcConstants.FrontImage, IdcConstants.BackImage,
        IdcConstants.Track1Jis1, IdcConstants.Track3Jis1, IdcConstants.Ddi
    };

    private readonly INativeBufferBuilderFactory _builders;
    private readonly IIdcDeviceService _service;
    private readonly IIdcDeviceRegistry _devices;

    public ReadRawDataCommand(INativeBufferBuilderFactory builders, IIdcDeviceService service, IIdcDeviceRegistry devices)
    {
        _builders = builders;
        _service = service;
        _devices = devices;
    }

    public uint Command => IdcConstants.ReadRawDataCommand;

    public async Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var requested = IdcInput.GetUShort(context.Input, "readData") ?? 0;
        if (requested == 0)
            return new CommandOutcome(XfsResultCodes.Success, null);

        var card = await _service.WaitForCardAsync(context.RemoteAddress, context.Events, cancellationToken);
        var entries = Sources.Where(source => (requested & source) != 0).Select(source => Read(context.RemoteAddress, card, source)).ToList();
        return new CommandOutcome(XfsResultCodes.Success, Build(entries));
    }

    private (ushort Source, ushort Status, byte[]? Data) Read(string address, IdcCard card, ushort source)
    {
        switch (source)
        {
            case IdcConstants.Track1:
                return Track(source, card.Track1);
            case IdcConstants.Track2:
                return Track(source, card.Track2);
            case IdcConstants.Track3:
                return Track(source, card.Track3);
            case IdcConstants.Chip when card.HasChip:
                _devices.Get(address).SetChipPower(true);
                return (source, IdcConstants.DataOk, card.Atr);
            case IdcConstants.Chip:
                return (source, IdcConstants.DataMissing, null);
            default:
                return (source, IdcConstants.DataSrcNotSupp, null);
        }
    }

    private static (ushort, ushort, byte[]?) Track(ushort source, string? text) =>
        text is null ? (source, IdcConstants.DataMissing, null) : (source, IdcConstants.DataOk, Encoding.Latin1.GetBytes(text));

    private ResultBuffer Build(IReadOnlyList<(ushort Source, ushort Status, byte[]? Data)> entries)
    {
        var builder = _builders.Create();
        var ptrSize = NativeBufferBuilderFactory.DefaultPtrSize;
        var array = builder.Reserve((entries.Count + 1) * ptrSize);

        for (var i = 0; i < entries.Count; i++)
        {
            var (source, status, data) = entries[i];
            var item = builder.Reserve(CardDataLayout.Size);
            builder.WritePointer(array + i * ptrSize, item);
            builder.WriteUInt16(item + CardDataLayout.DataSourceOffset, source);
            builder.WriteUInt16(item + CardDataLayout.StatusOffset, status);
            if (data is null)
                continue;

            builder.WriteUInt32(item + CardDataLayout.DataLengthOffset, (uint)data.Length);
            var payload = builder.AddBytes(data);
            builder.AddBytes(new byte[] { 0 }); // lpbData is terminated with a single null character
            builder.WritePointer(item + CardDataLayout.DataPointerOffset, payload);
        }

        return builder.Build();
    }
}
