using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using StoreRopa.Models;

namespace StoreRopa.Data.Configuration
{
    public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleados>
    {
        public void Configure(EntityTypeBuilder<Empleados> builder)
        {
            builder.ToTable("Empleado");
            builder.HasKey(e => e.Id).HasName("PK_empleado");
            builder.Property(e => e.Id).HasComment("Identificador de la tabla");
            builder.Property(e => e.PersonaId).HasColumnType("bigint").HasComment("Persona");
            builder.HasOne(e => e.Persona).WithOne(p => p.Empleado).HasForeignKey<Empleados>(p => p.PersonaId)
                .HasConstraintName("FK_Persona_Empleado");
            builder.HasOne(e => e.Rol).WithMany(p => p.Empleados).HasForeignKey(p => p.RolId)
                .HasConstraintName("FK_Rol_Empleado");
            builder.Property(e => e.Usuario).HasColumnType("varchar(50)").HasComment("Nombre del usuario");
            builder.Property(e => e.Password).HasColumnType("varchar(100)").HasComment("Clave de acceso");
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
