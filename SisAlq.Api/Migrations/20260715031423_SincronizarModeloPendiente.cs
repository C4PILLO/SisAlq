using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAlq.Api.Migrations
{
    /// <inheritdoc />
    public partial class SincronizarModeloPendiente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RINGRESO_CONSUMOCAB_IdInmueble_GlosaConcepto",
                table: "RINGRESO_CONSUMOCAB");

            migrationBuilder.AlterColumn<string>(
                name: "TipoRecibo",
                table: "RINGRESO_CONSUMOCAB",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "CONSUMO",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_RINGRESO_CONSUMOCAB_IdInmueble_GlosaConcepto_TipoRecibo",
                table: "RINGRESO_CONSUMOCAB",
                columns: new[] { "IdInmueble", "GlosaConcepto", "TipoRecibo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RINGRESO_CONSUMOCAB_IdInmueble_GlosaConcepto_TipoRecibo",
                table: "RINGRESO_CONSUMOCAB");

            migrationBuilder.AlterColumn<string>(
                name: "TipoRecibo",
                table: "RINGRESO_CONSUMOCAB",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "CONSUMO");

            migrationBuilder.CreateIndex(
                name: "IX_RINGRESO_CONSUMOCAB_IdInmueble_GlosaConcepto",
                table: "RINGRESO_CONSUMOCAB",
                columns: new[] { "IdInmueble", "GlosaConcepto" },
                unique: true);
        }
    }
}
