using Journey.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Journey.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<VehiclePosition> VehiclePositions { get; }
    DbSet<Stop> Stops { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
