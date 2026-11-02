using System.ComponentModel;
using System.Runtime.InteropServices;
using IdcConstants = DenoXfs.Idc.Idc;
using DenoXfs.Idc.Shared;

namespace DenoXfs.TestTool.Operations.Idc;

/// <summary>WFS_CMD_IDC_READ_TRACK: input LPSTR lpstrFormName, output LPSTR lpstrTrackData.</summary>
public class IdcReadTrackOperation : OperationDefinition<FormNameParameters>
{
    public override string Name => "IDC Read track";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcReadTrack;

    public override string Description => "WFS_CMD_IDC_READ_TRACK: reads the card through a form (waits for a card like READ_RAW_DATA).";

    protected override async Task<RawResponse> SendAsync(FormNameParameters parameters, IOperationInvoker invoker)
    {
        using var scope = new NativeScope();
        return await invoker.ExecuteAsync(Code, scope.Ansi(parameters.FormName));
    }

    protected override object? Decode(IntPtr buffer) => NativeText.TrackData(buffer);
}

public class WriteTrackParameters
{
    [DisplayName("Form name"), Description("lpstrFormName: a write form (see IDC Form list).")]
    public string FormName { get; set; } = "WRITETRACK3";

    [DisplayName("Track data"), Description("lpstrTrackData.")]
    public string TrackData { get; set; } = "ALL=016045551234567890=724724100000000000030300";

    [DisplayName("Write method"), Description("fwWriteMethod.")]
    public IdcWriteMethod WriteMethod { get; set; } = IdcWriteMethod.Auto;
}

/// <summary>WFS_CMD_IDC_WRITE_TRACK: input WFSIDCWRITETRACK (pack 1: form name 0, track data 4, write method 8).</summary>
public class IdcWriteTrackOperation : OperationDefinition<WriteTrackParameters>
{
    public override string Name => "IDC Write track";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcWriteTrack;

    public override string Description => "WFS_CMD_IDC_WRITE_TRACK: writes a track through a form.";

    protected override async Task<RawResponse> SendAsync(WriteTrackParameters parameters, IOperationInvoker invoker)
    {
        using var scope = new NativeScope();
        var input = scope.Block(10);
        Marshal.WriteIntPtr(input, 0, scope.Ansi(parameters.FormName));
        Marshal.WriteIntPtr(input, 4, scope.Ansi(parameters.TrackData));
        Marshal.WriteInt16(input, 8, unchecked((short)(ushort)parameters.WriteMethod));
        return await invoker.ExecuteAsync(Code, input);
    }

    protected override object? Decode(IntPtr buffer) => null;
}

public class WriteRawDataParameters
{
    [DisplayName("Track"), Description("wDataSource: the track to write.")]
    public IdcDataSource Track { get; set; } = IdcDataSource.Track3;

    [DisplayName("Data"), Description("lpbData as ASCII text.")]
    public string Data { get; set; } = "016045551234567890=724724100000000000030300";

    [DisplayName("Write method"), Description("fwWriteMethod.")]
    public IdcWriteMethod WriteMethod { get; set; } = IdcWriteMethod.Auto;
}

/// <summary>WFS_CMD_IDC_WRITE_RAW_DATA: input NULL terminated array of LPWFSIDCCARDDATA.</summary>
public class IdcWriteRawDataOperation : OperationDefinition<WriteRawDataParameters>
{
    public override string Name => "IDC Write raw data";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcWriteRawData;

    public override string Description => "WFS_CMD_IDC_WRITE_RAW_DATA: writes raw track data.";

    protected override async Task<RawResponse> SendAsync(WriteRawDataParameters parameters, IOperationInvoker invoker)
    {
        using var scope = new NativeScope();
        var array = scope.CardDataArray([((ushort)parameters.Track, NativeText.Latin1(parameters.Data), (ushort)parameters.WriteMethod)]);
        return await invoker.ExecuteAsync(Code, array);
    }

    protected override object? Decode(IntPtr buffer) => null;
}

public class ParseDataParameters
{
    [DisplayName("Form name"), Description("lpstrFormName: a read form (see IDC Form list).")]
    public string FormName { get; set; } = "READTRACK2";

    [DisplayName("Track 1"), Description("Track 1 as returned by READ_RAW_DATA; empty = not passed.")]
    public string Track1 { get; set; } = string.Empty;

    [DisplayName("Track 2"), Description("Track 2 as returned by READ_RAW_DATA; empty = not passed.")]
    public string Track2 { get; set; } = "4111111111111111=25122010000000000000";

    [DisplayName("Track 3"), Description("Track 3 as returned by READ_RAW_DATA; empty = not passed.")]
    public string Track3 { get; set; } = string.Empty;
}

/// <summary>WFS_CMD_IDC_PARSE_DATA: input WFSIDCPARSEDATA (pack 1: form name 0, card data array 4), output LPSTR lpstrTrackData.</summary>
public class IdcParseDataOperation : OperationDefinition<ParseDataParameters>
{
    public override string Name => "IDC Parse data";

    public override OperationKind Kind => OperationKind.Execute;

    public override uint Code => (uint)IdcConstants.CmdIdcParseData;

    public override string Description => "WFS_CMD_IDC_PARSE_DATA: applies a form to raw track data (no card needed).";

    protected override async Task<RawResponse> SendAsync(ParseDataParameters parameters, IOperationInvoker invoker)
    {
        var tracks = new List<(ushort, byte[], ushort)>();
        Add(tracks, IdcDataSource.Track1, parameters.Track1);
        Add(tracks, IdcDataSource.Track2, parameters.Track2);
        Add(tracks, IdcDataSource.Track3, parameters.Track3);

        using var scope = new NativeScope();
        var input = scope.Block(8);
        Marshal.WriteIntPtr(input, 0, scope.Ansi(parameters.FormName));
        Marshal.WriteIntPtr(input, 4, scope.CardDataArray(tracks));
        return await invoker.ExecuteAsync(Code, input);
    }

    protected override object? Decode(IntPtr buffer) => NativeText.TrackData(buffer);

    private static void Add(List<(ushort, byte[], ushort)> tracks, IdcDataSource source, string text)
    {
        if (text.Length > 0)
            tracks.Add(((ushort)source, NativeText.Latin1(text), 0));
    }
}
