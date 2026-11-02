using DenoXfs.Idc.Events;
using DenoXfs.Idc.Infos;
using IdcConstants = DenoXfs.Idc.Idc;

namespace DenoXfs.TestTool.Operations.Idc;

/// <summary>Payload decoders of the IDC events (XFSIDC.H message structures).</summary>
public static class IdcEventDecoders
{
    public static IEnumerable<IEventPayloadDecoder> All =>
    [
        new StructPayloadDecoder<XfsIdcTrackEvent>(XfsCodeKind.ExecuteEvent, IdcConstants.ExeeIdcInvalidTrackData),
        new StructPayloadDecoder<XfsIdcTrackDetected>(XfsCodeKind.ExecuteEvent, IdcConstants.ExeeIdcTrackDetected),
        new StructPayloadDecoder<XfsIdcEmvClessReadStatus>(XfsCodeKind.ExecuteEvent, IdcConstants.ExeeIdcEmvclessReadStatus),
        new StructPayloadDecoder<XfsIdcCardAct>(XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcCardAction),
        new StructPayloadDecoder<XfsIdcDevicePosition>(XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcDevicePosition),
        new StructPayloadDecoder<XfsIdcPowerSaveChange>(XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcPowerSaveChange),
        // LPWORD lpwResetOut: where the card went during the reset.
        new WordPayloadDecoder<IdcActionCardAct>(XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcMediaDetected),
        // LPWORD lpfwRetainBin: the new retain bin state.
        new WordPayloadDecoder<IdcRetainBinStatus>(XfsCodeKind.UserEvent, IdcConstants.UsreIdcRetainBinThreshold)
    ];
}
