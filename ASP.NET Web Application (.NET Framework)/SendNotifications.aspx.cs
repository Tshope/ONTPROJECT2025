using System;
using System.Data.SqlClient;
using System.Configuration;
using ASP.NET_Web_Application__.NET_Framework_.Models.Notifications;
using System.Web.UI.WebControls;
using System.Web.UI;
using ASP.NET_Web_Application__.NET_Framework_.Data;
using System.Collections.Generic;
using ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel;
using System.Diagnostics;
using System.Linq;

namespace ASP.NET_Web_Application__.NET_Framework_
{
    public partial class SendNotifications : System.Web.UI.Page
    {
        private SendNotificationsDAL processor = new SendNotificationsDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNotifications();
                LoadAppointments();
            }
        }


        private void LoadPendingNotifications()
        {
            gvNotifications.DataSource = processor.GetNotificationsWithAppointments()
                .Where(n => n.Status == "Pending")
                .ToList();
            gvNotifications.DataBind();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlAppointments.SelectedValue))
                {
                    lblResult.Text = "Please select an appointment.";
                    lblResult.CssClass = "text-danger";
                    return;
                }

                int appointmentId = Convert.ToInt32(ddlAppointments.SelectedValue);
                List<string> results = processor.ProcessNotificationsForAppointment(appointmentId);

                if (results.Count > 0)
                {
                    lblResult.Text = string.Join("<br/>", results);
                    lblResult.CssClass = "text-success";
                }
                else
                {
                    lblResult.Text = "No notifications were processed.";
                    lblResult.CssClass = "text-warning";
                }

                LoadPendingNotifications();
                LoadAppointments();
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error: " + ex.Message;
                lblResult.CssClass = "text-danger";
            }
        }




        private void LoadNotifications()
        {
            SendNotificationsDAL dal = new SendNotificationsDAL();
            var notifications = dal.GetNotificationsWithAppointments();

            gvNotifications.DataSource = notifications;
            gvNotifications.DataBind();
        }



        private void LoadAppointments()
        {
            SendNotificationsDAL dal = new SendNotificationsDAL();
            var appointments = dal.GetAllAppointments();

            ddlAppointments.DataSource = appointments;
            ddlAppointments.DataTextField = "AppointmentDescription"; // shows: appt for Name Date
            ddlAppointments.DataValueField = "AppointmentId";
            ddlAppointments.DataBind();


        }




    }
}
