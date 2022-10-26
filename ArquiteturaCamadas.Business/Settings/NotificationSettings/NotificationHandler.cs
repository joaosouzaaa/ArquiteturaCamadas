using ArquiteturaCamadas.Business.Interfaces.Notification;

namespace ArquiteturaCamadas.Business.Settings.NotificationSettings
{
    public class NotificationHandler : INotificationHandler
    {
        private List<DomainNotification> _notificationList;

        public NotificationHandler()
        {
            _notificationList = new List<DomainNotification>();
        }

        public List<DomainNotification> GetAllNotifications() => _notificationList;

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
