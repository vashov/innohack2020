using Microsoft.EntityFrameworkCore.Migrations;

namespace TestHackBlazor.Server.Migrations
{
    public partial class EmergecyLatLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Emergencies",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Emergencies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Emergencies");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Emergencies");
        }
    }
}
