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
                using (var command = new SqlCommand(
                    "INSERT INTO NotificationLogs (Recipient, NotificationType, Message, CreatedAt, PatientId) " +
                    "VALUES (@Recipient, @NotificationType, @Message, @CreatedAt, @PatientId)", connection))
                {
                    command.Parameters.AddWithValue("@Recipient", log.Recipient);
                    command.Parameters.AddWithValue("@NotificationType", log.NotificationType);
                    command.Parameters.AddWithValue("@Message", log.Message);
                    command.Parameters.AddWithValue("@CreatedAt", log.CreatedAt);
                    command.Parameters.AddWithValue("@PatientId", log.PatientId ?? (object)DBNull.Value); 

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public int SavePatient(Patient patient)
        {
            int newPatientId;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(
                    @"INSERT INTO Patients (Name, Email, PhoneNumber, EmailNotificationEnabled, 
            SmsNotificationEnabled, PushNotificationEnabled, CreatedAt, UpdatedAt) 
            VALUES (@Name, @Email, @PhoneNumber, @EmailNotificationEnabled, 
            @SmsNotificationEnabled, @PushNotificationEnabled, @CreatedAt, @UpdatedAt);
            SELECT SCOPE_IDENTITY();", connection))
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
                    newPatientId = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return newPatientId;
        }
   
        // reading patients from the table 
        public List<Patient> GetAllPatients()
        {
            var patients = new List<Patient>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string query = "SELECT * FROM Patients"; // Ensure table name matches
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    patients.Add(new Patient
                    {
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        EmailNotificationEnabled = Convert.ToBoolean(reader["EmailNotificationEnabled"]),
                        SmsNotificationEnabled = Convert.ToBoolean(reader["SmsNotificationEnabled"]),
                        PushNotificationEnabled = Convert.ToBoolean(reader["PushNotificationEnabled"])
                    });
                }
            }

            return patients;
        }
        // managing the patients subscriptions using update and delete 
        public void UpdatePatientSubscription(string email, bool emailSub, bool smsSub, bool pushSub)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE Patients SET EmailNotificationEnabled = @Email, SmsNotificationEnabled = @Sms, PushNotificationEnabled = @Push WHERE Email = @EmailAddress";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", emailSub);
                cmd.Parameters.AddWithValue("@Sms", smsSub);
                cmd.Parameters.AddWithValue("@Push", pushSub);
                cmd.Parameters.AddWithValue("@EmailAddress", email);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeletePatient(string email)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Patients WHERE Email = @EmailAddress";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@EmailAddress", email);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
}
 