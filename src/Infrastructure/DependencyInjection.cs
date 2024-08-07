﻿using Infrastructure.Configurations;
using Journey.Application.Common.Interfaces;
using Journey.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Journey.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b =>
                {
                    b.UseNetTopologySuite();
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                }
            ).UseSnakeCaseNamingConvention()
        );

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddSingleton(cfg => configuration.GetSection(nameof(MQTTOptions)).Get<MQTTOptions>());

        return services;
    }
}