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
	public class ConcreteStrategyBaseImmediateDelivery
	{
        public class ImmediateDeliveryStrategy : INotificationDeliveryStrategy
        {
            public string StrategyName => "Immediate Delivery";
            private readonly NotificationDAL _dal = NotificationDAL.GetInstance();

            public void Deliver(List<Notification> notifications)
            {
                foreach (var notification in notifications)
                {
                    Thread.Sleep(500); // Simulate delivery time
                    _dal.UpdateNotificationStatus(notification.NotificationId, "Delivered", StrategyName);
                }
            }
        }
    }
}