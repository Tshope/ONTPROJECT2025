using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel
{
    public class NotificationClient
    {
        public INotificationStrategy Strategy { get; set; }

        public NotificationClient(INotificationStrategy strategy)
        {
            this.Strategy = strategy;
        }

        public void Notify(Patient patient, string message)
        {
            Strategy.SendNotification(patient, message);
        }
    }
}