using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestion_Cartuchos.Migrations
{
    /// <inheritdoc />
    public partial class eliminoRelacionInnecesaria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "modelo_cartuchos",
                table: "Modelos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "modelo_cartuchos",
                table: "Modelos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
