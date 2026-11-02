using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Data;

/// <summary>Loads seed data into the database. Must be idempotent: it runs at every host start.</summary>
public interface IDataSeeder
{
    /// <summary>Seeders run in ascending order.</summary>
    int Order { get; }

    Task SeedAsync(SimDbContext db, CancellationToken cancellationToken);
}

/// <summary>At host start: creates the database folder, applies the migrations, runs every <see cref="IDataSeeder"/>.</summary>
public sealed class DatabaseInitializer : IHostedService
{
    private readonly IDbContextFactory<SimDbContext> _contexts;
    private readonly IEnumerable<IDataSeeder> _seeders;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(IDbContextFactory<SimDbContext> contexts, IEnumerable<IDataSeeder> seeders, ILogger<DatabaseInitializer> logger)
    {
        _contexts = contexts;
        _seeders = seeders;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        CreateFolder(db.Database.GetConnectionString());
        await db.Database.MigrateAsync(cancellationToken);

        foreach (var seeder in _seeders.OrderBy(s => s.Order))
        {
            await seeder.SeedAsync(db, cancellationToken);
            _logger.LogInformation("database seeded seeder={Seeder}", seeder.GetType().Name);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static void CreateFolder(string? connectionString)
    {
        var file = new SqliteConnectionStringBuilder(connectionString).DataSource;
        var folder = Path.GetDirectoryName(Path.GetFullPath(file));
        if (!string.IsNullOrEmpty(folder) && file != ":memory:")
            Directory.CreateDirectory(folder);
    }
}
