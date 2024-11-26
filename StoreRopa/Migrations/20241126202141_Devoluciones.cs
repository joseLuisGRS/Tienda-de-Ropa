using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreRopa.Migrations
{
    /// <inheritdoc />
    public partial class Devoluciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVentas_Devolucion",
                table: "Devoluciones");

            migrationBuilder.DropIndex(
                name: "IX_Devoluciones_ClienteId",
                table: "Devoluciones");

            migrationBuilder.CreateIndex(
                name: "IX_Devoluciones_ClienteId",
                table: "Devoluciones",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Devoluciones_DetalleVentaId",
                table: "Devoluciones",
                column: "DetalleVentaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentas_Devolucion",
                table: "Devoluciones",
                column: "DetalleVentaId",
                principalTable: "DetalleVentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVentas_Devolucion",
                table: "Devoluciones");

            migrationBuilder.DropIndex(
                name: "IX_Devoluciones_ClienteId",
                table: "Devoluciones");

            migrationBuilder.DropIndex(
                name: "IX_Devoluciones_DetalleVentaId",
                table: "Devoluciones");

            migrationBuilder.CreateIndex(
                name: "IX_Devoluciones_ClienteId",
                table: "Devoluciones",
                column: "ClienteId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentas_Devolucion",
                table: "Devoluciones",
                column: "ClienteId",
                principalTable: "DetalleVentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
