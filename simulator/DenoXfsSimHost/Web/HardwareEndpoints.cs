using DenoXfs.Sim.Core.Hardware;

namespace DenoXfs.Sim.Host.Web;

public sealed record TerminalRequest(string TerminalId, string Address1, string? Address2, int ModelId);

/// <summary>Hardware catalog and terminals for automation: <c>/api/hardware</c>, <c>/api/terminals</c>. The web page edits the catalog.</summary>
public static class HardwareEndpoints
{
    public static IEndpointRouteBuilder MapDenoXfsSimHardware(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/hardware", async (IHardwareCatalog catalog, CancellationToken cancellationToken) => Results.Ok(new
        {
            (await catalog.SnapshotAsync(cancellationToken)).Vendors,
            DeviceTypes = DeviceTypes.All
        }));

        var terminals = app.MapGroup("/api/terminals");

        terminals.MapGet("/", async (ITerminalCatalog catalog, CancellationToken cancellationToken) =>
            Results.Ok((await catalog.AllAsync(cancellationToken)).Select(t => t with { TerminalId = TerminalIds.Display(t.TerminalId) })));

        terminals.MapPost("/", async (TerminalRequest request, ITerminalCatalog catalog, CancellationToken cancellationToken) =>
            ToResult(await catalog.AddAsync(new TerminalInfo(request.TerminalId, request.Address1, request.Address2, request.ModelId), cancellationToken)));

        terminals.MapPut("/{terminalId}", async (string terminalId, TerminalRequest request, ITerminalCatalog catalog, CancellationToken cancellationToken) =>
            TerminalIds.Normalize(terminalId, out var id) is { } error
                ? Results.BadRequest(error)
                : ToResult(await catalog.UpdateAsync(new TerminalInfo(id, request.Address1, request.Address2, request.ModelId), cancellationToken)));

        terminals.MapDelete("/{terminalId}", async (string terminalId, ITerminalCatalog catalog, CancellationToken cancellationToken) =>
            TerminalIds.Normalize(terminalId, out var id) is { } error
                ? Results.BadRequest(error)
                : ToResult(await catalog.DeleteAsync(id, cancellationToken)));

        return app;
    }

    private static IResult ToResult(HardwareResult result) => result.Succeeded ? Results.Ok() : Results.BadRequest(result.Error);
}
