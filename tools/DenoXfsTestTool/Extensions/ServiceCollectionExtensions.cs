using DenoXfs.Core.Extensions;
using DenoXfs.TestTool.Logging;
using DenoXfs.TestTool.Operations;
using DenoXfs.TestTool.Operations.Idc;
using DenoXfs.TestTool.Session;
using DenoXfs.TestTool.Settings;
using DenoXfs.TestTool.Ui;
using Microsoft.Extensions.DependencyInjection;

namespace DenoXfs.TestTool.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>Registers the XFS core, the session, the operation definitions and the UI.</summary>
    public static IServiceCollection AddDenoXfsTestTool(this IServiceCollection services, IUiLogFeed logFeed)
    {
        services.AddDenoXfsCore("DenoXfsTestTool");

        services.AddSingleton(logFeed);
        services.AddSingleton<ISettingsStore, JsonSettingsStore>();
        services.AddSingleton<ILogicalServiceCatalog, XfsConfigLogicalServiceCatalog>();
        services.AddSingleton(sp => WithConfiguredLogicalName(sp.GetRequiredService<ISettingsStore>().Load(), sp.GetRequiredService<ILogicalServiceCatalog>()));
        services.AddSingleton<ISessionController, SessionController>();
        services.AddSingleton<OperationInvoker>();
        services.AddSingleton<IOperationInvoker>(sp => sp.GetRequiredService<OperationInvoker>());
        services.AddSingleton<ITrackedInvoker>(sp => sp.GetRequiredService<OperationInvoker>());
        services.AddSingleton<ICallCanceller>(sp => sp.GetRequiredService<OperationInvoker>());
        services.AddSingleton<IOperationRunner, OperationRunner>();

        // XFS log on screen: spec names per device class, traffic log, events.
        services.AddSingleton<IXfsCodeNameSource, ApiCodeNames>();
        services.AddSingleton<IXfsCodeNameSource, IdcCodeNames>();
        services.AddSingleton<IXfsCodeNames, XfsCodeNames>();
        services.AddSingleton<IXfsTrafficLog, XfsTrafficLog>();
        services.AddHostedService<XfsEventLogger>();
        foreach (var decoder in ApiEventDecoders.All.Concat(IdcEventDecoders.All))
            services.AddSingleton(decoder);
        services.AddSingleton<IEventPayloadDecoders, EventPayloadDecoders>();

        // Operation definitions: one line per command. A generated device module registers its own here.
        services.AddSingleton<IOperationDefinition, IdcStatusOperation>();
        services.AddSingleton<IOperationDefinition, IdcCapabilitiesOperation>();
        services.AddSingleton<IOperationDefinition, IdcReadRawDataOperation>();
        services.AddSingleton<IOperationDefinition, IdcEjectCardOperation>();
        services.AddSingleton<IOperationDefinition, IdcRetainCardOperation>();
        services.AddSingleton<IOperationDefinition, IdcResetCountOperation>();
        services.AddSingleton<IOperationDefinition, IdcResetOperation>();
        services.AddSingleton<IOperationDefinition, IdcChipPowerOperation>();
        services.AddSingleton<IOperationDefinition, IdcChipIoOperation>();
        services.AddSingleton<IOperationDefinition, IdcFormListOperation>();
        services.AddSingleton<IOperationDefinition, IdcQueryFormOperation>();
        services.AddSingleton<IOperationDefinition, IdcQueryIfmIdentifierOperation>();
        services.AddSingleton<IOperationDefinition, IdcEmvclessQueryApplicationsOperation>();
        services.AddSingleton<IOperationDefinition, IdcReadTrackOperation>();
        services.AddSingleton<IOperationDefinition, IdcWriteTrackOperation>();
        services.AddSingleton<IOperationDefinition, IdcWriteRawDataOperation>();
        services.AddSingleton<IOperationDefinition, IdcParseDataOperation>();
        services.AddSingleton<IOperationDefinition, IdcSetkeyOperation>();
        services.AddSingleton<IOperationDefinition, IdcSetGuidanceLightOperation>();
        services.AddSingleton<IOperationDefinition, IdcPowerSaveControlOperation>();
        services.AddSingleton<IOperationDefinition, IdcParkCardOperation>();
        services.AddSingleton<IOperationDefinition, IdcSynchronizeOperation>();
        services.AddSingleton<IOperationDefinition>(_ => IdcContactlessOperation.Configure());
        services.AddSingleton<IOperationDefinition>(_ => IdcContactlessOperation.PerformTransaction());
        services.AddSingleton<IOperationDefinition>(_ => IdcContactlessOperation.IssuerUpdate());

        services.AddSingleton<SettingsPanel>();
        services.AddSingleton<SessionPanel>();
        services.AddSingleton<XfsLogPanel>();
        services.AddSingleton<MainForm>();
        return services;
    }

    /// <summary>A stored logical name that is no longer configured gives way to the first configured one.</summary>
    private static TestToolSettings WithConfiguredLogicalName(TestToolSettings settings, ILogicalServiceCatalog logicalServices)
    {
        var configured = logicalServices.All();
        if (configured.Count > 0 && !configured.Any(s => string.Equals(s.Name, settings.LogicalName, StringComparison.OrdinalIgnoreCase)))
            settings.LogicalName = configured[0].Name;
        return settings;
    }
}
