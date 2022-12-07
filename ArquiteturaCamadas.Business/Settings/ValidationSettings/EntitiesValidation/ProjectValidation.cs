using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;

namespace ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation
{
    public sealed class ProjectValidation : AbstractValidator<Project>
    {
        public ProjectValidation()
        {
            RuleFor(p => p.Name).Length(3, 50)
                .WithMessage(p => string.IsNullOrWhiteSpace(p.Name)
                ? EMessage.Required.Description().FormatTo("Name")
                : EMessage.MoreExpected.Description().FormatTo("Name", "3 to 50"));

            RuleFor(p => p.Value).GreaterThan(0)
                .WithMessage(EMessage.GreaterThan.Description().FormatTo("Value", "0"));

            RuleFor(p => p.ExpiryDate).GreaterThan(DateTime.Now.AddDays(1))
                .WithMessage(EMessage.GreaterThan.Description().FormatTo("Expiry Date", "Today")); ;
        }
    }
}
