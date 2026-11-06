using System.Text.Json;
using System.Text.Json.Nodes;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Commands;
using DenoXfs.Sim.Idc.Forms;
using DenoXfs.Sim.Idc.Infos;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Tests;

public class NativeLayoutWriterTests
{
    private const string Address = "127.0.0.1";
    private readonly NativeLayoutWriter _writer = new(new NativeBufferBuilderFactory());

    [Fact]
    public void Struct_sizes_match_the_compiler_layout_of_XFSIDC_H()
    {
        Assert.Equal(164, IdcStructs.Status.Size);
        Assert.Equal(188, IdcStructs.Caps.Size);
        Assert.Equal(44, IdcStructs.Form.Size);
        Assert.Equal(14, IdcStructs.CardData.Size);
        Assert.Equal(4, IdcStructs.RetainCard.Size);
        Assert.Equal(10, IdcStructs.ChipIo.Size);
        Assert.Equal(8, IdcStructs.ChipPowerOut.Size);
        Assert.Equal(6, IdcStructs.IfmIdentifier.Size);
        Assert.Equal(8, IdcStructs.HexData.Size);
        Assert.Equal(8, IdcStructs.AppData.Size);
        Assert.Equal(10, IdcStructs.TrackEvent.Size);
        Assert.Equal(158, IdcStructs.Status["lpwParkingStationMedia"].Offset);
        Assert.Equal(184, IdcStructs.Caps["lpdwSynchronizableCommands"].Offset);
        Assert.Equal(43, IdcStructs.Form["cFieldSeparatorJIS1Track3"].Offset);
    }

    /// <summary>
    /// The blocks the hand written builders produced before the layout writer existed (data base64 | fixups). The writer
    /// must give the same bytes and fixups.
    /// </summary>
    [Fact]
    public async Task Idc_results_are_byte_identical_to_the_former_hand_written_builders()
    {
        var rig = new IdcTestRig();
        var ct = CancellationToken.None;
        InfoContext Info(uint category, string json = "{}") => new(Address, TestDevices.Of(Address), category, JsonDocument.Parse(json).RootElement);
        ExecuteContext Execute(uint command, string json = "{}") => new(Address, TestDevices.Of(Address), "Idc340", command, JsonDocument.Parse(json).RootElement, rig.ExecuteEvents);

        Golden("AAACAAEAAQAAAAUAAAAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQABAAEABAAEAAMAAAAAAAAAAAA=|",
            (await new StatusInfo(rig.Devices, rig.Profiles, rig.Merger).GetInfoAsync(Info(IdcConstants.StatusCategory), ct)).Output);
        Golden("AgABAAAAAAAHAAcAAwAyAAEAAQABAAAAAAAAAAAADgAOAAAAAAAAAAAAAACdBwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAQAAAAAAAAAAAAAAAAA=|",
            (await new CapabilitiesInfo(rig.Devices, rig.Profiles, rig.Merger).GetInfoAsync(Info(IdcConstants.CapabilitiesCategory), ct)).Output);
        Golden("UkVBRFRSQUNLMgBSRUFEVFJBQ0sxT1JUUkFDSzIAUkVBREFMTFRSQUNLUwBXUklURVRSQUNLMwAA|",
            (await new FormListInfo(new FakeFormCatalog()).GetInfoAsync(Info(IdcConstants.FormListCategory), ct)).Output);
        Golden("AAAAAF49PQEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABSRUFEVFJBQ0sxT1JUUkFDSzIAVFJBQ0sxfFRSQUNLMgBQQU4ATkFNRQBFWFBJUlkAAFBBTgBFWFBJUlkAAAA=|0,44;9,63;17,77;21,94;25,106",
            (await new QueryFormInfo(new FakeFormCatalog()).GetInfoAsync(Info(IdcConstants.QueryFormCategory, "{\"formName\":\"READTRACK1ORTRACK2\"}"), ct)).Output);
        Golden("AAAAAAAAAAABAAAAAABERU5PWEZTLVNJTS1JRk0tMDAwMQA=|0,8;10,14",
            (await new QueryIfmIdentifierInfo().GetInfoAsync(Info(IdcConstants.QueryIfmIdentifierCategory), ct)).Output);
        Golden("AAAAAAAAAAAAAAAAAAAAAAAAAAAHAAAAAAAAAKAAAAADEBABAAAAAAAAAAMAAAAAAAAAAAcAAAAAAAAAoAAAAAQQEAEAAAAAAAAAAg==|0,12;24,28;12,20;39,43;16,35;4,44;56,60;44,52;71,75;48,67",
            (await new EmvclessQueryApplicationsInfo().GetInfoAsync(Info(IdcConstants.EmvclessQueryApplicationsCategory), ct)).Output);

        rig.Insert(Address, "Visa chip + magstripe");
        Golden("AAAAAAAAAAAAAAAAAAAAAAAAAAABAAAAMAAAAAAAAAAAAEI0MTExMTExMTExMTExMTExXlRFU1QvQ0FSRF4yNTEyMjAxMDAwMDAwMDAwMDAwMAACAAAAJQAAAAAAAAAAADQxMTExMTExMTExMTExMTE9MjUxMjIwMTAwMDAwMDAwMDAwMDAABAABAAAAAAAAAAAAAAAIAAAAFAAAAAAAAAAAADuPgAGATwygAAADBgMAAQAAAABqAA==|0,20;28,34;4,83;91,97;8,135;12,149;157,163",
            (await new ReadRawDataCommand(rig.Service, rig.Devices).ExecuteAsync(Execute(IdcConstants.ReadRawDataCommand, "{\"readData\":15}"), ct)).Output);
        Golden("FAAAAAAAAAA7j4ABgE8MoAAAAwYDAAEAAAAAag==|4,8",
            (await new ChipPowerCommand(rig.Devices).ExecuteAsync(Execute(IdcConstants.ChipPowerCommand, "{\"chipPower\":2}"), ct)).Output);
        var apdu = Convert.ToBase64String(Convert.FromHexString("00A404000E315041592E5359532E444446303100"));
        Golden("AQAeAAAAAAAAAG8ahA4xUEFZLlNZUy5EREYwMaUIiAECXy0CZW6QAA==|6,10",
            (await new ChipIoCommand(rig.Devices).ExecuteAsync(Execute(IdcConstants.ChipIoCommand, $"{{\"chipProtocol\":1,\"chipData\":\"{apdu}\"}}"), ct)).Output);
        var readTrack = new ReadTrackCommand(new FakeFormCatalog(), rig.Service, new TrackDataFormatter(), new FormReadResult());
        Golden("VFJBQ0sxOkFMTD1CNDExMTExMTExMTExMTExMV5URVNUL0NBUkReMjUxMjIwMTAwMDAwMDAwMDAwMDAAAFRSQUNLMjpBTEw9NDExMTExMTExMTExMTExMT0yNTEyMjAxMDAwMDAwMDAwMDAwMAAAAA==|",
            (await readTrack.ExecuteAsync(Execute(IdcConstants.ReadTrackCommand, "{\"formName\":\"READALLTRACKS\"}"), ct)).Output);
        Golden("AQAAAAAAAAAAAFRSQUNLMwA=|2,10", rig.ExecuteEvents.Sent.Last(e => e.EventId == IdcConstants.InvalidTrackDataEvent).Payload);
        Golden("AQABAA==|", (await new RetainCardCommand(rig.Service).ExecuteAsync(Execute(IdcConstants.RetainCardCommand), ct)).Output);

        rig.Insert(Address, "Magstripe only (3 tracks)");
        await rig.Service.ResetAsync(TestDevices.Of(Address), IdcConstants.ResetRetain, ct);
        Golden("AQA=|", rig.Publisher.Published.Last(e => e.EventId == IdcConstants.MediaDetectedEvent).Payload);
        await new PowerSaveControlCommand(rig.Devices, rig.Publisher).ExecuteAsync(Execute(IdcConstants.PowerSaveControlCommand, "{\"maxPowerSaveRecoveryTime\":30}"), ct);
        Golden("HgA=|", rig.Publisher.Published.Last(e => e.EventId == IdcConstants.PowerSaveChangeEvent).Payload);
    }

    [Fact]
    public void Constant_names_short_or_full_and_flag_arrays_are_accepted()
    {
        var status = new JsonObject
        {
            ["fwDevice"] = "HWERROR",
            ["fwMedia"] = "WFS_IDC_MEDIAJAMMED",
            ["dwGuidLights"] = new JsonArray(new JsonArray("SLOW_FLASH", "RED"))
        };

        var data = Bytes(_writer.Write(new NativeValue(IdcResults.Status, status))!);

        Assert.Equal(4, BitConverter.ToUInt16(data, IdcStructs.Status["fwDevice"].Offset));
        Assert.Equal(3, BitConverter.ToUInt16(data, IdcStructs.Status["fwMedia"].Offset));
        Assert.Equal(0x104u, BitConverter.ToUInt32(data, IdcStructs.Status["dwGuidLights"].Offset));
    }

    [Fact]
    public void Byte_length_is_filled_from_the_data_unless_the_value_gives_it()
    {
        var entries = new JsonArray(
            new JsonObject { ["wDataSource"] = "TRACK2", ["wStatus"] = "OK", ["lpbData"] = new JsonObject { ["text"] = ";123=45?" } },
            new JsonObject { ["wDataSource"] = "TRACK3", ["wStatus"] = "INVALID", ["ulDataLength"] = 99, ["lpbData"] = "01 02" });

        var buffer = _writer.Write(new NativeValue(IdcResults.CardData, entries))!;
        var data = Bytes(buffer);
        var targets = buffer.Fixups.ToDictionary(f => f[0], f => f[1]);
        var length = IdcStructs.CardData["ulDataLength"].Offset;

        Assert.Equal(8u, BitConverter.ToUInt32(data, targets[0] + length));
        Assert.Equal(99u, BitConverter.ToUInt32(data, targets[4] + length));
        Assert.Equal(2, BitConverter.ToUInt16(data, targets[4] + IdcStructs.CardData["wStatus"].Offset));
    }

    [Theory]
    [InlineData("{\"fwDevise\":0}", "WFSIDCSTATUS.fwDevise")]
    [InlineData("{\"fwDevice\":\"BROKEN\"}", "WFSIDCSTATUS.fwDevice")]
    [InlineData("{\"fwDevice\":70000}", "WFSIDCSTATUS.fwDevice")]
    [InlineData("{\"dwGuidLights\":\"x\"}", "WFSIDCSTATUS.dwGuidLights")]
    public void Values_that_do_not_fit_the_layout_are_rejected_with_their_path(string json, string path)
    {
        var error = Assert.Throws<NativeValueException>(() => _writer.Write(new NativeValue(IdcResults.Status, JsonNode.Parse(json))));

        Assert.Equal(path, error.Path);
    }

    [Fact]
    public void Null_value_means_no_result_block()
    {
        Assert.Null(_writer.Write(null));
        Assert.Null(_writer.Write(new NativeValue(IdcResults.Status, null)));
    }

    private void Golden(string expected, NativeValue? value)
    {
        var buffer = _writer.Write(value)!;
        Assert.Equal(expected, $"{buffer.Data}|{string.Join(";", buffer.Fixups.Select(f => $"{f[0]},{f[1]}"))}");
    }

    private static byte[] Bytes(ResultBuffer buffer) => Convert.FromBase64String(buffer.Data);
}
