using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmaMeblarska.Data.Migrations
{
    public partial class zampl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "ZamowieniePlyta",
                columns: table => new
                {
                    ZamowienieId = table.Column<int>(nullable: false),
                    PlytaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZamowieniePlyta", x => new { x.ZamowienieId, x.PlytaId });
                    table.ForeignKey(
                        name: "FK_ZamowieniePlyta_Plyta_PlytaId",
                        column: x => x.PlytaId,
                        principalTable: "Plyta",
                        principalColumn: "PlytaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZamowieniePlyta_Zamowienie_ZamowienieId",
                        column: x => x.ZamowienieId,
                        principalTable: "Zamowienie",
                        principalColumn: "ZamowienieId",
                        onDelete: ReferentialAction.Cascade);
                });

          

            migrationBuilder.CreateIndex(
                name: "IX_ZamowieniePlyta_PlytaId",
                table: "ZamowieniePlyta",
                column: "PlytaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZamowieniePlyta");

           
        }
    }
}
