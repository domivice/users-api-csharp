/*
 * Users API
 *
 * The users API
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using Domivice.Users.Application;
using Domivice.Users.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Domivice.Users.Web;

/// <summary>
///     Startup
/// </summary>
public class Startup
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    ///     The application configuration.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    ///     This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddWebServices(Configuration);
        services.AddApplicationServices();
        services.AddInfrastructureServices(Configuration);
    }

    /// <summary>
    ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseHsts();

        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseSwagger(c => { c.RouteTemplate = "openapi/{documentName}/openapi.json"; })
            .UseSwaggerUI(c =>
            {
                // set route prefix to openapi, e.g. http://localhost:8080/openapi/index.html
                c.RoutePrefix = "openapi";
                //TODO: Either use the SwaggerGen generated OpenAPI contract (generated from C# classes)
                c.SwaggerEndpoint("/openapi/1.0.0/openapi.json", "Users API");

                //TODO: Or alternatively use the original OpenAPI contract that's included in the static files
                // c.SwaggerEndpoint("/openapi-original.json", "Users API Original");
            });
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers().RequireAuthorization(); });
    }
}