using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmaMeblarska.Data.Migrations
{
    public partial class narzedzie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "PracownikId",
                table: "Narzedzie",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Narzedzie_PracownikId",
                table: "Narzedzie",
                column: "PracownikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Narzedzie_Pracownik_PracownikId",
                table: "Narzedzie",
                column: "PracownikId",
                principalTable: "Pracownik",
                principalColumn: "PracownikId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Narzedzie_Pracownik_PracownikId",
                table: "Narzedzie");

            migrationBuilder.DropIndex(
                name: "IX_Narzedzie_PracownikId",
                table: "Narzedzie");

            migrationBuilder.DropColumn(
                name: "PracownikId",
                table: "Narzedzie");
           
        }
    }
}
