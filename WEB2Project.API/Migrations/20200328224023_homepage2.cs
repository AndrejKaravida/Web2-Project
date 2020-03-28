using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class homepage2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_AirCompanies_AirCompanyId",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "AirCompanyId",
                table: "Flights",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_AirCompanies_AirCompanyId",
                table: "Flights",
                column: "AirCompanyId",
                principalTable: "AirCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_AirCompanies_AirCompanyId",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "AirCompanyId",
                table: "Flights",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_AirCompanies_AirCompanyId",
                table: "Flights",
                column: "AirCompanyId",
                principalTable: "AirCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
