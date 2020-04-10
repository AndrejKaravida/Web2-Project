using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class vehiclechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Destinations_CurrentDestinationId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CurrentDestinationId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CurrentDestinationId",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "CurrentDestination",
                table: "Vehicles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentDestination",
                table: "Vehicles");

            migrationBuilder.AddColumn<int>(
                name: "CurrentDestinationId",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CurrentDestinationId",
                table: "Vehicles",
                column: "CurrentDestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Destinations_CurrentDestinationId",
                table: "Vehicles",
                column: "CurrentDestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
