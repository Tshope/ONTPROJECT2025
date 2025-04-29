using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_Web_Application__.NET_Framework_.Models
{
    public class NotificationLog
    {
        [Key]
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

        // This is the foreign key
        public int? PatientId { get; set; }

        // This is the navigation property (optional but highly recommended)
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}