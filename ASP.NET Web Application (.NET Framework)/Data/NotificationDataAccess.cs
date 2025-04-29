using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ASP.NET_Web_Application__.NET_Framework_.Models;
using ASP.NET_Web_Application__.NET_Framework_.Models.Appointment;
using ASP.NET_Web_Application__.NET_Framework_.Models.ObserverPatternModels;

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
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Check if patient exists
                  string checkSql = @"
                  SELECT COUNT(*) 
                  FROM Patients 
                  WHERE Email = @Email AND PhoneNumber = @PhoneNumber";

                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@Email", patient.Email);
                checkCmd.Parameters.AddWithValue("@PhoneNumber", patient.PhoneNumber);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                   
                    return -1;
                }

                //Insert new patient
                string insertSql = @"
            INSERT INTO Patients 
            (Name, Email, PhoneNumber, EmailNotificationEnabled, SmsNotificationEnabled, PushNotificationEnabled, CreatedAt, UpdatedAt)
            OUTPUT INSERTED.PatientId
            VALUES 
            (@Name, @Email, @PhoneNumber, @EmailNotificationEnabled, @SmsNotificationEnabled, @PushNotificationEnabled, @CreatedAt, @UpdatedAt)";

                SqlCommand insertCmd = new SqlCommand(insertSql, conn);
                insertCmd.Parameters.AddWithValue("@Name", patient.Name);
                insertCmd.Parameters.AddWithValue("@Email", patient.Email);
                insertCmd.Parameters.AddWithValue("@PhoneNumber", patient.PhoneNumber);
                insertCmd.Parameters.AddWithValue("@EmailNotificationEnabled", patient.EmailNotificationEnabled);
                insertCmd.Parameters.AddWithValue("@SmsNotificationEnabled", patient.SmsNotificationEnabled);
                insertCmd.Parameters.AddWithValue("@PushNotificationEnabled", patient.PushNotificationEnabled);
                insertCmd.Parameters.AddWithValue("@CreatedAt", patient.CreatedAt);
                insertCmd.Parameters.AddWithValue("@UpdatedAt", patient.UpdatedAt);

                int newId = (int)insertCmd.ExecuteScalar();
                patient.PatientId = newId;
                return newId;
            }
        }
   
        public List<Patient> GetAllPatients()
        {
            var patients = new List<Patient>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string query = @"
            SELECT 
                p.PatientId,
                p.Name,
                p.Email,
                p.PhoneNumber,
                p.EmailNotificationEnabled,
                p.SmsNotificationEnabled,
                p.PushNotificationEnabled,
                CASE 
                    WHEN EXISTS (SELECT 1 FROM Appointment a WHERE a.PatientId = p.PatientId) THEN 'Yes'
                    ELSE 'No'
                END AS HasAppointment
            FROM Patients p";

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
                        PushNotificationEnabled = Convert.ToBoolean(reader["PushNotificationEnabled"]),
                        HasAppointment = reader["HasAppointment"].ToString()
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
                conn.Open();

                // Start a transaction to ensure both deletions succeed together
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Get the PatientId based on email
                    string getIdSql = "SELECT PatientId FROM Patients WHERE Email = @EmailAddress";
                    SqlCommand getIdCmd = new SqlCommand(getIdSql, conn, transaction);
                    getIdCmd.Parameters.AddWithValue("@EmailAddress", email);
                    var patientId = getIdCmd.ExecuteScalar();

                    if (patientId != null)
                    {
                        // Delete related logs
                        string deleteLogsSql = "DELETE FROM NotificationLogs WHERE PatientId = @PatientId";
                        SqlCommand deleteLogsCmd = new SqlCommand(deleteLogsSql, conn, transaction);
                        deleteLogsCmd.Parameters.AddWithValue("@PatientId", (int)patientId);
                        deleteLogsCmd.ExecuteNonQuery();
                    }
                    // Delete the patient
                    string deletePatientSql = "DELETE FROM Patients WHERE Email = @EmailAddress";
                    SqlCommand deletePatientCmd = new SqlCommand(deletePatientSql, conn, transaction);
                    deletePatientCmd.Parameters.AddWithValue("@EmailAddress", email);
                    deletePatientCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
      }

   
    }
}
 