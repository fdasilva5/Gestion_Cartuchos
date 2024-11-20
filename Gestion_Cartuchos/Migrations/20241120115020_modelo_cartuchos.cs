using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestion_Cartuchos.Migrations
{
    /// <inheritdoc />
    public partial class modelo_cartuchos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "modelo_cartuchos",
                table: "Modelos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "modelo_cartuchos",
                table: "Modelos");
        }
    }
}
