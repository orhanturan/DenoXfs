using DenoXfs.Sim.Core.Ui;
using DenoXfs.Sim.Host.Components;
using DenoXfs.Sim.Host.Extensions;
using DenoXfs.Sim.Host.Transport;
using DenoXfs.Sim.Host.Web;
using DenoXfs.Sim.Idc.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, logger) => logger
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));
builder.Services.AddDenoXfsSimHost(builder.Configuration, builder.Environment.ContentRootPath);
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();
app.UseWebSockets();
app.UseAntiforgery();

// Front end of the simulator: the terminal page (Blazor; device modules add their panels) and the JSON APIs.
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapDenoXfsSimProfiles();
app.MapDenoXfsSimHardware();
app.MapDenoXfsSimIdc();

app.Map("/ws", async (HttpContext context, IWebSocketSessionRunner runner) =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
    }

    using var socket = await context.WebSockets.AcceptWebSocketAsync();
    var remote = TerminalAddress.Normalize(context.Connection.RemoteIpAddress);
    await runner.RunAsync(socket, remote, context.RequestAborted);
});

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
