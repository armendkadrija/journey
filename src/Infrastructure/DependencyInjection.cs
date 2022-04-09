using Application.Common.Interfaces;
using Infrastructure.Services;
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
        services.AddDbContext<ApplicationDbContext>(options =>
         options.UseNpgsql(
             configuration.GetConnectionString("DefaultConnection"),
             b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IMqqtService>(provider => provider.GetRequiredService<MqqtService>());

        return services;
    }
}
