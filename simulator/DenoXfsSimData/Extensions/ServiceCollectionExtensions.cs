using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Scenarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DenoXfs.Sim.Data.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// PostgreSQL database of the simulator (EF Core). Device class modules read it through <see cref="IDbContextFactory{TContext}"/>
    /// and add their seed data as <see cref="IDataSeeder"/>s.
    /// </summary>
    public static IServiceCollection AddDenoXfsSimData(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextFactory<SimDbContext>(options => options.UseNpgsql(connectionString));
        services.AddHostedService<DatabaseInitializer>();
        services.AddSingleton<IProfileCatalog, EfProfileCatalog>();
        services.AddSingleton<ITerminalSelectionStore, EfTerminalSelectionStore>();
        services.AddSingleton<IHardwareStore, EfHardwareStore>();
        services.AddSingleton<ITerminalStore, EfTerminalStore>();
        services.AddSingleton<IDataSeeder, HardwareSeeder>();
        return services;
    }
}
