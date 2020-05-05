using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class changeRatingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RatedUser");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "VehicleRatings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AirCompanyId",
                table: "CompanyRatings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CompanyRatings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FlightRating",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    FlightId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightRating_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRatings_AirCompanyId",
                table: "CompanyRatings",
                column: "AirCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightRating_FlightId",
                table: "FlightRating",
                column: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyRatings_AirCompanies_AirCompanyId",
                table: "CompanyRatings",
                column: "AirCompanyId",
                principalTable: "AirCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyRatings_AirCompanies_AirCompanyId",
                table: "CompanyRatings");

            migrationBuilder.DropTable(
                name: "FlightRating");

            migrationBuilder.DropIndex(
                name: "IX_CompanyRatings_AirCompanyId",
                table: "CompanyRatings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "VehicleRatings");

            migrationBuilder.DropColumn(
                name: "AirCompanyId",
                table: "CompanyRatings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CompanyRatings");

            migrationBuilder.CreateTable(
                name: "RatedUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirCompanyId = table.Column<int>(type: "int", nullable: true),
                    AuthId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentACarCompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatedUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatedUser_AirCompanies_AirCompanyId",
                        column: x => x.AirCompanyId,
                        principalTable: "AirCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RatedUser_RentACarCompanies_RentACarCompanyId",
                        column: x => x.RentACarCompanyId,
                        principalTable: "RentACarCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RatedUser_AirCompanyId",
                table: "RatedUser",
                column: "AirCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RatedUser_RentACarCompanyId",
                table: "RatedUser",
                column: "RentACarCompanyId");
        }
    }
}
