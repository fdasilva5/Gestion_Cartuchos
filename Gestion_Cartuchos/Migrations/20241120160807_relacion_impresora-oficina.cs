using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestion_Cartuchos.Migrations
{
    /// <inheritdoc />
    public partial class relacion_impresoraoficina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asignar_Impresoras_Oficinas_oficinaId",
                table: "Asignar_Impresoras");

            migrationBuilder.DropIndex(
                name: "IX_Asignar_Impresoras_oficinaId",
                table: "Asignar_Impresoras");

            migrationBuilder.DropColumn(
                name: "oficinaId",
                table: "Asignar_Impresoras");

            migrationBuilder.AddColumn<int>(
                name: "oficinaId",
                table: "Impresoras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "oficina_id",
                table: "Impresoras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Impresoras_oficinaId",
                table: "Impresoras",
                column: "oficinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Impresoras_Oficinas_oficinaId",
                table: "Impresoras",
                column: "oficinaId",
                principalTable: "Oficinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Impresoras_Oficinas_oficinaId",
                table: "Impresoras");

            migrationBuilder.DropIndex(
                name: "IX_Impresoras_oficinaId",
                table: "Impresoras");

            migrationBuilder.DropColumn(
                name: "oficinaId",
                table: "Impresoras");

            migrationBuilder.DropColumn(
                name: "oficina_id",
                table: "Impresoras");

            migrationBuilder.AddColumn<int>(
                name: "oficinaId",
                table: "Asignar_Impresoras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Asignar_Impresoras_oficinaId",
                table: "Asignar_Impresoras",
                column: "oficinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asignar_Impresoras_Oficinas_oficinaId",
                table: "Asignar_Impresoras",
                column: "oficinaId",
                principalTable: "Oficinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
