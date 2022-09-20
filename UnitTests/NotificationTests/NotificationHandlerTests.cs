using ArquiteturaCamadas.Business.Settings.NotificationSettings;

namespace UnitTests.NotificationTests
{
    public class NotificationHandlerTests
    {
        NotificationHandler _notification;

        public NotificationHandlerTests()
        {
            _notification = new NotificationHandler();
        }

        [Fact]
        public void AddNotification_HasNotification()
        {
            var key = "key here";
            var message = "message here";

            var addNotification = _notification.AddDomainNotification(key, message);
            var hasNotification = _notification.HasNotification();

            Assert.False(addNotification);
            Assert.True(hasNotification);
        }

        [Fact]
        public void AddNotifications_NotificationListHaveNotifications()
        {
            var addFirstNotification = _notification.AddDomainNotification("random", "random");
            var addSeconNotification = _notification.AddDomainNotification("random", "random");

            var domainNotificationList = _notification.GetAllNotifications();

            Assert.False(addFirstNotification);
            Assert.False(addSeconNotification);
            Assert.Equal(domainNotificationList.Count, 2);
        }
    }
}
