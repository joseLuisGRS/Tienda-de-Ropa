using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreRopa.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
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
                    Municipio = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false, comment: "Municipio donde vive la persona"),
                    Estado = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false, comment: "Estado donde vive la persona"),
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

            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador de la tabla")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonaId = table.Column<long>(type: "bigint", nullable: false, comment: "Persona"),
                    RolId = table.Column<long>(type: "bigint", nullable: false),
                    Usuario = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Nombre del usuario"),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Clave de acceso"),
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

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador de la tabla")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false, comment: "Cliente"),
                    EmpleadoId = table.Column<long>(type: "bigint", nullable: false, comment: "Empleado"),
                    ImporteVenta = table.Column<decimal>(type: "DECIMAL(14,2)", unicode: false, nullable: false, comment: "Importe total de la venta"),
                    AbonoVenta = table.Column<decimal>(type: "DECIMAL(14,2)", unicode: false, nullable: false, comment: "Dinero abonado a la venta"),
                    PendientePago = table.Column<decimal>(type: "DECIMAL(14,2)", unicode: false, nullable: false, comment: "Saldo pendiente por pagar"),
                    EsVentaCredito = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0", comment: "Indica si la venta es a crédito"),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1", comment: "Indica si el registro se encuentra activo y se puede usar"),
                    EsEliminado = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0", comment: "Indica si el registro a sido eliminado(eliminado logico)"),
                    FechaAlta = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()", comment: "Fecha en que se crea el registro"),
                    UsuarioAlta = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Usuario que crea el registro"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Ultima fecha que se actualiza el registro"),
                    UsuarioModificacion = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Ultimo usuario que actualiza el registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Venta_Cliente",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Venta_Empleado",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleVentas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador de la tabla")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VentaId = table.Column<long>(type: "bigint", nullable: false, comment: "Venta"),
                    Descripcion = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false, comment: "Descripción del articulo"),
                    Talla = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Talla del articulo"),
                    Color = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Color del articulo"),
                    Modelo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Modelo del articulo"),
                    PrecioArticulo = table.Column<decimal>(type: "DECIMAL(14,2)", unicode: false, nullable: false, comment: "Precio del articulo"),
                    Descuento = table.Column<decimal>(type: "DECIMAL(14,2)", unicode: false, nullable: false, comment: "Descuento que se aplica al articulo"),
                    PrecioVenta = table.Column<decimal>(type: "DECIMAL(14,2)", unicode: false, nullable: false, comment: "Precio real de venta"),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1", comment: "Indica si el registro se encuentra activo y se puede usar"),
                    EsEliminado = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0", comment: "Indica si el registro a sido eliminado(eliminado logico)"),
                    FechaAlta = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()", comment: "Fecha en que se crea el registro"),
                    UsuarioAlta = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Usuario que crea el registro"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Ultima fecha que se actualiza el registro"),
                    UsuarioModificacion = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Ultimo usuario que actualiza el registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalleVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Venta_Detalle",
                        column: x => x.VentaId,
                        principalTable: "Ventas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Creditos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador de la tabla")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetalleVentaId = table.Column<long>(type: "bigint", nullable: false, comment: "Detalle de la Venta"),
                    PrecioArticulo = table.Column<decimal>(type: "DECIMAL(14,2)", unicode: false, nullable: false, comment: "Precio del articulo"),
                    PagoPendiente = table.Column<decimal>(type: "DECIMAL(14,2)", unicode: false, nullable: false, comment: "Saldo pendiente de pago del articulo"),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1", comment: "Indica si el registro se encuentra activo y se puede usar"),
                    EsEliminado = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0", comment: "Indica si el registro a sido eliminado(eliminado logico)"),
                    FechaAlta = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()", comment: "Fecha en que se crea el registro"),
                    UsuarioAlta = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Usuario que crea el registro"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Ultima fecha que se actualiza el registro"),
                    UsuarioModificacion = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Ultimo usuario que actualiza el registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creditos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credito_Venta",
                        column: x => x.DetalleVentaId,
                        principalTable: "DetalleVentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_PersonaId",
                table: "Cliente",
                column: "PersonaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Creditos_DetalleVentaId",
                table: "Creditos",
                column: "DetalleVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVentas_VentaId",
                table: "DetalleVentas",
                column: "VentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_PersonaId",
                table: "Empleado",
                column: "PersonaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_RolId",
                table: "Empleado",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_ClienteId",
                table: "Ventas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_EmpleadoId",
                table: "Ventas",
                column: "EmpleadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Creditos");

            migrationBuilder.DropTable(
                name: "DetalleVentas");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Empleado");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
