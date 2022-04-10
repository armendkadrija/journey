using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Journey.Infrastructure.Persistence.Migrations
{
    public partial class added_location_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_vehicle_positions_latitude_longitude",
                table: "vehicle_positions");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_positions_latitude_longitude_location",
                table: "vehicle_positions",
                columns: new[] { "latitude", "longitude", "location" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_vehicle_positions_latitude_longitude_location",
                table: "vehicle_positions");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_positions_latitude_longitude",
                table: "vehicle_positions",
                columns: new[] { "latitude", "longitude" });
        }
    }
}
