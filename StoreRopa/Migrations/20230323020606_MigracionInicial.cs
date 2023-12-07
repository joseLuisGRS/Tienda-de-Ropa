using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreRopa.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
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
                    Nombres = table.Column<string>(type: "varchar(200)", unicode: false, nullable: false, comment: "Nombre de la persona"),
                    ApPaterno = table.Column<string>(type: "varchar(100)", unicode: false, nullable: false, comment: "Apellido paterno de la persona"),
                    ApMaterno = table.Column<string>(type: "varchar(100)", unicode: false, nullable: false, comment: "Apellido materno de la persona"),
                    Curp = table.Column<string>(type: "varchar(18)", unicode: false, nullable: false, comment: "Curp (unica) de la persona"),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime", unicode: false, nullable: false, comment: "fecha de nacimiento de la persona"),
                    Ciudad = table.Column<string>(type: "varchar(500)", unicode: false, nullable: false, comment: "Ciudad donde vive la persona"),
                    Pais = table.Column<string>(type: "varchar(500)", unicode: false, nullable: false, comment: "Pais donde vive la persona"),
                    Direccion = table.Column<string>(type: "varchar(1000)", unicode: false, nullable: false, comment: "Direccion especifica de la persona"),
                    Numero = table.Column<string>(type: "varchar(20)", unicode: false, nullable: false, comment: "Numero donde se situa su ubicación de la persona"),
                    Cp = table.Column<int>(type: "int", unicode: false, nullable: false, comment: "Codigo postal de su ubicación de la persona"),
                    Telefono = table.Column<string>(type: "varchar(10)", unicode: false, nullable: false, comment: "Numero telefonico de la persona"),
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
                },
                comment: "Registro de Personas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persona");
        }
    }
}
