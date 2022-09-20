using ArquiteturaCamadas.Business.Interfaces.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace ArquiteturaCamadas.Business.Settings.ValidationSettings
{
    public abstract class Validate<TEntity> : AbstractValidator<TEntity>, IValidate<TEntity>
        where TEntity : class
    {
        public async Task<ValidationResult> ValidateAsync(TEntity entity) =>
            await base.ValidateAsync(entity);
    }
}
