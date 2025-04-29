using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASP.NET_Web_Application__.NET_Framework_.Data;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.ObserverPatternModels
{
    public class PushObserver :IObserver
    {
        private readonly NotificationDataAccess _dataAccess;
        private readonly int _patientId;

        public PushObserver(int patientId)
        {
            _dataAccess = new NotificationDataAccess();
            _patientId = patientId;
        }

        public void Update(string message)
        {
            try
            {
                var notificationLog = new NotificationLog
                {
                    Recipient = "Push Notification",  // Generic recipient for push notifications
                    NotificationType = "Push",
                    Message = message,
                    PatientId = _patientId // Correctly using passed-in PatientId
                };

                _dataAccess.SaveNotificationLog(notificationLog);

                string logMessage = $"[{DateTime.Now}] Push notification sent: {message}";
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