using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreRopa.Models;

namespace StoreRopa.Data.Configuration
{
    public class CreditosConfiguration : IEntityTypeConfiguration<Creditos>
    {
        public void Configure(EntityTypeBuilder<Creditos> builder)
        {
            builder.ToTable("Creditos");
            builder.HasKey(e => e.Id).HasName("PK_Creditos");
            builder.Property(e => e.Id).HasComment("Identificador de la tabla");
            builder.Property(e => e.DetalleVentaId).HasColumnType("bigint").HasComment("Detalle de la Venta");
            builder.HasOne(e => e.DetalleVenta).WithMany(p => p.Creditos).HasForeignKey(p => p.DetalleVentaId)
                .HasConstraintName("FK_Credito_Venta");
            builder.Property(e => e.PrecioArticulo).HasColumnType("DECIMAL(14,2)").IsUnicode(false)
               .HasComment("Precio del articulo");
            builder.Property(e => e.PagoPendiente).HasColumnType("DECIMAL(14,2)").IsUnicode(false)
               .HasComment("Saldo pendiente de pago del articulo");
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
