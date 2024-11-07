using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Configuration;
using StoreRopa.Data.utils;
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
        public virtual DbSet<Ventas> Venta { get; set; } = null!;
        public virtual DbSet<DetalleVentas> DetalleVentas { get; set; } = null!;
        public virtual DbSet<Creditos> Creditos { get; set; } = null!;
        public virtual DbSet<Abonos> Abonos { get; set; } = null!; 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonaConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new RolConfiguration());
            modelBuilder.ApplyConfiguration(new EmpleadoConfiguration());
            modelBuilder.ApplyConfiguration(new VentasConfiguration());
            modelBuilder.ApplyConfiguration(new DetalleVentasConfiguration());
            modelBuilder.ApplyConfiguration(new CreditosConfiguration());
            modelBuilder.ApplyConfiguration(new AbonoConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    public class StoreIdentityDbContext : IdentityDbContext<ApplicationUser>
    {

        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) : base(options) { }

    }
}
