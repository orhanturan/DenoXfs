namespace DenoXfs.Idc;

public enum IdcCommandId : int
{
    ReadTrack = Idc.CmdIdcReadTrack,
    WriteTrack = Idc.CmdIdcWriteTrack,
    EjectCard = Idc.CmdIdcEjectCard,
    RetainCard = Idc.CmdIdcRetainCard,
    ResetCount = Idc.CmdIdcResetCount,
    Setkey = Idc.CmdIdcSetkey,
    ReadRawData = Idc.CmdIdcReadRawData,
    WriteRawData = Idc.CmdIdcWriteRawData,
    ChipIo = Idc.CmdIdcChipIo,
    Reset = Idc.CmdIdcReset,
    ChipPower = Idc.CmdIdcChipPower,
    ParseData = Idc.CmdIdcParseData,
    SetGuidanceLight = Idc.CmdIdcSetGuidanceLight,
    PowerSaveControl = Idc.CmdIdcPowerSaveControl,
    ParkCard = Idc.CmdIdcParkCard,
    EmvclessConfigure = Idc.CmdIdcEmvclessConfigure,
    EmvclessPerformTransaction = Idc.CmdIdcEmvclessPerformTransaction,
    EmvclessIssuerupdate = Idc.CmdIdcEmvclessIssuerupdate,
    SynchronizeCommand = Idc.CmdIdcSynchronizeCommand
}
