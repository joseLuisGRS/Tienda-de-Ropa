using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreRopa.Migrations
{
    /// <inheritdoc />
    public partial class Abonos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador de la tabla")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditoId = table.Column<long>(type: "bigint", nullable: false, comment: "Credito"),
                    Abono = table.Column<decimal>(type: "DECIMAL(14,2)", unicode: false, nullable: false, comment: "Importe del abono registrado a la venta"),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1", comment: "Indica si el registro se encuentra activo y se puede usar"),
                    EsEliminado = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0", comment: "Indica si el registro a sido eliminado(eliminado logico)"),
                    FechaAlta = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()", comment: "Fecha en que se crea el registro"),
                    UsuarioAlta = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Usuario que crea el registro"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Ultima fecha que se actualiza el registro"),
                    UsuarioModificacion = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Ultimo usuario que actualiza el registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abono", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Abonos_Creditos",
                        column: x => x.CreditoId,
                        principalTable: "Creditos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abonos_CreditoId",
                table: "Abonos",
                column: "CreditoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abonos");
        }
    }
}
