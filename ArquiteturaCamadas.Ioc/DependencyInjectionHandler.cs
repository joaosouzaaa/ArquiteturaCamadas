using ArquiteturaCamadas.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiteturaCamadas.Ioc
{
    public static class DependencyInjectionHandler
    {
        public static void AddDependencyInjectionHandler(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ArquiteturaCamadasDbContext>();

            services.AddDbContext<ArquiteturaCamadasDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddOthersConfiguration();
            services.AddValidateConfiguration();
            services.AddRepositoriesConfiguration();
            services.AddServicesConfigurations();
        }
    }
}
