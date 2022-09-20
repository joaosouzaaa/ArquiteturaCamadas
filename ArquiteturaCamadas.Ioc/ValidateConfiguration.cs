using ArquiteturaCamadas.Business.Interfaces.Validation;
using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using ArquiteturaCamadas.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiteturaCamadas.Ioc
{
    public static class ValidateConfiguration
    {
        public static void AddValidateConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IValidate<Person>, PersonValidation>();
        }
    }
}
