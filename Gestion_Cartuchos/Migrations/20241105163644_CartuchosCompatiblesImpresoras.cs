using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestion_Cartuchos.Migrations
{
    /// <inheritdoc />
    public partial class CartuchosCompatiblesImpresoras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImpresoraId",
                table: "Modelos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modelos_ImpresoraId",
                table: "Modelos",
                column: "ImpresoraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modelos_Impresoras_ImpresoraId",
                table: "Modelos",
                column: "ImpresoraId",
                principalTable: "Impresoras",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modelos_Impresoras_ImpresoraId",
                table: "Modelos");

            migrationBuilder.DropIndex(
                name: "IX_Modelos_ImpresoraId",
                table: "Modelos");

            migrationBuilder.DropColumn(
                name: "ImpresoraId",
                table: "Modelos");
        }
    }
}
