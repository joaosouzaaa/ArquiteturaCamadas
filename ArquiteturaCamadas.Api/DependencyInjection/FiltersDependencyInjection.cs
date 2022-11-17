using ArquiteturaCamadas.Api.Filters;

namespace ArquiteturaCamadas.Api.DependencyInjection
{
    public static class FiltersDependencyInjection
    {
        public static void AddFiltersDependencyInjection(this IServiceCollection services)
        {
            services.AddMvc(options => options.Filters.AddService<NotificationFilter>());
            services.AddMvc(options => options.Filters.AddService<UnitOfWorkFilter>());

            services.AddScoped<NotificationFilter>();
            services.AddScoped<UnitOfWorkFilter>();
        }
    }
}
