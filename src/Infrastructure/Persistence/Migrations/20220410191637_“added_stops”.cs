using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Journey.Infrastructure.Persistence.Migrations
{
    public partial class added_stops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_vehicle_positions_latitude_longitude_location",
                table: "vehicle_positions");

            migrationBuilder.AddColumn<int>(
                name: "stop_id",
                table: "vehicle_positions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "stops",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    zone_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stops", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_positions_stop_id",
                table: "vehicle_positions",
                column: "stop_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_positions_time_stamp_location",
                table: "vehicle_positions",
                columns: new[] { "time_stamp", "location" });

            migrationBuilder.AddForeignKey(
                name: "fk_vehicle_positions_stops_stop_id",
                table: "vehicle_positions",
                column: "stop_id",
                principalTable: "stops",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_vehicle_positions_stops_stop_id",
                table: "vehicle_positions");

            migrationBuilder.DropTable(
                name: "stops");

            migrationBuilder.DropIndex(
                name: "ix_vehicle_positions_stop_id",
                table: "vehicle_positions");

            migrationBuilder.DropIndex(
                name: "ix_vehicle_positions_time_stamp_location",
                table: "vehicle_positions");

            migrationBuilder.DropColumn(
                name: "stop_id",
                table: "vehicle_positions");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_positions_latitude_longitude_location",
                table: "vehicle_positions",
                columns: new[] { "latitude", "longitude", "location" });
        }
    }
}
