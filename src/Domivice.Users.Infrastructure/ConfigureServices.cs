using System.Reflection;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Infrastructure.Persistence;
using Domivice.Users.Infrastructure.Persistence.Interceptors;
using Domivice.Users.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domivice.Users.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<DomiviceSaveChangesInterceptor>();
        services.AddDbContext<UsersDbContext>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(UsersDbContext).Assembly.FullName));
                options.AddInterceptors(provider.GetRequiredService<DomiviceSaveChangesInterceptor>());
            }
        );
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetExecutingAssembly());
            x.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
        });
        services.AddTransient<IMessageBus, MassTransitMessageBus>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        return services;
    }
}