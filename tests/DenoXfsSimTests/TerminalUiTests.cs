using System.Net;
using DenoXfs.Sim.Core.Names;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Core.Ui;
using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Ui;

namespace DenoXfs.Sim.Tests;

public class TerminalUiTests
{
    private const string Address = "10.0.0.8";
    private readonly IdcTestRig _rig = new();

    [Theory]
    [InlineData("::1", "127.0.0.1")]
    [InlineData("::ffff:10.1.2.3", "10.1.2.3")]
    [InlineData("10.1.2.3", "10.1.2.3")]
    public void Browser_and_dll_addresses_are_normalized_the_same_way(string input, string expected) =>
        Assert.Equal(expected, TerminalAddress.Normalize(IPAddress.Parse(input)));

    [Fact]
    public void Code_names_cover_the_api_and_idc()
    {
        var names = new XfsCodeNames([new ApiCodeNames(), new IdcCodeNames()]);

        Assert.Equal("WFS_ERR_HARDWARE_ERROR", names.NameOf(XfsCodeKind.Result, -14));
        Assert.Equal("WFS_ERR_IDC_MEDIAJAM", names.NameOf(XfsCodeKind.Result, -200));
        Assert.Equal("WFS_ERR_IDC_COMMANDUNSUPP", names.NameOf(XfsCodeKind.Result, IdcConstants.ErrCommandUnsupp));
        Assert.Equal("WFS_SYSE_DEVICE_STATUS (4)", names.Describe(XfsCodeKind.Event, 4));
        Assert.Equal("WFS_SRVE_IDC_POWER_SAVE_CHANGE", names.NameOf(XfsCodeKind.Event, (int)IdcConstants.PowerSaveChangeEvent));
        Assert.Equal("WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS", names.NameOf(XfsCodeKind.Info, (int)IdcConstants.EmvclessQueryApplicationsCategory));
        Assert.True(names.TryCode(XfsCodeKind.Command, "wfs_cmd_idc_synchronize_command", out var code));
        Assert.Equal((int)IdcConstants.SynchronizeCommand, code);
    }

    [Fact]
    public async Task Device_changes_notify_the_terminal_page()
    {
        var changed = new List<string>();
        _rig.Notifier.Changed += changed.Add;

        _rig.Insert(Address, "Chip only");
        await _rig.Service.EjectAsync(TestDevices.Of(Address), CancellationToken.None);

        Assert.True(changed.Count >= 3); // insert, eject, take (auto remove)
        Assert.All(changed, a => Assert.Equal(Address, a));
    }

    [Fact]
    public async Task Card_reader_area_asks_for_attention_while_a_read_waits_and_shows_a_fault_with_device_error()
    {
        _rig.Catalog
            .Add(ProfileKind.Scenario, "Default", "{}")
            .Add(ProfileKind.Scenario, "DeviceError", """{ "status": { "fwDevice": "HWERROR" } }""")
            .Add(ProfileKind.DeviceProfile, "Default", "{}");
        var module = new IdcUiModule(_rig.Devices, _rig.Readers, _rig.Selections, _rig.Profiles);
        using var cancel = new CancellationTokenSource();

        var read = _rig.Devices.Get(TestDevices.Of(Address)).WaitForCardAsync(cancel.Token);
        var waiting = await module.VisualAsync(Address, CancellationToken.None);
        Assert.True(waiting.NeedsAttention);
        Assert.Equal("Please insert your card", waiting.ScreenHint);
        cancel.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => read);

        await _rig.SelectionService.SelectAsync(Address, new ProfileSelection("DeviceError", "Default"), CancellationToken.None);
        var fault = await module.VisualAsync(Address, CancellationToken.None);
        Assert.True(fault.Fault);
        Assert.Equal("Device HWERROR", fault.StatusText);
    }

    [Fact]
    public void Guidance_light_maps_to_colour_and_flash()
    {
        Assert.Equal(("#43a047", "medium"), IdcUiModule.Light(IdcConstants.GuidanceGreen | IdcConstants.GuidanceMediumFlash));
        Assert.Equal((null, null), IdcUiModule.Light(IdcConstants.GuidanceOff));
    }
}
