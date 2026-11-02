using System.Runtime.InteropServices;
using IdcConstants = DenoXfs.Idc.Idc;
using DenoXfs.Idc.Infos;

namespace DenoXfs.TestTool.Operations.Idc;

/// <summary>WFS_INF_IDC_STATUS: no input, output is a WFSIDCSTATUS.</summary>
public class IdcStatusOperation : OperationDefinition<NoParameters>
{
    public override string Name => "IDC Status";

    public override OperationKind Kind => OperationKind.Info;

    public override uint Code => (uint)IdcConstants.InfIdcStatus;

    public override string Description => "WFS_INF_IDC_STATUS: device, media, retain bin and module states.";

    protected override Task<RawResponse> SendAsync(NoParameters parameters, IOperationInvoker invoker) =>
        invoker.GetInfoAsync(Code, IntPtr.Zero);

    protected override object? Decode(IntPtr buffer) => Marshal.PtrToStructure<XfsIdcStatus>(buffer);
}
