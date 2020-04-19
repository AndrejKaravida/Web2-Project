using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class reserveddays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "Vehicles");

            migrationBuilder.CreateTable(
                name: "ReservedDate",
                columns: table => new
                {
                    Date = table.Column<DateTime>(nullable: false),
                    VehicleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedDate", x => x.Date);
                    table.ForeignKey(
                        name: "FK_ReservedDate_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservedDate_VehicleId",
                table: "ReservedDate",
                column: "VehicleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservedDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
