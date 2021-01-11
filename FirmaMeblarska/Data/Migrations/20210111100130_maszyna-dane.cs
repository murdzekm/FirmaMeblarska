using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmaMeblarska.Data.Migrations
{
    public partial class maszynadane : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PracownikMaszyna",
                columns: table => new
                {
                    PracownikId = table.Column<int>(nullable: false),
                    MaszynaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracownikMaszyna", x => new { x.MaszynaId, x.PracownikId });
                    table.ForeignKey(
                        name: "FK_PracownikMaszyna_Maszyna_MaszynaId",
                        column: x => x.MaszynaId,
                        principalTable: "Maszyna",
                        principalColumn: "MaszynaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PracownikMaszyna_Pracownik_PracownikId",
                        column: x => x.PracownikId,
                        principalTable: "Pracownik",
                        principalColumn: "PracownikId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PracownikMaszyna_PracownikId",
                table: "PracownikMaszyna",
                column: "PracownikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PracownikMaszyna");
        }
    }
}
