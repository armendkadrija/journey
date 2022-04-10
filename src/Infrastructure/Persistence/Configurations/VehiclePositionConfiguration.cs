using Journey.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;

namespace Journey.Infrastructure.Persistence.Configurations;

public class VehiclePositionCommand : IEntityTypeConfiguration<VehiclePosition>
{
    public void Configure(EntityTypeBuilder<VehiclePosition> builder)
    {
        builder.HasKey(prop => prop.Id);
        builder.HasIndex(prop => new { prop.Latitude, prop.Longitude });
        builder
            .Property(b => b.Location)
            .HasColumnType("geography (point)")
            .HasDefaultValue(Point.Empty);
    }
}
