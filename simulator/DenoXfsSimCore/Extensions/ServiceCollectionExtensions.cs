using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Messaging;
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
        services.AddSingleton<ISessionRegistry, SessionRegistry>();
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
