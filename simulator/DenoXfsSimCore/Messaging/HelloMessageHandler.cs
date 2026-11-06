using System.Text.Json;
using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Core.Ui;
using DenoXfs.Sim.Core.Versions;
using Microsoft.Extensions.Logging;

namespace DenoXfs.Sim.Core.Messaging;

/// <summary>
/// Answers WFPOpen: negotiates the SPI and service versions and resolves the logical name among the services of the
/// terminal's model. An IP without a terminal, an unknown name or a class without a module is WFS_ERR_SERVICE_NOT_FOUND.
/// </summary>
public sealed class HelloMessageHandler : IMessageHandler
{
    private readonly IVersionNegotiator _negotiator;
    private readonly SupportedVersions _supported;
    private readonly IServiceResolver _resolver;
    private readonly ISessionRegistry _sessions;
    private readonly IReadOnlySet<string> _simulatedClasses;
    private readonly ITrafficRecorder _traffic;
    private readonly ILogger<HelloMessageHandler> _logger;

    public HelloMessageHandler(IVersionNegotiator negotiator, SupportedVersions supported, IServiceResolver resolver, ISessionRegistry sessions,
        IEnumerable<IDeviceClassModule> modules, ITrafficRecorder traffic, ILogger<HelloMessageHandler> logger)
    {
        _negotiator = negotiator;
        _supported = supported;
        _resolver = resolver;
        _sessions = sessions;
        _simulatedClasses = modules.Select(m => m.DeviceClass).ToHashSet(StringComparer.OrdinalIgnoreCase);
        _traffic = traffic;
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
        var logicalName = WireJson.GetString(message, "logicalName");

        var hResult = spi.HResult != XfsResultCodes.Success ? spi.HResult : service.HResult;
        var resolution = await _resolver.ResolveAsync(channel.RemoteAddress, logicalName, cancellationToken);
        if (resolution.Service is { } found && !_simulatedClasses.Contains(found.XfsClass))
            resolution = ServiceResolution.NotFound($"the class {found.XfsClass} of '{found.ServiceName}' is not simulated");
        if (resolution.Service is null)
            hResult = XfsResultCodes.ServiceNotFound;

        _logger.LogInformation(
            "open from={Remote} logical={Logical} terminal={Terminal} class={Class} machine={Machine} pid={Pid} spi=0x{Spi:X4} service=0x{Service:X4} hResult={HResult} reason={Reason}",
            channel.RemoteAddress, logicalName, resolution.Service is { } s ? TerminalIds.Display(s.TerminalId) : "-", resolution.Service?.XfsClass ?? "-",
            WireJson.GetString(message, "machine"), WireJson.GetUInt(message, "pid"), spi.Version, service.Version, hResult, resolution.Reason ?? "-");

        if (hResult == XfsResultCodes.Success)
            _sessions.Bind(channel, resolution.Service!);
        else
            _traffic.Session(channel.RemoteAddress, $"open rejected {logicalName}: {resolution.Reason ?? $"version not supported ({hResult})"}", true);

        await channel.SendAsync(WireMessageFactory.HelloAck(hResult, spi.Version, service.Version, _supported), cancellationToken);
    }
}
