using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestion_Cartuchos.Migrations
{
    /// <inheritdoc />
    public partial class restore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Oficinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oficinas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Impresoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    modelo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    marca = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    oficinaId = table.Column<int>(type: "int", nullable: false),
                    oficina_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impresoras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impresoras_Oficinas_oficinaId",
                        column: x => x.oficinaId,
                        principalTable: "Oficinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Modelos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    modelo_cartuchos = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    marca = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stock = table.Column<int>(type: "int", nullable: false),
                    ImpresoraId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modelos_Impresoras_ImpresoraId",
                        column: x => x.ImpresoraId,
                        principalTable: "Impresoras",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cartuchos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    numero_serie = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_alta = table.Column<DateOnly>(type: "date", nullable: false),
                    cantidad_recargas = table.Column<int>(type: "int", nullable: false),
                    observaciones = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modeloId = table.Column<int>(type: "int", nullable: false),
                    modelo_id = table.Column<int>(type: "int", nullable: false),
                    estado_id = table.Column<int>(type: "int", nullable: false),
                    estadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartuchos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cartuchos_Estados_estadoId",
                        column: x => x.estadoId,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cartuchos_Modelos_modeloId",
                        column: x => x.modeloId,
                        principalTable: "Modelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Asignar_Impresoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    impresora_id = table.Column<int>(type: "int", nullable: false),
                    impresoraId = table.Column<int>(type: "int", nullable: false),
                    cartucho_id = table.Column<int>(type: "int", nullable: false),
                    cartuchoId = table.Column<int>(type: "int", nullable: false),
                    fecha_asignacion = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_desasignacion = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignar_Impresoras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asignar_Impresoras_Cartuchos_cartuchoId",
                        column: x => x.cartuchoId,
                        principalTable: "Cartuchos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asignar_Impresoras_Impresoras_impresoraId",
                        column: x => x.impresoraId,
                        principalTable: "Impresoras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Recargas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fecha_recarga = table.Column<DateOnly>(type: "date", nullable: false),
                    observaciones = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cartuchoId = table.Column<int>(type: "int", nullable: false),
                    cartucho_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recargas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recargas_Cartuchos_cartuchoId",
                        column: x => x.cartuchoId,
                        principalTable: "Cartuchos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Asignar_Impresoras_cartuchoId",
                table: "Asignar_Impresoras",
                column: "cartuchoId");

            migrationBuilder.CreateIndex(
                name: "IX_Asignar_Impresoras_impresoraId",
                table: "Asignar_Impresoras",
                column: "impresoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Cartuchos_estadoId",
                table: "Cartuchos",
                column: "estadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cartuchos_modeloId",
                table: "Cartuchos",
                column: "modeloId");

            migrationBuilder.CreateIndex(
                name: "IX_Impresoras_oficinaId",
                table: "Impresoras",
                column: "oficinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Modelos_ImpresoraId",
                table: "Modelos",
                column: "ImpresoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Recargas_cartuchoId",
                table: "Recargas",
                column: "cartuchoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asignar_Impresoras");

            migrationBuilder.DropTable(
                name: "Recargas");

            migrationBuilder.DropTable(
                name: "Cartuchos");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Modelos");

            migrationBuilder.DropTable(
                name: "Impresoras");

            migrationBuilder.DropTable(
                name: "Oficinas");
        }
    }
}
