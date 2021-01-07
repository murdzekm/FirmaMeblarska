using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmaMeblarska.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "Adres",
                columns: table => new
                {
                    AdresId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Miejscowosc = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Ulica = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    NrDomu = table.Column<string>(nullable: true),
                    NrLokalu = table.Column<string>(nullable: true),
                    KodPocztowy = table.Column<string>(type: "nvarchar(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adres", x => x.AdresId);
                });

            migrationBuilder.CreateTable(
                name: "Magazyn",
                columns: table => new
                {
                    CzescId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    Ilosc = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magazyn", x => x.CzescId);
                });

            migrationBuilder.CreateTable(
                name: "Maszyna",
                columns: table => new
                {
                    MaszynaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(40)", nullable: false),
                    NrSeryjny = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DataPrzegladu = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maszyna", x => x.MaszynaId);
                });

            migrationBuilder.CreateTable(
                name: "Narzedzie",
                columns: table => new
                {
                    NarzedzieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    NrSeryjny = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Narzedzie", x => x.NarzedzieId);
                });

            migrationBuilder.CreateTable(
                name: "Plyta",
                columns: table => new
                {
                    PlytaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(6)", nullable: false),
                    Ilosc = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plyta", x => x.PlytaId);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Zespol",
                columns: table => new
                {
                    ZespolId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zespol", x => x.ZespolId);
                });

            migrationBuilder.CreateTable(
                name: "Klient",
                columns: table => new
                {
                    KlientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    AdresId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klient", x => x.KlientId);
                    table.ForeignKey(
                        name: "FK_Klient_Adres_AdresId",
                        column: x => x.AdresId,
                        principalTable: "Adres",
                        principalColumn: "AdresId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pracownik",
                columns: table => new
                {
                    PracownikId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Telefon = table.Column<int>(nullable: false),
                    AdresId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pracownik", x => x.PracownikId);
                    table.ForeignKey(
                        name: "FK_Pracownik_Adres_AdresId",
                        column: x => x.AdresId,
                        principalTable: "Adres",
                        principalColumn: "AdresId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZespolPracownik",
                columns: table => new
                {
                    ZespolId = table.Column<int>(nullable: false),
                    PracownikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZespolPracownik", x => new { x.ZespolId, x.PracownikId });
                    table.ForeignKey(
                        name: "FK_ZespolPracownik_Pracownik_PracownikId",
                        column: x => x.PracownikId,
                        principalTable: "Pracownik",
                        principalColumn: "PracownikId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZespolPracownik_Zespol_ZespolId",
                        column: x => x.ZespolId,
                        principalTable: "Zespol",
                        principalColumn: "ZespolId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Klient_AdresId",
                table: "Klient",
                column: "AdresId");

            migrationBuilder.CreateIndex(
                name: "IX_Pracownik_AdresId",
                table: "Pracownik",
                column: "AdresId");

            migrationBuilder.CreateIndex(
                name: "IX_ZespolPracownik_PracownikId",
                table: "ZespolPracownik",
                column: "PracownikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Klient");

            migrationBuilder.DropTable(
                name: "Magazyn");

            migrationBuilder.DropTable(
                name: "Maszyna");

            migrationBuilder.DropTable(
                name: "Narzedzie");

            migrationBuilder.DropTable(
                name: "Plyta");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "ZespolPracownik");

            migrationBuilder.DropTable(
                name: "Pracownik");

            migrationBuilder.DropTable(
                name: "Zespol");

            migrationBuilder.DropTable(
                name: "Adres");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
