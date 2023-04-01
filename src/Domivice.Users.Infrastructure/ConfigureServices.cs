using System.Reflection;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Infrastructure.Consumers;
using Domivice.Users.Infrastructure.Consumers.Exceptions;
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
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetValue<string>("RabbitMq:Host"));
                cfg.ReceiveEndpoint(configuration.GetValue<string>("RabbitMq:NewUserIdentitiesQueue"), c =>
                {
                    c.ConfigureConsumer<UserIdentityCreatedConsumer>(context);
                    c.UseMessageRetry(r =>
                    {
                        r.Ignore(typeof(ValidationException),typeof(CreateUserCommandException));
                        r.Immediate(5);
                    });
                });
            });
        });
       
        services.AddTransient<IMessageBus, MassTransitMessageBus>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        return services;
    }
}