using ASP.NET_Web_Application__.NET_Framework_.Data;
using ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel.ASP.NET_Web_Application__.NET_Framework_.Models;
using ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel.ASP.NET_Web_Application__.NET_Framework_.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel
{
	public class ConcreteStrategyBaseScheduledDelivery
	{
        public class ScheduledDeliveryStrategy : INotificationDeliveryStrategy
        {
            public string StrategyName => "Scheduled Delivery";
            private readonly NotificationDAL _dal = NotificationDAL.GetInstance();
            private readonly DateTime _deliveryTime;

            public ScheduledDeliveryStrategy(DateTime deliveryTime)
            {
                _deliveryTime = deliveryTime;
            }

            public void Deliver(List<Notification> notifications)
            {
                var timeUntilDelivery = _deliveryTime - DateTime.Now;
                if (timeUntilDelivery > TimeSpan.Zero)
                {
                    Thread.Sleep(timeUntilDelivery);
                }

                foreach (var notification in notifications)
                {
                    _dal.UpdateNotificationStatus(notification.NotificationId, "Delivered", StrategyName);
                }
            }
        }
    }
}