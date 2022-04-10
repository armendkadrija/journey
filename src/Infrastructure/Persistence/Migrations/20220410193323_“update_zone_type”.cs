using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Journey.Infrastructure.Persistence.Migrations
{
    public partial class update_zone_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zone_id",
                table: "stops");

            migrationBuilder.AddColumn<string>(
                name: "zone",
                table: "stops",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zone",
                table: "stops");

            migrationBuilder.AddColumn<int>(
                name: "zone_id",
                table: "stops",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
