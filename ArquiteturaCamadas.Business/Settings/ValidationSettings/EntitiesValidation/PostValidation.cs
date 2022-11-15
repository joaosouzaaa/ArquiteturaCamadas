using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;

namespace ArquiteturaCamadas.Business.Settings.ValidationSettings.EntitiesValidation
{
    public sealed class PostValidation : AbstractValidator<Post>
    {
        public PostValidation()
        {
            RuleFor(p => p.Message).Length(0, 600)
                .WithMessage(p => string.IsNullOrWhiteSpace(p.Message)
               ? EMessage.Required.Description().FormatTo("Message")
               : EMessage.MoreExpected.Description().FormatTo("Message", "0 to 600"));
        }
    }
}
