using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SisAlq.Api.Migrations
{
    /// <inheritdoc />
    public partial class SprintDos_Contratos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESTADO_CONTRATO",
                columns: table => new
                {
                    IdEstadoContrato = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTADO_CONTRATO", x => x.IdEstadoContrato);
                });

            migrationBuilder.CreateTable(
                name: "CONTRATO_CAB",
                columns: table => new
                {
                    IdContrato = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NroContrato = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FechaContrato = table.Column<DateOnly>(type: "date", nullable: false),
                    IdInquilino = table.Column<int>(type: "integer", nullable: false),
                    Representante = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    TipoNegocio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaVcmto = table.Column<DateOnly>(type: "date", nullable: false),
                    IdEstadoContrato = table.Column<int>(type: "integer", nullable: false),
                    NroMeses = table.Column<int>(type: "integer", nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdMoneda = table.Column<int>(type: "integer", nullable: false),
                    Garantia = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRATO_CAB", x => x.IdContrato);
                    table.ForeignKey(
                        name: "FK_CONTRATO_CAB_CLIENTE_IdInquilino",
                        column: x => x.IdInquilino,
                        principalTable: "CLIENTE",
                        principalColumn: "IdInquilino",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CONTRATO_CAB_ESTADO_CONTRATO_IdEstadoContrato",
                        column: x => x.IdEstadoContrato,
                        principalTable: "ESTADO_CONTRATO",
                        principalColumn: "IdEstadoContrato",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CONTRATO_CAB_MONEDA_IdMoneda",
                        column: x => x.IdMoneda,
                        principalTable: "MONEDA",
                        principalColumn: "IdMoneda",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CONTRATO_CAB_USUARIO_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIO",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CONTRATO_DET",
                columns: table => new
                {
                    IdContratoDet = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdContrato = table.Column<int>(type: "integer", nullable: false),
                    IdInmueble = table.Column<int>(type: "integer", nullable: false),
                    RentaMensual = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    NroMeses = table.Column<int>(type: "integer", nullable: false),
                    NroMesPPago = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRATO_DET", x => x.IdContratoDet);
                    table.ForeignKey(
                        name: "FK_CONTRATO_DET_CONTRATO_CAB_IdContrato",
                        column: x => x.IdContrato,
                        principalTable: "CONTRATO_CAB",
                        principalColumn: "IdContrato",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CONTRATO_DET_INMUEBLES_IdInmueble",
                        column: x => x.IdInmueble,
                        principalTable: "INMUEBLES",
                        principalColumn: "IdInmueble",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CONTRATO_CAB_IdEstadoContrato",
                table: "CONTRATO_CAB",
                column: "IdEstadoContrato");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRATO_CAB_IdInquilino",
                table: "CONTRATO_CAB",
                column: "IdInquilino");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRATO_CAB_IdMoneda",
                table: "CONTRATO_CAB",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRATO_CAB_IdUsuario",
                table: "CONTRATO_CAB",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRATO_CAB_NroContrato",
                table: "CONTRATO_CAB",
                column: "NroContrato",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CONTRATO_DET_IdContrato",
                table: "CONTRATO_DET",
                column: "IdContrato");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRATO_DET_IdInmueble",
                table: "CONTRATO_DET",
                column: "IdInmueble");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CONTRATO_DET");

            migrationBuilder.DropTable(
                name: "CONTRATO_CAB");

            migrationBuilder.DropTable(
                name: "ESTADO_CONTRATO");
        }
    }
}
