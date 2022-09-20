using FluentValidation.Results;

namespace ArquiteturaCamadas.Business.Interfaces.Validation
{
    public interface IValidate<TEntity>
        where TEntity : class
    {
        Task<ValidationResult> ValidateAsync(TEntity entity);
    }
}
