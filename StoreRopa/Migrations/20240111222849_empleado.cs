using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreRopa.Migrations
{
    /// <inheritdoc />
    public partial class empleado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador de la tabla")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<long>(type: "bigint", nullable: false, comment: "Persona"),
                    RolId = table.Column<long>(type: "bigint", nullable: false),
                    Usuario = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Nombre del usuario"),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 1000, nullable: false, comment: "Clave de acceso"),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1", comment: "Indica si el registro se encuentra activo y se puede usar"),
                    EsEliminado = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0", comment: "Indica si el registro a sido eliminado(eliminado logico)"),
                    FechaAlta = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()", comment: "Fecha en que se crea el registro"),
                    UsuarioAlta = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Usuario que crea el registro"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Ultima fecha que se actualiza el registro"),
                    UsuarioModificacion = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Ultimo usuario que actualiza el registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empleado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persona_Empleado",
                        column: x => x.PersonaId,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rol_Empleado",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_PersonaId",
                table: "Empleado",
                column: "PersonaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_RolId",
                table: "Empleado",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empleado");
        }
    }
}
