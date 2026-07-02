using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SisAlq.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddReciboConsumo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CONCEPTO_CONSUMOSERVICIO",
                columns: table => new
                {
                    IdConceptoConsumo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DescCorta = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TipoConcepto = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UnidadMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Importe = table.Column<decimal>(type: "numeric(10,2)", nullable: false, defaultValue: 0m),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Vigente = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONCEPTO_CONSUMOSERVICIO", x => x.IdConceptoConsumo);
                });

            migrationBuilder.CreateTable(
                name: "RINGRESO_CONSUMOCAB",
                columns: table => new
                {
                    IdNroRecibo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdInmueble = table.Column<int>(type: "integer", nullable: false),
                    IdInquilino = table.Column<int>(type: "integer", nullable: false),
                    IdMoneda = table.Column<int>(type: "integer", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    FechaVencimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GlosaConcepto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalRecibo = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    Usuario = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RINGRESO_CONSUMOCAB", x => x.IdNroRecibo);
                    table.ForeignKey(
                        name: "FK_RINGRESO_CONSUMOCAB_CLIENTE_IdInquilino",
                        column: x => x.IdInquilino,
                        principalTable: "CLIENTE",
                        principalColumn: "IdInquilino",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RINGRESO_CONSUMOCAB_INMUEBLES_IdInmueble",
                        column: x => x.IdInmueble,
                        principalTable: "INMUEBLES",
                        principalColumn: "IdInmueble",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RINGRESO_CONSUMOCAB_MONEDA_IdMoneda",
                        column: x => x.IdMoneda,
                        principalTable: "MONEDA",
                        principalColumn: "IdMoneda",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RINGRESO_CONSUMODET",
                columns: table => new
                {
                    IdNroRecibo = table.Column<int>(type: "integer", nullable: false),
                    Item = table.Column<int>(type: "integer", nullable: false),
                    IdConceptoConsumo = table.Column<int>(type: "integer", nullable: false),
                    LecturaInicial = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    FLecturaInicial = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LecturaFinal = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    FLecturaFinal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Importe = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RINGRESO_CONSUMODET", x => new { x.IdNroRecibo, x.Item });
                    table.ForeignKey(
                        name: "FK_RINGRESO_CONSUMODET_CONCEPTO_CONSUMOSERVICIO_IdConceptoCons~",
                        column: x => x.IdConceptoConsumo,
                        principalTable: "CONCEPTO_CONSUMOSERVICIO",
                        principalColumn: "IdConceptoConsumo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RINGRESO_CONSUMODET_RINGRESO_CONSUMOCAB_IdNroRecibo",
                        column: x => x.IdNroRecibo,
                        principalTable: "RINGRESO_CONSUMOCAB",
                        principalColumn: "IdNroRecibo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RINGRESO_CONSUMOCAB_IdInmueble_GlosaConcepto",
                table: "RINGRESO_CONSUMOCAB",
                columns: new[] { "IdInmueble", "GlosaConcepto" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RINGRESO_CONSUMOCAB_IdInquilino",
                table: "RINGRESO_CONSUMOCAB",
                column: "IdInquilino");

            migrationBuilder.CreateIndex(
                name: "IX_RINGRESO_CONSUMOCAB_IdMoneda",
                table: "RINGRESO_CONSUMOCAB",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_RINGRESO_CONSUMODET_IdConceptoConsumo",
                table: "RINGRESO_CONSUMODET",
                column: "IdConceptoConsumo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RINGRESO_CONSUMODET");

            migrationBuilder.DropTable(
                name: "CONCEPTO_CONSUMOSERVICIO");

            migrationBuilder.DropTable(
                name: "RINGRESO_CONSUMOCAB");
        }
    }
}
