namespace DenoXfs.TestTool.Settings;

/// <summary>A logical service configured under LOGICAL_SERVICES: the WFSOpen name, its class and its provider key.</summary>
public sealed record LogicalService(string Name, string Class, string Provider);

/// <summary>The logical services WFSOpen can open on this machine.</summary>
public interface ILogicalServiceCatalog
{
    /// <summary>Every configured logical service by name; empty when the configuration cannot be read.</summary>
    IReadOnlyList<LogicalService> All();
}
