using System;
using System.IO;
using Domivice.Users.Web.Filters;
using Domivice.Users.Web.Formatters;
using Domivice.Users.Web.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Domivice.Users.Web;

/// <summary>
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration.GetValue<string>("IdentityServer:Authority");
                options.Audience = configuration.GetValue<string>("IdentityServer:Audience");
                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.ValidTypes = new[] {"at+jwt"};
            });

        services.AddSwagger();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
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
                    $"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}Domivice.Users.Web.xml");

                // Include DataAnnotation attributes on Controller Action parameters as OpenAPI validation rules (e.g required, pattern, ..)
                // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                c.OperationFilter<GeneratePathParamsValidationFilter>();
            });
        services
            .AddSwaggerGenNewtonsoftSupport();

        return services;
    }
}