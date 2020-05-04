using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class added_rated_users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RatedUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthId = table.Column<string>(nullable: true),
                    AirCompanyId = table.Column<int>(nullable: true),
                    RentACarCompanyId = table.Column<int>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RatedUser");
        }
    }
}
