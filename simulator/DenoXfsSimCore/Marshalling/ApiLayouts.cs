using static DenoXfs.Sim.Core.Marshalling.NativeField;

namespace DenoXfs.Sim.Core.Marshalling;

/// <summary>XFSAPI.H values and structures shared by every device class.</summary>
public static class ApiLayouts
{
    /// <summary>WFS_SYSE_DEVICE_STATUS, payload WFSDEVSTATUS.</summary>
    public const uint DeviceStatusEvent = 4;

    /// <summary>dwState of WFSDEVSTATUS (WFS_STAT_DEV*); the device classes use the same values for fwDevice.</summary>
    public static readonly ConstantSet DeviceState = new("WFS_STAT_DEV",
        ("ONLINE", 0), ("OFFLINE", 1), ("POWEROFF", 2), ("NODEVICE", 3), ("HWERROR", 4), ("USERERROR", 5), ("BUSY", 6),
        ("FRAUDATTEMPT", 7), ("POTENTIALFRAUD", 8));

    public static readonly StructLayout DevStatus = new("WFSDEVSTATUS",
        AnsiString("lpszPhysicalName"),
        AnsiString("lpszWorkstationName"),
        DWord("dwState", DeviceState));

    public static readonly NativeLayout DevStatusEvent = NativeLayout.Of(DevStatus);
}
