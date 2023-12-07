using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StoreRopa.Data
{
    public class DBContextFactory : IDesignTimeDbContextFactory<StoreDBContext>
    {
        public IConfiguration? Configuration { get; }
        StoreDBContext IDesignTimeDbContextFactory<StoreDBContext>.CreateDbContext(string[] args)
        {
            var pathConnection = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(pathConnection, false);
            var fileConfiguration = configBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<StoreDBContext>();
            optionsBuilder.UseSqlServer(fileConfiguration.GetConnectionString("StringConnection"));

            return new StoreDBContext(optionsBuilder.Options);
        }
    }
}
