using Microsoft.EntityFrameworkCore.Migrations;

namespace TestHackBlazor.Server.Migrations
{
    public partial class ConstructionWithShiftsV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ConstructionId",
                table: "UserShiftEvents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Constructions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Constructions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserShiftEvents_ConstructionId",
                table: "UserShiftEvents",
                column: "ConstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_Constructions_Code",
                table: "Constructions",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserShiftEvents_Constructions_ConstructionId",
                table: "UserShiftEvents",
                column: "ConstructionId",
                principalTable: "Constructions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShiftEvents_Constructions_ConstructionId",
                table: "UserShiftEvents");

            migrationBuilder.DropIndex(
                name: "IX_UserShiftEvents_ConstructionId",
                table: "UserShiftEvents");

            migrationBuilder.DropIndex(
                name: "IX_Constructions_Code",
                table: "Constructions");

            migrationBuilder.DropColumn(
                name: "ConstructionId",
                table: "UserShiftEvents");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Constructions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Constructions");
        }
    }
}
