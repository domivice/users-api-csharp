using Domivice.Users.Infrastructure.Persistence;
using Domivice.Users.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;

namespace Domivice.Users.Tests;

public class TestServer : WebApplicationFactory<Startup>
{
    private Respawner _reSpawner;

    public TestServer()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();
    }

    private IConfiguration Configuration { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config => { config.AddConfiguration(Configuration); });
        builder.ConfigureTestServices(services =>
        {
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<UsersDbContext>();
            dbContext.Database.Migrate();
            dbContext.Database.EnsureCreated();

            _reSpawner = Respawner.CreateAsync(
                    Configuration.GetConnectionString("DefaultConnection"),
                    new RespawnerOptions
                    {
                        SchemasToInclude = new[] {"dbo"}, WithReseed = true,
                        TablesToIgnore = new[] {new Table("__EFMigrationsHistory")}
                    }).GetAwaiter()
                .GetResult();
        });
        base.ConfigureWebHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        _reSpawner.ResetAsync(Configuration.GetConnectionString("DefaultConnection")).GetAwaiter().GetResult();
        base.Dispose(disposing);
    }

    public IServiceScope GetServiceScope()
    {
        var serviceScope = Services.GetRequiredService<IServiceScopeFactory>();
        return serviceScope.CreateScope();
    }
}