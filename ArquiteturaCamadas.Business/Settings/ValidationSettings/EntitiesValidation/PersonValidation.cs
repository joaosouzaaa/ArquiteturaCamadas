using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;

namespace ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation
{
    public class PersonValidation : Validate<Person>
    {
        public PersonValidation()
        {
            RuleFor(p => p.Name).Length(3, 50)
                .WithMessage(p => string.IsNullOrWhiteSpace(p.Name)
               ? EMessage.Required.Description().FormatTo("Name")
               : EMessage.MoreExpected.Description().FormatTo("Name", "3 to 50"));

            RuleFor(p => p.Age).GreaterThan(18)
                .WithMessage(EMessage.InvalidAge.Description().FormatTo("{PropertyValue}"));
        }
    }
}
