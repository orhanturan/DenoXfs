using DenoXfs.Sim.Core.Scenarios;

namespace DenoXfs.Sim.Host.Web;

public sealed record SelectionRequest(string Address, string Scenario, string DeviceProfile);

/// <summary>Scenarios and device profiles for every device class: <c>/api/profiles</c>.</summary>
public static class ProfileEndpoints
{
    public static IEndpointRouteBuilder MapDenoXfsSimProfiles(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/profiles");

        api.MapGet("/", async (IProfileCatalog catalog, CancellationToken cancellationToken) => Results.Ok(new
        {
            scenarios = await catalog.AllAsync(ProfileKind.Scenario, cancellationToken),
            deviceProfiles = await catalog.AllAsync(ProfileKind.DeviceProfile, cancellationToken)
        }));

        api.MapPost("/selection", async (SelectionRequest request, IProfileSelectionService service, CancellationToken cancellationToken) =>
            await service.SelectAsync(request.Address, new ProfileSelection(request.Scenario, request.DeviceProfile), cancellationToken) is { } error
                ? Results.BadRequest(error)
                : Results.Ok());

        return app;
    }
}
