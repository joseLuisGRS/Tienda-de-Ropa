using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreRopa.Models;

namespace StoreRopa.Data.Configuration
{
    public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Persona");

            builder.HasKey(e => e.Id)
                 .HasName("PK_persona");

            builder.Property(e => e.Id).HasComment("Identificador de la tabla");

            builder.Property(e => e.Nombres)
                .HasColumnType("varchar(200)")
                .IsUnicode(false)
                .HasComment("Nombre de la persona");

            builder.Property(e => e.ApPaterno)
                .HasColumnType("varchar(100)")
                .IsUnicode(false)
                .HasComment("Apellido paterno de la persona");

            builder.Property(e => e.ApMaterno)
                .HasColumnType("varchar(100)")
                .IsUnicode(false)
                .HasComment("Apellido materno de la persona");

            builder.Property(e => e.Curp)
                .HasColumnType("varchar(18)")
                .IsUnicode(false)
                .HasComment("Curp (unica) de la persona");

            builder.Property(e => e.FechaNacimiento)
               .HasColumnType("datetime")
               .IsUnicode(false)
               .HasComment("fecha de nacimiento de la persona");

            builder.Property(e => e.Ciudad)
               .HasColumnType("varchar(500)")
               .IsUnicode(false)
               .HasComment("Ciudad donde vive la persona");

            builder.Property(e => e.Pais)
               .HasColumnType("varchar(500)")
               .IsUnicode(false)
               .HasComment("Pais donde vive la persona");

            builder.Property(e => e.Direccion)
               .HasColumnType("varchar(1000)")
               .IsUnicode(false)
               .HasComment("Direccion especifica de la persona");

            builder.Property(e => e.Numero)
               .HasColumnType("varchar(20)")
               .IsUnicode(false)
               .HasComment("Numero donde se situa su ubicación de la persona");

            builder.Property(e => e.Cp)
               .IsUnicode(false)
               .HasComment("Codigo postal de su ubicación de la persona");

            builder.Property(e => e.Telefono)
               .HasColumnType("varchar(10)")
               .IsUnicode(false)
               .HasComment("Numero telefonico de la persona");

            builder.Property(columna => columna.EsActivo)
                .HasComment("Indica si el registro se encuentra activo y se puede usar");

            builder.Property(columna => columna.EsEliminado)
                .HasComment("Indica si el registro a sido eliminado(eliminado logico)");

            builder.Property(e => e.EsEliminado).HasComment("Indica si el registro a sido eliminado(eliminado logico)");

            builder.Property(e => e.FechaAlta)
                .HasColumnType("datetime")
                .HasComment("Fecha en que se crea el registro");

            builder.Property(columna => columna.UsuarioAlta)
                .HasColumnType("varchar(200)")
                .HasComment("Usuario que crea el registro");

            builder.Property(columna => columna.FechaModificacion)
                .HasColumnType("datetime")
                .HasComment("Ultima fecha que se actualiza el registro");

            builder.Property(columna => columna.UsuarioModificacion)
                .HasColumnType("varchar(20)")
                .HasComment("Ultimo usuario que actualiza el registro");
        }
    }
}
