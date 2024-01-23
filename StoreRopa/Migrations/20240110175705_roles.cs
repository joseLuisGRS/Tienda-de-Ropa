using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreRopa.Migrations
{
    /// <inheritdoc />
    public partial class roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador de la tabla")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Nombre del rol"),
                    Descripcion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Descripción de las funciones del rol"),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica si el registro se encuentra activo y se puede usar"),
                    EsEliminado = table.Column<bool>(type: "bit", nullable: false, comment: "Indica si el registro a sido eliminado(eliminado logico)"),
                    FechaAlta = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Fecha en que se crea el registro"),
                    UsuarioAlta = table.Column<string>(type: "varchar(200)", nullable: true, comment: "Usuario que crea el registro"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Ultima fecha que se actualiza el registro"),
                    UsuarioModificacion = table.Column<string>(type: "varchar(20)", nullable: true, comment: "Ultimo usuario que actualiza el registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
