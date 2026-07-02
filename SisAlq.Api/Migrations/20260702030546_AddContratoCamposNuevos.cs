using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAlq.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddContratoCamposNuevos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CuotasPendientes",
                table: "CONTRATO_CAB",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MesesGarantia",
                table: "CONTRATO_CAB",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ModalidadPago",
                table: "CONTRATO_CAB",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuotasPendientes",
                table: "CONTRATO_CAB");

            migrationBuilder.DropColumn(
                name: "MesesGarantia",
                table: "CONTRATO_CAB");

            migrationBuilder.DropColumn(
                name: "ModalidadPago",
                table: "CONTRATO_CAB");
        }
    }
}
