using System.Runtime.CompilerServices;
using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Protocol;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

/// <summary>
/// Routes WFPExecute to the registered <see cref="IExecuteCommand"/> and sends the completion. Commands of one session
/// run one at a time (like a real device). A request ends with WFS_ERR_CANCELED when WFPCancelAsyncRequest arrives (also
/// while it still waits in the queue) and with WFS_ERR_TIMEOUT when its timeout (0 = WFS_INDEFINITE_WAIT) passes.
/// </summary>
public sealed class ExecuteMessageHandler : IMessageHandler
{
    private readonly Dictionary<uint, IExecuteCommand> _commands;
    private readonly IRequestCancellation _cancellation;
    private readonly ConditionalWeakTable<ISessionChannel, SemaphoreSlim> _queues = new();
    private readonly ILogger<ExecuteMessageHandler> _logger;

    public ExecuteMessageHandler(IEnumerable<IExecuteCommand> commands, IRequestCancellation cancellation, ILogger<ExecuteMessageHandler> logger)
    {
        _commands = commands.ToDictionary(c => c.Command);
        _cancellation = cancellation;
        _logger = logger;
    }

    public string MessageType => WireTypes.Execute;

    public async Task HandleAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken)
    {
        var reqId = WireJson.GetUInt(message, "reqId");
        var command = WireJson.GetUInt(message, "command");
        var timeoutMs = WireJson.GetUInt(message, "timeoutMs");
        message.TryGetProperty("input", out var input);

        var cancelled = _cancellation.Register(channel, reqId);
        CommandOutcome outcome;
        try
        {
            outcome = await QueueAndRunAsync(channel, reqId, command, timeoutMs, input, cancelled, cancellationToken);
        }
        finally
        {
            _cancellation.Complete(channel, reqId);
        }

        _logger.LogInformation("execute from={Remote} reqId={ReqId} command={Command} hResult={HResult} hasResult={HasResult}",
            channel.RemoteAddress, reqId, command, outcome.HResult, outcome.Result is not null);
        await channel.SendAsync(WireMessageFactory.Completion(reqId, WireTypes.Execute, outcome.HResult, command, outcome.Result), cancellationToken);
    }

    private async Task<CommandOutcome> QueueAndRunAsync(ISessionChannel channel, uint reqId, uint command, uint timeoutMs, JsonElement input,
        CancellationToken cancelled, CancellationToken sessionToken)
    {
        var queue = _queues.GetValue(channel, _ => new SemaphoreSlim(1, 1));
        using var waiting = CancellationTokenSource.CreateLinkedTokenSource(cancelled, sessionToken);
        try
        {
            await queue.WaitAsync(waiting.Token);
        }
        catch (OperationCanceledException) when (cancelled.IsCancellationRequested && !sessionToken.IsCancellationRequested)
        {
            _logger.LogInformation("execute cancelled in queue from={Remote} reqId={ReqId} command={Command}", channel.RemoteAddress, reqId, command);
            return new CommandOutcome(XfsResultCodes.Canceled, null);
        }

        try
        {
            return await RunAsync(channel, reqId, command, timeoutMs, input, cancelled, sessionToken);
        }
        finally
        {
            queue.Release();
        }
    }

    private async Task<CommandOutcome> RunAsync(ISessionChannel channel, uint reqId, uint command, uint timeoutMs, JsonElement input,
        CancellationToken cancelled, CancellationToken sessionToken)
    {
        if (!_commands.TryGetValue(command, out var handler))
            return new CommandOutcome(XfsResultCodes.UnsupportedCommand, null);

        using var timeout = new CancellationTokenSource();
        if (timeoutMs > 0)
            timeout.CancelAfter(TimeSpan.FromMilliseconds(timeoutMs));
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancelled, timeout.Token, sessionToken);

        var context = new ExecuteContext(channel.RemoteAddress, command, input, new ChannelExecuteEvents(channel, reqId, _logger));
        try
        {
            return await handler.ExecuteAsync(context, linked.Token);
        }
        catch (OperationCanceledException) when (!sessionToken.IsCancellationRequested)
        {
            var hResult = cancelled.IsCancellationRequested ? XfsResultCodes.Canceled : XfsResultCodes.Timeout;
            _logger.LogInformation("execute ended from={Remote} reqId={ReqId} command={Command} reason={Reason} timeoutMs={TimeoutMs}",
                channel.RemoteAddress, reqId, command, hResult == XfsResultCodes.Canceled ? "cancelled" : "timeout", timeoutMs);
            return new CommandOutcome(hResult, null);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "execute failed from={Remote} reqId={ReqId} command={Command}", channel.RemoteAddress, reqId, command);
            return new CommandOutcome(XfsResultCodes.InternalError, null);
        }
    }
}
