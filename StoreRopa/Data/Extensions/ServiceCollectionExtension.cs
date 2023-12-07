using Microsoft.EntityFrameworkCore;

namespace StoreRopa.Data.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("StringConnection"));
            });
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            return services;
        }
    }
}
