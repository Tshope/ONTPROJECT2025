using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.Appointment
{
    public class AppointmentViewModel
    {

        public string PatientName { get; set; }
        public string Doctor { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Reason { get; set; }
    }
}