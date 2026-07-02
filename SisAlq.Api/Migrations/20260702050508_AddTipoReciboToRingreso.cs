using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAlq.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoReciboToRingreso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoRecibo",
                table: "RINGRESO_CONSUMOCAB",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoRecibo",
                table: "RINGRESO_CONSUMOCAB");
        }
    }
}
