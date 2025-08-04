using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_IK_Uygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class BordroModeliEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Maas",
                table: "Personeller",
                newName: "BrutMaas");

            migrationBuilder.CreateTable(
                name: "Bordrolar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Donem = table.Column<string>(type: "TEXT", nullable: false),
                    BrutMaas = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    EkOdemeler = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ToplamKesintiler = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    NetMaas = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bordrolar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bordrolar_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bordrolar_PersonelId",
                table: "Bordrolar",
                column: "PersonelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bordrolar");

            migrationBuilder.RenameColumn(
                name: "BrutMaas",
                table: "Personeller",
                newName: "Maas");
        }
    }
}
