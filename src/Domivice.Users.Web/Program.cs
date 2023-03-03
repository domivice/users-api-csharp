using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    ///     Create the host builder.
    /// </summary>
    /// <param name="args"></param>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>()
                    .UseUrls("https://0.0.0.0:5005/");
            });
    }
}