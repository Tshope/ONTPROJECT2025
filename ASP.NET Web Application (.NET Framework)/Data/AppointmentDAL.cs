using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using ASP.NET_Web_Application__.NET_Framework_.Models.Appointment;
using ASP.NET_Web_Application__.NET_Framework_.Models;
using System.Data.SqlTypes;

namespace ASP.NET_Web_Application__.NET_Framework_.Data
{
    public class AppointmentDAL
    {
        private static AppointmentDAL _instance;
        private static readonly object _lock = new object();
        private readonly string connectionString;

        // Private constructor to prevent external instantiation
        private AppointmentDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // Singleton instance getter
        public static AppointmentDAL GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppointmentDAL();
                    }
                }
            }
            return _instance;
        }

        // Add appointment using model
        public bool AddAppointment(Appointment appointment)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string query = @"INSERT INTO Appointment (PatientId, AppointmentDate, Reason, Doctor, Notify24h, Notify1h)
                 VALUES (@PatientId, @AppointmentDate, @Reason, @Doctor, @Notify24h, @Notify1h)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                cmd.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                cmd.Parameters.AddWithValue("@Reason", appointment.Reason);
                cmd.Parameters.AddWithValue("@Doctor", appointment.Doctor);
                cmd.Parameters.AddWithValue("@Notify24h", appointment.Notify24h);
                cmd.Parameters.AddWithValue("@Notify1h", appointment.Notify1h);


                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }


        public List<Patient> GetCustomersWithoutAppointments()
        {
            List<Patient> customers = new List<Patient>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Query to find patients who do not have an appointment
                string query = @"
            SELECT p.PatientId, p.Name 
            FROM Patients p
            LEFT JOIN Appointment a ON p.PatientId = a.PatientId
            WHERE a.AppointmentId IS NULL
            ORDER BY p.Name";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Patient
                        {
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            Name = reader["Name"].ToString()
                        });
                    }
                }
            }

            return customers;
        }

        public bool IsSlotTaken(DateTime dateTime, string doctor)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                string query = @"SELECT COUNT(*) 
                         FROM Appointment 
                         WHERE AppointmentDate = @DateTime AND Doctor = @Doctor";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DateTime", dateTime);
                cmd.Parameters.AddWithValue("@Doctor", doctor);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }


    }
}