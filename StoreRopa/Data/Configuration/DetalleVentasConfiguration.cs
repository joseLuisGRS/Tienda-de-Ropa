using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreRopa.Models;

namespace StoreRopa.Data.Configuration
{
    public class DetalleVentasConfiguration : IEntityTypeConfiguration<DetalleVentas>
    {
        public void Configure(EntityTypeBuilder<DetalleVentas> builder)
        {
            builder.ToTable("DetalleVentas");
            builder.HasKey(e => e.Id).HasName("PK_detalleVenta");
            builder.Property(e => e.Id).HasComment("Identificador de la tabla");
            builder.Property(e => e.VentaId).HasColumnType("bigint").HasComment("Venta");
            builder.HasOne(e => e.Venta).WithMany(p => p.DetalleVentas).HasForeignKey(p => p.VentaId)
                .HasConstraintName("FK_Venta_Detalle");
            builder.Property(e => e.Descripcion).HasColumnType("varchar(1000)").HasComment("Descripción del articulo");
            builder.Property(e => e.Talla).HasColumnType("varchar(50)").HasComment("Talla del articulo");
            builder.Property(e => e.Color).HasColumnType("varchar(100)").HasComment("Color del articulo");
            builder.Property(e => e.Modelo).HasColumnType("varchar(50)").HasComment("Modelo del articulo");
            builder.Property(e => e.PrecioArticulo).HasColumnType("DECIMAL(14,2)").IsUnicode(false)
               .HasComment("Precio del articulo");
            builder.Property(e => e.Descuento).HasColumnType("DECIMAL(14,2)").IsUnicode(false)
               .HasComment("Descuento que se aplica al articulo");
            builder.Property(e => e.PrecioVenta).HasColumnType("DECIMAL(14,2)").IsUnicode(false)
               .HasComment("Precio real de venta");
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
