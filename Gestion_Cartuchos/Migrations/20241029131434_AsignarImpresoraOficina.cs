using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestion_Cartuchos.Migrations
{
    /// <inheritdoc />
    public partial class AsignarImpresoraOficina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "fecha_desasignacion",
                table: "Asignar_Impresoras",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "oficinaId",
                table: "Asignar_Impresoras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "oficina_id",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "oficina_id",
                table: "Asignar_Impresoras");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "fecha_desasignacion",
                table: "Asignar_Impresoras",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }
    }
}
