using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel
{
    public class SmsNotificationStrategy
    {
        public void SendNotification(Patient patient, string message)
        {
            // Implement SMS sending logic
            Console.WriteLine($"Sending SMS to {patient.PhoneNumber}: {message}");
        }
    }
}