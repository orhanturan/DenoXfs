using DenoXfs.Sim.Core.Names;

namespace DenoXfs.Sim.Idc;

/// <summary>
/// Spec names of the IDC info categories, commands, events and errors (XFSIDC.H, IDC_SERVICE_OFFSET 200). The spec is not
/// consistent (MEDIAREMOVED but POWER_SAVE_CHANGE), so the names are listed by hand until HtmlToXfs2 generates them.
/// </summary>
public sealed class IdcCodeNames : IXfsCodeNameSource
{
    private static readonly string[] Infos =
        ["STATUS", "CAPABILITIES", "FORM_LIST", "QUERY_FORM", "QUERY_IFM_IDENTIFIER", "EMVCLESS_QUERY_APPLICATIONS"];

    private static readonly (int Offset, string Name)[] Commands =
    [
        (1, "READ_TRACK"), (2, "WRITE_TRACK"), (3, "EJECT_CARD"), (4, "RETAIN_CARD"), (5, "RESET_COUNT"), (6, "SETKEY"), (7, "READ_RAW_DATA"),
        (8, "WRITE_RAW_DATA"), (9, "CHIP_IO"), (10, "RESET"), (11, "CHIP_POWER"), (12, "PARSE_DATA"), (13, "SET_GUIDANCE_LIGHT"),
        (14, "POWER_SAVE_CONTROL"), (15, "PARK_CARD"), (16, "EMVCLESS_CONFIGURE"), (17, "EMVCLESS_PERFORM_TRANSACTION"),
        (18, "EMVCLESS_ISSUERUPDATE"), (19, "SYNCHRONIZE_COMMAND")
    ];

    private static readonly (int Offset, string Name)[] Events =
    [
        (1, "WFS_EXEE_IDC_INVALIDTRACKDATA"), (3, "WFS_EXEE_IDC_MEDIAINSERTED"), (4, "WFS_SRVE_IDC_MEDIAREMOVED"), (5, "WFS_SRVE_IDC_CARDACTION"),
        (6, "WFS_USRE_IDC_RETAINBINTHRESHOLD"), (7, "WFS_EXEE_IDC_INVALIDMEDIA"), (8, "WFS_EXEE_IDC_MEDIARETAINED"),
        (9, "WFS_SRVE_IDC_MEDIADETECTED"), (10, "WFS_SRVE_IDC_RETAINBININSERTED"), (11, "WFS_SRVE_IDC_RETAINBINREMOVED"),
        (12, "WFS_EXEE_IDC_INSERTCARD"), (13, "WFS_SRVE_IDC_DEVICEPOSITION"), (14, "WFS_SRVE_IDC_POWER_SAVE_CHANGE"),
        (15, "WFS_EXEE_IDC_TRACKDETECTED"), (16, "WFS_EXEE_IDC_EMVCLESSREADSTATUS"), (17, "WFS_SRVE_IDC_MEDIARETAINED")
    ];

    private static readonly string[] Errors =
    [
        "MEDIAJAM", "NOMEDIA", "MEDIARETAINED", "RETAINBINFULL", "INVALIDDATA", "INVALIDMEDIA", "FORMNOTFOUND", "FORMINVALID", "DATASYNTAX",
        "SHUTTERFAIL", "SECURITYFAIL", "PROTOCOLNOTSUPP", "ATRNOTOBTAINED", "INVALIDKEY", "WRITE_METHOD", "CHIPPOWERNOTSUPP", "CARDTOOSHORT",
        "CARDTOOLONG", "INVALID_PORT", "POWERSAVETOOSHORT", "POWERSAVEMEDIAPRESENT", "CARDPRESENT", "POSITIONINVALID", "INVALIDTERMINALDATA",
        "INVALIDAIDDATA", "INVALIDKEYDATA", "READERNOTCONFIGURED", "TRANSACTIONNOTINITIATED", "COMMANDUNSUPP", "SYNCHRONIZEUNSUPP", "CARDCOLLISION"
    ];

    private const int Offset = 200;

    public IEnumerable<(XfsCodeKind Kind, int Code, string Name)> Names =>
        Infos.Select((name, i) => (XfsCodeKind.Info, Offset + i + 1, $"WFS_INF_IDC_{name}"))
            .Concat(Commands.Select(c => (XfsCodeKind.Command, Offset + c.Offset, $"WFS_CMD_IDC_{c.Name}")))
            .Concat(Events.Select(e => (XfsCodeKind.Event, Offset + e.Offset, e.Name)))
            .Concat(Errors.Select((name, i) => (XfsCodeKind.Result, -(Offset + i), $"WFS_ERR_IDC_{name}")));
}
