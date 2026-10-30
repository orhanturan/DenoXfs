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
        
        // Command builders (Transient - new instance per request)
        services.AddTransient<IOpenCommandBuilder, OpenCommandBuilder>();
        services.AddTransient<IExecuteCommandBuilder, ExecuteCommandBuilder>();
        services.AddTransient<IGetInfoCommandBuilder, GetInfoCommandBuilder>();
        
        // Command builder factory (Singleton)
        services.AddSingleton<ICommandBuilderFactory, CommandBuilderFactory>();
        
        services.AddSingleton<IXfsMessageProcessor, XfsMessageProcessor>();
        services.AddHostedService(sp => (IHostedService)sp.GetRequiredService<IXfsMessageProcessor>());
        services.AddSingleton<IXfsService, XfsService>();

        return services;
    }
}

