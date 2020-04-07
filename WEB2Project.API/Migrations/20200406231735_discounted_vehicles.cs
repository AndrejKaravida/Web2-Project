using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class discounted_vehicles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleOnDiscountId",
                table: "VehicleRatings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VehiclesOnDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    AverageGrade = table.Column<double>(nullable: false),
                    Doors = table.Column<int>(nullable: false),
                    Seats = table.Column<int>(nullable: false),
                    OldPrice = table.Column<int>(nullable: false),
                    NewPrice = table.Column<int>(nullable: false),
                    Photo = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsReserved = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    RentACarCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclesOnDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehiclesOnDiscount_RentACarCompanies_RentACarCompanyId",
                        column: x => x.RentACarCompanyId,
                        principalTable: "RentACarCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRatings_VehicleOnDiscountId",
                table: "VehicleRatings",
                column: "VehicleOnDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclesOnDiscount_RentACarCompanyId",
                table: "VehiclesOnDiscount",
                column: "RentACarCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleRatings_VehiclesOnDiscount_VehicleOnDiscountId",
                table: "VehicleRatings",
                column: "VehicleOnDiscountId",
                principalTable: "VehiclesOnDiscount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleRatings_VehiclesOnDiscount_VehicleOnDiscountId",
                table: "VehicleRatings");

            migrationBuilder.DropTable(
                name: "VehiclesOnDiscount");

            migrationBuilder.DropIndex(
                name: "IX_VehicleRatings_VehicleOnDiscountId",
                table: "VehicleRatings");

            migrationBuilder.DropColumn(
                name: "VehicleOnDiscountId",
                table: "VehicleRatings");
        }
    }
}
