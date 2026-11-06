using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DenoXfs.Sim.Data;

/// <summary>
/// The simulator database. Database names follow one convention (<see cref="ApplyNamingConvention"/>): tables deno_ +
/// singular snake_case entity name, columns snake_case, constraints pk_/fk_/ix_ + table (+ columns).
/// </summary>
public class SimDbContext : DbContext
{
    public const string TablePrefix = "deno_";

    public SimDbContext(DbContextOptions<SimDbContext> options) : base(options)
    {
    }

    public DbSet<Vendor> Vendors => Set<Vendor>();

    public DbSet<TerminalModel> TerminalModels => Set<TerminalModel>();

    public DbSet<ModelDevice> ModelDevices => Set<ModelDevice>();

    public DbSet<XfsService> XfsServices => Set<XfsService>();

    public DbSet<Terminal> Terminals => Set<Terminal>();

    public DbSet<Card> Cards => Set<Card>();

    public DbSet<CardChipResponse> CardChipResponses => Set<CardChipResponse>();

    public DbSet<DeviceData> DeviceData => Set<DeviceData>();

    public DbSet<DeviceState> DeviceStates => Set<DeviceState>();

    public DbSet<Profile> Profiles => Set<Profile>();

    public DbSet<ProfileSection> ProfileSections => Set<ProfileSection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Hardware catalog: vendor → model → device → service; deleting a parent deletes what is under it.
        modelBuilder.Entity<Vendor>().HasIndex(v => v.Name).IsUnique();
        modelBuilder.Entity<Vendor>().HasMany(v => v.Models).WithOne().HasForeignKey(m => m.VendorId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<TerminalModel>().HasIndex(m => new { m.VendorId, m.Name }).IsUnique();
        modelBuilder.Entity<TerminalModel>().HasMany(m => m.Devices).WithOne().HasForeignKey(d => d.ModelId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ModelDevice>().HasIndex(d => new { d.ModelId, d.Name }).IsUnique();
        modelBuilder.Entity<ModelDevice>().HasMany(d => d.Services).WithOne().HasForeignKey(s => s.ModelDeviceId).OnDelete(DeleteBehavior.Cascade);
        CaseInsensitive(modelBuilder.Entity<Vendor>().Property(v => v.Name));
        CaseInsensitive(modelBuilder.Entity<TerminalModel>().Property(m => m.Name));
        CaseInsensitive(modelBuilder.Entity<ModelDevice>().Property(d => d.Name));
        CaseInsensitive(modelBuilder.Entity<XfsService>().Property(s => s.Name));

        // Terminal: ISO 8583 Terminal Id as key (char(8), padded by the code, compared exactly); a model in use cannot be deleted.
        modelBuilder.Entity<Terminal>().HasKey(t => t.TerminalId);
        TerminalIdColumn(modelBuilder.Entity<Terminal>().Property(t => t.TerminalId));
        modelBuilder.Entity<Terminal>().HasIndex(t => t.Address1).IsUnique();
        modelBuilder.Entity<Terminal>().HasIndex(t => t.Address2).IsUnique();
        modelBuilder.Entity<Terminal>().HasOne<TerminalModel>().WithMany().HasForeignKey(t => t.ModelId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Terminal>().HasMany(t => t.DeviceStates).WithOne().HasForeignKey(s => s.TerminalId).OnDelete(DeleteBehavior.Cascade);
        TerminalIdColumn(modelBuilder.Entity<DeviceState>().Property(s => s.TerminalId));
        modelBuilder.Entity<DeviceState>().HasIndex(s => new { s.TerminalId, s.ModelDeviceId, s.XfsClass }).IsUnique();
        modelBuilder.Entity<DeviceState>().HasOne<ModelDevice>().WithMany().HasForeignKey(s => s.ModelDeviceId).OnDelete(DeleteBehavior.Cascade);
        // SQL Server would reject these two SET NULL paths to deno_profile (multiple cascade paths): make one NO ACTION there.
        modelBuilder.Entity<Terminal>().HasOne<Profile>().WithMany().HasForeignKey(t => t.ScenarioId).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Terminal>().HasOne<Profile>().WithMany().HasForeignKey(t => t.DeviceProfileId).OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Card>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Card>().HasMany(c => c.ChipResponses).WithOne().HasForeignKey(r => r.CardId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<CardChipResponse>().HasIndex(r => new { r.CardId, r.Command }).IsUnique();
        modelBuilder.Entity<DeviceData>().HasIndex(d => new { d.XfsClass, d.Category }).IsUnique();

        modelBuilder.Entity<Profile>().Property(p => p.Kind).HasConversion<string>();
        CaseInsensitive(modelBuilder.Entity<Profile>().Property(p => p.Name));
        modelBuilder.Entity<Profile>().HasIndex(p => new { p.Kind, p.Name }).IsUnique();
        modelBuilder.Entity<Profile>().HasMany(p => p.Sections).WithOne().HasForeignKey(s => s.ProfileId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ProfileSection>().HasIndex(s => new { s.ProfileId, s.XfsClass }).IsUnique();

        ApplyNamingConvention(modelBuilder.Model);
    }

    /// <summary>NOCASE exists only in SQLite; SQL Server compares case insensitive by default, PostgreSQL needs citext or a collation.</summary>
    private void CaseInsensitive(PropertyBuilder<string> property)
    {
        if (Database.IsSqlite())
            property.UseCollation("NOCASE");
    }

    /// <summary>char(8), not unicode. SQLite compares exactly (BINARY); on SQL Server add a case sensitive collation here.</summary>
    private static void TerminalIdColumn(PropertyBuilder<string> property) =>
        property.HasMaxLength(8).IsFixedLength().IsUnicode(false);

    /// <summary>deno_ + singular snake_case table names, snake_case columns, pk_/fk_/ix_ constraint names.</summary>
    private static void ApplyNamingConvention(IMutableModel model)
    {
        foreach (var entity in model.GetEntityTypes())
        {
            var table = TablePrefix + SnakeCase(entity.ClrType.Name);
            entity.SetTableName(table);
            foreach (var property in entity.GetProperties())
                property.SetColumnName(SnakeCase(property.Name));
            foreach (var key in entity.GetKeys())
                key.SetName(key.IsPrimaryKey() ? $"pk_{table}" : $"ak_{table}_{Columns(key.Properties)}");
            foreach (var foreignKey in entity.GetForeignKeys())
                foreignKey.SetConstraintName($"fk_{table}_{Columns(foreignKey.Properties)}");
            foreach (var index in entity.GetIndexes())
                index.SetDatabaseName($"ix_{table}_{Columns(index.Properties)}");
        }
    }

    private static string Columns(IEnumerable<IReadOnlyProperty> properties) => string.Join("_", properties.Select(p => SnakeCase(p.Name)));

    /// <summary>TerminalModel → terminal_model, XfsClass → xfs_class, Address1 → address1.</summary>
    public static string SnakeCase(string name)
    {
        var text = new StringBuilder(name.Length + 8);
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                var startsWord = i > 0 && (char.IsLower(name[i - 1]) || char.IsDigit(name[i - 1]) ||
                                           (char.IsUpper(name[i - 1]) && i + 1 < name.Length && char.IsLower(name[i + 1])));
                if (startsWord)
                    text.Append('_');
                text.Append(char.ToLowerInvariant(c));
            }
            else
            {
                text.Append(c);
            }
        }
        return text.ToString();
    }
}

/// <summary>Used by <c>dotnet ef migrations</c> only; the host configures the real connection.</summary>
public sealed class SimDbContextDesignFactory : IDesignTimeDbContextFactory<SimDbContext>
{
    public SimDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<SimDbContext>().UseSqlite("Data Source=design.db").Options);
}

/// <summary>Terminal lookups by the IP of a DLL connection or a browser.</summary>
public static class TerminalQueries
{
    /// <summary>The terminal with this IP as address1 or address2; the first by Terminal Id if an IP was entered twice.</summary>
    public static IQueryable<Terminal> OfAddress(this IQueryable<Terminal> terminals, string address) =>
        terminals.Where(t => t.Address1 == address || t.Address2 == address).OrderBy(t => t.TerminalId);
}
