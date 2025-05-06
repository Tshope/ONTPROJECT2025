using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models
{
    public class NotificationWithAppointmentViewModel
    {

        public string NotificationType { get; set; }
        public string Reason { get; set; }
        public string Doctor { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PatientName { get; set; }

        public string AppointmentDescription =>
        AppointmentDate > DateTime.MinValue
            ? $"appt for {PatientName} {AppointmentDate:yyyy-MM-dd HH:mm}"
            : "";
    }
}