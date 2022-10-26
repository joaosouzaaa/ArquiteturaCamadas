using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Settings.NotificationSettings;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiteturaCamadas.Ioc
{
    public static class OthersConfiguration
    {
        public static void AddOthersConfiguration(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler, NotificationHandler>();
            
            AutoMapperConfigurations.Inicialize();
        }
    }
}
