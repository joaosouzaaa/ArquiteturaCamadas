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
            var notificationList = _notification.GetAllNotifications();

            if (notificationList.Any())
                context.Result = new BadRequestObjectResult(notificationList);

            base.OnActionExecuted(context);
        }
    }
}
