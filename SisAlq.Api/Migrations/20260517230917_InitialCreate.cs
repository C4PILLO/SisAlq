using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SisAlq.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESTADO_INMUEBLE",
                columns: table => new
                {
                    IdEstadoInmueble = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTADO_INMUEBLE", x => x.IdEstadoInmueble);
                });

            migrationBuilder.CreateTable(
                name: "MONEDA",
                columns: table => new
                {
                    IdMoneda = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MONEDA", x => x.IdMoneda);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "SECTOR_ZONA",
                columns: table => new
                {
                    IdSector = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SECTOR_ZONA", x => x.IdSector);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_CLIENTE",
                columns: table => new
                {
                    IdTipoCliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_CLIENTE", x => x.IdTipoCliente);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_DOCUMENTO",
                columns: table => new
                {
                    IdTDocumento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_DOCUMENTO", x => x.IdTDocumento);
                });

            migrationBuilder.CreateTable(
                name: "TIPO_INMUEBLE",
                columns: table => new
                {
                    IdTipoInmueble = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_INMUEBLE", x => x.IdTipoInmueble);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreUsuario = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NombreApellidos = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    IdRol = table.Column<int>(type: "integer", nullable: false),
                    Correo = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    CelularTelefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_USUARIO_ROLES_IdRol",
                        column: x => x.IdRol,
                        principalTable: "ROLES",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CLIENTE",
                columns: table => new
                {
                    IdInquilino = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTipoCliente = table.Column<int>(type: "integer", nullable: false),
                    IdTDocumento = table.Column<int>(type: "integer", nullable: false),
                    NroDocumento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RsocialNApellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CelularTelefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    Referencia = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Vigente = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTE", x => x.IdInquilino);
                    table.ForeignKey(
                        name: "FK_CLIENTE_TIPO_CLIENTE_IdTipoCliente",
                        column: x => x.IdTipoCliente,
                        principalTable: "TIPO_CLIENTE",
                        principalColumn: "IdTipoCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CLIENTE_TIPO_DOCUMENTO_IdTDocumento",
                        column: x => x.IdTDocumento,
                        principalTable: "TIPO_DOCUMENTO",
                        principalColumn: "IdTDocumento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INMUEBLES",
                columns: table => new
                {
                    IdInmueble = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTipoInmueble = table.Column<int>(type: "integer", nullable: false),
                    IdSector = table.Column<int>(type: "integer", nullable: false),
                    IdEstadoInmueble = table.Column<int>(type: "integer", nullable: false),
                    IdMoneda = table.Column<int>(type: "integer", nullable: false),
                    CodigoInmueble = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DescripcionInmueble = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PisoInmueble = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PrecioAlquiler = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    IncluyeServicios = table.Column<char>(type: "character(1)", maxLength: 1, nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INMUEBLES", x => x.IdInmueble);
                    table.ForeignKey(
                        name: "FK_INMUEBLES_ESTADO_INMUEBLE_IdEstadoInmueble",
                        column: x => x.IdEstadoInmueble,
                        principalTable: "ESTADO_INMUEBLE",
                        principalColumn: "IdEstadoInmueble",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INMUEBLES_MONEDA_IdMoneda",
                        column: x => x.IdMoneda,
                        principalTable: "MONEDA",
                        principalColumn: "IdMoneda",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INMUEBLES_SECTOR_ZONA_IdSector",
                        column: x => x.IdSector,
                        principalTable: "SECTOR_ZONA",
                        principalColumn: "IdSector",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INMUEBLES_TIPO_INMUEBLE_IdTipoInmueble",
                        column: x => x.IdTipoInmueble,
                        principalTable: "TIPO_INMUEBLE",
                        principalColumn: "IdTipoInmueble",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTE_IdTDocumento",
                table: "CLIENTE",
                column: "IdTDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTE_IdTipoCliente",
                table: "CLIENTE",
                column: "IdTipoCliente");

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTE_NroDocumento",
                table: "CLIENTE",
                column: "NroDocumento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_INMUEBLES_CodigoInmueble",
                table: "INMUEBLES",
                column: "CodigoInmueble",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_INMUEBLES_IdEstadoInmueble",
                table: "INMUEBLES",
                column: "IdEstadoInmueble");

            migrationBuilder.CreateIndex(
                name: "IX_INMUEBLES_IdMoneda",
                table: "INMUEBLES",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_INMUEBLES_IdSector",
                table: "INMUEBLES",
                column: "IdSector");

            migrationBuilder.CreateIndex(
                name: "IX_INMUEBLES_IdTipoInmueble",
                table: "INMUEBLES",
                column: "IdTipoInmueble");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_IdRol",
                table: "USUARIO",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLIENTE");

            migrationBuilder.DropTable(
                name: "INMUEBLES");

            migrationBuilder.DropTable(
                name: "USUARIO");

            migrationBuilder.DropTable(
                name: "TIPO_CLIENTE");

            migrationBuilder.DropTable(
                name: "TIPO_DOCUMENTO");

            migrationBuilder.DropTable(
                name: "ESTADO_INMUEBLE");

            migrationBuilder.DropTable(
                name: "MONEDA");

            migrationBuilder.DropTable(
                name: "SECTOR_ZONA");

            migrationBuilder.DropTable(
                name: "TIPO_INMUEBLE");

            migrationBuilder.DropTable(
                name: "ROLES");
        }
    }
}
