namespace DenoXfs.Idc;

public enum IdcEventId : int
{
    InvalidTrackData = Idc.ExeeIdcInvalidTrackData,
    MediaInserted = Idc.ExeeIdcMediaInserted,
    MediaRemoved = Idc.SrveIdcMediaRemoved,
    CardAction = Idc.SrveIdcCardAction,
    RetainBinThreshold = Idc.UsreIdcRetainBinThreshold,
    InvalidMedia = Idc.ExeeIdcInvalidMedia,
    ExeeMediaRetained = Idc.ExeeIdcMediaRetained,
    MediaDetected = Idc.SrveIdcMediaDetected,
    RetainBinInserted = Idc.SrveIdcRetainBinInserted,
    RetainBinRemoved = Idc.SrveIdcRetainBinRemoved,
    InsertCard = Idc.ExeeIdcInsertCard,
    DevicePosition = Idc.SrveIdcDevicePosition,
    PowerSaveChange = Idc.SrveIdcPowerSaveChange,
    TrackDetected = Idc.ExeeIdcTrackDetected,
    EmvclessReadStatus = Idc.ExeeIdcEmvclessReadStatus,
    SrveMediaRetained = Idc.SrveIdcMediaRetained
}
