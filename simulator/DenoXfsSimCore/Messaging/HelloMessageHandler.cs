using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Versions;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

/// <summary>Answers WFPOpen: negotiates the SPI and service versions.</summary>
public sealed class HelloMessageHandler : IMessageHandler
{
    private readonly IVersionNegotiator _negotiator;
    private readonly SupportedVersions _supported;
    private readonly ILogger<HelloMessageHandler> _logger;

    public HelloMessageHandler(IVersionNegotiator negotiator, SupportedVersions supported, ILogger<HelloMessageHandler> logger)
    {
        _negotiator = negotiator;
        _supported = supported;
        _logger = logger;
    }

    public string MessageType => WireTypes.Hello;

    public async Task HandleAsync(ISessionChannel channel, JsonElement message, CancellationToken cancellationToken)
    {
        var spi = _negotiator.Negotiate(
            XfsVersionRange.FromRequired(WireJson.GetUInt(message, "spiVersionsRequired")),
            _supported.Spi, XfsResultCodes.SpiVersionTooLow, XfsResultCodes.SpiVersionTooHigh);
        var service = _negotiator.Negotiate(
            XfsVersionRange.FromRequired(WireJson.GetUInt(message, "srvcVersionsRequired")),
            _supported.Service, XfsResultCodes.ServiceVersionTooLow, XfsResultCodes.ServiceVersionTooHigh);

        var hResult = spi.HResult != XfsResultCodes.Success ? spi.HResult : service.HResult;
        _logger.LogInformation(
            "open from={Remote} logical={Logical} machine={Machine} pid={Pid} spi=0x{Spi:X4} service=0x{Service:X4} hResult={HResult}",
            channel.RemoteAddress, WireJson.GetString(message, "logicalName"), WireJson.GetString(message, "machine"),
            WireJson.GetUInt(message, "pid"), spi.Version, service.Version, hResult);

        await channel.SendAsync(WireMessageFactory.HelloAck(hResult, spi.Version, service.Version, _supported), cancellationToken);
    }
}
