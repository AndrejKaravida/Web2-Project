using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class addedaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirCompanies_Destinations_HeadOfficeId",
                table: "AirCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_RentACarCompanies_RentACarCompanyId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_RentACarCompanies_Destinations_HeadOfficeId",
                table: "RentACarCompanies");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_RentACarCompanyId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "MapString",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "RentACarCompanyId",
                table: "Destinations");

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    MapString = table.Column<string>(nullable: true),
                    RentACarCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_RentACarCompanies_RentACarCompanyId",
                        column: x => x.RentACarCompanyId,
                        principalTable: "RentACarCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_RentACarCompanyId",
                table: "Branches",
                column: "RentACarCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirCompanies_Branches_HeadOfficeId",
                table: "AirCompanies",
                column: "HeadOfficeId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentACarCompanies_Branches_HeadOfficeId",
                table: "RentACarCompanies",
                column: "HeadOfficeId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirCompanies_Branches_HeadOfficeId",
                table: "AirCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_RentACarCompanies_Branches_HeadOfficeId",
                table: "RentACarCompanies");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.AddColumn<string>(
                name: "MapString",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentACarCompanyId",
                table: "Destinations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_RentACarCompanyId",
                table: "Destinations",
                column: "RentACarCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirCompanies_Destinations_HeadOfficeId",
                table: "AirCompanies",
                column: "HeadOfficeId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_RentACarCompanies_RentACarCompanyId",
                table: "Destinations",
                column: "RentACarCompanyId",
                principalTable: "RentACarCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentACarCompanies_Destinations_HeadOfficeId",
                table: "RentACarCompanies",
                column: "HeadOfficeId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
