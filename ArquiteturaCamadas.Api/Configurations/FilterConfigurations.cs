using ArquiteturaCamadas.Api.Filters;

namespace ArquiteturaCamadas.Api.Configurations
{
    public static class FilterConfigurations
    {
        public static void AddFilterConfigurations(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.AddService<NotificationFilter>();
            });

            services.AddScoped<NotificationFilter>();
        }
    }
}
