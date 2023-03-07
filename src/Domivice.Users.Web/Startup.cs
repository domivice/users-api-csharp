/*
 * Users API
 *
 * The users API
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.IO;
using System.Reflection;
using Domivice.Users.Application;
using Domivice.Users.Infrastructure;
using Domivice.Users.Web.OpenApi;
using Domivice.Users.Web.Filters;
using Domivice.Users.Web.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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
        services.AddWebServices();
        services.AddApplicationServices();
        services.AddInfrastructureServices(Configuration);
        // Add framework services.
        services
            // Don't need the full MVC stack for an API, see https://andrewlock.net/comparing-startup-between-the-asp-net-core-3-templates/
            .AddControllers(options => { options.InputFormatters.Insert(0, new InputFormatterStream()); })
            .AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opts.SerializerSettings.Converters.Add(new StringEnumConverter
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                });
            });

        services
            .AddSwaggerGen(c =>
            {
                c.EnableAnnotations(true, true);

                c.SwaggerDoc("1.0.0", new OpenApiInfo
                {
                    Title = "Users API",
                    Description = "Users API (ASP.NET Core 6.0)",
                    TermsOfService = new Uri("https://github.com/openapitools/openapi-generator"),
                    Contact = new OpenApiContact
                    {
                        Name = "Domivice Development Team",
                        Url = new Uri("https://development.domivice.com"),
                        Email = ""
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Domivice License",
                        Url = new Uri("https://domivice.com")
                    },
                    Version = "1.0.0"
                });
                c.CustomSchemaIds(type => type.FriendlyId(true));
                c.IncludeXmlComments(
                    $"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly().GetName().Name}.xml");

                // Include DataAnnotation attributes on Controller Action parameters as OpenAPI validation rules (e.g required, pattern, ..)
                // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                c.OperationFilter<GeneratePathParamsValidationFilter>();
            });
        services
            .AddSwaggerGenNewtonsoftSupport();
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
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}