using ASP.NET_Web_Application__.NET_Framework_.Models;
using ASP.NET_Web_Application__.NET_Framework_.Models.Appointment;
using ASP.NET_Web_Application__.NET_Framework_.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Data
{
    public class SendNotificationsDAL
    {
        private readonly string _connectionString;

        public SendNotificationsDAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<NotificationWithAppointmentViewModel> GetNotificationsWithAppointments()
        {
            List<NotificationWithAppointmentViewModel> notifications = new List<NotificationWithAppointmentViewModel>();

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                            SELECT 
                                p.Name AS PatientName,
                                STRING_AGG(n.NotificationType, ', ') AS NotificationTypes,
                                MAX(a.Reason) AS Reason,
                                MAX(a.Doctor) AS Doctor,
                                MAX(n.Status) AS Status,
                                MAX(n.CreatedAt) AS CreatedAt,
                                MAX(a.AppointmentDate) AS AppointmentDate
                            FROM NotificationLogs n
                            INNER JOIN Patients p ON n.PatientId = p.PatientId
                            INNER JOIN Appointment a ON p.PatientId = a.PatientId -- Only include those with appointments
                            GROUP BY p.PatientId, p.Name
                            ORDER BY MAX(n.CreatedAt) DESC;";




                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime appointmentDate = reader["AppointmentDate"] != DBNull.Value
                                ? Convert.ToDateTime(reader["AppointmentDate"])
                                : DateTime.MinValue; // Handle DBNull case

                            notifications.Add(new NotificationWithAppointmentViewModel
                            {
                                PatientName = reader["PatientName"].ToString(),
                                NotificationType = reader["NotificationTypes"].ToString(),
                                Reason = reader["Reason"].ToString(),
                                Doctor = reader["Doctor"].ToString(),
                                Status = reader["Status"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                AppointmentDate = appointmentDate // No nullable DateTime

                            });
                        }
                    }
                }
            }

            return notifications;
        }





        public List<Appointment> GetAllAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                                a.AppointmentId, 
                                a.AppointmentDate, 
                                a.PatientId, 
                                p.Name AS PatientName
                            FROM Appointment a
                            INNER JOIN Patients p ON a.PatientId = p.PatientId
                            ORDER BY a.AppointmentDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(new Appointment
                            {
                                AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                                AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                                PatientId = Convert.ToInt32(reader["PatientId"]),
                                PatientName = reader["PatientName"].ToString()
                            });


                        }
                    }
                }
            }

            return appointments;
        }


        public List<NotificationLog> GetPendingNotificationsByAppointmentId(int appointmentId)
        {
            List<NotificationLog> notifications = new List<NotificationLog>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        n.NotificationLogId,
                        n.PatientId,
                        n.NotificationType,
                        n.Status,
                        n.CreatedAt,
                        n.Recipient,
                        n.Message
                    FROM NotificationLogs n
                    INNER JOIN Patients p ON n.PatientId = p.PatientId
                    INNER JOIN Appointment a ON p.PatientId = a.PatientId
                    WHERE a.AppointmentId = @AppointmentId AND n.Status = 'Pending'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new NotificationLog
                            {
                                NotificationLogId = Convert.ToInt32(reader["NotificationLogId"]),
                                PatientId = Convert.ToInt32(reader["PatientId"]),
                                NotificationType = reader["NotificationType"].ToString(),
                                Status = reader["Status"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                Recipient = reader["Recipient"].ToString(),
                                Message = reader["Message"].ToString()
                            });
                        }
                    }
                }
            }

            return notifications;
        }

        // Send notifications for a specific appointment and update their status
        public List<string> ProcessNotificationsForAppointment(int appointmentId)
        {
            List<string> results = new List<string>();
            List<NotificationLog> notifications = GetPendingNotificationsByAppointmentId(appointmentId);

            // If there are no pending notifications for this appointment
            if (notifications.Count == 0)
            {
                results.Add("No pending notifications found for this appointment.");
                return results;
            }

            foreach (var notification in notifications)
            {
                try
                {
                    string result = string.Empty;

                    // Check if recipient is an email or phone number
                    if (IsValidEmail(notification.Recipient))
                    {
                        // Send Email
                        INotification emailNotifier = new EmailNotification();
                        result = emailNotifier.Send(notification.Recipient, notification.Message);
                    }
                    else if (IsValidPhoneNumber(notification.Recipient))
                    {
                        // Send SMS
                        INotification smsNotifier = new SmsNotification();
                        result = smsNotifier.Send(notification.Message, notification.Recipient);
                    }
                    else
                    {
                        result = $"Invalid recipient format: {notification.Recipient}";
                    }

                    // Log the result of sending the notification
                    results.Add($"{notification.NotificationType}: {result}");

                    // Update notification status to Sent if valid
                    if (result.Contains("sent"))
                    {
                        UpdateNotificationStatus(notification.NotificationLogId, "Sent");
                    }
                    else
                    {
                        // If failed, log the error message and update status to Failed
                        UpdateNotificationStatus(notification.NotificationLogId, "Failed");
                        results.Add($"Error: {result} for {notification.NotificationType}");
                    }
                }
                catch (Exception ex)
                {
                    // Log exception and set notification status to Failed
                    results.Add($"Error processing {notification.NotificationType}: {ex.Message}");
                    UpdateNotificationStatus(notification.NotificationLogId, "Failed");
                }
            }

            return results;
        }


        private bool IsValidEmail(string recipient)
        {
            try
            {
                var addr = new MailAddress(recipient);
                return addr.Address == recipient;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string recipient)
        {
            // Assuming phone numbers start with a '0' and are 10 digits long (modify this as per your requirement)
            return recipient.Length == 10 && recipient.StartsWith("0") && recipient.All(char.IsDigit);
        }

        // Update notification status in the database
        private void UpdateNotificationStatus(int notificationLogId, string status)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE NotificationLogs SET Status = @Status";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    
                    cmd.Parameters.AddWithValue("@NotificationLogId", notificationLogId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
    }