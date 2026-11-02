namespace DenoXfs.Sim.Idc;

/// <summary>Values from XFSIDC.H (spec 3.40). IDC_SERVICE_OFFSET = WFS_SERVICE_CLASS_IDC * 100 = 200.</summary>
public static class IdcConstants
{
    // Info categories
    public const uint StatusCategory = 201;
    public const uint CapabilitiesCategory = 202;
    public const uint FormListCategory = 203;
    public const uint QueryFormCategory = 204;
    public const uint QueryIfmIdentifierCategory = 205;
    public const uint EmvclessQueryApplicationsCategory = 206;

    // Commands
    public const uint ReadTrackCommand = 201;
    public const uint WriteTrackCommand = 202;
    public const uint EjectCardCommand = 203;
    public const uint RetainCardCommand = 204;
    public const uint ResetCountCommand = 205;
    public const uint SetkeyCommand = 206;
    public const uint ReadRawDataCommand = 207;
    public const uint WriteRawDataCommand = 208;
    public const uint ChipIoCommand = 209;
    public const uint ResetCommand = 210;
    public const uint ChipPowerCommand = 211;
    public const uint ParseDataCommand = 212;
    public const uint SetGuidanceLightCommand = 213;
    public const uint PowerSaveControlCommand = 214;
    public const uint ParkCardCommand = 215;
    public const uint SynchronizeCommand = 219;

    // Events
    public const uint InvalidTrackDataEvent = 201;   // WFS_EXEE_IDC_INVALIDTRACKDATA, payload WFSIDCTRACKEVENT
    public const uint MediaInsertedEvent = 203;      // WFS_EXEE_IDC_MEDIAINSERTED
    public const uint MediaRemovedEvent = 204;       // WFS_SRVE_IDC_MEDIAREMOVED
    public const uint RetainBinThresholdEvent = 206; // WFS_USRE_IDC_RETAINBINTHRESHOLD, payload LPWORD fwRetainBin
    public const uint MediaRetainedEvent = 208;      // WFS_EXEE_IDC_MEDIARETAINED
    public const uint MediaDetectedEvent = 209;      // WFS_SRVE_IDC_MEDIADETECTED, payload LPWORD lpwResetOut
    public const uint InsertCardEvent = 212;         // WFS_EXEE_IDC_INSERTCARD
    public const uint PowerSaveChangeEvent = 214;    // WFS_SRVE_IDC_POWER_SAVE_CHANGE, payload WFSIDCPOWERSAVECHANGE

    // Forms
    public const ushort ActionRead = 0x0001;
    public const ushort ActionWrite = 0x0002;

    // Write methods (fwWriteMode in the capabilities)
    public const ushort WriteLoco = 0x0002;
    public const ushort WriteHico = 0x0004;
    public const ushort WriteAuto = 0x0008;

    // Guidance lights: dwGuidLights index of the card unit and the WFS_IDC_GUIDANCE_* flags
    public const int GuidanceCardUnit = 0;
    public const int GuidLightsMax = 31;
    public const uint GuidanceOff = 0x0001;
    public const uint GuidanceSlowFlash = 0x0004;
    public const uint GuidanceMediumFlash = 0x0008;
    public const uint GuidanceQuickFlash = 0x0010;
    public const uint GuidanceContinuous = 0x0080;
    public const uint GuidanceRed = 0x0100;
    public const uint GuidanceGreen = 0x0200;
    public const uint GuidanceYellow = 0x0400;

    // IFM authority
    public const ushort IfmEmv = 1;

    // Data sources (lpwReadData, wDataSource)
    public const ushort Track1 = 0x0001;
    public const ushort Track2 = 0x0002;
    public const ushort Track3 = 0x0004;
    public const ushort Chip = 0x0008;
    public const ushort Security = 0x0010;
    public const ushort FluxInactive = 0x0020;
    public const ushort MemoryChip = 0x0040;
    public const ushort FrontTrack1 = 0x0080;
    public const ushort FrontImage = 0x0100;
    public const ushort BackImage = 0x0200;
    public const ushort Track1Jis1 = 0x0400;
    public const ushort Track3Jis1 = 0x0800;
    public const ushort Ddi = 0x4000;
    public const ushort TrackWm = 0x8000;

    // wStatus of WFSIDCCARDDATA
    public const ushort DataOk = 0;
    public const ushort DataMissing = 1;
    public const ushort DataSrcNotSupp = 5;

    // fwDevice
    public const ushort DevOnline = 0;

    // fwMedia
    public const ushort MediaPresent = 1;
    public const ushort MediaNotPresent = 2;
    public const ushort MediaEntering = 6;

    // fwRetainBin
    public const ushort RetainBinOk = 1;
    public const ushort RetainBinFull = 3;
    public const ushort RetainBinHigh = 4;

    // fwSecurity / fwSecType
    public const ushort SecNotSupp = 1;

    // fwChipPower (status)
    public const ushort ChipOnline = 0;
    public const ushort ChipPoweredOff = 1;
    public const ushort ChipNoCard = 5;

    // Modules (status)
    public const ushort ChipModOk = 1;
    public const ushort MagModOk = 1;
    public const ushort MagModNotSupp = 4;
    public const ushort ImgModNotSupp = 4;
    public const ushort DevicePosNotSupp = 3;
    public const ushort AfmNotSupp = 0;

    // Capabilities
    public const ushort ServiceClassIdc = 2;
    public const ushort TypeMotor = 1;
    public const ushort ChipT0 = 0x0001;
    public const ushort ChipT1 = 0x0002;
    public const ushort ChipPowerCold = 0x0002;
    public const ushort ChipPowerWarm = 0x0004;
    public const ushort ChipPowerOff = 0x0008;
    public const ushort ExitPosition = 0x0001;

    // lpwResetIn
    public const ushort ResetNoAction = 1;
    public const ushort ResetEject = 2;
    public const ushort ResetRetain = 3;

    // lpwResetOut (WFS_SRVE_IDC_MEDIADETECTED)
    public const ushort CardRetained = 1;
    public const ushort CardEjected = 2;
    public const ushort CardReadPosition = 3;

    // Errors
    public const int ErrNoMedia = -201;
    public const int ErrRetainBinFull = -203;
    public const int ErrInvalidData = -204;
    public const int ErrInvalidMedia = -205;
    public const int ErrFormNotFound = -206;
    public const int ErrFormInvalid = -207;
    public const int ErrInvalidKey = -213;
    public const int ErrCommandUnsupp = -228;
    public const int ErrProtocolNotSupp = -211;
    public const int ErrAtrNotObtained = -212;
}
