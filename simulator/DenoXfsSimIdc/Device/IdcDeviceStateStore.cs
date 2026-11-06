using System.Text.Json;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Data;
using Microsoft.EntityFrameworkCore;

namespace DenoXfs.Sim.Idc.Device;

/// <summary>What survives a host restart for one card reader of a terminal: the settings and the retain count.</summary>
public sealed record IdcDeviceState(bool AutoRemoveAfterEject, int RetainCapacity, int RetainHighThreshold, int RetainCount);

public interface IIdcDeviceStateStore
{
    IdcDeviceState? Load(DeviceRef reader);

    void Save(DeviceRef reader, IdcDeviceState state);
}

/// <summary>DeviceState row (XfsClass "IDC") of a card reader of a terminal; nothing is stored for a device that is not in the terminal's model.</summary>
public sealed class EfDeviceStateStore : IIdcDeviceStateStore
{
    private const string DeviceClass = "IDC";
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);
    private readonly IDbContextFactory<SimDbContext> _contexts;

    public EfDeviceStateStore(IDbContextFactory<SimDbContext> contexts)
    {
        _contexts = contexts;
    }

    public IdcDeviceState? Load(DeviceRef reader)
    {
        using var db = _contexts.CreateDbContext();
        var json = db.DeviceStates.AsNoTracking()
            .Where(s => s.TerminalId == reader.TerminalId && s.ModelDeviceId == reader.DeviceId && s.XfsClass == DeviceClass)
            .Select(s => s.Json)
            .FirstOrDefault();
        return json is null ? null : JsonSerializer.Deserialize<IdcDeviceState>(json, Options);
    }

    public void Save(DeviceRef reader, IdcDeviceState state)
    {
        using var db = _contexts.CreateDbContext();
        var terminal = db.Terminals.Include(t => t.DeviceStates).FirstOrDefault(t => t.TerminalId == reader.TerminalId);
        if (terminal is null || !db.ModelDevices.Any(d => d.Id == reader.DeviceId && d.ModelId == terminal.ModelId))
            return;

        var row = terminal.DeviceStates.FirstOrDefault(s => s.ModelDeviceId == reader.DeviceId && s.XfsClass == DeviceClass);
        if (row is null)
        {
            row = new DeviceState { ModelDeviceId = reader.DeviceId, XfsClass = DeviceClass };
            terminal.DeviceStates.Add(row);
        }
        row.Json = JsonSerializer.Serialize(state, Options);
        db.SaveChanges();
    }
}
