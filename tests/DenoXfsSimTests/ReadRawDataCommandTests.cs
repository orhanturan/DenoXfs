using System.Buffers.Binary;
using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Commands;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Tests;

public class ReadRawDataCommandTests
{
    private const int PtrSize = NativeBufferBuilderFactory.DefaultPtrSize;
    private const string Address = "127.0.0.1";
    private readonly IdcTestRig _rig = new();

    private ExecuteContext Context(int readData) =>
        new(Address, IdcConstants.ReadRawDataCommand, JsonDocument.Parse($"{{\"readData\":{readData}}}").RootElement, _rig.ExecuteEvents);

    private ReadRawDataCommand Command() => new(new NativeBufferBuilderFactory(), _rig.Service, _rig.Devices);

    [Fact]
    public async Task Zero_read_data_returns_success_without_buffer()
    {
        var outcome = await Command().ExecuteAsync(Context(0), CancellationToken.None);

        Assert.Equal(XfsResultCodes.Success, outcome.HResult);
        Assert.Null(outcome.Result);
    }

    [Fact]
    public async Task Track2_and_chip_are_laid_out_as_a_null_terminated_pointer_array_of_card_data()
    {
        _rig.Insert(Address, "Visa chip + magstripe");
        var outcome = await Command().ExecuteAsync(Context(IdcConstants.Track2 | IdcConstants.Chip), CancellationToken.None);
        var buffer = outcome.Result!;
        var data = Convert.FromBase64String(buffer.Data);

        // Root: two pointers followed by the NULL terminator.
        Assert.Equal(PtrSize, buffer.PtrSize);
        Assert.True(data.AsSpan(2 * PtrSize, PtrSize).ToArray().All(b => b == 0));
        var targets = buffer.Fixups.ToDictionary(f => f[0], f => f[1]);

        AssertCardData(data, targets, targets[0], IdcConstants.Track2, "4111111111111111=25122010000000000000"u8.ToArray());
        AssertCardData(data, targets, targets[PtrSize], IdcConstants.Chip, FakeCardCatalog.TestAtr);
        Assert.Equal(Idc.Device.ChipState.Online, _rig.Devices.Get(Address).Chip);
    }

    [Fact]
    public async Task Track_the_card_does_not_have_is_reported_as_data_missing()
    {
        _rig.Insert(Address, "Visa chip + magstripe");
        var outcome = await Command().ExecuteAsync(Context(IdcConstants.Track3), CancellationToken.None);
        var data = Convert.FromBase64String(outcome.Result!.Data);
        var item = outcome.Result.Fixups.Single(f => f[0] == 0)[1];

        Assert.Equal(IdcConstants.Track3, BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(item + CardDataLayout.DataSourceOffset)));
        Assert.Equal(IdcConstants.DataMissing, BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(item + CardDataLayout.StatusOffset)));
        Assert.Equal(0u, BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(item + CardDataLayout.DataLengthOffset)));
    }

    [Fact]
    public async Task Empty_reader_sends_insert_card_waits_for_the_web_page_then_sends_media_inserted()
    {
        var read = Command().ExecuteAsync(Context(IdcConstants.Track2), CancellationToken.None);
        Assert.False(read.IsCompleted);
        Assert.Equal(new[] { IdcConstants.InsertCardEvent }, _rig.ExecuteEventIds);

        _rig.Insert(Address, "Magstripe only (3 tracks)");
        var outcome = await read;

        Assert.Equal(XfsResultCodes.Success, outcome.HResult);
        Assert.Equal(new[] { IdcConstants.InsertCardEvent, IdcConstants.MediaInsertedEvent }, _rig.ExecuteEventIds);
    }

    [Fact]
    public async Task Waiting_read_ends_when_the_request_times_out()
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Command().ExecuteAsync(Context(IdcConstants.Track2), timeout.Token));
        Assert.False(_rig.Devices.Get(Address).Snapshot().WaitingForCard);
    }

    private static void AssertCardData(byte[] data, Dictionary<int, int> targets, int item, ushort source, byte[] expected)
    {
        Assert.Equal(source, BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(item + CardDataLayout.DataSourceOffset)));
        Assert.Equal(IdcConstants.DataOk, BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(item + CardDataLayout.StatusOffset)));
        var length = (int)BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(item + CardDataLayout.DataLengthOffset));
        Assert.Equal(expected.Length, length);

        var payload = targets[item + CardDataLayout.DataPointerOffset];
        Assert.Equal(expected, data.AsSpan(payload, length).ToArray());
        Assert.Equal(0, data[payload + length]); // single terminating null
    }
}
