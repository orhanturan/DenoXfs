using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Buffers;
using DenoXfs.Sim.Core.Messaging;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Commands;
using Microsoft.Extensions.Logging.Abstractions;

namespace DenoXfs.Sim.Tests;

public class CancelTests
{
    private readonly IdcTestRig _rig = new();
    private readonly RequestCancellation _cancellation = new();
    private readonly RecordingChannel _channel = new("127.0.0.1");

    private ExecuteMessageHandler Handler() => new(
        [new ReadRawDataCommand(new NativeBufferBuilderFactory(), _rig.Service, _rig.Devices)], _cancellation,
        NullLogger<ExecuteMessageHandler>.Instance);

    private static JsonElement ReadRawData(uint reqId) => JsonDocument.Parse(
        $"{{\"type\":\"execute\",\"reqId\":{reqId},\"command\":{IdcConstants.ReadRawDataCommand},\"timeoutMs\":0,\"input\":{{\"readData\":2}}}}").RootElement;

    private CancelMessageHandler CancelHandler() => new(_cancellation, NullLogger<CancelMessageHandler>.Instance);

    private static JsonElement Cancel(uint reqId) => JsonDocument.Parse($"{{\"type\":\"cancel\",\"reqId\":{reqId}}}").RootElement;

    [Fact]
    public async Task Read_waiting_for_a_card_completes_with_canceled()
    {
        var handler = Handler();
        var read = handler.HandleAsync(_channel, ReadRawData(7), CancellationToken.None);
        Assert.False(read.IsCompleted);

        await CancelHandler().HandleAsync(_channel, Cancel(7), CancellationToken.None);
        await read;

        Assert.Equal(XfsResultCodes.Canceled, _channel.Completion(7).HResult);
        Assert.False(_rig.Devices.Get("127.0.0.1").Snapshot().WaitingForCard);
    }

    [Fact]
    public async Task Request_waiting_in_the_session_queue_is_cancelled_without_running()
    {
        var handler = Handler();
        var first = handler.HandleAsync(_channel, ReadRawData(1), CancellationToken.None);
        var second = handler.HandleAsync(_channel, ReadRawData(2), CancellationToken.None);

        await CancelHandler().HandleAsync(_channel, Cancel(2), CancellationToken.None);
        await second;
        Assert.Equal(XfsResultCodes.Canceled, _channel.Completion(2).HResult);
        Assert.False(first.IsCompleted);

        // Request id 0 cancels everything left in the session.
        await CancelHandler().HandleAsync(_channel, Cancel(0), CancellationToken.None);
        await first;
        Assert.Equal(XfsResultCodes.Canceled, _channel.Completion(1).HResult);
    }

    [Fact]
    public async Task Read_without_cancel_still_completes_normally()
    {
        var read = Handler().HandleAsync(_channel, ReadRawData(3), CancellationToken.None);
        _rig.Insert("127.0.0.1", "Visa chip + magstripe");
        await read;

        Assert.Equal(XfsResultCodes.Success, _channel.Completion(3).HResult);
    }
}

/// <summary>Session channel that keeps every message sent to the DLL.</summary>
internal sealed class RecordingChannel : ISessionChannel
{
    private readonly List<object> _sent = new();

    public RecordingChannel(string remoteAddress)
    {
        RemoteAddress = remoteAddress;
    }

    public string RemoteAddress { get; }

    public Task SendAsync(object message, CancellationToken cancellationToken)
    {
        lock (_sent)
            _sent.Add(message);
        return Task.CompletedTask;
    }

    public CompletionMessage Completion(uint reqId)
    {
        lock (_sent)
            return _sent.OfType<CompletionMessage>().Single(c => c.ReqId == reqId);
    }
}
