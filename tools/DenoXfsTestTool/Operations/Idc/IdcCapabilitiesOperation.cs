using System.Runtime.InteropServices;
using IdcConstants = DenoXfs.Idc.Idc;
using DenoXfs.Idc.Infos;

namespace DenoXfs.TestTool.Operations.Idc;

/// <summary>WFS_INF_IDC_CAPABILITIES: no input, output is a WFSIDCCAPS.</summary>
public class IdcCapabilitiesOperation : OperationDefinition<NoParameters>
{
    public override string Name => "IDC Capabilities";

    public override OperationKind Kind => OperationKind.Info;

    public override uint Code => (uint)IdcConstants.InfIdcCapabilities;

    public override string Description => "WFS_INF_IDC_CAPABILITIES: reader type, tracks, chip protocols, retain bin size.";

    protected override Task<RawResponse> SendAsync(NoParameters parameters, IOperationInvoker invoker) =>
        invoker.GetInfoAsync(Code, IntPtr.Zero);

    protected override object? Decode(IntPtr buffer) => Marshal.PtrToStructure<XfsIdcCaps>(buffer);
}
