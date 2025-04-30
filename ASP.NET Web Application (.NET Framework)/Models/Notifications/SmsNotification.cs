using ASP.NET_Web_Application__.NET_Framework_.Models;
using System;
namespace ASP.NET_Web_Application__.NET_Framework_.Models.Notifications
{
    public class SmsNotification : INotification
    {
        public string Send(string message, string recipient)
        {
            return $"[SMS to {recipient}]: {message}";
        }
    }
}
