using ASP.NET_Web_Application__.NET_Framework_.Data;
using ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel.ASP.NET_Web_Application__.NET_Framework_.Models;
using ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel.ASP.NET_Web_Application__.NET_Framework_.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel
{
	public class ConcreteStrategyBaseBatchDelivery
	{
        public class BatchDeliveryStrategy : INotificationDeliveryStrategy
        {
            public string StrategyName => "Batch Delivery";
            private readonly NotificationDAL _dal = NotificationDAL.GetInstance();
            private readonly int _batchSize;

            public BatchDeliveryStrategy(int batchSize)
            {
                _batchSize = batchSize;
            }

            public void Deliver(List<Notification> notifications)
            {
                if (notifications.Count < _batchSize)
                {
                    return;
                }

                foreach (var notification in notifications)
                {
                    _dal.UpdateNotificationStatus(
                        notification.NotificationId,
                        "Delivered",
                        $"{StrategyName} ({_batchSize})"
                    );
                }
            }
        }
    }
}