using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;

namespace ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation
{
    public sealed class TagValidation : AbstractValidator<Tag>
    {
        public TagValidation()
        {
            RuleFor(t => t.TagName).Length(2, 50)
                .WithMessage(t => string.IsNullOrWhiteSpace(t.TagName)
               ? EMessage.Required.Description().FormatTo("Tag Name")
               : EMessage.MoreExpected.Description().FormatTo("Tag Name", "2 to 50"));
        }
    }
}
