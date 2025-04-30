using System;
using System.Data.SqlClient;
using System.Configuration;
using ASP.NET_Web_Application__.NET_Framework_.Models.Notifications;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace ASP.NET_Web_Application__.NET_Framework_
{
    public partial class SendNotifications : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblResult.Text = "";
                gvNotifications.RowCreated += gvNotifications_RowCreated;
                LoadAppointments();
                LoadPendingNotifications();

            }
        }
        protected void gvNotifications_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
            else if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.TableSection = TableRowSection.TableBody;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string type = ddlNotificationType.SelectedValue;
            string message = txtMessage.Text.Trim();
            lblResult.ForeColor = System.Drawing.Color.Red;

            if (string.IsNullOrEmpty(type))
            {
                lblResult.Text = "Please select a notification type.";
                gvNotifications.RowCreated += gvNotifications_RowCreated;

                LoadPendingNotifications();
                return;
            }

            if (string.IsNullOrEmpty(message))
            {
                lblResult.Text = "Message cannot be empty!";
                lblResult.Style["display"] = "block";
                gvNotifications.RowCreated += gvNotifications_RowCreated;

                LoadPendingNotifications(); 
                return;
            }

            if (string.IsNullOrEmpty(ddlAppointments.SelectedValue))
            {
                lblResult.Text = "Please select an appointment.";
                gvNotifications.RowCreated += gvNotifications_RowCreated;

                LoadPendingNotifications();
                return;
            }

            if (!int.TryParse(ddlAppointments.SelectedValue, out int appointmentId))
            {
                lblResult.Text = "Invalid appointment selected.";
                gvNotifications.RowCreated += gvNotifications_RowCreated;

                LoadPendingNotifications(); 
                return;
            }

            int patientId = GetPatientIdFromAppointment(appointmentId);
            string recipient = GetPatientRecipient(patientId, type);

            if (recipient == "Email Notification not enabled" ||
        recipient == "SMS Notification not enabled" ||
        recipient == "Push Notification not enabled")
            {
                lblResult.Text = recipient;
                gvNotifications.RowCreated += gvNotifications_RowCreated;

                LoadPendingNotifications();
                return;
            }

            var notification = NotificationFactory.CreateNotification(type);
            string output = notification.Send(message, recipient);

            SaveToLog(appointmentId, recipient, type, message); // DB log save

            lblResult.ForeColor = System.Drawing.Color.Green;
            lblResult.Text = "Notification created: " + output;
            gvNotifications.RowCreated += gvNotifications_RowCreated;
          
            
            LoadAppointments();
            LoadPendingNotifications();
        }
        private void LoadPendingNotifications(string filter = "")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM NotificationLogs WHERE Status = 'Pending'";

                if (!string.IsNullOrEmpty(filter))
                {
                    query += " AND (Recipient LIKE @Filter OR Message LIKE @Filter)";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(filter))
                {
                    cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                }

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvNotifications.DataSource = reader;
                gvNotifications.DataBind();
            }
        }

        private void LoadAppointments()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
            SELECT A.AppointmentId, 
                   CONCAT('Appt for ', P.Name, ' on ', FORMAT(A.AppointmentDate, 'yyyy-MM-dd HH:mm')) AS Info
            FROM Appointment A
            JOIN Patients P ON A.PatientId = P.PatientId
            WHERE A.AppointmentId NOT IN (
                SELECT AppointmentId 
                FROM NotificationLogs 
                WHERE Status = 'Pending' AND AppointmentId IS NOT NULL
            )";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlAppointments.Items.Clear(); // Always clear first

                if (reader.HasRows)
                {
                    ddlAppointments.DataSource = reader;
                    ddlAppointments.DataTextField = "Info";
                    ddlAppointments.DataValueField = "AppointmentId";
                    ddlAppointments.DataBind();

                    ddlAppointments.Items.Insert(0, new ListItem("-- Select an Appointment --", ""));
                    ddlAppointments.Enabled = true;
                }
                else
                {
                    ddlAppointments.Items.Add(new ListItem("No appointments available", ""));
                    ddlAppointments.Enabled = false; // Disable if empty
                }
            }
        }

        private int GetPatientIdFromAppointment(int appointmentId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT PatientId FROM Appointment WHERE AppointmentId = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", appointmentId);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        private string GetPatientRecipient(int patientId, string type)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
            SELECT 
                Email, 
                PhoneNumber, 
                Name, 
                EmailNotificationEnabled, 
                SmsNotificationEnabled, 
                PushNotificationEnabled 
            FROM Patients 
            WHERE PatientId = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", patientId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool isEmailEnabled = reader["EmailNotificationEnabled"] != DBNull.Value && (bool)reader["EmailNotificationEnabled"];
                        bool isSmsEnabled = reader["SmsNotificationEnabled"] != DBNull.Value && (bool)reader["SmsNotificationEnabled"];
                        bool isPushEnabled = reader["PushNotificationEnabled"] != DBNull.Value && (bool)reader["PushNotificationEnabled"];

                        switch (type)
                        {
                            case "Email":
                                if (isEmailEnabled)
                                    return reader["Email"]?.ToString() ?? string.Empty;
                                else
                                    return "Email Notification not enabled";
                            case "SMS":
                                if (isSmsEnabled)
                                    return reader["PhoneNumber"]?.ToString() ?? string.Empty;
                                else
                                    return "SMS Notification not enabled";
                            case "Push":
                                if (isPushEnabled)
                                {
                                    string name = reader["Name"]?.ToString() ?? string.Empty;
                                    return $"{name}".Trim();
                                }
                                else
                                    return "Push Notification not enabled";
                        }
                    }
                }
            }
            return string.Empty;
        }


        private void SaveToLog(int appointmentId, string recipient, string type, string message, string status = "Pending")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    INSERT INTO NotificationLogs 
                    (AppointmentId, Recipient, NotificationType, Message, Status, CreatedAt)
                    VALUES 
                    (@AId, @Recipient, @Type, @Msg, @Status, @CreatedAt)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AId", appointmentId);
                cmd.Parameters.AddWithValue("@Recipient", recipient);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Msg", message);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
