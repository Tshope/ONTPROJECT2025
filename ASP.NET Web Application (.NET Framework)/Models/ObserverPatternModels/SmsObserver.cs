using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASP.NET_Web_Application__.NET_Framework_.Data;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.ObserverPatternModels
{
    public class SmsObserver : IObserver
    {
        private readonly string _phoneNumber;
        private readonly NotificationDataAccess _dataAccess;

        public SmsObserver(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            _dataAccess = new NotificationDataAccess();
        }

        public void Update(string message)
        {
            try
            {
                var notificationLog = new NotificationLog
                {
                    Recipient = _phoneNumber,
                    NotificationType = "SMS",
                    Message = message
                };

                _dataAccess.SaveNotificationLog(notificationLog);

                string logMessage = $"[{DateTime.Now}] SMS notification sent to {_phoneNumber}: {message}";
                System.Diagnostics.Debug.WriteLine(logMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging notification: {ex.Message}");
                throw;
            }
        }
    }
}