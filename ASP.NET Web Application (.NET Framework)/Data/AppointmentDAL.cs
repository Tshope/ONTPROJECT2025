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


        public List<AppointmentViewModel> GetUpcomingAppointments()
        {
            var appointments = new List<AppointmentViewModel>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(@"
    SELECT a.AppointmentId, p.Name AS PatientName, a.Doctor, 
           a.AppointmentDate, a.Reason
    FROM Appointment a
    JOIN Patients p ON a.PatientId = p.PatientId
    WHERE a.AppointmentDate >= @Today
    ORDER BY a.AppointmentDate ASC", connection))

                {
                    command.Parameters.AddWithValue("@Today", DateTime.Today);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime appointmentDate = (DateTime)reader["AppointmentDate"];
                            appointments.Add(new AppointmentViewModel
                            {
                                PatientName = reader["PatientName"].ToString(),
                                Doctor = reader["Doctor"].ToString(),
                                AppointmentDate = appointmentDate.Date,
                                AppointmentTime = appointmentDate.ToString("h:mm tt"),
                                Reason = reader["Reason"].ToString()
                            });
                        }
                    }
                }
            }
            return appointments;
        }

        public bool AreAllSlotsTakenForDate(DateTime date)
        {
            // Get count of all possible slots per day
            const int totalSlotsPerDay = 7; // Based on your dropdown (7 time slots)
            const int totalDoctors = 5; // Based on your code (5 doctors)
            int maxPossibleAppointments = totalSlotsPerDay * totalDoctors;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(@"
                    SELECT COUNT(*) FROM Appointment 
                    WHERE CONVERT(date, AppointmentDate) = @Date", connection))
                {
                    command.Parameters.AddWithValue("@Date", date.Date);
                    connection.Open();
                    int bookedAppointments = (int)command.ExecuteScalar();

                    // If all slots are taken, return true
                    return bookedAppointments >= maxPossibleAppointments;
                }
            }
        }

        public List<string> GetAvailableTimeSlotsForDate(DateTime date, string doctor)
        {
            // Define all possible time slots based on your dropdown
            List<string> allTimeSlots = new List<string> { "9:00 AM", "10:00 AM", "11:00 AM", "1:00 PM", "2:00 PM", "3:00 PM", "4:00 PM" };
            List<string> bookedTimes = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(@"
                    SELECT CONVERT(varchar(15), AppointmentDate, 100) as BookedTime
                    FROM Appointment 
                    WHERE CONVERT(date, AppointmentDate) = @Date AND Doctor = @Doctor", connection))
                {
                    command.Parameters.AddWithValue("@Date", date.Date);
                    command.Parameters.AddWithValue("@Doctor", doctor);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string timeStr = reader["BookedTime"].ToString();
                            // Extract time part only (format: "MMM DD YYYY HH:MM(AM/PM)")
                            timeStr = timeStr.Substring(timeStr.IndexOf(' ', 7) + 1);
                            bookedTimes.Add(timeStr);
                        }
                    }
                }
            }

            // Return available time slots by removing booked ones
            return allTimeSlots.Except(bookedTimes).ToList();
        }

        public bool IsSlotTaken(DateTime appointmentDateTime, string doctor)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(@"
                    SELECT COUNT(*) FROM Appointment
                    WHERE Doctor = @Doctor 
                    AND CONVERT(smalldatetime, AppointmentDate) = CONVERT(smalldatetime, @AppointmentDate)", connection))
                {
                    command.Parameters.AddWithValue("@Doctor", doctor);
                    command.Parameters.AddWithValue("@AppointmentDate", appointmentDateTime);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

    }
}