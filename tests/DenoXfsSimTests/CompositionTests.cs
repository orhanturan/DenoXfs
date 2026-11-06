using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Extensions;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Messaging;
using DenoXfs.Sim.Data.Extensions;
using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DenoXfs.Sim.Tests;

/// <summary>The host's composition (data, core, IDC module) builds and finds the IDC handlers keyed by class.</summary>
public class CompositionTests
{
    [Fact]
    public void Idc_commands_and_infos_are_registered_for_the_IDC_class_only()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
        services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        services.AddDenoXfsSimData("Data Source=:memory:");
        services.AddDenoXfsSimCore();
        services.AddDenoXfsSimIdc();
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        var commands = provider.GetRequiredService<IClassHandlers<IExecuteCommand>>();
        var infos = provider.GetRequiredService<IClassHandlers<IInfoCommand>>();
        Assert.True(commands.TryGet("IDC", IdcConstants.ReadRawDataCommand, out _));
        Assert.False(commands.TryGet("CDM", IdcConstants.ReadRawDataCommand, out _));
        Assert.True(infos.TryGet("idc", IdcConstants.StatusCategory, out _));
        Assert.Equal(["IDC"], provider.GetServices<IDeviceClassModule>().Select(m => m.DeviceClass));
        Assert.NotNull(provider.GetRequiredService<IMessageDispatcher>());
        Assert.NotNull(provider.GetRequiredService<IHardwareCatalog>());
        Assert.NotNull(provider.GetRequiredService<ITerminalCatalog>());
    }
}
