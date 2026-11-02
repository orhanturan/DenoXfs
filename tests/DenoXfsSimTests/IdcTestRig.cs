using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Device;
using Microsoft.Extensions.Logging.Abstractions;

namespace DenoXfs.Sim.Tests;

/// <summary>Real IDC device service with recording fakes for the event paths.</summary>
internal sealed class IdcTestRig
{
    public IdcTestRig()
    {
        Devices = new IdcDeviceRegistry(Store);
        Service = new IdcDeviceService(Devices, new FakeCardCatalog(), Publisher, new NativeBufferBuilderFactory(),
            NullLogger<IdcDeviceService>.Instance);
    }

    public FakeDeviceStateStore Store { get; } = new();

    public IdcDeviceRegistry Devices { get; }

    public RecordingPublisher Publisher { get; } = new();

    public RecordingExecuteEvents ExecuteEvents { get; } = new();

    public IdcDeviceService Service { get; }

    public uint[] ExecuteEventIds => ExecuteEvents.Sent.Select(e => e.EventId).ToArray();

    public void Insert(string address, string card) =>
        Assert.Equal(InsertOutcome.Inserted, Service.InsertAsync(address, card, CancellationToken.None).GetAwaiter().GetResult());
}

internal sealed class RecordingPublisher : IEventPublisher
{
    public List<(string Address, XfsEventKind Kind, uint EventId, ResultBuffer? Payload)> Published { get; } = new();

    public Task PublishAsync(string remoteAddress, XfsEventKind kind, uint eventId, ResultBuffer? payload, CancellationToken cancellationToken)
    {
        Published.Add((remoteAddress, kind, eventId, payload));
        return Task.CompletedTask;
    }
}

internal sealed class RecordingExecuteEvents : IExecuteEvents
{
    public List<(uint EventId, ResultBuffer? Payload)> Sent { get; } = new();

    public Task SendAsync(uint eventId, ResultBuffer? payload, CancellationToken cancellationToken)
    {
        Sent.Add((eventId, payload));
        return Task.CompletedTask;
    }
}
