using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Marshalling;
using DenoXfs.Sim.Core.Messaging;
using DenoXfs.Sim.Core.Names;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Core.Ui;
using DenoXfs.Sim.Core.Versions;
using Microsoft.Extensions.DependencyInjection;

namespace DenoXfs.Sim.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDenoXfsSimCore(this IServiceCollection services)
    {
        services.AddSingleton(SupportedVersions.Spec340);
        services.AddSingleton<IVersionNegotiator, VersionNegotiator>();
        services.AddSingleton<INativeBufferBuilderFactory, NativeBufferBuilderFactory>();
        services.AddSingleton<INativeLayoutWriter, NativeLayoutWriter>();

        // Scenarios and device profiles; IProfileCatalog and ITerminalSelectionStore come from the database (AddDenoXfsSimData).
        services.AddSingleton<IJsonMerger, JsonMerger>();
        services.AddSingleton<ITerminalSelections, CachedTerminalSelections>();
        services.AddSingleton<IEffectiveProfileResolver, EffectiveProfileResolver>();
        services.AddSingleton<IProfileSelectionService, ProfileSelectionService>();
        services.AddSingleton<ISessionRegistry, SessionRegistry>();

        // Hardware catalog (vendor, model, device, service) and terminals; IHardwareStore and ITerminalStore come from the database.
        services.AddSingleton<IHardwareSnapshots, HardwareSnapshots>();
        services.AddSingleton<IHardwareCatalog, HardwareCatalog>();
        services.AddSingleton<ITerminalCatalog, TerminalCatalog>();
        services.AddSingleton<IServiceResolver, ServiceResolver>();

        // Commands and infos by device class: every IDeviceClassModule registers its handlers keyed by its class.
        services.AddSingleton<IClassHandlers<IExecuteCommand>>(sp => new ClassHandlers<IExecuteCommand>(
            sp.GetServices<IDeviceClassModule>().SelectMany(m => sp.GetKeyedServices<IExecuteCommand>(m.DeviceClass)
                .Select(c => new ClassHandler<IExecuteCommand>(m.DeviceClass, c.Command, c)))));
        services.AddSingleton<IClassHandlers<IInfoCommand>>(sp => new ClassHandlers<IInfoCommand>(
            sp.GetServices<IDeviceClassModule>().SelectMany(m => sp.GetKeyedServices<IInfoCommand>(m.DeviceClass)
                .Select(c => new ClassHandler<IInfoCommand>(m.DeviceClass, c.Category, c)))));

        // Web front end: change notifications, live XFS log with spec names (device modules add their IXfsCodeNameSource).
        services.AddSingleton<ITerminalNotifier, TerminalNotifier>();
        services.AddSingleton<ITrafficFeed, TrafficFeed>();
        services.AddSingleton<ITrafficRecorder, TrafficRecorder>();
        services.AddSingleton<IXfsCodeNameSource, ApiCodeNames>();
        services.AddSingleton<IXfsCodeNames, XfsCodeNames>();
        services.AddSingleton<IProfileSelectionListener, NotifyingSelectionListener>();
        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddSingleton<IRequestCancellation, RequestCancellation>();

        services.AddSingleton<IMessageHandler, HelloMessageHandler>();
        services.AddSingleton<IMessageHandler, ExecuteMessageHandler>();
        services.AddSingleton<IMessageHandler, GetInfoMessageHandler>();
        services.AddSingleton<IMessageHandler, CloseMessageHandler>();
        services.AddSingleton<IMessageHandler, CancelMessageHandler>();
        services.AddSingleton<IMessageDispatcher, MessageDispatcher>();

        return services;
    }
}
