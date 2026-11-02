using IdcConstants = DenoXfs.Idc.Idc;

namespace DenoXfs.TestTool.Operations.Idc;

/// <summary>
/// Spec names of the IDC info categories, commands and events (XFSIDC.H). The spec is not consistent
/// (MEDIAREMOVED but POWER_SAVE_CHANGE), so the names cannot be derived from the constants. HtmlToXfs2 will generate this.
/// </summary>
public class IdcCodeNames : IXfsCodeNameSource
{
    public IEnumerable<(XfsCodeKind Kind, uint Code, string Name)> Names =>
    [
        (XfsCodeKind.Info, IdcConstants.InfIdcStatus, "WFS_INF_IDC_STATUS"),
        (XfsCodeKind.Info, IdcConstants.InfIdcCapabilities, "WFS_INF_IDC_CAPABILITIES"),
        (XfsCodeKind.Info, IdcConstants.InfIdcFormList, "WFS_INF_IDC_FORM_LIST"),
        (XfsCodeKind.Info, IdcConstants.InfIdcQueryForm, "WFS_INF_IDC_QUERY_FORM"),
        (XfsCodeKind.Info, IdcConstants.InfIdcQueryIfmIdentifier, "WFS_INF_IDC_QUERY_IFM_IDENTIFIER"),
        (XfsCodeKind.Info, IdcConstants.InfIdcEmvclessQueryApplications, "WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS"),

        (XfsCodeKind.Command, IdcConstants.CmdIdcReadTrack, "WFS_CMD_IDC_READ_TRACK"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcWriteTrack, "WFS_CMD_IDC_WRITE_TRACK"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcEjectCard, "WFS_CMD_IDC_EJECT_CARD"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcRetainCard, "WFS_CMD_IDC_RETAIN_CARD"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcResetCount, "WFS_CMD_IDC_RESET_COUNT"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcSetkey, "WFS_CMD_IDC_SETKEY"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcReadRawData, "WFS_CMD_IDC_READ_RAW_DATA"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcWriteRawData, "WFS_CMD_IDC_WRITE_RAW_DATA"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcChipIo, "WFS_CMD_IDC_CHIP_IO"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcReset, "WFS_CMD_IDC_RESET"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcChipPower, "WFS_CMD_IDC_CHIP_POWER"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcParseData, "WFS_CMD_IDC_PARSE_DATA"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcSetGuidanceLight, "WFS_CMD_IDC_SET_GUIDANCE_LIGHT"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcPowerSaveControl, "WFS_CMD_IDC_POWER_SAVE_CONTROL"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcParkCard, "WFS_CMD_IDC_PARK_CARD"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcEmvclessConfigure, "WFS_CMD_IDC_EMVCLESS_CONFIGURE"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcEmvclessPerformTransaction, "WFS_CMD_IDC_EMVCLESS_PERFORM_TRANSACTION"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcEmvclessIssuerupdate, "WFS_CMD_IDC_EMVCLESS_ISSUERUPDATE"),
        (XfsCodeKind.Command, IdcConstants.CmdIdcSynchronizeCommand, "WFS_CMD_IDC_SYNCHRONIZE_COMMAND"),

        (XfsCodeKind.ExecuteEvent, IdcConstants.ExeeIdcInvalidTrackData, "WFS_EXEE_IDC_INVALIDTRACKDATA"),
        (XfsCodeKind.ExecuteEvent, IdcConstants.ExeeIdcMediaInserted, "WFS_EXEE_IDC_MEDIAINSERTED"),
        (XfsCodeKind.ExecuteEvent, IdcConstants.ExeeIdcInvalidMedia, "WFS_EXEE_IDC_INVALIDMEDIA"),
        (XfsCodeKind.ExecuteEvent, IdcConstants.ExeeIdcMediaRetained, "WFS_EXEE_IDC_MEDIARETAINED"),
        (XfsCodeKind.ExecuteEvent, IdcConstants.ExeeIdcInsertCard, "WFS_EXEE_IDC_INSERTCARD"),
        (XfsCodeKind.ExecuteEvent, IdcConstants.ExeeIdcTrackDetected, "WFS_EXEE_IDC_TRACKDETECTED"),
        (XfsCodeKind.ExecuteEvent, IdcConstants.ExeeIdcEmvclessReadStatus, "WFS_EXEE_IDC_EMVCLESSREADSTATUS"),

        (XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcMediaRemoved, "WFS_SRVE_IDC_MEDIAREMOVED"),
        (XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcCardAction, "WFS_SRVE_IDC_CARDACTION"),
        (XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcMediaDetected, "WFS_SRVE_IDC_MEDIADETECTED"),
        (XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcRetainBinInserted, "WFS_SRVE_IDC_RETAINBININSERTED"),
        (XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcRetainBinRemoved, "WFS_SRVE_IDC_RETAINBINREMOVED"),
        (XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcDevicePosition, "WFS_SRVE_IDC_DEVICEPOSITION"),
        (XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcPowerSaveChange, "WFS_SRVE_IDC_POWER_SAVE_CHANGE"),
        (XfsCodeKind.ServiceEvent, IdcConstants.SrveIdcMediaRetained, "WFS_SRVE_IDC_MEDIARETAINED"),

        (XfsCodeKind.UserEvent, IdcConstants.UsreIdcRetainBinThreshold, "WFS_USRE_IDC_RETAINBINTHRESHOLD")
    ];
}
