using DenoXfs.Sim.Core.Abstractions;
using DenoXfs.Sim.Idc.Cards;
using DenoXfs.Sim.Idc.Device;
using DenoXfs.Sim.Idc.Forms;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DenoXfs.Sim.Idc.Web;

public sealed record InsertRequest(string Address, string Card);

public sealed record AddressRequest(string Address);

public sealed record SettingsRequest(string Address, bool AutoRemoveAfterEject, int? RetainCapacity, int? RetainHighThreshold);

public sealed record ChipResponseDto(string Command, string Response);

/// <summary>A card as the page edits it: tracks as text (binary bytes as JSON \u escapes), ATR and APDUs as hex.</summary>
public sealed record CardDto(string Name, string? Track1, string? Track2, string? Track3, string? Atr, IReadOnlyList<ChipResponseDto> ChipResponses,
    string? OriginalName = null);

/// <summary>Front end of the card reader simulator: page <c>/idc</c> and its JSON API under <c>/api/idc</c>.</summary>
public static class IdcEndpoints
{
    public static IEndpointRouteBuilder MapDenoXfsSimIdc(this IEndpointRouteBuilder app)
    {
        app.MapGet("/idc", () => Results.Content(Page.Value, "text/html; charset=utf-8"));

        var api = app.MapGroup("/api/idc");
        api.MapGet("/state", async (ICardCatalog cards, IIdcFormCatalog forms, IIdcDeviceRegistry devices, ISessionRegistry sessions,
            CancellationToken cancellationToken) =>
        {
            var addresses = devices.All.Select(d => d.Address).Union(sessions.Terminals).OrderBy(a => a);
            return Results.Ok(new
            {
                forms = (await forms.AllAsync(cancellationToken))
                    .Select(f => new { f.Name, f.Tracks, Action = f.Action == IdcConstants.ActionRead ? "read" : "write" }),
                cards = (await cards.AllAsync(cancellationToken)).Select(ToDto),
                terminals = addresses.Select(address =>
                {
                    var state = devices.Get(address).Snapshot();
                    var settings = devices.Get(address).Settings;
                    return new
                    {
                        state.Address,
                        Sessions = sessions.SessionsOf(address).Count,
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
                        state.PowerSaveRecoveryTime
                    };
                })
            });
        });

        api.MapPost("/insert", async (InsertRequest request, IIdcDeviceService service, CancellationToken cancellationToken) =>
            await service.InsertAsync(request.Address, request.Card, cancellationToken) switch
            {
                InsertOutcome.Inserted => Results.Ok(),
                InsertOutcome.UnknownCard => Results.BadRequest("Unknown card."),
                _ => Results.Conflict("The reader is not empty: take the card first.")
            });

        api.MapPost("/take", async (AddressRequest request, IIdcDeviceService service, CancellationToken cancellationToken) =>
            await service.TakeAsync(request.Address, cancellationToken) ? Results.Ok() : Results.Conflict("There is no card at the exit."));

        api.MapPost("/settings", (SettingsRequest request, IIdcDeviceService service) =>
        {
            service.UpdateSettings(request.Address, request.AutoRemoveAfterEject, request.RetainCapacity, request.RetainHighThreshold);
            return Results.Ok();
        });

        api.MapGet("/cards", async (ICardCatalog cards, CancellationToken cancellationToken) =>
            Results.Ok((await cards.AllAsync(cancellationToken)).Select(ToDto)));

        api.MapPut("/cards", async (CardDto dto, ICardCatalog cards, CancellationToken cancellationToken) =>
        {
            var (card, error) = FromDto(dto);
            if (card is null)
                return Results.BadRequest(error);
            await cards.SaveAsync(card, dto.OriginalName, cancellationToken);
            return Results.Ok();
        });

        api.MapDelete("/cards/{name}", async (string name, ICardCatalog cards, CancellationToken cancellationToken) =>
            await cards.DeleteAsync(name, cancellationToken) ? Results.Ok() : Results.NotFound("Unknown card."));

        return app;
    }

    private static CardDto ToDto(IdcCard card) => new(
        card.Name, card.Track1, card.Track2, card.Track3, card.Atr is null ? null : Convert.ToHexString(card.Atr),
        card.ChipResponses.Select(r => new ChipResponseDto(r.Key, r.Value)).ToList());

    /// <summary>Validates the page's input: a name, hex ATR and APDUs (spaces allowed), no duplicate command.</summary>
    private static (IdcCard? Card, string Error) FromDto(CardDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return (null, "The card needs a name.");
        if (!TryHex(dto.Atr, out var atr))
            return (null, "The ATR is not valid hex.");

        var responses = new Dictionary<string, string>();
        foreach (var row in dto.ChipResponses)
        {
            if (!TryHex(row.Command, out var command) || command is not { Length: > 0 } || !TryHex(row.Response, out var response) || response is not { Length: >= 2 })
                return (null, $"APDU row '{row.Command}' is not valid hex (the response needs at least SW1 SW2).");
            if (!responses.TryAdd(Convert.ToHexString(command), Convert.ToHexString(response)))
                return (null, $"Command {Convert.ToHexString(command)} appears twice.");
        }

        return (new IdcCard(dto.Name.Trim(), Empty(dto.Track1), Empty(dto.Track2), Empty(dto.Track3), atr, responses), string.Empty);
    }

    private static string? Empty(string? text) => string.IsNullOrEmpty(text) ? null : text;

    private static bool TryHex(string? text, out byte[]? bytes)
    {
        bytes = null;
        var hex = (text ?? string.Empty).Replace(" ", string.Empty);
        if (hex.Length == 0)
            return true;
        if (hex.Length % 2 != 0 || !hex.All(Uri.IsHexDigit))
            return false;
        bytes = Convert.FromHexString(hex);
        return true;
    }

    private static readonly Lazy<string> Page = new(() =>
    {
        using var stream = typeof(IdcEndpoints).Assembly.GetManifestResourceStream("DenoXfs.Sim.Idc.Web.idc.html")
            ?? throw new InvalidOperationException("Embedded page idc.html is missing.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    });
}
