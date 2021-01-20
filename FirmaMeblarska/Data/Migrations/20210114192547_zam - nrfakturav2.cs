using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmaMeblarska.Data.Migrations
{
    public partial class zamnrfakturav2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HasChild",
                table: "Zamowienie",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "Zamowienie",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasChild",
                table: "Zamowienie");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Zamowienie");
        }
    }
}
