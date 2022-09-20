using ArquiteturaCamadas.Business.Interfaces.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArquiteturaCamadas.Api.Filters
{
    public class NotificationFilter : ActionFilterAttribute
    {
        private readonly INotificationHandler _notification;

        public NotificationFilter(INotificationHandler notification)
        {
            _notification = notification;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (_notification.HasNotification())
                context.Result = new BadRequestObjectResult(_notification.GetAllNotifications());

            base.OnActionExecuted(context);
        }
    }
}
