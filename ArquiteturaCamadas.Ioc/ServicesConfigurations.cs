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
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICepService, CepService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IProjectService, ProjectService>();
        }
    }
}
