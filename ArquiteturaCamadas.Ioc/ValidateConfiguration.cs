using ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation;
using ArquiteturaCamadas.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ArquiteturaCamadas.Ioc
{
    public static class ValidateConfiguration
    {
        public static void AddValidateConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IValidator<Person>, PersonValidation>();
        }
    }
}
