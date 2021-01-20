using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmaMeblarska.Data.Migrations
{
    public partial class zamnrfaktura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NrFaktura",
                table: "Zamowienie",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NrFaktura",
                table: "Zamowienie");
        }
    }
}
