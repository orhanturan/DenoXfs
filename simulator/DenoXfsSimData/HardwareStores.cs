using DenoXfs.Sim.Core.Hardware;
using Microsoft.EntityFrameworkCore;

namespace DenoXfs.Sim.Data;

/// <summary>Vendor, model, device and service tables. The catalog (core) has checked the rules before a save.</summary>
public sealed class EfHardwareStore : IHardwareStore
{
    private readonly IDbContextFactory<SimDbContext> _contexts;

    public EfHardwareStore(IDbContextFactory<SimDbContext> contexts)
    {
        _contexts = contexts;
    }

    public async Task<HardwareSnapshot> LoadAsync(CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var vendors = await db.Vendors.AsNoTracking()
            .Include(v => v.Models).ThenInclude(m => m.Devices).ThenInclude(d => d.Services)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
        var terminals = await db.Terminals.AsNoTracking().OrderBy(t => t.TerminalId).ToListAsync(cancellationToken);

        return new HardwareSnapshot(
            vendors.OrderBy(v => v.Name, StringComparer.OrdinalIgnoreCase).Select(ToInfo).ToList(),
            terminals.Select(t => new TerminalInfo(t.TerminalId, t.Address1, t.Address2, t.ModelId)).ToList());
    }

    public async Task<int> SaveAsync(VendorEdit vendor, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var row = vendor.Id == 0 ? db.Vendors.Add(new Vendor()).Entity : await db.Vendors.SingleAsync(v => v.Id == vendor.Id, cancellationToken);
        row.Name = vendor.Name;
        await db.SaveChangesAsync(cancellationToken);
        return row.Id;
    }

    public async Task<int> SaveAsync(ModelEdit model, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var row = model.Id == 0 ? db.TerminalModels.Add(new TerminalModel()).Entity : await db.TerminalModels.SingleAsync(m => m.Id == model.Id, cancellationToken);
        row.VendorId = model.VendorId;
        row.Name = model.Name;
        await db.SaveChangesAsync(cancellationToken);
        return row.Id;
    }

    public async Task<int> SaveAsync(DeviceEdit device, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var row = device.Id == 0 ? db.ModelDevices.Add(new ModelDevice()).Entity : await db.ModelDevices.SingleAsync(d => d.Id == device.Id, cancellationToken);
        row.ModelId = device.ModelId;
        row.DeviceType = device.DeviceType;
        row.Name = device.Name;
        await db.SaveChangesAsync(cancellationToken);
        return row.Id;
    }

    public async Task<int> SaveAsync(ServiceEdit service, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var row = service.Id == 0 ? db.XfsServices.Add(new XfsService()).Entity : await db.XfsServices.SingleAsync(s => s.Id == service.Id, cancellationToken);
        row.ModelDeviceId = service.DeviceId;
        row.Name = service.Name;
        row.XfsClass = service.XfsClass;
        await db.SaveChangesAsync(cancellationToken);
        return row.Id;
    }

    /// <summary>The database cascades to the children (foreign keys ON DELETE CASCADE).</summary>
    public async Task DeleteAsync(HardwareItem item, int id, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var deleted = item switch
        {
            HardwareItem.Vendor => await db.Vendors.Where(v => v.Id == id).ExecuteDeleteAsync(cancellationToken),
            HardwareItem.Model => await db.TerminalModels.Where(m => m.Id == id).ExecuteDeleteAsync(cancellationToken),
            HardwareItem.Device => await db.ModelDevices.Where(d => d.Id == id).ExecuteDeleteAsync(cancellationToken),
            HardwareItem.Service => await db.XfsServices.Where(s => s.Id == id).ExecuteDeleteAsync(cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
        };
        if (deleted == 0)
            throw new InvalidOperationException($"{item} {id} does not exist.");
    }

    private static VendorInfo ToInfo(Vendor vendor) => new(vendor.Id, vendor.Name,
        vendor.Models.OrderBy(m => m.Name, StringComparer.OrdinalIgnoreCase).Select(m => new ModelInfo(m.Id, m.VendorId, m.Name,
            m.Devices.OrderBy(d => d.Name, StringComparer.OrdinalIgnoreCase).Select(d => new DeviceInfo(d.Id, d.ModelId, d.DeviceType, d.Name,
                d.Services.OrderBy(s => s.XfsClass, StringComparer.Ordinal).Select(s => new ServiceInfo(s.Id, s.ModelDeviceId, s.Name, s.XfsClass))
                    .ToList())).ToList())).ToList());
}

/// <summary>Terminal table. The catalog (core) has checked Terminal Id, addresses and model before a save.</summary>
public sealed class EfTerminalStore : ITerminalStore
{
    private readonly IDbContextFactory<SimDbContext> _contexts;

    public EfTerminalStore(IDbContextFactory<SimDbContext> contexts)
    {
        _contexts = contexts;
    }

    public async Task AddAsync(TerminalInfo terminal, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        db.Terminals.Add(new Terminal { TerminalId = terminal.TerminalId, Address1 = terminal.Address1, Address2 = terminal.Address2, ModelId = terminal.ModelId });
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TerminalInfo terminal, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        var row = await db.Terminals.SingleAsync(t => t.TerminalId == terminal.TerminalId, cancellationToken);
        row.Address1 = terminal.Address1;
        row.Address2 = terminal.Address2;
        row.ModelId = terminal.ModelId;
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string terminalId, CancellationToken cancellationToken)
    {
        await using var db = await _contexts.CreateDbContextAsync(cancellationToken);
        await db.Terminals.Where(t => t.TerminalId == terminalId).ExecuteDeleteAsync(cancellationToken);
    }
}

/// <summary>
/// Seeds an empty catalog with Deno → Deno26 → CardReader → Idc340 (IDC) and an empty terminal table with the dev
/// terminal DEV00001 on 127.0.0.1. Once the tables hold rows the seeder leaves them alone, so deletions on the web stay.
/// </summary>
public sealed class HardwareSeeder : IDataSeeder
{
    public const string DevTerminalId = "DEV00001";
    public const string DevAddress = "127.0.0.1";

    public int Order => 10;

    public async Task SeedAsync(SimDbContext db, CancellationToken cancellationToken)
    {
        if (!await db.Vendors.AnyAsync(cancellationToken))
        {
            db.Vendors.Add(new Vendor
            {
                Name = "Deno",
                Models =
                [
                    new TerminalModel
                    {
                        Name = "Deno26",
                        Devices = [new ModelDevice { Name = "CardReader", DeviceType = "CardReader", Services = [new XfsService { Name = "Idc340", XfsClass = "IDC" }] }]
                    }
                ]
            });
            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.Terminals.AnyAsync(cancellationToken))
        {
            var model = await db.TerminalModels.OrderBy(m => m.Id).FirstOrDefaultAsync(m => m.Name == "Deno26", cancellationToken)
                        ?? await db.TerminalModels.OrderBy(m => m.Id).FirstOrDefaultAsync(cancellationToken);
            if (model is null)
                return;
            db.Terminals.Add(new Terminal { TerminalId = DevTerminalId, Address1 = DevAddress, ModelId = model.Id });
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
