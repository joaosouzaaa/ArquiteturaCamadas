using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Domain.Enums;

namespace ArquiteturaCamadas.Business.Settings.NotificationSettings
{
    public class NotificationHandler : INotificationHandler
    {
        private readonly List<DomainNotification> _notificationList;

        public NotificationHandler()
        {
            _notificationList = new List<DomainNotification>();
        }

        public List<DomainNotification> GetAllNotifications() => _notificationList;

        public bool HasNotifications() => _notificationList.Any();

        public bool AddDomainNotification(string key, string message)
        {
            var domainNotification = new DomainNotification
            {
                Key = key,
                Message = message
            };

            _notificationList.Add(domainNotification);

            return false;
        }
    }
}
