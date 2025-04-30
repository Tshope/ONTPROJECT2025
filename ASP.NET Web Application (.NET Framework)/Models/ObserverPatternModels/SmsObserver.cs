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
        private readonly int _patientId;

        public SmsObserver(string phoneNumber, int patientId)
        {
            _phoneNumber = phoneNumber;
            _patientId = patientId;
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
                    Message = message,
                    PatientId = _patientId // Correctly using passed-in PatientId
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