using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using ASP.NET_Web_Application__.NET_Framework_.Models;
using ASP.NET_Web_Application__.NET_Framework_.Models.Appointment;
using ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel.ASP.NET_Web_Application__.NET_Framework_.Models;

namespace ASP.NET_Web_Application__.NET_Framework_.Data
{
    public class NotificationDAL
    {
        //    private static NotificationDAL _instance;
        //    private static readonly object _lock = new object();
        //    private readonly string connectionString;

        //    // Private constructor to prevent external instantiation
        //    private NotificationDAL()
        //    {
        //        connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //    }

        //    // Singleton instance getter
        //    public static NotificationDAL GetInstance()
        //    {
        //        if (_instance == null)
        //        {
        //            lock (_lock)
        //            {
        //                if (_instance == null)
        //                {
        //                    _instance = new NotificationDAL();
        //                }
        //            }
        //        }
        //        return _instance;
        //    }

        //    // Add notification to database
        //    public bool AddNotification(Notification notification)
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            string query = @"INSERT INTO Notifications (Message, CreatedAt, Status, DeliveryMethod)
        //                         VALUES (@Message, @CreatedAt, @Status, @DeliveryMethod)";

        //            SqlCommand cmd = new SqlCommand(query, conn);
        //            cmd.Parameters.AddWithValue("@Message", notification.Message);
        //            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
        //            cmd.Parameters.AddWithValue("@Status", "Pending");
        //            cmd.Parameters.AddWithValue("@DeliveryMethod", DBNull.Value);

        //            conn.Open();
        //            int rowsAffected = cmd.ExecuteNonQuery();
        //            return rowsAffected > 0;
        //        }
        //    }

        //    // Get all pending notifications
        //    public List<Notification> GetPendingNotifications()
        //    {
        //        List<Notification> notifications = new List<Notification>();

        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            string query = @"SELECT NotificationId, Message, CreatedAt 
        //                        FROM Notifications 
        //                        WHERE Status = 'Pending'
        //                        ORDER BY CreatedAt";

        //            SqlCommand cmd = new SqlCommand(query, con);
        //            con.Open();

        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    notifications.Add(new Notification
        //                    {
        //                        NotificationId = Convert.ToInt32(reader["NotificationId"]),
        //                        Message = reader["Message"].ToString(),
        //                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
        //                        Status = "Pending"
        //                    });
        //                }
        //            }
        //        }

        //        return notifications;
        //    }

        //    // Update notification status after delivery
        //    public bool UpdateNotificationStatus(int notificationId, string status, string deliveryMethod)
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            string query = @"UPDATE Notifications 
        //                        SET Status = @Status, 
        //                            DeliveryMethod = @DeliveryMethod,
        //                            DeliveredAt = CASE WHEN @Status = 'Delivered' THEN GETDATE() ELSE NULL END
        //                        WHERE NotificationId = @NotificationId";

        //            SqlCommand cmd = new SqlCommand(query, conn);
        //            cmd.Parameters.AddWithValue("@Status", status);
        //            cmd.Parameters.AddWithValue("@DeliveryMethod", deliveryMethod);
        //            cmd.Parameters.AddWithValue("@NotificationId", notificationId);

        //            conn.Open();
        //            int rowsAffected = cmd.ExecuteNonQuery();
        //            return rowsAffected > 0;
        //        }
        //    }

        //    // Get delivery history
        //    public List<Notification> GetDeliveryHistory()
        //    {
        //        List<Notification> notifications = new List<Notification>();

        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            string query = @"SELECT NotificationId, Message, CreatedAt, DeliveredAt, DeliveryMethod
        //                        FROM Notifications 
        //                        WHERE Status = 'Delivered'
        //                        ORDER BY DeliveredAt DESC";

        //            SqlCommand cmd = new SqlCommand(query, con);
        //            con.Open();

        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    notifications.Add(new Notification
        //                    {
        //                        NotificationId = Convert.ToInt32(reader["NotificationId"]),
        //                        Message = reader["Message"].ToString(),
        //                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
        //                        DeliveredAt = reader["DeliveredAt"] != DBNull.Value ?
        //                                      Convert.ToDateTime(reader["DeliveredAt"]) : (DateTime?)null,
        //                        DeliveryMethod = reader["DeliveryMethod"].ToString(),
        //                        Status = "Delivered"
        //                    });
        //                }
        //            }
        //        }

        //        return notifications;
        //    }
        //}
        // new code for DAL  

        private static NotificationDAL _instance;
        private static readonly object _lock = new object();
        private readonly string connectionString;

        private NotificationDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public static NotificationDAL GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new NotificationDAL();
                }
            }
            return _instance;
        }

        // Log notification to NotificationLogs
        public bool AddNotificationLog(NotificationLog log)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO NotificationLogs 
                                 (Recipient, NotificationType, Message, CreatedAt, PatientId, AppointmentId, Status)
                                 VALUES (@Recipient, @NotificationType, @Message, @CreatedAt, @PatientId, @AppointmentId, @Status)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Recipient", log.Recipient ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NotificationType", log.NotificationType);
                cmd.Parameters.AddWithValue("@Message", log.Message);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@PatientId", log.PatientId);
                cmd.Parameters.AddWithValue("@AppointmentId", log.AppointmentId);
                cmd.Parameters.AddWithValue("@Status", log.Status);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Get appointments requiring notification
        public List<Appointment> GetUpcomingAppointmentsToNotify(TimeSpan timeBeforeAppointment)
        {
            var appointments = new List<Appointment>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT a.AppointmentId, a.PatientId, a.AppointmentDate, a.Reason, a.Doctor,
                                        p.Name, p.Email, p.PhoneNumber, p.EmailNotificationEnabled, p.SmsNotificationEnabled
                                 FROM Appointment a
                                 JOIN Patients p ON a.PatientId = p.PatientId
                                 WHERE a.AppointmentDate BETWEEN GETDATE() AND DATEADD(MINUTE, @TimeWindowMinutes, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TimeWindowMinutes", (int)timeBeforeAppointment.TotalMinutes);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                            Reason = reader["Reason"].ToString(),
                            Doctor = reader["Doctor"].ToString(),
                            Patient = new Patient
                            {
                                PatientId = Convert.ToInt32(reader["PatientId"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                EmailNotificationEnabled = Convert.ToBoolean(reader["EmailNotificationEnabled"]),
                                SmsNotificationEnabled = Convert.ToBoolean(reader["SmsNotificationEnabled"])
                            }
                        });
                    }
                }
            }

            return appointments;
        }

        // Get notification log history
        public List<NotificationLog> GetNotificationHistory()
        {
            var logs = new List<NotificationLog>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM NotificationLogs ORDER BY CreatedAt DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logs.Add(new NotificationLog
                        {
                            NotificationLogId = Convert.ToInt32(reader["NotificationLogId"]),
                            Recipient = reader["Recipient"].ToString(),
                            NotificationType = reader["NotificationType"].ToString(),
                            Message = reader["Message"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }

            return logs;
        }
    }
}
