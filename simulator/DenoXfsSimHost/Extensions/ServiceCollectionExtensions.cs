using DenoXfs.Sim.Core.Extensions;
using DenoXfs.Sim.Data.Extensions;
using Microsoft.Data.Sqlite;
using DenoXfs.Sim.Host.Transport;
using DenoXfs.Sim.Idc.Extensions;

namespace DenoXfs.Sim.Host.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>Registers the transport, the database, the device-agnostic core and the device class modules.</summary>
    public static IServiceCollection AddDenoXfsSimHost(this IServiceCollection services, IConfiguration configuration, string contentRoot)
    {
        services.AddSingleton<IWebSocketSessionRunner, WebSocketSessionRunner>();
        services.AddDenoXfsSimData(DatabaseConnection(configuration, contentRoot));
        services.AddDenoXfsSimCore();
        services.AddDenoXfsSimIdc();
        return services;
    }

    /// <summary>ConnectionStrings:Simulator with a relative database file resolved against the content root.</summary>
    private static string DatabaseConnection(IConfiguration configuration, string contentRoot)
    {
        var builder = new SqliteConnectionStringBuilder(configuration.GetConnectionString("Simulator") ?? "Data Source=data/DenoXfsSim.db");
        if (!Path.IsPathRooted(builder.DataSource) && builder.DataSource != ":memory:")
            builder.DataSource = Path.Combine(contentRoot, builder.DataSource);
        return builder.ToString();
    }
}
