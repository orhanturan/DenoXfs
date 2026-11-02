using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Forms;

namespace DenoXfs.Sim.Tests;

/// <summary>The built-in test cards (same as Seeds/cards.json) without a database.</summary>
internal sealed class FakeCardCatalog : ICardCatalog
{
    public static readonly byte[] TestAtr =
        { 0x3B, 0x8F, 0x80, 0x01, 0x80, 0x4F, 0x0C, 0xA0, 0x00, 0x00, 0x03, 0x06, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x6A };

    private static readonly IReadOnlyDictionary<string, string> NoChip = new Dictionary<string, string>();

    private readonly List<IdcCard> _cards =
    [
        new("Visa chip + magstripe", "B4111111111111111^TEST/CARD^25122010000000000000", "4111111111111111=25122010000000000000", null, TestAtr,
            new Dictionary<string, string> { ["00A404000E315041592E5359532E444446303100"] = "6F1A840E315041592E5359532E4444463031A5088801025F2D02656E9000" }),
        new("Magstripe only (3 tracks)", "B6045551234567890^TEST/MAGSTRIPE^2612101", "6045551234567890=2612101123456789",
            "016045551234567890=724724100000000000030300", null, NoChip),
        new("Chip only", null, null, null, TestAtr, NoChip)
    ];

    public Task<IReadOnlyList<IdcCard>> AllAsync(CancellationToken cancellationToken) => Task.FromResult<IReadOnlyList<IdcCard>>(_cards);

    public Task<IdcCard?> FindAsync(string name, CancellationToken cancellationToken) =>
        Task.FromResult(_cards.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)));

    public Task SaveAsync(IdcCard card, string? originalName, CancellationToken cancellationToken)
    {
        _cards.RemoveAll(c => c.Name == (originalName ?? card.Name));
        _cards.Add(card);
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(string name, CancellationToken cancellationToken) => Task.FromResult(_cards.RemoveAll(c => c.Name == name) > 0);
}

/// <summary>The built-in forms (same as Seeds/forms.json) without a database.</summary>
internal sealed class FakeFormCatalog : IIdcFormCatalog
{
    private static readonly string[] None = [];

    public static readonly IReadOnlyList<IdcForm> Forms =
    [
        new("READTRACK2", IdcConstants.ActionRead, "TRACK2", false, None, ["PAN", "EXPIRY", "SERVICECODE", "DISCRETIONARY"], None),
        new("READTRACK1ORTRACK2", IdcConstants.ActionRead, "TRACK1|TRACK2", false, ["PAN", "NAME", "EXPIRY"], ["PAN", "EXPIRY"], None),
        new("READALLTRACKS", IdcConstants.ActionRead, "TRACK1&TRACK2&TRACK3", false, ["ALL"], ["ALL"], ["ALL"]),
        new("WRITETRACK3", IdcConstants.ActionWrite, "TRACK3", false, None, None, ["ALL"])
    ];

    public static IdcForm Find(string name) => Forms.Single(f => f.Name == name);

    public Task<IReadOnlyList<IdcForm>> AllAsync(CancellationToken cancellationToken) => Task.FromResult(Forms);

    public Task<IdcForm?> FindAsync(string name, CancellationToken cancellationToken) =>
        Task.FromResult(Forms.FirstOrDefault(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase)));
}

/// <summary>Device state store that keeps everything in a dictionary.</summary>
internal sealed class FakeDeviceStateStore : IIdcDeviceStateStore
{
    public Dictionary<string, IdcDeviceState> States { get; } = new();

    public IdcDeviceState? Load(string address) => States.GetValueOrDefault(address);

    public void Save(string address, IdcDeviceState state) => States[address] = state;
}
