using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Protocol;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

/// <summary>WFPClose: logs and completes the request.</summary>
public sealed class CloseMessageHandler : IMessageHandler
{
    private readonly ILogger<CloseMessageHandler> _logger;

    public CloseMessageHandler(ILogger<CloseMessageHandler> logger)
    {
        _logger = logger;
    }

    public string MessageType => WireTypes.Close;

    public async Task HandleAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken)
    {
        var reqId = WireJson.GetUInt(message, "reqId");
        _logger.LogInformation("close from={Remote} reqId={ReqId}", channel.RemoteAddress, reqId);
        await channel.SendAsync(WireMessageFactory.Completion(reqId, WireTypes.Close, XfsResultCodes.Success, 0, null), cancellationToken);
    }
}
