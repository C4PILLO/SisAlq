using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAlq.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddContratoActivar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ModalidadPago",
                table: "CONTRATO_CAB",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Adelantado",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "MesesGarantia",
                table: "CONTRATO_CAB",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CuotasPendientes",
                table: "CONTRATO_CAB",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "UrlDocumento",
                table: "CONTRATO_CAB",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlDocumento",
                table: "CONTRATO_CAB");

            migrationBuilder.AlterColumn<string>(
                name: "ModalidadPago",
                table: "CONTRATO_CAB",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Adelantado");

            migrationBuilder.AlterColumn<int>(
                name: "MesesGarantia",
                table: "CONTRATO_CAB",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CuotasPendientes",
                table: "CONTRATO_CAB",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);
        }
    }
}
