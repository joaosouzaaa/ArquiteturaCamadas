using ArquiteturaCamadas.Business.Settings.NotificationSettings;

namespace UnitTests.NotificationTests
{
    public sealed class NotificationHandlerTests
    {
        private readonly NotificationHandler _notification;

        public NotificationHandlerTests()
        {
            _notification = new NotificationHandler();
        }

        [Fact]
        public void AddNotification_ReturnsFalse_NotificationList_HasNotifications()
        {
            // A
            var key = "key here";
            var message = "message here";

            // A
            var addNotification = _notification.AddDomainNotification(key, message);
            var domainNotificationList = _notification.GetAllNotifications();

            // A
            Assert.False(addNotification);
            Assert.NotEmpty(domainNotificationList);
        }

        [Fact]
        public void AddNotification_ReturnsFalse_HasNotifications()
        {
            // A
            var key = "key here";
            var message = "message here";

            // A
            var addNotification = _notification.AddDomainNotification(key, message);
            var hasNotification = _notification.HasNotifications();

            // A
            Assert.False(addNotification);
            Assert.True(hasNotification);
        }
    }
}
