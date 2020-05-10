using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class modelimpoveed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "FlightReservations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "FlightReservations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPhoto",
                table: "FlightReservations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "FlightReservations");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "FlightReservations");

            migrationBuilder.DropColumn(
                name: "CompanyPhoto",
                table: "FlightReservations");
        }
    }
}
