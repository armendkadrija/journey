using System.Reflection;
using Journey.Application.Common.Interfaces;
using Journey.Domain.Common;
using Journey.Domain.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Journey.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Route> Routes => Set<Route>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
