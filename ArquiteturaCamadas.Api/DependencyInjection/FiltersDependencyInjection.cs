using ArquiteturaCamadas.Api.Filters;

namespace ArquiteturaCamadas.Api.DependencyInjection
{
    public static class FiltersDependencyInjection
    {
        public static void AddFiltersDependencyInjection(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.AddService<NotificationFilter>();
            });

            services.AddScoped<NotificationFilter>();
        }
    }
}
