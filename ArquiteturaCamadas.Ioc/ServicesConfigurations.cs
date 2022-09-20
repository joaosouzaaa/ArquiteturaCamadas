using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiteturaCamadas.Ioc
{
    public static class ServicesConfigurations
    {
        public static void AddServicesConfigurations(this IServiceCollection services)
        {
            services.AddScoped<IPersonService, PersonService>();
        }
    }
}
