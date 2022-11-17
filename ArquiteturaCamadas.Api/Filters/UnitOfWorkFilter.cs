using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.OtherInterfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArquiteturaCamadas.Api.Filters
{
    public sealed class UnitOfWorkFilter : ActionFilterAttribute
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationHandler _notificationHandler;

        public UnitOfWorkFilter(IUnitOfWork unitOfWork, INotificationHandler notificationHandler)
        {
            _unitOfWork = unitOfWork;
            _notificationHandler = notificationHandler;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (ExternalMethodFilter.IsMethodGet(context))
                return;

            if (context.Exception == null && context.ModelState.IsValid && _notificationHandler.HasNotifications())
                _unitOfWork.CommitTransaction();
            else
                _unitOfWork.RollbackTransaction();

            base.OnActionExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (ExternalMethodFilter.IsMethodGet(context))
                return;

            _unitOfWork.BeginTransaction();

            base.OnResultExecuting(context);
        }
    }
}
