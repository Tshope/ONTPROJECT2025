using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ASP.NET_Web_Application__.NET_Framework_.Models;

namespace ASP.NET_Web_Application__.NET_Framework_.Data
{
    
        public class NotificationDataAccess
        {
            private readonly string _connectionString;

            public NotificationDataAccess()
            {
                _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }

            public void SaveNotificationLog(NotificationLog log)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("INSERT INTO NotificationLogs (Recipient, NotificationType, Message, CreatedAt) VALUES (@Recipient, @NotificationType, @Message, @CreatedAt)", connection))
                    {
                        command.Parameters.AddWithValue("@Recipient", log.Recipient);
                        command.Parameters.AddWithValue("@NotificationType", log.NotificationType);
                        command.Parameters.AddWithValue("@Message", log.Message);
                        command.Parameters.AddWithValue("@CreatedAt", log.CreatedAt);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }

            public void SavePatient(Patient patient)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand(
                        @"INSERT INTO Patients (Name, Email, PhoneNumber, EmailNotificationEnabled, 
                    SmsNotificationEnabled, PushNotificationEnabled, CreatedAt, UpdatedAt) 
                    VALUES (@Name, @Email, @PhoneNumber, @EmailNotificationEnabled, 
                    @SmsNotificationEnabled, @PushNotificationEnabled, @CreatedAt, @UpdatedAt)", connection))
                    {
                        command.Parameters.AddWithValue("@Name", patient.Name);
                        command.Parameters.AddWithValue("@Email", patient.Email);
                        command.Parameters.AddWithValue("@PhoneNumber", patient.PhoneNumber);
                        command.Parameters.AddWithValue("@EmailNotificationEnabled", patient.EmailNotificationEnabled);
                        command.Parameters.AddWithValue("@SmsNotificationEnabled", patient.SmsNotificationEnabled);
                        command.Parameters.AddWithValue("@PushNotificationEnabled", patient.PushNotificationEnabled);
                        command.Parameters.AddWithValue("@CreatedAt", patient.CreatedAt);
                        command.Parameters.AddWithValue("@UpdatedAt", patient.UpdatedAt);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
 