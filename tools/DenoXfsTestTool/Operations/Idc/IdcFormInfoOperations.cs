using System.ComponentModel;
using System.Runtime.InteropServices;
using IdcConstants = DenoXfs.Idc.Idc;
using DenoXfs.Idc.Infos;
using DenoXfs.Interop.Convert;

namespace DenoXfs.TestTool.Operations.Idc;

/// <summary>WFS_INF_IDC_FORM_LIST: no input, output LPSTR lpszFormList (double null terminated).</summary>
public class IdcFormListOperation : OperationDefinition<NoParameters>
{
    public override string Name => "IDC Form list";

    public override OperationKind Kind => OperationKind.Info;

    public override uint Code => (uint)IdcConstants.InfIdcFormList;

    public override string Description => "WFS_INF_IDC_FORM_LIST: names of the forms the reader knows.";

    protected override Task<RawResponse> SendAsync(NoParameters parameters, IOperationInvoker invoker) =>
        invoker.GetInfoAsync(Code, IntPtr.Zero);

    protected override object? Decode(IntPtr buffer) => MultiStringConvert.ToManaged(buffer);
}

public class FormNameParameters
{
    [DisplayName("Form name"), Description("lpszFormName (see IDC Form list).")]
    public string FormName { get; set; } = "READTRACK2";
}

/// <summary>WFS_INF_IDC_QUERY_FORM: input LPSTR lpszFormName, output WFSIDCFORM (pack 1, 44 bytes).</summary>
public class IdcQueryFormOperation : OperationDefinition<FormNameParameters>
{
    public override string Name => "IDC Query form";

    public override OperationKind Kind => OperationKind.Info;

    public override uint Code => (uint)IdcConstants.InfIdcQueryForm;

    public override string Description => "WFS_INF_IDC_QUERY_FORM: definition of one form.";

    protected override async Task<RawResponse> SendAsync(FormNameParameters parameters, IOperationInvoker invoker)
    {
        using var scope = new NativeScope();
        return await invoker.GetInfoAsync(Code, scope.Ansi(parameters.FormName));
    }

    protected override object? Decode(IntPtr buffer) => Marshal.PtrToStructure<XfsIdcForm>(buffer);
}

/// <summary>WFS_INF_IDC_QUERY_IFM_IDENTIFIER: no input, output NULL terminated array of LPWFSIDCIFMIDENTIFIER.</summary>
public class IdcQueryIfmIdentifierOperation : OperationDefinition<NoParameters>
{
    public override string Name => "IDC Query IFM identifier";

    public override OperationKind Kind => OperationKind.Info;

    public override uint Code => (uint)IdcConstants.InfIdcQueryIfmIdentifier;

    public override string Description => "WFS_INF_IDC_QUERY_IFM_IDENTIFIER: IFM (EMV level 1) identifiers of the reader.";

    protected override Task<RawResponse> SendAsync(NoParameters parameters, IOperationInvoker invoker) =>
        invoker.GetInfoAsync(Code, IntPtr.Zero);

    protected override object? Decode(IntPtr buffer) =>
        (PointerArray.ToStructs<XfsIdcIfmIdentifier>(buffer) ?? []).Where(i => i.HasValue).Select(i => i!.Value).ToList();
}

public sealed record AppDataView(string Aid, string KernelIdentifier);

/// <summary>WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS: no input, output NULL terminated array of LPWFSIDCAPPDATA.</summary>
public class IdcEmvclessQueryApplicationsOperation : OperationDefinition<NoParameters>
{
    public override string Name => "IDC Contactless applications";

    public override OperationKind Kind => OperationKind.Info;

    public override uint Code => (uint)IdcConstants.InfIdcEmvclessQueryApplications;

    public override string Description => "WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS: AIDs and kernels (fixed answer of the simulator).";

    protected override Task<RawResponse> SendAsync(NoParameters parameters, IOperationInvoker invoker) =>
        invoker.GetInfoAsync(Code, IntPtr.Zero);

    protected override object? Decode(IntPtr buffer)
    {
        var apps = new List<AppDataView>();
        for (var i = 0; ; i++)
        {
            var item = Marshal.ReadIntPtr(buffer, i * IntPtr.Size);
            if (item == IntPtr.Zero)
                break;
            apps.Add(new AppDataView(HexData(Marshal.ReadIntPtr(item, 0)), HexData(Marshal.ReadIntPtr(item, IntPtr.Size))));
        }
        return apps;
    }

    /// <summary>WFSIDCHEXDATA: ULONG ulLength, LPBYTE lpbData.</summary>
    private static string HexData(IntPtr hex)
    {
        if (hex == IntPtr.Zero)
            return "null";
        var data = PrimitiveArray.ToManaged<byte>(Marshal.ReadIntPtr(hex, 4), Marshal.ReadInt32(hex, 0)) ?? [];
        return ObjectDumper.Hex(data);
    }
}
