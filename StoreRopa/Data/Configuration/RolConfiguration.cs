using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreRopa.Models;

namespace StoreRopa.Data.Configuration
{
    public class RolConfiguration: IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(e => e.Id).HasName("PK_roles");
            builder.Property(e => e.Id).HasComment("Identificador de la tabla");
            builder.Property(e => e.Nombre).HasColumnType("varchar(50)").HasComment("Nombre del rol");
            builder.Property(e => e.Descripcion).HasColumnType("varchar(100)")
                .HasComment("Descripción de las funciones del rol");
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
