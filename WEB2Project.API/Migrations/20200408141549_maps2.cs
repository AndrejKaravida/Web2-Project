using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class maps2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AirCompanies");

            migrationBuilder.AddColumn<int>(
                name: "HeadOfficeId",
                table: "AirCompanies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AirCompanies_HeadOfficeId",
                table: "AirCompanies",
                column: "HeadOfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirCompanies_Destinations_HeadOfficeId",
                table: "AirCompanies",
                column: "HeadOfficeId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirCompanies_Destinations_HeadOfficeId",
                table: "AirCompanies");

            migrationBuilder.DropIndex(
                name: "IX_AirCompanies_HeadOfficeId",
                table: "AirCompanies");

            migrationBuilder.DropColumn(
                name: "HeadOfficeId",
                table: "AirCompanies");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AirCompanies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
