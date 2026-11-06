using System.Text;
using System.Text.Json.Nodes;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Forms;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Idc.Commands;

/// <summary>Builds the LPSTR track data result and the WFS_EXEE_IDC_INVALIDTRACKDATA events of a form read.</summary>
public sealed class FormReadResult
{
    public async Task<CommandOutcome> CompleteAsync(FormattedTracks tracks, IExecuteEvents events, CancellationToken cancellationToken)
    {
        foreach (var track in tracks.MissingTracks)
        {
            var missing = new JsonObject { ["fwStatus"] = IdcConstants.DataMissing, ["lpstrTrack"] = $"TRACK{track}" };
            await events.SendAsync(IdcConstants.InvalidTrackDataEvent, new NativeValue(IdcResults.TrackEvent, missing), cancellationToken);
        }

        return new CommandOutcome(tracks.HResult, new NativeValue(IdcResults.TrackData, NativeJson.Hex(tracks.TrackData)));
    }
}

/// <summary>WFS_CMD_IDC_READ_TRACK: input LPSTR lpstrFormName; waits for a card, output LPSTR lpstrTrackData built by the form.</summary>
public sealed class ReadTrackCommand : IExecuteCommand
{
    private readonly IIdcFormCatalog _forms;
    private readonly IIdcDeviceService _service;
    private readonly ITrackDataFormatter _formatter;
    private readonly FormReadResult _result;

    public ReadTrackCommand(IIdcFormCatalog forms, IIdcDeviceService service, ITrackDataFormatter formatter, FormReadResult result)
    {
        _forms = forms;
        _service = service;
        _formatter = formatter;
        _result = result;
    }

    public uint Command => IdcConstants.ReadTrackCommand;

    public async Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var form = await _forms.FindAsync(IdcInput.GetString(context.Input, "formName") ?? string.Empty, cancellationToken);
        if (form is null)
            return new CommandOutcome(IdcConstants.ErrFormNotFound, null);
        if (!form.IsValid || form.Action != IdcConstants.ActionRead)
            return new CommandOutcome(IdcConstants.ErrFormInvalid, null);

        var card = await _service.WaitForCardAsync(context.Device, context.Events, cancellationToken);
        var tracks = new Dictionary<int, string?> { [1] = card.Track1, [2] = card.Track2, [3] = card.Track3 };
        return await _result.CompleteAsync(_formatter.Format(form, tracks), context.Events, cancellationToken);
    }
}

/// <summary>WFS_CMD_IDC_PARSE_DATA: input WFSIDCPARSEDATA (form + card data from READ_RAW_DATA), output LPSTR lpstrTrackData.</summary>
public sealed class ParseDataCommand : IExecuteCommand
{
    private readonly IIdcFormCatalog _forms;
    private readonly ITrackDataFormatter _formatter;
    private readonly FormReadResult _result;

    public ParseDataCommand(IIdcFormCatalog forms, ITrackDataFormatter formatter, FormReadResult result)
    {
        _forms = forms;
        _formatter = formatter;
        _result = result;
    }

    public uint Command => IdcConstants.ParseDataCommand;

    public async Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var form = await _forms.FindAsync(IdcInput.GetString(context.Input, "formName") ?? string.Empty, cancellationToken);
        if (form is null)
            return new CommandOutcome(IdcConstants.ErrFormNotFound, null);
        if (!form.IsValid || form.Action != IdcConstants.ActionRead)
            return new CommandOutcome(IdcConstants.ErrFormInvalid, null);

        var cardData = IdcInput.GetCardData(context.Input, "cardData") ?? [];
        var tracks = new Dictionary<int, string?>();
        foreach (var (source, data, _) in cardData)
        {
            var number = source switch
            {
                IdcConstants.Track1 => 1,
                IdcConstants.Track2 => 2,
                IdcConstants.Track3 => 3,
                _ => 0
            };
            if (number > 0)
                tracks[number] = Encoding.Latin1.GetString(data);
        }
        return await _result.CompleteAsync(_formatter.Format(form, tracks), context.Events, cancellationToken);
    }
}

/// <summary>WFS_CMD_IDC_WRITE_TRACK: input WFSIDCWRITETRACK; success when the write form exists and there is data (waits for a card).</summary>
public sealed class WriteTrackCommand : IExecuteCommand
{
    private readonly IIdcFormCatalog _forms;
    private readonly IIdcDeviceService _service;

    public WriteTrackCommand(IIdcFormCatalog forms, IIdcDeviceService service)
    {
        _forms = forms;
        _service = service;
    }

    public uint Command => IdcConstants.WriteTrackCommand;

    public async Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var form = await _forms.FindAsync(IdcInput.GetString(context.Input, "formName") ?? string.Empty, cancellationToken);
        if (form is null)
            return new CommandOutcome(IdcConstants.ErrFormNotFound, null);
        if (!form.IsValid || form.Action != IdcConstants.ActionWrite)
            return new CommandOutcome(IdcConstants.ErrFormInvalid, null);
        if (string.IsNullOrEmpty(IdcInput.GetString(context.Input, "trackData")))
            return new CommandOutcome(XfsResultCodes.InvalidData, null);

        await _service.WaitForCardAsync(context.Device, context.Events, cancellationToken);
        return new CommandOutcome(XfsResultCodes.Success, null);
    }
}

/// <summary>WFS_CMD_IDC_WRITE_RAW_DATA: input NULL terminated LPWFSIDCCARDDATA array; success when it holds track data (waits for a card).</summary>
public sealed class WriteRawDataCommand : IExecuteCommand
{
    private const ushort WritableTracks = IdcConstants.Track1 | IdcConstants.Track2 | IdcConstants.Track3;
    private readonly IIdcDeviceService _service;

    public WriteRawDataCommand(IIdcDeviceService service)
    {
        _service = service;
    }

    public uint Command => IdcConstants.WriteRawDataCommand;

    public async Task<CommandOutcome> ExecuteAsync(ExecuteContext context, CancellationToken cancellationToken)
    {
        var cardData = IdcInput.GetCardData(context.Input, "cardData");
        if (cardData is not { Count: > 0 } || cardData.Any(c => c.Data.Length == 0))
            return new CommandOutcome(XfsResultCodes.InvalidData, null);
        if (cardData.Any(c => (c.Source & ~WritableTracks) != 0 || c.Source == 0))
            return new CommandOutcome(XfsResultCodes.UnsupportedData, null);

        await _service.WaitForCardAsync(context.Device, context.Events, cancellationToken);
        return new CommandOutcome(XfsResultCodes.Success, null);
    }
}
