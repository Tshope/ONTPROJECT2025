using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.ObserverPatternModels
{
    public class PatientWithAppointmentStatusViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailNotificationEnabled { get; set; }
        public bool SmsNotificationEnabled { get; set; }
        public bool PushNotificationEnabled { get; set; }
        public string HasAppointment { get; set; }  // "Yes" or "No"
    }
}