using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreRopa.Models;

namespace StoreRopa.Data.Configuration
{
    public class DevolucionesConfiguration : IEntityTypeConfiguration<Devoluciones>
    {
        public void Configure(EntityTypeBuilder<Devoluciones> builder)
        {
            builder.ToTable("Devoluciones");
            builder.HasKey(e => e.Id).HasName("PK_devolucion");
            builder.Property(e => e.Id).HasComment("Identificador de la tabla");
            builder.Property(e => e.DetalleVentaId).HasColumnType("bigint").HasComment("DetalleVenta");
            builder.HasOne(e => e.DetalleVentas).WithOne(p => p.Devolucion).HasForeignKey<Devoluciones>(p => p.DetalleVentaId)
                .HasConstraintName("FK_DetalleVentas_Devolucion");
            builder.HasOne(e => e.Cliente).WithMany(p => p.Devoluciones).HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Cliente_Devoluciones");
            builder.Property(e => e.Importe).HasColumnType("DECIMAL(14,2)").IsUnicode(false)
                .HasComment("Importe recibido del articulo");
            builder.Property(columna => columna.EsActivo)
                .HasComment("Indica si el registro se encuentra activo y se puede usar");
            builder.Property(columna => columna.EsEliminado)
                .HasComment("Indica si el registro a sido eliminado(eliminado logico)");
            builder.Property(e => e.EsEliminado)
                .HasComment("Indica si el registro a sido eliminado(eliminado logico)");
            builder.Property(e => e.FechaAlta).HasColumnType("datetime")
                .HasComment("Fecha en que se crea el registro");
            builder.Property(columna => columna.UsuarioAlta).HasColumnType("varchar(200)")
                .HasComment("Usuario que crea el registro");
            builder.Property(columna => columna.FechaModificacion).HasColumnType("datetime")
                .HasComment("Ultima fecha que se actualiza el registro");
            builder.Property(columna => columna.UsuarioModificacion).HasColumnType("varchar(20)")
                .HasComment("Ultimo usuario que actualiza el registro");
        }
    }
}
