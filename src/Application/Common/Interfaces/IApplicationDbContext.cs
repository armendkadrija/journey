using Journey.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Journey.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Route> Routes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
