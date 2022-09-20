using ArquiteturaCamadas.Business.Settings.NotificationSettings;

namespace ArquiteturaCamadas.Business.Interfaces.Notification
{
    public interface INotificationHandler
    {
        List<DomainNotification> GetAllNotifications();
        bool HasNotification();
        bool AddDomainNotification(string key, string message);
    }
}
