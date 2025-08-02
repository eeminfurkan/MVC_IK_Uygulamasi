using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_IK_Uygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class SicilNoEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SicilNo",
                table: "Personeller",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SicilNo",
                table: "Personeller");
        }
    }
}
