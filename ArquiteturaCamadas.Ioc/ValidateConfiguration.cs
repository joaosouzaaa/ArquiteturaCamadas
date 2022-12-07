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
            services.AddScoped<IValidator<Address>, AddressValidation>();
            services.AddScoped<IValidator<Post>, PostValidation>();
            services.AddScoped<IValidator<Tag>, TagValidation>();
            services.AddScoped<IValidator<Student>, PersonValidation>();
            services.AddScoped<IValidator<Project>, ProjectValidation>();
        }
    }
}
