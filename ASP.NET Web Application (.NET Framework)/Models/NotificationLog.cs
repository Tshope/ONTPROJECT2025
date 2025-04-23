using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models
{
    public class NotificationLog
    {
        public int NotificationLogId { get; set; }

        [Required]
        [StringLength(100)]
        public string Recipient { get; set; }

        [Required]
        [StringLength(50)]
        public string NotificationType { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }

        public NotificationLog()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public int? PatientId { get; set; }
    }
}