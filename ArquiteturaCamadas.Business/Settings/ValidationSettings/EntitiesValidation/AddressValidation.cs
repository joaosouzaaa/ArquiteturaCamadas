using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;

namespace ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation
{
    public sealed class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(a => a.ZipCode.CleanCaracters()).Length(8)
                .WithMessage(EMessage.MoreExpected.Description().FormatTo("Zip Code", "8"));

            RuleFor(a => a.City).Length(3, 50)
                .WithMessage(a => string.IsNullOrWhiteSpace(a.City)
                ? EMessage.Required.Description().FormatTo("City")
                : EMessage.MoreExpected.Description().FormatTo("City", "3 to 50"));

            RuleFor(a => a.Street).Length(3, 50)
                .WithMessage(a => string.IsNullOrWhiteSpace(a.Street)
                ? EMessage.Required.Description().FormatTo("Street")
                : EMessage.MoreExpected.Description().FormatTo("Street", "3 to 50"));

            RuleFor(a => a.State).Length(2)
                .WithMessage(EMessage.MoreExpected.Description().FormatTo("State", "2"));

            RuleFor(a => a.Number).Length(1, 10)
                .WithMessage(a => string.IsNullOrWhiteSpace(a.Number)
                    ? EMessage.Required.Description().FormatTo("Number")
                    : EMessage.MoreExpected.Description().FormatTo("Number", "1 to 10"));

            RuleFor(a => a.District).Length(3, 50)
                .WithMessage(a => string.IsNullOrWhiteSpace(a.District)
                ? EMessage.Required.Description().FormatTo("District")
                : EMessage.MoreExpected.Description().FormatTo("District", "3 to 50"));

            When(a => a.Complement is not null, () =>
            {
                RuleFor(a => a.Complement).Length(3, 50)
                    .WithMessage(a => string.IsNullOrWhiteSpace(a.Complement)
                    ? EMessage.Required.Description().FormatTo("Complement")
                    : EMessage.MoreExpected.Description().FormatTo("Complement", "3 to 50"));
            });
        }
    }
}
