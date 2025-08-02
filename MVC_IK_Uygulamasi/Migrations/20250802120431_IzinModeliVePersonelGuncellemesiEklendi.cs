using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_IK_Uygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class IzinModeliVePersonelGuncellemesiEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Maas",
                table: "Personeller",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TCKimlikNo",
                table: "Personeller",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefonNumarasi",
                table: "Personeller",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Izinler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonelId = table.Column<int>(type: "INTEGER", nullable: false),
                    IzinTuru = table.Column<string>(type: "TEXT", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OnayDurumu = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izinler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Izinler_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Izinler_PersonelId",
                table: "Izinler",
                column: "PersonelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Izinler");

            migrationBuilder.DropColumn(
                name: "Maas",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "TCKimlikNo",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "TelefonNumarasi",
                table: "Personeller");
        }
    }
}
