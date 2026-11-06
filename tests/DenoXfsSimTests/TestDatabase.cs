using DenoXfs.Sim.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DenoXfs.Sim.Tests;

/// <summary>
/// A PostgreSQL database of its own for one test class instance: deno_xfs_sim_test_&lt;guid&gt;, built with the real migrations,
/// dropped at <see cref="Dispose"/>. Server, user and password come from the host's ConnectionStrings:Simulator (user secrets
/// shared with DenoXfsSimHost, or the ConnectionStrings__Simulator environment variable); its database is never touched.
/// </summary>
internal sealed class TestDatabase : IDbContextFactory<SimDbContext>, IDisposable
{
    private readonly DbContextOptions<SimDbContext> _options;

    public TestDatabase()
    {
        var connection = new NpgsqlConnectionStringBuilder(ServerConnection()) { Database = $"deno_xfs_sim_test_{Guid.NewGuid():N}" };
        _options = new DbContextOptionsBuilder<SimDbContext>().UseNpgsql(connection.ConnectionString).Options;
        using var db = CreateDbContext();
        db.Database.Migrate();
    }

    public SimDbContext CreateDbContext() => new(_options);

    public void Dispose()
    {
        using var db = CreateDbContext();
        db.Database.EnsureDeleted();
    }

    private static string ServerConnection()
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<TestDatabase>().AddEnvironmentVariables().Build();
        return configuration.GetConnectionString("Simulator") is { Length: > 0 } connection
            ? connection
            : throw new InvalidOperationException(
                "ConnectionStrings:Simulator is not set: Manage User Secrets on DenoXfsSimHost (the tests share them) or set ConnectionStrings__Simulator.");
    }
}
