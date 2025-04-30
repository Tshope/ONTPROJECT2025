using System;
namespace ASP.NET_Web_Application__.NET_Framework_.Models.Notifications
{
    public static class NotificationFactory
    {
        public static INotification CreateNotification(string type)
        {
            switch (type.ToLower())
            {
                case "email":
                    return new EmailNotification();
                case "sms":
                    return new SmsNotification();
                case "push":
                    return new PushNotification();
                default:
                    throw new ArgumentException("Invalid notification type");
            }
        }
    }
}
