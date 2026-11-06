using System.Text.Json;
using System.Text.Json.Nodes;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Commands;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Tests;

public class ReadRawDataCommandTests
{
    private const string Address = "127.0.0.1";
    private readonly IdcTestRig _rig = new();

    private ExecuteContext Context(int readData) =>
        new(Address, TestDevices.Of(Address), "Idc340", IdcConstants.ReadRawDataCommand, JsonDocument.Parse($"{{\"readData\":{readData}}}").RootElement, _rig.ExecuteEvents);

    private ReadRawDataCommand Command() => new(_rig.Service, _rig.Devices);

    [Fact]
    public async Task Zero_read_data_returns_success_without_output()
    {
        var outcome = await Command().ExecuteAsync(Context(0), CancellationToken.None);

        Assert.Equal(XfsResultCodes.Success, outcome.HResult);
        Assert.Null(outcome.Output);
    }

    [Fact]
    public async Task Track2_and_chip_are_returned_as_card_data_entries()
    {
        _rig.Insert(Address, "Visa chip + magstripe");
        var outcome = await Command().ExecuteAsync(Context(IdcConstants.Track2 | IdcConstants.Chip), CancellationToken.None);

        Assert.Same(IdcResults.CardData, outcome.Output!.Layout);
        var entries = Assert.IsType<JsonArray>(outcome.Output.Value);
        Assert.Equal(2, entries.Count);
        AssertEntry(entries[0]!, IdcConstants.Track2, IdcConstants.DataOk, "4111111111111111=25122010000000000000"u8.ToArray());
        AssertEntry(entries[1]!, IdcConstants.Chip, IdcConstants.DataOk, FakeCardCatalog.TestAtr);
        Assert.Equal(Idc.Device.ChipState.Online, _rig.Devices.Get(TestDevices.Of(Address)).Chip);
    }

    [Fact]
    public async Task Track_the_card_does_not_have_is_reported_as_data_missing()
    {
        _rig.Insert(Address, "Visa chip + magstripe");
        var outcome = await Command().ExecuteAsync(Context(IdcConstants.Track3), CancellationToken.None);

        AssertEntry(Assert.Single(Assert.IsType<JsonArray>(outcome.Output!.Value))!, IdcConstants.Track3, IdcConstants.DataMissing, null);
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
        Assert.False(_rig.Devices.Get(TestDevices.Of(Address)).Snapshot().WaitingForCard);
    }

    private static void AssertEntry(JsonNode entry, ushort source, ushort status, byte[]? data)
    {
        Assert.Equal(source, entry["wDataSource"]!.GetValue<ushort>());
        Assert.Equal(status, entry["wStatus"]!.GetValue<ushort>());
        Assert.Equal(data is null ? null : Convert.ToHexString(data), entry["lpbData"]?.GetValue<string>());
    }
}
