using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DenoXfs.Sim.Data;

public class SimDbContext : DbContext
{
    public SimDbContext(DbContextOptions<SimDbContext> options) : base(options)
    {
    }

    public DbSet<Terminal> Terminals => Set<Terminal>();

    public DbSet<Card> Cards => Set<Card>();

    public DbSet<CardChipResponse> CardChipResponses => Set<CardChipResponse>();

    public DbSet<DeviceData> DeviceData => Set<DeviceData>();

    public DbSet<DeviceState> DeviceStates => Set<DeviceState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Terminal>().HasIndex(t => t.Address1).IsUnique();
        modelBuilder.Entity<Card>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Card>().HasMany(c => c.ChipResponses).WithOne().HasForeignKey(r => r.CardId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CardChipResponse>().HasIndex(r => new { r.CardId, r.Command }).IsUnique();
        modelBuilder.Entity<DeviceData>().HasIndex(d => new { d.DeviceClass, d.Category }).IsUnique();
        modelBuilder.Entity<Terminal>().HasMany(t => t.DeviceStates).WithOne().HasForeignKey(s => s.TerminalId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<DeviceState>().HasIndex(s => new { s.TerminalId, s.DeviceClass }).IsUnique();
    }
}

/// <summary>Used by <c>dotnet ef migrations</c> only; the host configures the real connection.</summary>
public sealed class SimDbContextDesignFactory : IDesignTimeDbContextFactory<SimDbContext>
{
    public SimDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<SimDbContext>().UseSqlite("Data Source=design.db").Options);
}
