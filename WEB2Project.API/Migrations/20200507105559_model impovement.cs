using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class modelimpovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "FlightReservations");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "FlightReservations");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "FlightReservations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAuthId",
                table: "FlightReservations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FlightReservations");

            migrationBuilder.DropColumn(
                name: "UserAuthId",
                table: "FlightReservations");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "FlightReservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "FlightReservations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
