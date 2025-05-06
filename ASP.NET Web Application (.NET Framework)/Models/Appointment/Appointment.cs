using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.Appointment
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Patient is required.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Reason for visit is required.")]
        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters.")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Doctor name is required.")]
        [StringLength(100, ErrorMessage = "Doctor name cannot exceed 100 characters.")]
        public string Doctor { get; set; }

        public bool Notify24h { get; set; }
        public bool Notify1h { get; set; }

        public Patient Patient { get; set; }  // navigation-like property for joined data
    }
}