using ASP.NET_Web_Application__.NET_Framework_.Data;
using ASP.NET_Web_Application__.NET_Framework_.Models.Appointment;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace ASP.NET_Web_Application__.NET_Framework_
{
    public partial class BookAppointment : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate doctor dropdown
                PopulateDoctors();

                // Set calendar to not allow past dates
                calendar.DayRender += Calendar_DayRender;

                // Set default date to tomorrow
                calendar.SelectedDate = DateTime.Today.AddDays(1);

                // Clear any previous messages
                lblMessage.Text = string.Empty;

                // ✅ Load patients from DAL
                ddlPatient.DataSource = AppointmentDAL.GetInstance().GetCustomersWithoutAppointments();
                ddlPatient.DataTextField = "Name";
                ddlPatient.DataValueField = "PatientId";
                ddlPatient.DataBind();

                ddlPatient.Items.Insert(0, new ListItem("-- Select Patient --", ""));

            }

        }

        private void PopulateDoctors()
        {
            // Ensure dropdown only has the default item
            ddlDoctor.Items.Clear();
            ddlDoctor.Items.Add(new ListItem("-- Select Doctor --", ""));

            // Add doctors
            ddlDoctor.Items.Add(new ListItem("Dr. Smith", "Smith"));
            ddlDoctor.Items.Add(new ListItem("Dr. Jones", "Jones"));
            ddlDoctor.Items.Add(new ListItem("Dr. Adams", "Adams"));
            ddlDoctor.Items.Add(new ListItem("Dr. Wilson", "Wilson"));
            ddlDoctor.Items.Add(new ListItem("Dr. Taylor", "Taylor"));
        }

        protected void Calendar_DayRender(object sender, DayRenderEventArgs e)
        {
            // Disable past dates
            if (e.Day.Date < DateTime.Today)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
                e.Cell.BackColor = System.Drawing.Color.LightGray;
            }

        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {

                    int patientId = int.Parse(ddlPatient.SelectedValue);
                    string reason = txtReason.Text.Trim();
                    string doctor = ddlDoctor.SelectedValue;
                    string time = ddlTime.SelectedValue;


                    // Validate required fields
                    if (string.IsNullOrEmpty(doctor))
                    {
                        ShowErrorMessage("Please select a doctor");
                        return;
                    }

                    if (string.IsNullOrEmpty(time))
                    {
                        ShowErrorMessage("Please select a time");
                        return;
                    }

                    // Validate appointment date
                    if (calendar.SelectedDate == DateTime.MinValue)
                    {
                        ShowErrorMessage("Please select a valid appointment date.");
                        return;
                    }

                    // Create appointment date with time component
                    DateTime appointmentDate = calendar.SelectedDate;
                    if (!string.IsNullOrEmpty(time))
                    {
                        try
                        {
                            TimeSpan selectedTime = TimeSpan.Parse(time);
                            appointmentDate = appointmentDate.Date + selectedTime;
                        }
                        catch (FormatException)
                        {
                            ShowErrorMessage("Invalid time format selected");
                            return;
                        }
                    }

                    // Get notification preferences
                    bool notify24h = chkNotify24h.Checked;
                    bool notify1h = chkNotify1h.Checked;

                    // Create appointment object
                    Appointment appointment = new Appointment
                    {
                        PatientId = int.Parse(ddlPatient.SelectedValue),
                        AppointmentDate = appointmentDate,
                        Reason = reason,
                        Doctor = doctor,
                        Notify24h = notify24h,
                        Notify1h = notify1h,
                    };

                    // Save appointment to database
                    bool success = AppointmentDAL.GetInstance().AddAppointment(appointment);

                    if (success)
                    {
                        // Show success message
                        ShowSuccessMessage($"Your appointment has been successfully booked with {ddlDoctor.SelectedItem.Text} on {appointmentDate.ToString("MMM dd, yyyy")} at {appointmentDate.ToString("h:mm tt")}.");

                        // Clear the form
                        ClearForm();
                    }
                    else
                    {
                        ShowErrorMessage("There was an error booking your appointment. Please try again later.");
                    }
                }
                catch (SqlException sqlEx)
                {
                    System.Diagnostics.Debug.WriteLine($"SQL Error booking appointment: {sqlEx.Message}\n{sqlEx.StackTrace}");
                    ShowErrorMessage("Database error. Please contact support if the problem persists.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error booking appointment: {ex.Message}\n{ex.StackTrace}");
                    ShowErrorMessage($"An unexpected error occurred: {ex.Message}");
                }
            }
        }

        protected void calendar_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime today = DateTime.Today;

           
            if (e.Day.Date < today || e.Day.Date.DayOfWeek == DayOfWeek.Saturday || e.Day.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
                e.Cell.BackColor = System.Drawing.Color.LightGray;
                e.Cell.Font.Strikeout = true;
            }
        }


        protected void BtnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            // Clear text fields
            txtReason.Text = string.Empty;

            // Reset dropdowns
            ddlDoctor.SelectedIndex = 0;
            ddlTime.SelectedIndex = 0;
            ddlPatient.SelectedIndex = 0;

            // Reset calendar to tomorrow
            calendar.SelectedDate = DateTime.Today.AddDays(1);

            // Reset checkboxes
            chkNotify24h.Checked = true;
            chkNotify1h.Checked = false;

            // Clear messages
            lblMessage.Text = string.Empty;
            lblMessage.CssClass = string.Empty;
        }


        private void ShowSuccessMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.CssClass = "appointment-message appointment-success";
        }

        private void ShowErrorMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.CssClass = "appointment-message appointment-error";
        }
    }
}