using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASP.NET_Web_Application__.NET_Framework_.Data;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.ObserverPatternModels
{
    public class EmailObserver : IObserver
    {
        private readonly string _emailAddress;
        private readonly NotificationDataAccess _dataAccess;
        private readonly int _patientId;

        public EmailObserver(string emailAddress, int patientId)
        {
            _emailAddress = emailAddress;
            _patientId = patientId;
            _dataAccess = new NotificationDataAccess();
        }

        public void Update(string message)
        {
            try
            {
                var notificationLog = new NotificationLog
                {
                    Recipient = _emailAddress,
                    NotificationType = "Email",
                    Message = message,
                    PatientId = _patientId // Correctly using passed-in PatientId
                };

                _dataAccess.SaveNotificationLog(notificationLog);

                // Simulate sending an email (for debugging/logging)
                string logMessage = $"[{DateTime.Now}] Email notification sent to {_emailAddress} (PatientID: {_patientId}): {message}";
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
