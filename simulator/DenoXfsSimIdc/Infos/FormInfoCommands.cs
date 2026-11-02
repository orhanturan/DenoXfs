using System.Text;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Commands;
using DenoXfs.Sim.Idc.Forms;
using DenoXfs.Sim.Idc.Layouts;

namespace DenoXfs.Sim.Idc.Infos;

/// <summary>Writes "a\0b\0\0" style lists (form names, field names).</summary>
internal static class DoubleNullList
{
    public static int Add(INativeBufferBuilder builder, IEnumerable<string> names) =>
        builder.AddBytes(Encoding.Latin1.GetBytes(string.Concat(names.Select(n => n + "\0")) + "\0"));
}

/// <summary>WFS_INF_IDC_FORM_LIST: LPSTR lpszFormList, double null terminated.</summary>
public sealed class FormListInfo : IInfoCommand
{
    private readonly IIdcFormCatalog _forms;
    private readonly INativeBufferBuilderFactory _builders;

    public FormListInfo(IIdcFormCatalog forms, INativeBufferBuilderFactory builders)
    {
        _forms = forms;
        _builders = builders;
    }

    public uint Category => IdcConstants.FormListCategory;

    public async Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var builder = _builders.Create();
        DoubleNullList.Add(builder, (await _forms.AllAsync(cancellationToken)).Select(f => f.Name));
        return new CommandOutcome(XfsResultCodes.Success, builder.Build());
    }
}

/// <summary>WFS_INF_IDC_QUERY_FORM: input LPSTR lpszFormName, output WFSIDCFORM.</summary>
public sealed class QueryFormInfo : IInfoCommand
{
    private readonly IIdcFormCatalog _forms;
    private readonly INativeBufferBuilderFactory _builders;

    public QueryFormInfo(IIdcFormCatalog forms, INativeBufferBuilderFactory builders)
    {
        _forms = forms;
        _builders = builders;
    }

    public uint Category => IdcConstants.QueryFormCategory;

    public async Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var form = await _forms.FindAsync(IdcInput.GetString(context.Input, "formName") ?? string.Empty, cancellationToken);
        if (form is null)
            return new CommandOutcome(IdcConstants.ErrFormNotFound, null);

        var builder = _builders.Create();
        var root = builder.Reserve(FormLayout.Size);
        builder.WritePointer(root + FormLayout.FormName, builder.AddAnsiString(form.Name));
        builder.WriteByte(root + FormLayout.FieldSeparatorTrack1, (byte)IdcForm.Track1Separator);
        builder.WriteByte(root + FormLayout.FieldSeparatorTrack2, (byte)IdcForm.Track2Separator);
        builder.WriteByte(root + FormLayout.FieldSeparatorTrack3, (byte)IdcForm.Track3Separator);
        builder.WriteUInt16(root + FormLayout.Action, form.Action);
        builder.WritePointer(root + FormLayout.Tracks, builder.AddAnsiString(form.Tracks));
        builder.WriteUInt32(root + FormLayout.Secure, form.Secure ? 1u : 0u);
        builder.WritePointer(root + FormLayout.Track1Fields, DoubleNullList.Add(builder, form.Track1Fields));
        builder.WritePointer(root + FormLayout.Track2Fields, DoubleNullList.Add(builder, form.Track2Fields));
        builder.WritePointer(root + FormLayout.Track3Fields, DoubleNullList.Add(builder, form.Track3Fields));
        // Front track 1 and JIS I tracks are not supported: NULL lists and zero separators.
        return new CommandOutcome(XfsResultCodes.Success, builder.Build());
    }
}

/// <summary>WFS_INF_IDC_QUERY_IFM_IDENTIFIER: NULL terminated array of LPWFSIDCIFMIDENTIFIER; one EMV identifier.</summary>
public sealed class QueryIfmIdentifierInfo : IInfoCommand
{
    public const string Identifier = "DENOXFS-SIM-IFM-0001";
    private readonly INativeBufferBuilderFactory _builders;

    public QueryIfmIdentifierInfo(INativeBufferBuilderFactory builders)
    {
        _builders = builders;
    }

    public uint Category => IdcConstants.QueryIfmIdentifierCategory;

    public Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var builder = _builders.Create();
        var ptrSize = NativeBufferBuilderFactory.DefaultPtrSize;
        var array = builder.Reserve(2 * ptrSize);
        var item = builder.Reserve(IfmIdentifierLayout.Size);
        builder.WritePointer(array, item);
        builder.WriteUInt16(item + IfmIdentifierLayout.Authority, IdcConstants.IfmEmv);
        builder.WritePointer(item + IfmIdentifierLayout.Identifier, builder.AddAnsiString(Identifier));
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, builder.Build()));
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

    private readonly INativeBufferBuilderFactory _builders;

    public EmvclessQueryApplicationsInfo(INativeBufferBuilderFactory builders)
    {
        _builders = builders;
    }

    public uint Category => IdcConstants.EmvclessQueryApplicationsCategory;

    public Task<CommandOutcome> GetInfoAsync(InfoContext context, CancellationToken cancellationToken)
    {
        var builder = _builders.Create();
        var ptrSize = NativeBufferBuilderFactory.DefaultPtrSize;
        var array = builder.Reserve((Applications.Length + 1) * ptrSize);
        for (var i = 0; i < Applications.Length; i++)
        {
            var item = builder.Reserve(AppDataLayout.Size);
            builder.WritePointer(array + i * ptrSize, item);
            builder.WritePointer(item + AppDataLayout.Aid, HexData(builder, Applications[i].Aid));
            builder.WritePointer(item + AppDataLayout.KernelIdentifier, HexData(builder, Applications[i].Kernel));
        }
        return Task.FromResult(new CommandOutcome(XfsResultCodes.Success, builder.Build()));
    }

    private static int HexData(INativeBufferBuilder builder, byte[] data)
    {
        var hex = builder.Reserve(HexDataLayout.Size);
        builder.WriteUInt32(hex + HexDataLayout.Length, (uint)data.Length);
        builder.WritePointer(hex + HexDataLayout.Data, builder.AddBytes(data));
        return hex;
    }
}
