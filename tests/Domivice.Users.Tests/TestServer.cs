using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Domivice.Users.Infrastructure.Persistence;
using Domivice.Users.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Respawn;
using Respawn.Graph;

namespace Domivice.Users.Tests;

public class TestServer : WebApplicationFactory<Startup>
{
    private Respawner _reSpawner;
    public const string BaseAddress = "https://localhost:5005/";

    public TestServer()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();
    }

    public HttpClient CreateAuthenticatedClient(IEnumerable<Claim>? claims = null)
    {
        var client = CreateClient();
        client.BaseAddress = new Uri(BaseAddress);
        var accessToken = TestJwtManager.GenerateJwtToken(claims);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return client;
    }

    private IConfiguration Configuration { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config => { config.AddConfiguration(Configuration); });
        builder.ConfigureTestServices(services =>
        {

            services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.ConfigurationManager = null;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = TestJwtManager.SecurityKey,
                    ValidIssuer = TestJwtManager.Issuer,
                    ValidAudience = TestJwtManager.Audience
                };
            });

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