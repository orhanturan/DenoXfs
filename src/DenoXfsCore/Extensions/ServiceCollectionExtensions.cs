using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DenoXfs.Core.Abstractions;
using DenoXfs.Core.Commands;
using DenoXfs.Core.Services;

namespace DenoXfs.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDenoXfsCore(this IServiceCollection services, string appName)
    {
        services.AddSingleton<IXfsApp>(sp => new XfsApp(appName));
        services.AddSingleton<IXfsSession, XfsSession>();
        services.AddSingleton<IXfsResultReleaser, XfsResultReleaser>();

        // Command builders (Transient - new instance per request)
        services.AddTransient<IOpenCommandBuilder, OpenCommandBuilder>();
        services.AddTransient<ICloseCommandBuilder, CloseCommandBuilder>();
        services.AddTransient<IRegisterCommandBuilder, RegisterCommandBuilder>();
        services.AddTransient<IExecuteCommandBuilder, ExecuteCommandBuilder>();
        services.AddTransient<IGetInfoCommandBuilder, GetInfoCommandBuilder>();

        // Command builder factory (Singleton)
        services.AddSingleton<ICommandBuilderFactory, CommandBuilderFactory>();

        // One message processor instance serves as processor, event source and hosted service
        services.AddSingleton<XfsMessageProcessor>();
        services.AddSingleton<IXfsMessageProcessor>(sp => sp.GetRequiredService<XfsMessageProcessor>());
        services.AddSingleton<IXfsEventSource>(sp => sp.GetRequiredService<XfsMessageProcessor>());
        services.AddHostedService(sp => sp.GetRequiredService<XfsMessageProcessor>());
        services.AddSingleton<IXfsService, XfsService>();
        services.AddSingleton<IXfsSyncService, XfsSyncService>();

        return services;
    }
}
