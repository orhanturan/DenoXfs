using DenoXfs.TestTool.Extensions;
using DenoXfs.TestTool.Logging;
using DenoXfs.TestTool.Ui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DenoXfs.TestTool;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // Relative paths (appsettings, logs) are resolved against the program folder.
        Environment.CurrentDirectory = AppContext.BaseDirectory;
        ApplicationConfiguration.Initialize();

        var logFeed = new UiLogFeed();
        using var host = Host.CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            // Serilog writes the detailed log to the file only; the screen shows the XFS log (IXfsTrafficLog).
            .UseSerilog((context, services, logger) => logger
                .ReadFrom.Configuration(context.Configuration))
            .ConfigureServices(services => services.AddDenoXfsTestTool(logFeed))
            .Build();

        // Starts the XFS message thread (hidden window) that receives completions and events.
        host.Start();
        try
        {
            Application.Run(host.Services.GetRequiredService<MainForm>());
        }
        finally
        {
            host.StopAsync().GetAwaiter().GetResult();
            Log.CloseAndFlush();
        }
    }
}
