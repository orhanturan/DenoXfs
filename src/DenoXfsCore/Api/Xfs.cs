namespace DenoXfs.Core.Api;

/// <summary>
/// XFS (eXtensions for Financial Services) sabit değerleri.
/// </summary>
public static class Xfs
{
    public const int ApiServiceOffSet = 0;
    public const int WfsddescriptionLen = 256;
    public const int WfsdsysstatusLen = 256;
    public const int StatDevOnline = 0;
    public const int StatDevOffline = 1;
    public const int StatDevPowerOff = 2;
    public const int StatDevNodevice = 3;
    public const int StatDevHwError = 4;
    public const int StatDevUserError = 5;
    public const int StatDevBusy = 6;
    public const int StatDevFraudAttempt = 7;
    public const int StatDevPotentialFraud = 8;
    public const int DefaultHapp = 0;
    public const int ApiTransActive = 0;
    public const int ApiTransInactive = 1;
    public const int Success = 0;
    public const int IndefiniteWait = 0;
    public const int ServiceEvents = 1;
    public const int UserEvents = 2;
    public const int SystemEvents = 4;
    public const int ExecuteEvents = 8;
    public const int SyseUndeliverableMsg = 1;
    public const int SyseHardwareError = 2;
    public const int SyseVersionError = 3;
    public const int SyseDeviceStatus = 4;
    public const int SyseAppDisconnect = 5;
    public const int SyseSoftwareError = 6;
    public const int SyseUserError = 7;
    public const int SyseLockRequested = 8;
    public const int SyseFraudAttempt = 9;
    public const ushort TraceApi = 0x00000001;
    public const ushort TraceAllApi = 0x00000002;
    public const ushort TraceSpi = 0x00000004;
    public const ushort TraceAllSpi = 0x00000008;
    public const ushort TraceMgr = 0x00000010;
    public const int ErrActNoaction = 0x0000;
    public const int ErrActReset = 0x0001;
    public const int ErrActSwerror = 0x0002;
    public const int ErrActConfig = 0x0004;
    public const int ErrActHwClear = 0x0008;
    public const int ErrActHwMaint = 0x0010;
    public const int ErrActSuspend = 0x0020;
    public const int InfApiTransactionState = 1;
    public const int InfApiServiceInfo = 2;
    public const int CmdApiSetTransactionState = 1;
    public const int SrveApiStatusChanged = 1;
    public const int ExeeApiErrorInfo = 2;
    public const uint OpenComplete = 1025;
    public const uint CloseComplete = 1026;
    public const uint LockComplete = 1027;
    public const uint UnlockComplete = 1028;
    public const uint RegisterComplete = 1029;
    public const uint DeregisterComplete = 1030;
    public const uint GetinfoComplete = 1031;
    public const uint ExecuteComplete = 1032;
    public const uint ExecuteEvent = 1044;
    public const uint ServiceEvent = 1045;
    public const uint UserEvent = 1046;
    public const uint SystemEvent = 1047;
    public const uint TimerEvent = 1124;

    // Header Enums

    // XfsMessage

    // XfsErrorCode

    // XfsEventClasses

    // XfsEventId

    // XfsTraceLevels

    // XfsErrorActions

    // Platform
    public const uint WmXfsProcessRequest = 0x8001u;
}
