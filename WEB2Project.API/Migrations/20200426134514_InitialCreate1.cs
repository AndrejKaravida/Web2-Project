using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class InitialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_AirCompanies_AirCompanyId",
                table: "Destinations");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_AirCompanyId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "AirCompanyId",
                table: "Destinations");

            migrationBuilder.CreateTable(
                name: "DestinationsOfCompany",
                columns: table => new
                {
                    CompanyDestinationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: true),
                    DestinationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinationsOfCompany", x => x.CompanyDestinationId);
                    table.ForeignKey(
                        name: "FK_DestinationsOfCompany_AirCompanies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "AirCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DestinationsOfCompany_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DestinationsOfCompany_CompanyId",
                table: "DestinationsOfCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DestinationsOfCompany_DestinationId",
                table: "DestinationsOfCompany",
                column: "DestinationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DestinationsOfCompany");

            migrationBuilder.AddColumn<int>(
                name: "AirCompanyId",
                table: "Destinations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_AirCompanyId",
                table: "Destinations",
                column: "AirCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_AirCompanies_AirCompanyId",
                table: "Destinations",
                column: "AirCompanyId",
                principalTable: "AirCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
