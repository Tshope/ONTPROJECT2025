using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel
{
    public class EmailNotificationStrategy : INotificationStrategy
    {
        public void SendNotification(Patient patient, string message)
        {
            // Implement email sending logic
            Console.WriteLine($"Sending email to {patient.Email}: {message}");
        }
    }
}