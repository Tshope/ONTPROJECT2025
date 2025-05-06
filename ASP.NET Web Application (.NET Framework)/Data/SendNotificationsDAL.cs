using ASP.NET_Web_Application__.NET_Framework_.Models;
using ASP.NET_Web_Application__.NET_Framework_.Models.Appointment;
using ASP.NET_Web_Application__.NET_Framework_.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                    // Create the appropriate notification sender using factory pattern
                    INotification notifier = NotificationFactory.CreateNotification(notification.NotificationType);

                    // Send the notification
                    string result = notifier.Send(notification.Message, notification.Recipient);
                    results.Add($"{notification.NotificationType}: {result}");

                    // Update notification status to Sent
                    UpdateNotificationStatus(notification.NotificationLogId, "Sent");
                }
                catch (Exception ex)
                {
                    results.Add($"Error processing {notification.NotificationType}: {ex.Message}");
                    // Update status to Failed
                    UpdateNotificationStatus(notification.NotificationLogId, "Pending");
                }
            }

            return results;
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