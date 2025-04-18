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

            public EmailObserver(string emailAddress)
            {
                _emailAddress = emailAddress;
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
                        Message = message
                    };

                    _dataAccess.SaveNotificationLog(notificationLog);

                    // In a real application, this would also send an actual email
                    string logMessage = $"[{DateTime.Now}] Email notification sent to {_emailAddress}: {message}";
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
