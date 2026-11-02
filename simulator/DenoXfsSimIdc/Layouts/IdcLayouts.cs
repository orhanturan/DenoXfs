namespace DenoXfs.Sim.Idc.Layouts;

// Packed layouts (pack 1, 32 bit pointers) of the XFSIDC.H structures the simulator returns. BOOL is 4 bytes.

/// <summary>WFSIDCCARDDATA: 14 bytes.</summary>
public static class CardDataLayout
{
    public const int Size = 14;
    public const int DataSourceOffset = 0;
    public const int StatusOffset = 2;
    public const int DataLengthOffset = 4;
    public const int DataPointerOffset = 8;
    public const int WriteMethodOffset = 12;
}

/// <summary>WFSIDCSTATUS: 164 bytes.</summary>
public static class StatusLayout
{
    public const int GuidLightsCount = 32;
    public const int Size = 164;
    public const int Device = 0;
    public const int Media = 2;
    public const int RetainBin = 4;
    public const int Security = 6;
    public const int Cards = 8;
    public const int ChipPower = 10;
    public const int Extra = 12;                     // LPSTR
    public const int GuidLights = 16;                // DWORD[32]
    public const int ChipModule = 144;
    public const int MagReadModule = 146;
    public const int MagWriteModule = 148;
    public const int FrontImageModule = 150;
    public const int BackImageModule = 152;
    public const int DevicePosition = 154;
    public const int PowerSaveRecoveryTime = 156;
    public const int ParkingStationMedia = 158;      // LPWORD
    public const int AntiFraudModule = 162;
}

/// <summary>WFSIDCCAPS: 188 bytes.</summary>
public static class CapsLayout
{
    public const int Size = 188;
    public const int Class = 0;
    public const int Type = 2;
    public const int Compound = 4;                   // BOOL
    public const int ReadTracks = 8;
    public const int WriteTracks = 10;
    public const int ChipProtocols = 12;
    public const int Cards = 14;
    public const int SecType = 16;
    public const int PowerOnOption = 18;
    public const int PowerOffOption = 20;
    public const int FluxSensorProgrammable = 22;    // BOOL
    public const int ReadWriteAccessFollowingEject = 26; // BOOL
    public const int WriteMode = 30;
    public const int ChipPower = 32;
    public const int Extra = 34;                     // LPSTR
    public const int DipMode = 38;
    public const int MemoryChipProtocols = 40;       // LPWORD
    public const int GuidLights = 44;                // DWORD[32]
    public const int EjectPosition = 172;
    public const int PowerSaveControl = 174;         // BOOL
    public const int ParkingStations = 178;
    public const int AntiFraudModule = 180;          // BOOL
    public const int SynchronizableCommands = 184;   // LPDWORD
}

/// <summary>WFSIDCRETAINCARD: 4 bytes.</summary>
public static class RetainCardLayout
{
    public const int Size = 4;
    public const int Count = 0;
    public const int Position = 2;
}

/// <summary>WFSIDCCHIPIO: 10 bytes.</summary>
public static class ChipIoLayout
{
    public const int Size = 10;
    public const int Protocol = 0;
    public const int DataLength = 2;
    public const int DataPointer = 6;
}

/// <summary>WFSIDCFORM: 44 bytes.</summary>
public static class FormLayout
{
    public const int Size = 44;
    public const int FormName = 0;                   // LPSTR
    public const int FieldSeparatorTrack1 = 4;
    public const int FieldSeparatorTrack2 = 5;
    public const int FieldSeparatorTrack3 = 6;
    public const int Action = 7;
    public const int Tracks = 9;                     // LPSTR
    public const int Secure = 13;                    // BOOL
    public const int Track1Fields = 17;              // LPSTR, double null terminated list
    public const int Track2Fields = 21;
    public const int Track3Fields = 25;
    public const int FrontTrack1Fields = 29;
    public const int FieldSeparatorFrontTrack1 = 33;
    public const int Jis1Track1Fields = 34;
    public const int Jis1Track3Fields = 38;
    public const int FieldSeparatorJis1Track1 = 42;
    public const int FieldSeparatorJis1Track3 = 43;
}

/// <summary>WFSIDCIFMIDENTIFIER: 6 bytes.</summary>
public static class IfmIdentifierLayout
{
    public const int Size = 6;
    public const int Authority = 0;
    public const int Identifier = 2;                 // LPSTR
}

/// <summary>WFSIDCHEXDATA: 8 bytes.</summary>
public static class HexDataLayout
{
    public const int Size = 8;
    public const int Length = 0;
    public const int Data = 4;                       // LPBYTE
}

/// <summary>WFSIDCAPPDATA: 8 bytes.</summary>
public static class AppDataLayout
{
    public const int Size = 8;
    public const int Aid = 0;                        // LPWFSIDCHEXDATA
    public const int KernelIdentifier = 4;           // LPWFSIDCHEXDATA
}

/// <summary>WFSIDCTRACKEVENT: 10 bytes.</summary>
public static class TrackEventLayout
{
    public const int Size = 10;
    public const int Status = 0;
    public const int Track = 2;                      // LPSTR
    public const int Data = 6;                       // LPSTR
}

/// <summary>WFSIDCCHIPPOWEROUT: 8 bytes.</summary>
public static class ChipPowerOutLayout
{
    public const int Size = 8;
    public const int DataLength = 0;
    public const int DataPointer = 4;
}
