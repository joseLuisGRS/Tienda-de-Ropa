using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Configuration;
using StoreRopa.Models;

namespace StoreRopa.Data
{
    public partial class StoreDBContext : DbContext
    {
        public StoreDBContext() { }

        public StoreDBContext(DbContextOptions<StoreDBContext> options) : base(options) { }

        public virtual DbSet<Persona> Persona { get; set; } = null!;
        public virtual DbSet<Cliente> Cliente { get; set; } = null!;
        public virtual DbSet<Roles> Roles { get; set; } = null!;
        public virtual DbSet<Empleados> Empleados { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonaConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new RolConfiguration());
            modelBuilder.ApplyConfiguration(new EmpleadoConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
