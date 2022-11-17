using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.OtherInterfaces;
using ArquiteturaCamadas.Business.Settings.NotificationSettings;
using ArquiteturaCamadas.Infra.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiteturaCamadas.Ioc
{
    public static class OthersConfiguration
    {
        public static void AddOthersConfiguration(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler, NotificationHandler>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            AutoMapperSettings.Inicialize();
        }
    }
}
