using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAlq.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentosXCobrar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DOCUMENTOS_X_COBRAR",
                columns: table => new
                {
                    IdInquilino = table.Column<int>(type: "integer", nullable: false),
                    CodigoTD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    NroDocumento = table.Column<int>(type: "integer", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaVcmto = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdMoneda = table.Column<int>(type: "integer", nullable: false),
                    Importe = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Saldo = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Usuario = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENTOS_X_COBRAR", x => new { x.IdInquilino, x.CodigoTD, x.NroDocumento });
                    table.ForeignKey(
                        name: "FK_DOCUMENTOS_X_COBRAR_CLIENTE_IdInquilino",
                        column: x => x.IdInquilino,
                        principalTable: "CLIENTE",
                        principalColumn: "IdInquilino",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DOCUMENTOS_X_COBRAR_MONEDA_IdMoneda",
                        column: x => x.IdMoneda,
                        principalTable: "MONEDA",
                        principalColumn: "IdMoneda",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DOCUMENTOS_X_COBRAR_TIPO_COMPROBANTE_CodigoTD",
                        column: x => x.CodigoTD,
                        principalTable: "TIPO_COMPROBANTE",
                        principalColumn: "CodigoTD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENTOS_X_COBRAR_CodigoTD",
                table: "DOCUMENTOS_X_COBRAR",
                column: "CodigoTD");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENTOS_X_COBRAR_IdMoneda",
                table: "DOCUMENTOS_X_COBRAR",
                column: "IdMoneda");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DOCUMENTOS_X_COBRAR");
        }
    }
}
