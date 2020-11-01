using Microsoft.EntityFrameworkCore.Migrations;

namespace TestHackBlazor.Server.Migrations
{
    public partial class EmGpsToCnstrct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ConstructionId",
                table: "GpsTracks",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "Emergencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ConstructionId",
                table: "Emergencies",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_GpsTracks_ConstructionId",
                table: "GpsTracks",
                column: "ConstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_Emergencies_ConstructionId",
                table: "Emergencies",
                column: "ConstructionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emergencies_Constructions_ConstructionId",
                table: "Emergencies",
                column: "ConstructionId",
                principalTable: "Constructions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GpsTracks_Constructions_ConstructionId",
                table: "GpsTracks",
                column: "ConstructionId",
                principalTable: "Constructions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emergencies_Constructions_ConstructionId",
                table: "Emergencies");

            migrationBuilder.DropForeignKey(
                name: "FK_GpsTracks_Constructions_ConstructionId",
                table: "GpsTracks");

            migrationBuilder.DropIndex(
                name: "IX_GpsTracks_ConstructionId",
                table: "GpsTracks");

            migrationBuilder.DropIndex(
                name: "IX_Emergencies_ConstructionId",
                table: "Emergencies");

            migrationBuilder.DropColumn(
                name: "ConstructionId",
                table: "GpsTracks");

            migrationBuilder.DropColumn(
                name: "Checked",
                table: "Emergencies");

            migrationBuilder.DropColumn(
                name: "ConstructionId",
                table: "Emergencies");
        }
    }
}
