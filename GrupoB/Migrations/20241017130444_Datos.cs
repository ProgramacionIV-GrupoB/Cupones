using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CuponesApi.Migrations
{
    /// <inheritdoc />
    public partial class Datos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id_Categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id_Categoria);
                });

            migrationBuilder.CreateTable(
                name: "Cupones_Clientes",
                columns: table => new
                {
                    NroCupon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Cupon = table.Column<int>(type: "int", nullable: false),
                    FechaAsignado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CodCliente = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupones_Clientes", x => x.NroCupon);
                });

            migrationBuilder.CreateTable(
                name: "Cupones_Detalle",
                columns: table => new
                {
                    Id_Cupon = table.Column<int>(type: "int", nullable: false),
                    Id_Articulo = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupones_Detalle", x => new { x.Id_Cupon, x.Id_Articulo });
                });

            migrationBuilder.CreateTable(
                name: "Cupones_Historial",
                columns: table => new
                {
                    Id_Cupon = table.Column<int>(type: "int", nullable: false),
                    NroCupon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaUso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodCliente = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupones_Historial", x => new { x.Id_Cupon, x.NroCupon });
                });

            migrationBuilder.CreateTable(
                name: "Precios",
                columns: table => new
                {
                    Id_Precio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Articulo = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Precios", x => x.Id_Precio);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Cupon",
                columns: table => new
                {
                    Id_Tipo_Cupon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Cupon", x => x.Id_Tipo_Cupon);
                });

            migrationBuilder.CreateTable(
                name: "Articulos",
                columns: table => new
                {
                    Id_Articulo = table.Column<int>(type: "int", nullable: false),
                    Nombre_Articulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion_Articulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articulos", x => x.Id_Articulo);
                    table.ForeignKey(
                        name: "FK_Articulos_Precios_Id_Articulo",
                        column: x => x.Id_Articulo,
                        principalTable: "Precios",
                        principalColumn: "Id_Precio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cupones",
                columns: table => new
                {
                    Id_Cupon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcentajeDto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImportePromo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_Tipo_Cupon = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupones", x => x.Id_Cupon);
                    table.ForeignKey(
                        name: "FK_Cupones_Tipo_Cupon_Id_Tipo_Cupon",
                        column: x => x.Id_Tipo_Cupon,
                        principalTable: "Tipo_Cupon",
                        principalColumn: "Id_Tipo_Cupon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cupones_Categorias",
                columns: table => new
                {
                    Id_Cupones_Categorias = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Cupon = table.Column<int>(type: "int", nullable: false),
                    Id_Categoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupones_Categorias", x => x.Id_Cupones_Categorias);
                    table.ForeignKey(
                        name: "FK_Cupones_Categorias_Categorias_Id_Categoria",
                        column: x => x.Id_Categoria,
                        principalTable: "Categorias",
                        principalColumn: "Id_Categoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cupones_Categorias_Cupones_Id_Cupon",
                        column: x => x.Id_Cupon,
                        principalTable: "Cupones",
                        principalColumn: "Id_Cupon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cupones_Id_Tipo_Cupon",
                table: "Cupones",
                column: "Id_Tipo_Cupon");

            migrationBuilder.CreateIndex(
                name: "IX_Cupones_Categorias_Id_Categoria",
                table: "Cupones_Categorias",
                column: "Id_Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Cupones_Categorias_Id_Cupon",
                table: "Cupones_Categorias",
                column: "Id_Cupon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articulos");

            migrationBuilder.DropTable(
                name: "Cupones_Categorias");

            migrationBuilder.DropTable(
                name: "Cupones_Clientes");

            migrationBuilder.DropTable(
                name: "Cupones_Detalle");

            migrationBuilder.DropTable(
                name: "Cupones_Historial");

            migrationBuilder.DropTable(
                name: "Precios");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Cupones");

            migrationBuilder.DropTable(
                name: "Tipo_Cupon");
        }
    }
}
