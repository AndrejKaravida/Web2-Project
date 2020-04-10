using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB2Project.Migrations
{
    public partial class reservedDatechanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservedDate",
                table: "ReservedDate");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ReservedDate",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservedDate",
                table: "ReservedDate",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservedDate",
                table: "ReservedDate");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ReservedDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservedDate",
                table: "ReservedDate",
                column: "Date");
        }
    }
}
