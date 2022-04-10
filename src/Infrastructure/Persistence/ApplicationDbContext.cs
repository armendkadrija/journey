using System.Reflection;
using Journey.Application.Common.Interfaces;
using Journey.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Journey.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<VehiclePosition> VehiclePositions => Set<VehiclePosition>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
