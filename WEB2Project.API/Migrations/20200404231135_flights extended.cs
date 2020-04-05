using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class flightsextended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Mileage",
                table: "Flights",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TicketPrice",
                table: "Flights",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TravelTime",
                table: "Flights",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "TravelTime",
                table: "Flights");
        }
    }
}
