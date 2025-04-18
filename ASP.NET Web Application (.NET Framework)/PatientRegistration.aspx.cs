using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASP.NET_Web_Application__.NET_Framework_.Data;
using ASP.NET_Web_Application__.NET_Framework_.Models;
using ASP.NET_Web_Application__.NET_Framework_.Models.ObserverPatternModels;

namespace ASP.NET_Web_Application__.NET_Framework_
{
    public partial class PatientRegistration : System.Web.UI.Page
    {
        private NotificationSubject _notificationSubject; // This is the subject that will notify observers
        private NotificationDataAccess _dataAccess; // Data access object for saving notifications 

        protected void Page_Load(object sender, EventArgs e)
        {
            _notificationSubject = new NotificationSubject();
            _dataAccess = new NotificationDataAccess();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                var patient = new Patient
                {
                    Name = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    PhoneNumber = txtPhone.Text.Trim(),
                    EmailNotificationEnabled = chkEmail.Checked,
                    SmsNotificationEnabled = chkSms.Checked,
                    PushNotificationEnabled = chkPush.Checked
                };

                // Set up observers based on preferences
                if (patient.EmailNotificationEnabled)
                {
                    _notificationSubject.Attach(new EmailObserver(patient.Email));
                }
                if (patient.SmsNotificationEnabled)
                {
                    _notificationSubject.Attach(new SmsObserver(patient.PhoneNumber));
                }
                if (patient.PushNotificationEnabled)
                {
                    _notificationSubject.Attach(new PushObserver());
                }

                // Save to database
                _dataAccess.SavePatient(patient);

                // Notify observers
                _notificationSubject.Notify($"New patient registered: {patient.Name}");

                // Show success message
                pnlSuccess.Visible = true;
                litSuccess.Text = "Patient registered successfully!"; 

                ClearForm(); // calling the method to clear the form controlls
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                litError.Text = "Error registering patient: " + ex.Message;
            }

            // a method to clear controlls after submission
            chkPush.Checked = false;
        }
        // a method to clear controlls after submission
        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            chkEmail.Checked = false;
            chkSms.Checked = false;
            chkPush.Checked = false;
        }
    }
} 