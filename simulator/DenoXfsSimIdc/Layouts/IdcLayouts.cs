using DenoXfs.Sim.Core.Marshalling;
using static DenoXfs.Sim.Core.Marshalling.NativeField;

namespace DenoXfs.Sim.Idc.Layouts;

// Hand written from XFSIDC.H (spec 3.40) until HtmlToXfs2 generates them. Offsets follow from the field order (pack 1,
// 32 bit pointers, BOOL is 4 bytes); the sizes are checked by the tests.

/// <summary>Named values per field; a scenario may use the short names (<c>HWERROR</c>) or the full ones (<c>WFS_IDC_DEVHWERROR</c>).</summary>
public static class IdcConstantSets
{
    public static readonly ConstantSet Device = new("WFS_IDC_DEV",
        ("ONLINE", 0), ("OFFLINE", 1), ("POWEROFF", 2), ("NODEVICE", 3), ("HWERROR", 4), ("USERERROR", 5), ("BUSY", 6),
        ("FRAUDATTEMPT", 7), ("POTENTIALFRAUD", 8));

    public static readonly ConstantSet Media = new("WFS_IDC_MEDIA",
        ("PRESENT", 1), ("NOTPRESENT", 2), ("JAMMED", 3), ("NOTSUPP", 4), ("UNKNOWN", 5), ("ENTERING", 6), ("LATCHED", 7));

    public static readonly ConstantSet RetainBin = new("WFS_IDC_RETAIN",
        ("BINOK", 1), ("NOTSUPP", 2), ("BINFULL", 3), ("BINHIGH", 4), ("BINMISSING", 5));

    public static readonly ConstantSet Security = new("WFS_IDC_SEC", ("NOTSUPP", 1), ("NOTREADY", 2), ("OPEN", 3));

    public static readonly ConstantSet ChipPowerStatus = new("WFS_IDC_CHIP",
        ("ONLINE", 0), ("POWEREDOFF", 1), ("BUSY", 2), ("NODEVICE", 3), ("HWERROR", 4), ("NOCARD", 5), ("NOTSUPP", 6), ("UNKNOWN", 7));

    public static readonly ConstantSet ChipModule = new("WFS_IDC_CHIPMOD", ("OK", 1), ("INOP", 2), ("UNKNOWN", 3), ("NOTSUPP", 4));

    public static readonly ConstantSet MagModule = new("WFS_IDC_MAGMOD", ("OK", 1), ("INOP", 2), ("UNKNOWN", 3), ("NOTSUPP", 4));

    public static readonly ConstantSet ImageModule = new("WFS_IDC_IMGMOD", ("OK", 1), ("INOP", 2), ("UNKNOWN", 3), ("NOTSUPP", 4));

    public static readonly ConstantSet DevicePosition = new("WFS_IDC_DEVICE",
        ("INPOSITION", 0), ("NOTINPOSITION", 1), ("POSUNKNOWN", 2), ("POSNOTSUPP", 3));

    public static readonly ConstantSet AntiFraudModule = new("WFS_IDC_AFM",
        ("NOTSUPP", 0), ("OK", 1), ("INOP", 2), ("DEVICEDETECTED", 3), ("UNKNOWN", 4));

    public static readonly ConstantSet Guidance = new("WFS_IDC_GUIDANCE_",
        ("NOT_AVAILABLE", 0), ("OFF", 0x1), ("SLOW_FLASH", 0x4), ("MEDIUM_FLASH", 0x8), ("QUICK_FLASH", 0x10), ("CONTINUOUS", 0x80),
        ("RED", 0x100), ("GREEN", 0x200), ("YELLOW", 0x400), ("BLUE", 0x800), ("CYAN", 0x1000), ("MAGENTA", 0x2000), ("WHITE", 0x4000),
        ("ENTRY", 0x100000), ("EXIT", 0x200000));

    public static readonly ConstantSet Type = new("WFS_IDC_TYPE",
        ("MOTOR", 1), ("SWIPE", 2), ("DIP", 3), ("CONTACTLESS", 4), ("LATCHEDDIP", 5), ("PERMANENT", 6), ("INTELLIGENTCONTACTLESS", 7));

    /// <summary>Data sources (fwReadTracks, fwWriteTracks, wDataSource), flags.</summary>
    public static readonly ConstantSet DataSource = new("WFS_IDC_",
        ("NOTSUPP", 0), ("TRACK1", 0x1), ("TRACK2", 0x2), ("TRACK3", 0x4), ("CHIP", 0x8), ("SECURITY", 0x10), ("FLUXINACTIVE", 0x20),
        ("MEMORY_CHIP", 0x40), ("FRONT_TRACK_1", 0x80), ("FRONTIMAGE", 0x100), ("BACKIMAGE", 0x200), ("TRACK1_JIS1", 0x400),
        ("TRACK3_JIS1", 0x800), ("DDI", 0x4000), ("TRACK_WM", 0x8000));

    public static readonly ConstantSet ChipProtocol = new("WFS_IDC_",
        ("NOTSUPP", 0), ("CHIPT0", 0x1), ("CHIPT1", 0x2), ("CHIP_PROTOCOL_NOT_REQUIRED", 0x4), ("CHIPTYPEA_PART3", 0x8),
        ("CHIPTYPEA_PART4", 0x10), ("CHIPTYPEB", 0x20), ("CHIPNFC", 0x40));

    public static readonly ConstantSet SecType = new("WFS_IDC_SEC", ("NOTSUPP", 1), ("MMBOX", 2), ("CIM86", 3));

    /// <summary>fwPowerOnOption, fwPowerOffOption, lpwResetIn.</summary>
    public static readonly ConstantSet PowerOption = new("WFS_IDC_",
        ("NOTSUPP", 0), ("NOACTION", 1), ("EJECT", 2), ("RETAIN", 3), ("EJECTTHENRETAIN", 4));

    public static readonly ConstantSet WriteMethod = new("WFS_IDC_", ("NOTSUPP", 0), ("LOCO", 0x2), ("HICO", 0x4), ("AUTO", 0x8));

    public static readonly ConstantSet ChipPowerCaps = new("WFS_IDC_CHIPPOWER", ("NOTSUPP", 0), ("COLD", 0x2), ("WARM", 0x4), ("OFF", 0x8));

    public static readonly ConstantSet DipMode = new("WFS_IDC_DIP_", ("NOTSUPP", 0), ("UNKNOWN", 0x1), ("EXIT", 0x2), ("ENTRY", 0x4), ("ENTRY_EXIT", 0x8));

    public static readonly ConstantSet EjectPosition = new("WFS_IDC_", ("NOTSUPP", 0), ("EXITPOSITION", 0x1), ("TRANSPORTPOSITION", 0x2));

    public static readonly ConstantSet DataStatus = new("WFS_IDC_DATA",
        ("OK", 0), ("MISSING", 1), ("INVALID", 2), ("TOOLONG", 3), ("TOOSHORT", 4), ("SRCNOTSUPP", 5), ("SRCMISSING", 6));

    public static readonly ConstantSet FormAction = new("WFS_IDC_ACTION", ("READ", 0x1), ("WRITE", 0x2));

    public static readonly ConstantSet IfmAuthority = new("WFS_IDC_IFM", ("EMV", 1), ("EUROPAY", 2), ("VISA", 3), ("GIECB", 4));

    /// <summary>lpwResetOut of WFS_SRVE_IDC_MEDIADETECTED.</summary>
    public static readonly ConstantSet ResetOut = new("WFS_IDC_CARD", ("RETAINED", 1), ("EJECTED", 2), ("READPOSITION", 3), ("JAMMED", 4));
}

/// <summary>The XFSIDC.H structures the simulator returns.</summary>
public static class IdcStructs
{
    public const int GuidLightsSize = 32;

    public static readonly StructLayout Status = new("WFSIDCSTATUS",
        Word("fwDevice", IdcConstantSets.Device),
        Word("fwMedia", IdcConstantSets.Media),
        Word("fwRetainBin", IdcConstantSets.RetainBin),
        Word("fwSecurity", IdcConstantSets.Security),
        Word("usCards"),
        Word("fwChipPower", IdcConstantSets.ChipPowerStatus),
        MultiString("lpszExtra"),
        DWordArray("dwGuidLights", GuidLightsSize, IdcConstantSets.Guidance),
        Word("fwChipModule", IdcConstantSets.ChipModule),
        Word("fwMagReadModule", IdcConstantSets.MagModule),
        Word("fwMagWriteModule", IdcConstantSets.MagModule),
        Word("fwFrontImageModule", IdcConstantSets.ImageModule),
        Word("fwBackImageModule", IdcConstantSets.ImageModule),
        Word("wDevicePosition", IdcConstantSets.DevicePosition),
        Word("usPowerSaveRecoveryTime"),
        // One entry per parking station (usParkingStations of the capabilities), no terminator.
        new NativeField("lpwParkingStationMedia", new NativeType(NativeKind.WordList) { Constants = IdcConstantSets.Media, Terminated = false }),
        Word("wAntiFraudModule", IdcConstantSets.AntiFraudModule));

    public static readonly StructLayout Caps = new("WFSIDCCAPS",
        Word("wClass"),
        Word("fwType", IdcConstantSets.Type),
        Bool("bCompound"),
        Word("fwReadTracks", IdcConstantSets.DataSource),
        Word("fwWriteTracks", IdcConstantSets.DataSource),
        Word("fwChipProtocols", IdcConstantSets.ChipProtocol),
        Word("usCards"),
        Word("fwSecType", IdcConstantSets.SecType),
        Word("fwPowerOnOption", IdcConstantSets.PowerOption),
        Word("fwPowerOffOption", IdcConstantSets.PowerOption),
        Bool("bFluxSensorProgrammable"),
        Bool("bReadWriteAccessFollowingEject"),
        Word("fwWriteMode", IdcConstantSets.WriteMethod),
        Word("fwChipPower", IdcConstantSets.ChipPowerCaps),
        MultiString("lpszExtra"),
        Word("fwDIPMode", IdcConstantSets.DipMode),
        WordList("lpwMemoryChipProtocols"),
        DWordArray("dwGuidLights", GuidLightsSize, IdcConstantSets.Guidance),
        Word("fwEjectPosition", IdcConstantSets.EjectPosition),
        Bool("bPowerSaveControl"),
        Word("usParkingStations"),
        Bool("bAntiFraudModule"),
        DWordList("lpdwSynchronizableCommands"));

    public static readonly StructLayout Form = new("WFSIDCFORM",
        AnsiString("lpszFormName"),
        Char("cFieldSeparatorTrack1"),
        Char("cFieldSeparatorTrack2"),
        Char("cFieldSeparatorTrack3"),
        Word("fwAction", IdcConstantSets.FormAction),
        AnsiString("lpszTracks"),
        Bool("bSecure"),
        MultiString("lpszTrack1Fields"),
        MultiString("lpszTrack2Fields"),
        MultiString("lpszTrack3Fields"),
        MultiString("lpszFrontTrack1Fields"),
        Char("cFieldSeparatorFrontTrack1"),
        MultiString("lpszJIS1Track1Fields"),
        MultiString("lpszJIS1Track3Fields"),
        Char("cFieldSeparatorJIS1Track1"),
        Char("cFieldSeparatorJIS1Track3"));

    public static readonly StructLayout IfmIdentifier = new("WFSIDCIFMIDENTIFIER",
        Word("wIFMAuthority", IdcConstantSets.IfmAuthority),
        AnsiString("lpszIFMIdentifier"));

    public static readonly StructLayout HexData = new("WFSIDCHEXDATA",
        DWord("ulLength"),
        Bytes("lpbData", "ulLength"));

    public static readonly StructLayout AppData = new("WFSIDCAPPDATA",
        StructPointer("lpAID", HexData),
        StructPointer("lpKernelIdentifier", HexData));

    public static readonly StructLayout RetainCard = new("WFSIDCRETAINCARD",
        Word("usCount"),
        Word("fwPosition", IdcConstantSets.Media));

    /// <summary>lpbData is followed by a single null byte.</summary>
    public static readonly StructLayout CardData = new("WFSIDCCARDDATA",
        Word("wDataSource", IdcConstantSets.DataSource),
        Word("wStatus", IdcConstantSets.DataStatus),
        DWord("ulDataLength"),
        Bytes("lpbData", "ulDataLength", nullTerminated: true),
        Word("fwWriteMethod", IdcConstantSets.WriteMethod));

    public static readonly StructLayout ChipIo = new("WFSIDCCHIPIO",
        Word("wChipProtocol", IdcConstantSets.ChipProtocol),
        DWord("ulChipDataLength"),
        Bytes("lpbChipData", "ulChipDataLength"));

    public static readonly StructLayout ChipPowerOut = new("WFSIDCCHIPPOWEROUT",
        DWord("ulChipDataLength"),
        Bytes("lpbChipData", "ulChipDataLength"));

    public static readonly StructLayout TrackEvent = new("WFSIDCTRACKEVENT",
        Word("fwStatus", IdcConstantSets.DataStatus),
        AnsiString("lpstrTrack"),
        AnsiString("lpstrData"));

    public static readonly StructLayout PowerSaveChange = new("WFSIDCPOWERSAVECHANGE",
        Word("usPowerSaveRecoveryTime"));
}

/// <summary>Result and event layouts (what lpBuffer points at).</summary>
public static class IdcResults
{
    public static readonly NativeLayout Status = NativeLayout.Of(IdcStructs.Status);
    public static readonly NativeLayout Caps = NativeLayout.Of(IdcStructs.Caps);
    public static readonly NativeLayout FormList = NativeLayout.MultiString("lpszFormList");
    public static readonly NativeLayout Form = NativeLayout.Of(IdcStructs.Form);
    public static readonly NativeLayout IfmIdentifiers = NativeLayout.ArrayOf(IdcStructs.IfmIdentifier, "wIFMAuthority");
    public static readonly NativeLayout Applications = NativeLayout.ArrayOf(IdcStructs.AppData);
    public static readonly NativeLayout CardData = NativeLayout.ArrayOf(IdcStructs.CardData, "wDataSource");
    public static readonly NativeLayout RetainCard = NativeLayout.Of(IdcStructs.RetainCard);
    public static readonly NativeLayout ChipIo = NativeLayout.Of(IdcStructs.ChipIo);
    public static readonly NativeLayout ChipPowerOut = NativeLayout.Of(IdcStructs.ChipPowerOut);

    /// <summary>LPSTR lpstrTrackData of READ_TRACK and PARSE_DATA ("TRACK2:PAN=..\0..\0\0").</summary>
    public static readonly NativeLayout TrackData = NativeLayout.Bytes("lpstrTrackData");

    public static readonly NativeLayout TrackEvent = NativeLayout.Of(IdcStructs.TrackEvent);
    public static readonly NativeLayout PowerSaveChange = NativeLayout.Of(IdcStructs.PowerSaveChange);
    public static readonly NativeLayout RetainBinThreshold = NativeLayout.Word("fwRetainBin", IdcConstantSets.RetainBin);
    public static readonly NativeLayout MediaDetected = NativeLayout.Word("lpwResetOut", IdcConstantSets.ResetOut);
}
