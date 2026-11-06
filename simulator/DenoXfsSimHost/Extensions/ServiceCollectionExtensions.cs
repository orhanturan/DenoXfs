using DenoXfs.Sim.Core.Extensions;
using DenoXfs.Sim.Data.Extensions;
using DenoXfs.Sim.Host.Transport;
using DenoXfs.Sim.Idc.Extensions;

namespace DenoXfs.Sim.Host.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>Registers the transport, the database, the device-agnostic core and the device class modules.</summary>
    public static IServiceCollection AddDenoXfsSimHost(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IWebSocketSessionRunner, WebSocketSessionRunner>();
        services.AddDenoXfsSimData(DatabaseConnection(configuration));
        services.AddDenoXfsSimCore();
        services.AddDenoXfsSimIdc();
        return services;
    }

    /// <summary>
    /// ConnectionStrings:Simulator. It holds the password, so it is not in appsettings.json: user secrets in development,
    /// the ConnectionStrings__Simulator environment variable elsewhere.
    /// </summary>
    private static string DatabaseConnection(IConfiguration configuration) =>
        configuration.GetConnectionString("Simulator") is { Length: > 0 } connection
            ? connection
            : throw new InvalidOperationException(
                "ConnectionStrings:Simulator is not set. Development: Manage User Secrets on DenoXfsSimHost; elsewhere: environment variable ConnectionStrings__Simulator.");
}
