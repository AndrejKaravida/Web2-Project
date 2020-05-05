using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class initialmigratioon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightReservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    DepartureDestination = table.Column<string>(nullable: true),
                    ArrivalDestination = table.Column<string>(nullable: true),
                    DepartureDate = table.Column<DateTime>(nullable: false),
                    ArrivalDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    TravelLength = table.Column<double>(nullable: false),
                    Seats = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightReservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    NeedToChangePassword = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    MapString = table.Column<string>(nullable: true),
                    AirCompanyId = table.Column<int>(nullable: true),
                    RentACarCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    PromoDescription = table.Column<string>(nullable: true),
                    AverageGrade = table.Column<double>(nullable: false),
                    Photo = table.Column<string>(nullable: true),
                    HeadOfficeId = table.Column<int>(nullable: true),
                    AdminId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirCompanies_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AirCompanies_Destinations_HeadOfficeId",
                        column: x => x.HeadOfficeId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentACarCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    PromoDescription = table.Column<string>(nullable: true),
                    AverageGrade = table.Column<double>(nullable: false),
                    WeekRentalDiscount = table.Column<double>(nullable: false),
                    MonthRentalDiscount = table.Column<double>(nullable: false),
                    HeadOfficeId = table.Column<int>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    AdminId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentACarCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentACarCompanies_Users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentACarCompanies_Destinations_HeadOfficeId",
                        column: x => x.HeadOfficeId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartureTime = table.Column<DateTime>(nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    DepartureDestinationId = table.Column<int>(nullable: true),
                    ArrivalDestinationId = table.Column<int>(nullable: true),
                    TravelTime = table.Column<double>(nullable: false),
                    Mileage = table.Column<double>(nullable: false),
                    AverageGrade = table.Column<double>(nullable: false),
                    Discount = table.Column<bool>(nullable: false),
                    TicketPrice = table.Column<double>(nullable: false),
                    AirCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_AirCompanies_AirCompanyId",
                        column: x => x.AirCompanyId,
                        principalTable: "AirCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_Destinations_ArrivalDestinationId",
                        column: x => x.ArrivalDestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flights_Destinations_DepartureDestinationId",
                        column: x => x.DepartureDestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyRatings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(nullable: false),
                    RentACarCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyRatings_RentACarCompanies_RentACarCompanyId",
                        column: x => x.RentACarCompanyId,
                        principalTable: "RentACarCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Income",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    RentACarCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Income", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Income_RentACarCompanies_RentACarCompanyId",
                        column: x => x.RentACarCompanyId,
                        principalTable: "RentACarCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    AverageGrade = table.Column<double>(nullable: false),
                    CurrentDestination = table.Column<string>(nullable: true),
                    Doors = table.Column<int>(nullable: false),
                    Seats = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Photo = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsOnDiscount = table.Column<bool>(nullable: false),
                    OldPrice = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    RentACarCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_RentACarCompanies_RentACarCompanyId",
                        column: x => x.RentACarCompanyId,
                        principalTable: "RentACarCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    VehicleId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    NumberOfDays = table.Column<int>(nullable: false),
                    DaysLeft = table.Column<double>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservedDate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    VehicleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservedDate_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleRatings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(nullable: false),
                    VehicleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleRatings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirCompanies_AdminId",
                table: "AirCompanies",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AirCompanies_HeadOfficeId",
                table: "AirCompanies",
                column: "HeadOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRatings_RentACarCompanyId",
                table: "CompanyRatings",
                column: "RentACarCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_AirCompanyId",
                table: "Destinations",
                column: "AirCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_RentACarCompanyId",
                table: "Destinations",
                column: "RentACarCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirCompanyId",
                table: "Flights",
                column: "AirCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ArrivalDestinationId",
                table: "Flights",
                column: "ArrivalDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DepartureDestinationId",
                table: "Flights",
                column: "DepartureDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Income_RentACarCompanyId",
                table: "Income",
                column: "RentACarCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RentACarCompanies_AdminId",
                table: "RentACarCompanies",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_RentACarCompanies_HeadOfficeId",
                table: "RentACarCompanies",
                column: "HeadOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VehicleId",
                table: "Reservations",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedDate_VehicleId",
                table: "ReservedDate",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRatings_VehicleId",
                table: "VehicleRatings",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RentACarCompanyId",
                table: "Vehicles",
                column: "RentACarCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_RentACarCompanies_RentACarCompanyId",
                table: "Destinations",
                column: "RentACarCompanyId",
                principalTable: "RentACarCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_AirCompanies_AirCompanyId",
                table: "Destinations",
                column: "AirCompanyId",
                principalTable: "AirCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirCompanies_Users_AdminId",
                table: "AirCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_RentACarCompanies_Users_AdminId",
                table: "RentACarCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_AirCompanies_Destinations_HeadOfficeId",
                table: "AirCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_RentACarCompanies_Destinations_HeadOfficeId",
                table: "RentACarCompanies");

            migrationBuilder.DropTable(
                name: "CompanyRatings");

            migrationBuilder.DropTable(
                name: "FlightReservations");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Income");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "ReservedDate");

            migrationBuilder.DropTable(
                name: "VehicleRatings");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "AirCompanies");

            migrationBuilder.DropTable(
                name: "RentACarCompanies");
        }
    }
}
