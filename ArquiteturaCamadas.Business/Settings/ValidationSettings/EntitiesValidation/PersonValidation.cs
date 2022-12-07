using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;

namespace ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation
{
    public sealed class PersonValidation : AbstractValidator<Person>
    {
        public PersonValidation()
        {
            RuleFor(p => p.Address).SetValidator(new AddressValidation());

            RuleFor(p => p.Name).Length(3, 50)
                .WithMessage(p => string.IsNullOrWhiteSpace(p.Name)
               ? EMessage.Required.Description().FormatTo("Name")
               : EMessage.MoreExpected.Description().FormatTo("Name", "3 to 50"));

            RuleFor(p => p.Age).GreaterThan(0)
                .WithMessage(EMessage.GreaterThan.Description().FormatTo("Age", "0 years"));
        }
    }
}
