using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiteturaCamadas.Ioc
{
    public static class RepositoriesConfiguration
    {
        public static void AddRepositoriesConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();
        }
    }
}
