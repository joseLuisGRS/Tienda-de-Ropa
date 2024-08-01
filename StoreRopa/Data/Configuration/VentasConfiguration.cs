using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreRopa.Models;

namespace StoreRopa.Data.Configuration
{
    public class VentasConfiguration : IEntityTypeConfiguration<Ventas>
    {
        public void Configure(EntityTypeBuilder<Ventas> builder)
        {
            builder.ToTable("Ventas");
            builder.HasKey(e => e.Id).HasName("PK_venta");
            builder.Property(e => e.Id).HasComment("Identificador de la tabla");
            builder.Property(e => e.ClienteId).HasColumnType("bigint").HasComment("Cliente");
            builder.HasOne(e => e.Cliente).WithMany(p => p.Ventas).HasForeignKey(p => p.ClienteId) 
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Venta_Cliente");
            builder.Property(e => e.EmpleadoId).HasColumnType("bigint").HasComment("Empleado");
            builder.HasOne(e => e.Empleado).WithMany(p => p.Ventas).HasForeignKey(p => p.EmpleadoId)
                .HasConstraintName("FK_Venta_Empleado");
            builder.Property(e => e.ImporteVenta).HasColumnType("DECIMAL(14,2)").IsUnicode(false)
               .HasComment("Importe total de la venta");
            builder.Property(e => e.AbonoVenta).HasColumnType("DECIMAL(14,2)").IsUnicode(false)
               .HasComment("Dinero abonado a la venta");
            builder.Property(e => e.PendientePago).HasColumnType("DECIMAL(14,2)").IsUnicode(false)
               .HasComment("Saldo pendiente por pagar");
            builder.Property(columna => columna.EsVentaCredito).HasDefaultValueSql("0")
                .HasComment("Indica si la venta es a crédito");
            builder.Property(columna => columna.EsActivo).HasDefaultValueSql("1")
                .HasComment("Indica si el registro se encuentra activo y se puede usar");
            builder.Property(columna => columna.EsEliminado).HasDefaultValueSql("0")
                .HasComment("Indica si el registro a sido eliminado(eliminado logico)");
            builder.Property(e => e.FechaAlta).HasColumnType("datetime").HasDefaultValueSql("GETDATE()")
                .HasComment("Fecha en que se crea el registro");
            builder.Property(columna => columna.UsuarioAlta).HasColumnType("varchar(50)")
                .HasComment("Usuario que crea el registro");
            builder.Property(columna => columna.FechaModificacion).HasColumnType("datetime")
                .HasComment("Ultima fecha que se actualiza el registro");
            builder.Property(columna => columna.UsuarioModificacion).HasColumnType("varchar(50)")
                .HasComment("Ultimo usuario que actualiza el registro");
        }
    }
}
