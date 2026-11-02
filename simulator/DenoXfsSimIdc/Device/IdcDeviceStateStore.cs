using System.Text.Json;
using DenoXfs.Sim.Data;
using Microsoft.EntityFrameworkCore;

namespace DenoXfs.Sim.Idc.Device;

/// <summary>What survives a host restart for one terminal's card reader: the settings and the retain count.</summary>
public sealed record IdcDeviceState(bool AutoRemoveAfterEject, int RetainCapacity, int RetainHighThreshold, int RetainCount);

public interface IIdcDeviceStateStore
{
    IdcDeviceState? Load(string address);

    void Save(string address, IdcDeviceState state);
}

/// <summary>DeviceState row (DeviceClass "IDC") of the terminal; the Terminal row is created on first save.</summary>
public sealed class EfDeviceStateStore : IIdcDeviceStateStore
{
    private const string DeviceClass = "IDC";
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);
    private readonly IDbContextFactory<SimDbContext> _contexts;

    public EfDeviceStateStore(IDbContextFactory<SimDbContext> contexts)
    {
        _contexts = contexts;
    }

    public IdcDeviceState? Load(string address)
    {
        using var db = _contexts.CreateDbContext();
        var json = db.Terminals.AsNoTracking()
            .Where(t => t.Address1 == address)
            .SelectMany(t => t.DeviceStates)
            .Where(s => s.DeviceClass == DeviceClass)
            .Select(s => s.Json)
            .FirstOrDefault();
        return json is null ? null : JsonSerializer.Deserialize<IdcDeviceState>(json, Options);
    }

    public void Save(string address, IdcDeviceState state)
    {
        using var db = _contexts.CreateDbContext();
        var terminal = db.Terminals.Include(t => t.DeviceStates).FirstOrDefault(t => t.Address1 == address);
        if (terminal is null)
        {
            terminal = new Terminal { Address1 = address };
            db.Terminals.Add(terminal);
        }

        var row = terminal.DeviceStates.FirstOrDefault(s => s.DeviceClass == DeviceClass);
        if (row is null)
        {
            row = new DeviceState { DeviceClass = DeviceClass };
            terminal.DeviceStates.Add(row);
        }
        row.Json = JsonSerializer.Serialize(state, Options);
        db.SaveChanges();
    }
}
