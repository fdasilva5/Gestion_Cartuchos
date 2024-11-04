using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestion_Cartuchos.Migrations
{
    /// <inheritdoc />
    public partial class stock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stock",
                table: "Cartuchos");

            migrationBuilder.AddColumn<int>(
                name: "stock",
                table: "Modelos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stock",
                table: "Modelos");

            migrationBuilder.AddColumn<int>(
                name: "stock",
                table: "Cartuchos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
