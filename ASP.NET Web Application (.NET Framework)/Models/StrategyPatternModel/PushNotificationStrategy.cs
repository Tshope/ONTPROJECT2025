using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel
{
    public class PushNotificationStrategy : INotificationStrategy
    {
        public void SendNotification(Patient patient, string message)
        {
            // Implement push notification logic
            Console.WriteLine($"Sending Push Notification to {patient.Name}: {message}");
        }
    }
}