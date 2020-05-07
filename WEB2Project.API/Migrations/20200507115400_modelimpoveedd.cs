using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class modelimpoveedd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "FlightReservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightReservations_FlightId",
                table: "FlightReservations",
                column: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightReservations_Flights_FlightId",
                table: "FlightReservations",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightReservations_Flights_FlightId",
                table: "FlightReservations");

            migrationBuilder.DropIndex(
                name: "IX_FlightReservations_FlightId",
                table: "FlightReservations");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "FlightReservations");
        }
    }
}
