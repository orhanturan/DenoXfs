using DenoXfs.Api;
using DenoXfs.Api.Common;

namespace DenoXfs.TestTool.Operations;

/// <summary>Payload decoders of the system events (XFSAPI.H), sent by every device class.</summary>
public static class ApiEventDecoders
{
    public static IEnumerable<IEventPayloadDecoder> All =>
    [
        new StructPayloadDecoder<XfsUndevMsg>(XfsCodeKind.SystemEvent, Xfs.SyseUndeliverableMsg),
        new StructPayloadDecoder<XfsHwError>(XfsCodeKind.SystemEvent, Xfs.SyseHardwareError),
        new StructPayloadDecoder<XfsVrsnError>(XfsCodeKind.SystemEvent, Xfs.SyseVersionError),
        new StructPayloadDecoder<XfsDevStatus>(XfsCodeKind.SystemEvent, Xfs.SyseDeviceStatus),
        new StructPayloadDecoder<XfsAppDisc>(XfsCodeKind.SystemEvent, Xfs.SyseAppDisconnect),
        new StructPayloadDecoder<XfsHwError>(XfsCodeKind.SystemEvent, Xfs.SyseSoftwareError),
        new StructPayloadDecoder<XfsHwError>(XfsCodeKind.SystemEvent, Xfs.SyseUserError),
        new StructPayloadDecoder<XfsHwError>(XfsCodeKind.SystemEvent, Xfs.SyseFraudAttempt)
    ];
}
