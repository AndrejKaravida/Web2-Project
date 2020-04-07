using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class Modelrework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleRatings_VehiclesOnDiscount_VehicleOnDiscountId",
                table: "VehicleRatings");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "VehiclesOnDiscount");

            migrationBuilder.DropIndex(
                name: "IX_VehicleRatings_VehicleOnDiscountId",
                table: "VehicleRatings");

            migrationBuilder.DropColumn(
                name: "VehicleOnDiscountId",
                table: "VehicleRatings");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "RentACarCompanies");

            migrationBuilder.AddColumn<int>(
                name: "CurrentDestinationId",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnDiscount",
                table: "Vehicles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OldPrice",
                table: "Vehicles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RentACarCompanyId",
                table: "Destinations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CurrentDestinationId",
                table: "Vehicles",
                column: "CurrentDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_RentACarCompanyId",
                table: "Destinations",
                column: "RentACarCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_RentACarCompanies_RentACarCompanyId",
                table: "Destinations",
                column: "RentACarCompanyId",
                principalTable: "RentACarCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Destinations_CurrentDestinationId",
                table: "Vehicles",
                column: "CurrentDestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_RentACarCompanies_RentACarCompanyId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Destinations_CurrentDestinationId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CurrentDestinationId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_RentACarCompanyId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "CurrentDestinationId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "IsOnDiscount",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "OldPrice",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "RentACarCompanyId",
                table: "Destinations");

            migrationBuilder.AddColumn<int>(
                name: "VehicleOnDiscountId",
                table: "VehicleRatings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "RentACarCompanies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentACarCompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_RentACarCompanies_RentACarCompanyId",
                        column: x => x.RentACarCompanyId,
                        principalTable: "RentACarCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehiclesOnDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AverageGrade = table.Column<double>(type: "float", nullable: false),
                    Doors = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsReserved = table.Column<bool>(type: "bit", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewPrice = table.Column<int>(type: "int", nullable: false),
                    OldPrice = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentACarCompanyId = table.Column<int>(type: "int", nullable: true),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_Locations_RentACarCompanyId",
                table: "Locations",
                column: "RentACarCompanyId");

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
    }
}
