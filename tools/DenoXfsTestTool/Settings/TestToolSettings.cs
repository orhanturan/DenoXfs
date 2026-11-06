using System.ComponentModel;

namespace DenoXfs.TestTool.Settings;

/// <summary>Editable settings of the test tool. Defaults follow the spec; edit them in the Session tab.</summary>
public class TestToolSettings
{
    [Category("Service"), DisplayName("Logical name"), Description("Logical service to open, one of HKEY_USERS\\.DEFAULT\\XFS\\LOGICAL_SERVICES. The simulator accepts it only if the terminal's model has a service of that name (e.g. Idc340).")]
    public string LogicalName { get; set; } = string.Empty;

    [Category("Versions"), DisplayName("StartUp versions required"), Description("WFSStartUp dwVersionsRequired: low word = highest, high word = lowest (3.00 - 3.40 = 0x00032803).")]
    public uint StartUpVersionsRequired { get; set; } = 0x00032803;

    [Category("Versions"), DisplayName("Service versions required"), Description("WFSOpen dwSrvcVersionsRequired (3.00 - 3.40 = 0x00032803).")]
    public uint ServiceVersionsRequired { get; set; } = 0x00032803;

    [Category("Open sequence"), DisplayName("Trace level"), Description("Trace level passed to open and to WFMSetTraceLevel.")]
    public uint TraceLevel { get; set; }

    [Category("Open sequence"), DisplayName("Set trace level after open"), Description("Call WFMSetTraceLevel after a successful open.")]
    public bool SetTraceLevelAfterOpen { get; set; } = true;

    [Category("Open sequence"), DisplayName("Register after open"), Description("Call WFSRegister after a successful open.")]
    public bool RegisterAfterOpen { get; set; } = true;

    [Category("Open sequence"), DisplayName("Event classes"), Description("Event classes registered after open.")]
    public EventClasses EventClasses { get; set; } = EventClasses.All;

    [Category("Call modes"), DisplayName("Open"), Description("WFSAsyncOpen or WFSOpen.")]
    public CallMode OpenMode { get; set; } = CallMode.Async;

    [Category("Call modes"), DisplayName("Register"), Description("WFSAsyncRegister or WFSRegister.")]
    public CallMode RegisterMode { get; set; } = CallMode.Async;

    [Category("Call modes"), DisplayName("Close"), Description("WFSAsyncClose or WFSClose.")]
    public CallMode CloseMode { get; set; } = CallMode.Async;

    [Category("Call modes"), DisplayName("GetInfo"), Description("WFSAsyncGetInfo or WFSGetInfo.")]
    public CallMode GetInfoMode { get; set; } = CallMode.Async;

    [Category("Call modes"), DisplayName("Execute"), Description("WFSAsyncExecute or WFSExecute.")]
    public CallMode ExecuteMode { get; set; } = CallMode.Async;

    [Category("Timeouts"), DisplayName("Timeout (ms)"), Description("Timeout of every call in milliseconds; 0 waits indefinitely.")]
    public uint TimeoutMs { get; set; } = 60000;
}
