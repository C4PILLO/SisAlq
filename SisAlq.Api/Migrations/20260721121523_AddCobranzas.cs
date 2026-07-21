using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SisAlq.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCobranzas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COBRANZA_CAB",
                columns: table => new
                {
                    IdCobranza = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaCobro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    IdInquilino = table.Column<int>(type: "integer", nullable: false),
                    IdMoneda = table.Column<int>(type: "integer", nullable: false),
                    TotalCobrado = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Mora = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    Observacion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Usuario = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Estado = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false, defaultValue: "A")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COBRANZA_CAB", x => x.IdCobranza);
                    table.ForeignKey(
                        name: "FK_COBRANZA_CAB_CLIENTE_IdInquilino",
                        column: x => x.IdInquilino,
                        principalTable: "CLIENTE",
                        principalColumn: "IdInquilino",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COBRANZA_CAB_MONEDA_IdMoneda",
                        column: x => x.IdMoneda,
                        principalTable: "MONEDA",
                        principalColumn: "IdMoneda",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COBRANZA_DET",
                columns: table => new
                {
                    IdCobranza = table.Column<int>(type: "integer", nullable: false),
                    Secuencia = table.Column<int>(type: "integer", nullable: false),
                    CodigoTD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    NumeroDoc = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    CodigoMPago = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CodigoBanco = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    AliasBanco = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    NroOperacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Importe = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Mora = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    Descuento = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    TotalPagado = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    EstadoPago = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COBRANZA_DET", x => new { x.IdCobranza, x.Secuencia });
                    table.ForeignKey(
                        name: "FK_COBRANZA_DET_BANCOS_CodigoBanco",
                        column: x => x.CodigoBanco,
                        principalTable: "BANCOS",
                        principalColumn: "CodigoBanco");
                    table.ForeignKey(
                        name: "FK_COBRANZA_DET_COBRANZA_CAB_IdCobranza",
                        column: x => x.IdCobranza,
                        principalTable: "COBRANZA_CAB",
                        principalColumn: "IdCobranza",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COBRANZA_DET_MEDIO_PAGO_CodigoMPago",
                        column: x => x.CodigoMPago,
                        principalTable: "MEDIO_PAGO",
                        principalColumn: "CodigoMedioPago",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COBRANZA_DET_TIPO_COMPROBANTE_CodigoTD",
                        column: x => x.CodigoTD,
                        principalTable: "TIPO_COMPROBANTE",
                        principalColumn: "CodigoTD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COBRANZA_CAB_IdInquilino",
                table: "COBRANZA_CAB",
                column: "IdInquilino");

            migrationBuilder.CreateIndex(
                name: "IX_COBRANZA_CAB_IdMoneda",
                table: "COBRANZA_CAB",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_COBRANZA_DET_CodigoBanco",
                table: "COBRANZA_DET",
                column: "CodigoBanco");

            migrationBuilder.CreateIndex(
                name: "IX_COBRANZA_DET_CodigoMPago",
                table: "COBRANZA_DET",
                column: "CodigoMPago");

            migrationBuilder.CreateIndex(
                name: "IX_COBRANZA_DET_CodigoTD",
                table: "COBRANZA_DET",
                column: "CodigoTD");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COBRANZA_DET");

            migrationBuilder.DropTable(
                name: "COBRANZA_CAB");
        }
    }
}
