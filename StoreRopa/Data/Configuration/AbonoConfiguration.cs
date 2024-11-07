using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreRopa.Models;

namespace StoreRopa.Data.Configuration
{
    public class AbonoConfiguration : IEntityTypeConfiguration<Abonos>
    {
        public void Configure(EntityTypeBuilder<Abonos> builder)
        {
            builder.ToTable("Abonos");
            builder.HasKey(e => e.Id).HasName("PK_abono");
            builder.Property(e => e.Id).HasComment("Identificador de la tabla");
            builder.Property(e => e.CreditoId).HasColumnType("bigint").HasComment("Credito");
            builder.HasOne(e => e.Credito).WithMany(p => p.Abonos).HasForeignKey(p => p.CreditoId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Abonos_Creditos");
            builder.Property(e => e.Abono).HasColumnType("DECIMAL(14,2)").IsUnicode(false)
               .HasComment("Importe del abono registrado a la venta");
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
