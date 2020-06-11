using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvioIncomes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    AirCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvioIncomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvioIncomes_AirCompanies_AirCompanyId",
                        column: x => x.AirCompanyId,
                        principalTable: "AirCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvioIncomes_AirCompanyId",
                table: "AvioIncomes",
                column: "AirCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvioIncomes");
        }
    }
}
