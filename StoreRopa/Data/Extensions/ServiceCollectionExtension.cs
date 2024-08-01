using Microsoft.EntityFrameworkCore;
using StoreRopa.Data.Repository.Interfeces;
using StoreRopa.Data.Repository;
using StoreRopa.Models;
using StoreRopa.Models.Vo;

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
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPersonasRepository, PersonasRepository>();
            services.AddTransient<IClientesRepository, ClientesRepository>();
            services.AddTransient<IRolesRepository, RolesRepository>();
            services.AddTransient<IEmpleadosRepository, EmpleadosRepository>();
            services.AddSingleton<CurrentUser>();

            return services;
        }
    }
}
