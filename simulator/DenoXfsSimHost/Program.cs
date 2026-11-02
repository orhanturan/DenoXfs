using DenoXfs.Sim.Host.Extensions;
using DenoXfs.Sim.Host.Transport;
using DenoXfs.Sim.Idc.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, logger) => logger
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));
builder.Services.AddDenoXfsSimHost(builder.Configuration, builder.Environment.ContentRootPath);

var app = builder.Build();
app.UseWebSockets();

// Front end of the simulator: one page per device class module.
app.MapGet("/", () => Results.Redirect("/idc"));
app.MapDenoXfsSimIdc();

app.Map("/ws", async (HttpContext context, IWebSocketSessionRunner runner) =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
    }

    using var socket = await context.WebSockets.AcceptWebSocketAsync();
    var remote = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
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
