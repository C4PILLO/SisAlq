using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisAlq.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogosCobranza : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BANCOS",
                columns: table => new
                {
                    CodigoBanco = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false, defaultValue: "A"),
                    Alias = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANCOS", x => x.CodigoBanco);
                });

            migrationBuilder.CreateTable(
                name: "MEDIO_PAGO",
                columns: table => new
                {
                    CodigoMedioPago = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDIO_PAGO", x => x.CodigoMedioPago);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_COMPROBANTE",
                columns: table => new
                {
                    CodigoTD = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Correlativo = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_COMPROBANTE", x => x.CodigoTD);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BANCOS");

            migrationBuilder.DropTable(
                name: "MEDIO_PAGO");

            migrationBuilder.DropTable(
                name: "TIPO_COMPROBANTE");
        }
    }
}
