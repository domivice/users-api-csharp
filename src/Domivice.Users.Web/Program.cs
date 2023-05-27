using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace Domivice.Users.Web;

/// <summary>
///     Program
/// </summary>
public class Program
{
    /// <summary>
    ///     Main
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        //Creating the Logger with Minimum Settings
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
            .CreateLogger();

        //try/catch block will ensure any configuration issues are appropriately logged
        try
        {
            Log.Information("Staring the Host");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host Terminated Unexpectedly");
        }

        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    ///     Create the host builder.
    /// </summary>
    /// <param name="args"></param>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilog((ctx, cfg) =>
            {
                //Override Few of the Configurations
                cfg.Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
                    .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
                    .WriteTo.Console(
                        outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                    .WriteTo.GrafanaLoki(ctx.Configuration["Loki"]);
            })
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}