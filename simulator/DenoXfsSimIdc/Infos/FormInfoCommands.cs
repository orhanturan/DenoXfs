using System.Text.Json.Nodes;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Commands;
using DenoXfs.Sim.Idc.Forms;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Idc.Infos;

/// <summary>WFS_INF_IDC_FORM_LIST: LPSTR lpszFormList, double null terminated.</summary>
public sealed class FormListInfo : IInfoCommand
{
    private readonly IIdcFormCatalog _forms;

    public FormListInfo(IIdcFormCatalog forms)
    {
        _forms = forms;
    }

    public uint Category => IdcConstants.FormListCategory;

    public async Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var names = NativeJson.Strings((await _forms.AllAsync(cancellationToken)).Select(f => f.Name));
        return new CommandOutcome(XfsResultCodes.Success, new NativeValue(IdcResults.FormList, names));
    }
}

/// <summary>WFS_INF_IDC_QUERY_FORM: input LPSTR lpszFormName, output WFSIDCFORM.</summary>
public sealed class QueryFormInfo : IInfoCommand
{
    private readonly IIdcFormCatalog _forms;

    public QueryFormInfo(IIdcFormCatalog forms)
    {
        _forms = forms;
    }

    public uint Category => IdcConstants.QueryFormCategory;

    public async Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var form = await _forms.FindAsync(IdcInput.GetString(context.Input, "formName") ?? string.Empty, cancellationToken);
        if (form is null)
            return new CommandOutcome(IdcConstants.ErrFormNotFound, null);

        var result = new JsonObject
        {
            ["lpszFormName"] = form.Name,
            ["cFieldSeparatorTrack1"] = (byte)IdcForm.Track1Separator,
            ["cFieldSeparatorTrack2"] = (byte)IdcForm.Track2Separator,
            ["cFieldSeparatorTrack3"] = (byte)IdcForm.Track3Separator,
            ["fwAction"] = form.Action,
            ["lpszTracks"] = form.Tracks,
            ["bSecure"] = form.Secure,
            ["lpszTrack1Fields"] = NativeJson.Strings(form.Track1Fields),
            ["lpszTrack2Fields"] = NativeJson.Strings(form.Track2Fields),
            ["lpszTrack3Fields"] = NativeJson.Strings(form.Track3Fields)
            // Front track 1 and JIS I tracks are not supported: NULL lists and zero separators.
        };
        return new CommandOutcome(XfsResultCodes.Success, new NativeValue(IdcResults.Form, result));
    }
}

/// <summary>WFS_INF_IDC_QUERY_IFM_IDENTIFIER: NULL terminated array of LPWFSIDCIFMIDENTIFIER; one EMV identifier.</summary>
public sealed class QueryIfmIdentifierInfo : IInfoCommand
{
    public const string Identifier = "DENOXFS-SIM-IFM-0001";

    public uint Category => IdcConstants.QueryIfmIdentifierCategory;

    public Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var identifiers = new JsonArray(new JsonObject { ["wIFMAuthority"] = IdcConstants.IfmEmv, ["lpszIFMIdentifier"] = Identifier });
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, new NativeValue(IdcResults.IfmIdentifiers, identifiers)));
    }
}

/// <summary>
/// WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS: fixed answer (contactless itself is not simulated): NULL terminated array of
/// LPWFSIDCAPPDATA with the Visa and Mastercard AIDs and their kernel identifiers.
/// </summary>
public sealed class EmvclessQueryApplicationsInfo : IInfoCommand
{
    private static readonly (byte[] Aid, byte[] Kernel)[] Applications =
    [
        (Convert.FromHexString("A0000000031010"), [0x03]),
        (Convert.FromHexString("A0000000041010"), [0x02])
    ];

    public uint Category => IdcConstants.EmvclessQueryApplicationsCategory;

    public Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var applications = NativeJson.Array(Applications, app => new JsonObject
        {
            ["lpAID"] = HexData(app.Aid),
            ["lpKernelIdentifier"] = HexData(app.Kernel)
        });
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, new NativeValue(IdcResults.Applications, applications)));
    }

    private static JsonObject HexData(byte[] data) => new() { ["lpbData"] = NativeJson.Hex(data) };
}
