using DenoXfs.Sim.Core.Ui;

namespace DenoXfs.Sim.Host.Components;

/// <summary>A device area of the ATM picture: the module it belongs to and how it looks now.</summary>
public sealed record AtmAreaState(IDeviceUiModule Module, DeviceVisual Visual);
