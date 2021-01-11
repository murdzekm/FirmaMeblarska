using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmaMeblarska.Data.Migrations
{
    public partial class narzedzienull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Narzedzie_Pracownik_PracownikId",
                table: "Narzedzie");

            migrationBuilder.AlterColumn<int>(
                name: "PracownikId",
                table: "Narzedzie",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Narzedzie_Pracownik_PracownikId",
                table: "Narzedzie",
                column: "PracownikId",
                principalTable: "Pracownik",
                principalColumn: "PracownikId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Narzedzie_Pracownik_PracownikId",
                table: "Narzedzie");

            migrationBuilder.AlterColumn<int>(
                name: "PracownikId",
                table: "Narzedzie",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Narzedzie_Pracownik_PracownikId",
                table: "Narzedzie",
                column: "PracownikId",
                principalTable: "Pracownik",
                principalColumn: "PracownikId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
