using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using ASP.NET_Web_Application__.NET_Framework_.Data;
using ASP.NET_Web_Application__.NET_Framework_.Models;
using ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel.ASP.NET_Web_Application__.NET_Framework_.Models;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel
{
    namespace ASP.NET_Web_Application__.NET_Framework_.Models
    {
        public class Notification
        {
            public int NotificationId { get; set; }

            [Required(ErrorMessage = "Message is required.")]
            [DataType(DataType.DateTime)]
            public string Message { get; set; }

            [Required(ErrorMessage = "Created date is required.")]
            [DataType(DataType.DateTime)]
            [Display(Name = "Created At")]
            public DateTime CreatedAt { get; set; }

            [Required(ErrorMessage = "Delivery date is required.")]
            [DataType(DataType.DateTime)]
            [Display(Name = "Delivered At")]
            public DateTime? DeliveredAt { get; set; }

            [Required(ErrorMessage = "Delivery date is required.")]
            [DataType(DataType.DateTime)]
            [Display(Name = "Delivery Method")]
            public string DeliveryMethod { get; set; }

            [Required(ErrorMessage = "Status is required.")]
            [DataType(DataType.DateTime)]
            public string Status { get; set; }
        }
    }

    namespace ASP.NET_Web_Application__.NET_Framework_.Strategies
    {
        public interface INotificationDeliveryStrategy
        {
            string StrategyName { get; }
            void Deliver(List<Notification> notifications);
        }

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