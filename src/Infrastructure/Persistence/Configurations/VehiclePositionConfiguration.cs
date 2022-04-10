using Journey.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Journey.Infrastructure.Persistence.Configurations;

public class VehiclePositionCommand : IEntityTypeConfiguration<VehiclePosition>
{
    public void Configure(EntityTypeBuilder<VehiclePosition> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.HasIndex(prop => new { prop.Latitude, prop.Longitude });
    }
}
