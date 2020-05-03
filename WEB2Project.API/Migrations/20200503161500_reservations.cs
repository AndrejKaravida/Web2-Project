using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class reservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReturningLocation",
                table: "Reservations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartingLocation",
                table: "Reservations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturningLocation",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "StartingLocation",
                table: "Reservations");
        }
    }
}
