using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Core.Ui;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Layouts;
using DenoXfs.Sim.Idc.Scenarios;

namespace DenoXfs.Sim.Idc.Ui;

/// <summary>
/// The card reader on the ATM picture: slot state, guidance light, and <see cref="IdcPanel"/> as its panel. With several
/// card readers in the model the area shows the one that needs the user (else the first); the panel picks the reader.
/// </summary>
public sealed class IdcUiModule : IDeviceUiModule
{
    private readonly IIdcDeviceRegistry _devices;
    private readonly IIdcReaders _readers;
    private readonly ITerminalSelections _selections;
    private readonly IIdcProfiles _profiles;

    public IdcUiModule(IIdcDeviceRegistry devices, IIdcReaders readers, ITerminalSelections selections, IIdcProfiles profiles)
    {
        _devices = devices;
        _readers = readers;
        _selections = selections;
        _profiles = profiles;
    }

    public string DeviceClass => IdcProfiles.DeviceClass;

    public AtmArea Area => AtmArea.CardReader;

    public string Title => "Card reader";

    public Type PanelComponent => typeof(IdcPanel);

    public async Task<DeviceVisual> VisualAsync(string address, CancellationToken cancellationToken)
    {
        var readers = await _readers.OfTerminalAsync(address, cancellationToken);
        if (readers.Count == 0)
            return new DeviceVisual(false, false, "No card reader in the model");
        var states = readers.Select(r => (Reader: r, State: _devices.Get(r.Ref).Snapshot())).ToList();
        var (reader, state) = states.FirstOrDefault(s => s.State.WaitingForCard || s.State.Media == MediaPosition.AtExit);
        if (reader is null)
            (reader, state) = states[0];
        var prefix = readers.Count > 1 ? $"{reader.Name}: " : string.Empty;
        var device = await _profiles.DeviceStateAsync(await _selections.GetAsync(address, cancellationToken), cancellationToken);
        var fault = device != IdcConstants.DevOnline;
        var (color, flash) = Light(state.GuidanceLights[IdcConstants.GuidanceCardUnit]);

        var text = prefix + (fault ? $"Device {IdcConstantSets.Device.NameOf(device)}"
            : state.WaitingForCard ? "Waiting for a card"
            : state.Media switch
            {
                MediaPosition.Inside => $"Card inside: {state.CardName}",
                MediaPosition.AtExit => "Card at the exit",
                _ => "No card"
            });
        var hint = state.WaitingForCard ? "Please insert your card" : state.Media == MediaPosition.AtExit ? "Please take your card" : null;

        return new DeviceVisual(state.WaitingForCard || state.Media == MediaPosition.AtExit, fault, text, color, flash,
            state.Media == MediaPosition.Inside, state.Media == MediaPosition.AtExit, hint);
    }

    /// <summary>dwGuidLights of the card unit (WFS_IDC_GUIDANCE_*) as a CSS colour and a flash speed.</summary>
    public static (string? Color, string? Flash) Light(uint value)
    {
        if (value == 0 || (value & IdcConstants.GuidanceOff) != 0)
            return (null, null);
        var color = (value & IdcConstants.GuidanceRed) != 0 ? "#e53935"
            : (value & IdcConstants.GuidanceGreen) != 0 ? "#43a047"
            : (value & IdcConstants.GuidanceYellow) != 0 ? "#fdd835"
            : "#ffffff";
        var flash = (value & IdcConstants.GuidanceSlowFlash) != 0 ? "slow"
            : (value & IdcConstants.GuidanceMediumFlash) != 0 ? "medium"
            : (value & IdcConstants.GuidanceQuickFlash) != 0 ? "quick"
            : null;
        return (color, flash);
    }
}
