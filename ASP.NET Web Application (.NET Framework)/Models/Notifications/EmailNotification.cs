using ASP.NET_Web_Application__.NET_Framework_.Models;
using System;
namespace ASP.NET_Web_Application__.NET_Framework_.Models.Notifications
{
    public class EmailNotification : INotification
    {
        public string Send(string recipient, string message)
        {
            return $"[EMAIL to {recipient}]: {message}";
        }
    }
}
