using DenoXfs.Api;

namespace DenoXfs.TestTool.Operations;

/// <summary>System events of XFSAPI.H, sent by every device class. Hand written for now; HtmlToXfs2 will generate it.</summary>
public class ApiCodeNames : IXfsCodeNameSource
{
    public IEnumerable<(XfsCodeKind Kind, uint Code, string Name)> Names =>
    [
        (XfsCodeKind.SystemEvent, Xfs.SyseUndeliverableMsg, "WFS_SYSE_UNDELIVERABLE_MSG"),
        (XfsCodeKind.SystemEvent, Xfs.SyseHardwareError, "WFS_SYSE_HARDWARE_ERROR"),
        (XfsCodeKind.SystemEvent, Xfs.SyseVersionError, "WFS_SYSE_VERSION_ERROR"),
        (XfsCodeKind.SystemEvent, Xfs.SyseDeviceStatus, "WFS_SYSE_DEVICE_STATUS"),
        (XfsCodeKind.SystemEvent, Xfs.SyseAppDisconnect, "WFS_SYSE_APP_DISCONNECT"),
        (XfsCodeKind.SystemEvent, Xfs.SyseSoftwareError, "WFS_SYSE_SOFTWARE_ERROR"),
        (XfsCodeKind.SystemEvent, Xfs.SyseUserError, "WFS_SYSE_USER_ERROR"),
        (XfsCodeKind.SystemEvent, Xfs.SyseLockRequested, "WFS_SYSE_LOCK_REQUESTED"),
        (XfsCodeKind.SystemEvent, Xfs.SyseFraudAttempt, "WFS_SYSE_FRAUD_ATTEMPT")
    ];
}
