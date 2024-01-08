using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreRopa.Migrations
{
    /// <inheritdoc />
    public partial class InitMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador de la tabla")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false, comment: "Nombre de la persona"),
                    ApPaterno = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, comment: "Apellido paterno de la persona"),
                    ApMaterno = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, comment: "Apellido materno de la persona"),
                    Curp = table.Column<string>(type: "varchar(18)", unicode: false, maxLength: 18, nullable: false, comment: "Curp (unica) de la persona"),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime", unicode: false, nullable: false, comment: "fecha de nacimiento de la persona"),
                    Ciudad = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false, comment: "Ciudad donde vive la persona"),
                    Pais = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false, comment: "Pais donde vive la persona"),
                    Direccion = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false, comment: "Direccion especifica de la persona"),
                    Numero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, comment: "Numero donde se situa su ubicación de la persona"),
                    Cp = table.Column<int>(type: "int", unicode: false, nullable: false, comment: "Codigo postal de su ubicación de la persona"),
                    Telefono = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false, comment: "Numero telefonico de la persona"),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica si el registro se encuentra activo y se puede usar"),
                    EsEliminado = table.Column<bool>(type: "bit", nullable: false, comment: "Indica si el registro a sido eliminado(eliminado logico)"),
                    FechaAlta = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Fecha en que se crea el registro"),
                    UsuarioAlta = table.Column<string>(type: "varchar(200)", nullable: true, comment: "Usuario que crea el registro"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Ultima fecha que se actualiza el registro"),
                    UsuarioModificacion = table.Column<string>(type: "varchar(20)", nullable: true, comment: "Ultimo usuario que actualiza el registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persona", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador de la tabla")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<long>(type: "bigint", nullable: false, comment: "Persona"),
                    Saldo = table.Column<decimal>(type: "DECIMAL(14,2)", unicode: false, nullable: false, comment: "Saldo del cliente"),
                    TipoVenta = table.Column<int>(type: "int", unicode: false, nullable: false, comment: "Tipo de venta que se le hará al cliente"),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica si el registro se encuentra activo y se puede usar"),
                    EsEliminado = table.Column<bool>(type: "bit", nullable: false, comment: "Indica si el registro a sido eliminado(eliminado logico)"),
                    FechaAlta = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Fecha en que se crea el registro"),
                    UsuarioAlta = table.Column<string>(type: "varchar(200)", nullable: true, comment: "Usuario que crea el registro"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Ultima fecha que se actualiza el registro"),
                    UsuarioModificacion = table.Column<string>(type: "varchar(20)", nullable: true, comment: "Ultimo usuario que actualiza el registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persona_Cliente",
                        column: x => x.PersonaId,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_PersonaId",
                table: "Cliente",
                column: "PersonaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Persona");
        }
    }
}
