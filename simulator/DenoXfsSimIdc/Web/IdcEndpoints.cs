using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Core.Hardware;
using DenoXfs.Sim.Core.Scenarios;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Forms;
using DenoXfs.Sim.Idc.Layouts;
using DenoXfs.Sim.Idc.Scenarios;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DenoXfs.Sim.Idc.Web;

// Address finds the terminal (any of its IPs); DeviceId picks one of its card readers (model device id), default the first.

public sealed record InsertRequest(string Address, string Card, int? DeviceId = null);

public sealed record AddressRequest(string Address, int? DeviceId = null);

public sealed record SettingsRequest(string Address, bool AutoRemoveAfterEject, int? RetainCapacity, int? RetainHighThreshold, int? DeviceId = null);

/// <summary>JSON API of the card reader simulator under <c>/api/idc</c> (the terminal page itself is a Blazor component of the host).</summary>
public static class IdcEndpoints
{
    public static IEndpointRouteBuilder MapDenoXfsSimIdc(this IEndpointRouteBuilder app)
    {
        app.MapGet("/idc", () => Results.Redirect("/"));

        var api = app.MapGroup("/api/idc");
        api.MapGet("/state", async (ICardCatalog cards, IIdcFormCatalog forms, IIdcDeviceRegistry devices, IIdcReaders readers,
            ITerminalCatalog terminalCatalog, ISessionRegistry sessions, IProfileCatalog profiles, ITerminalSelections selections,
            IIdcProfiles idcProfiles, CancellationToken cancellationToken) =>
        {
            var terminals = new List<object>();
            foreach (var terminal in await terminalCatalog.AllAsync(cancellationToken))
            {
                var selection = await selections.GetAsync(terminal.Address1, cancellationToken);
                var deviceState = IdcConstantSets.Device.NameOf(await idcProfiles.DeviceStateAsync(selection, cancellationToken));
                foreach (var reader in await readers.OfTerminalAsync(terminal.Address1, cancellationToken))
                    terminals.Add(Reader(devices, sessions, terminal, reader, selection, deviceState));
            }

            return Results.Ok(new
            {
                forms = (await forms.AllAsync(cancellationToken))
                    .Select(f => new { f.Name, f.Tracks, Action = f.Action == IdcConstants.ActionRead ? "read" : "write" }),
                cards = (await cards.AllAsync(cancellationToken)).Select(CardDtos.ToDto),
                scenarios = (await profiles.AllAsync(ProfileKind.Scenario, cancellationToken)).Select(p => new { p.Name, p.Description }),
                deviceProfiles = (await profiles.AllAsync(ProfileKind.DeviceProfile, cancellationToken)).Select(p => new { p.Name, p.Description }),
                terminals
            });
        });

        api.MapPost("/insert", async (InsertRequest request, IIdcReaders readers, IIdcDeviceService service, CancellationToken cancellationToken) =>
            await FindAsync(readers, request.Address, request.DeviceId, cancellationToken) is not { } reader
                ? NoReader(request.Address)
                : await service.InsertAsync(reader.Ref, request.Card, cancellationToken) switch
                {
                    InsertOutcome.Inserted => Results.Ok(),
                    InsertOutcome.UnknownCard => Results.BadRequest("Unknown card."),
                    _ => Results.Conflict("The reader is not empty: take the card first.")
                });

        api.MapPost("/take", async (AddressRequest request, IIdcReaders readers, IIdcDeviceService service, CancellationToken cancellationToken) =>
            await FindAsync(readers, request.Address, request.DeviceId, cancellationToken) is not { } reader
                ? NoReader(request.Address)
                : await service.TakeAsync(reader.Ref, cancellationToken) ? Results.Ok() : Results.Conflict("There is no card at the exit."));

        api.MapPost("/settings", async (SettingsRequest request, IIdcReaders readers, IIdcDeviceService service, CancellationToken cancellationToken) =>
        {
            if (await FindAsync(readers, request.Address, request.DeviceId, cancellationToken) is not { } reader)
                return NoReader(request.Address);
            service.UpdateSettings(reader.Ref, request.AutoRemoveAfterEject, request.RetainCapacity, request.RetainHighThreshold);
            return Results.Ok();
        });

        api.MapGet("/cards", async (ICardCatalog cards, CancellationToken cancellationToken) =>
            Results.Ok((await cards.AllAsync(cancellationToken)).Select(CardDtos.ToDto)));

        api.MapPut("/cards", async (CardDto dto, ICardCatalog cards, CancellationToken cancellationToken) =>
        {
            var (card, error) = CardDtos.FromDto(dto);
            if (card is null)
                return Results.BadRequest(error);
            await cards.SaveAsync(card, dto.OriginalName, cancellationToken);
            return Results.Ok();
        });

        api.MapDelete("/cards/{name}", async (string name, ICardCatalog cards, CancellationToken cancellationToken) =>
            await cards.DeleteAsync(name, cancellationToken) ? Results.Ok() : Results.NotFound("Unknown card."));

        return app;
    }

    private static async Task<IdcReader?> FindAsync(IIdcReaders readers, string address, int? deviceId, CancellationToken cancellationToken)
    {
        var all = await readers.OfTerminalAsync(address, cancellationToken);
        return deviceId is null ? all.FirstOrDefault() : all.FirstOrDefault(r => r.Ref.DeviceId == deviceId);
    }

    private static IResult NoReader(string address) => Results.NotFound($"No such card reader on the terminal of {address}.");

    private static object Reader(IIdcDeviceRegistry devices, ISessionRegistry sessions, TerminalInfo terminal, IdcReader reader,
        ProfileSelection selection, string deviceState)
    {
        var state = devices.Get(reader.Ref).Snapshot();
        var settings = devices.Get(reader.Ref).Settings;
        return new
        {
            TerminalId = TerminalIds.Display(terminal.TerminalId),
            Address = terminal.Address1,
            terminal.Address2,
            reader.Ref.DeviceId,
            Device = reader.Name,
            Services = reader.ServiceNames,
            Sessions = sessions.SessionsOf(reader.Ref, IdcProfiles.DeviceClass).Count,
            Media = state.Media.ToString(),
            Card = state.CardName,
            Chip = state.Chip.ToString(),
            state.RetainCount,
            state.RetainCapacity,
            settings.RetainHighThreshold,
            RetainBin = state.RetainBin.ToString(),
            state.WaitingForCard,
            state.AutoRemoveAfterEject,
            CardUnitLight = state.GuidanceLights[IdcConstants.GuidanceCardUnit],
            state.PowerSaveRecoveryTime,
            selection.Scenario,
            selection.DeviceProfile,
            DeviceState = deviceState
        };
    }
}
