using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Validation;
using ArquiteturaCamadas.Domain.Entities.EntityBase;
using FluentValidation;

namespace ArquiteturaCamadas.ApplicationService.Services.ServiceBase
{
    public abstract class BaseService<TEntity>
        where TEntity : BaseEntity
    {
        private readonly IValidate<TEntity> _validator;
        protected readonly INotificationHandler _notification;

        protected BaseService(IValidate<TEntity> validator, INotificationHandler notification)
        {
            _validator = validator;
            _notification = notification;
        }

        protected async Task<bool> ValidateAsync(TEntity entity)
        {
            var validationResult = await _validator.ValidateAsync(entity);

            if (validationResult.IsValid)
                return validationResult.IsValid;

            foreach(var error in validationResult.Errors)
                _notification.AddDomainNotification(error.PropertyName, error.ErrorMessage);

            return validationResult.IsValid;
        }
    }
}
