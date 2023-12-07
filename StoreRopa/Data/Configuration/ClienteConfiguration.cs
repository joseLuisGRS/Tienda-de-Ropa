using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreRopa.Models;

namespace StoreRopa.Data.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");

            builder.HasKey(e => e.Id)
                 .HasName("PK_cliente");

            builder.Property(e => e.Id).HasComment("Identificador de la tabla");

            builder.Property(e => e.PersonaId)
                .HasColumnType("bigint")
                .HasComment("Persona");

            builder.HasOne(e => e.Persona)
           .WithOne(p => p.Cliente)
           .HasForeignKey<Cliente>(p => p.PersonaId)
           .HasConstraintName("FK_Persona_Cliente");

            builder.Property(e => e.Saldo)
                .HasColumnType("DECIMAL(14,2)")
                .IsUnicode(false)
                .HasComment("Saldo del cliente");

            builder.Property(e => e.TipoVenta)
                .HasColumnType("int")
                .IsUnicode(false)
                .HasComment("Tipo de venta que se le hará al cliente");

            builder.Property(columna => columna.EsActivo)
                .HasComment("Indica si el registro se encuentra activo y se puede usar");

            builder.Property(columna => columna.EsEliminado)
                .HasComment("Indica si el registro a sido eliminado(eliminado logico)");

            builder.Property(e => e.EsEliminado).HasComment("Indica si el registro a sido eliminado(eliminado logico)");

        }
    }
}
