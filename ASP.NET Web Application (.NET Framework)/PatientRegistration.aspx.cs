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
        private readonly int _patientId;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Always initialize your fields
            _notificationSubject = new NotificationSubject();
            _dataAccess = new NotificationDataAccess();

            if (!IsPostBack)
            {
                LoadPatients();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            pnlSuccess.Visible = false;
            pnlError.Visible = false;

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
                    PushNotificationEnabled = chkPush.Checked,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                int patientId = _dataAccess.SavePatient(patient);

                if (patientId == -1)
                {
                    // Duplicate found
                    pnlError.Visible = true;
                    litError.Text = "A patient with this email and phone number already exists.";

                    ClearForm();
                    return; 
                }

                patient.PatientId = patientId;

                _notificationSubject = new NotificationSubject();

                if (patient.EmailNotificationEnabled)
                    _notificationSubject.Attach(new EmailObserver(patient.Email, patient.PatientId));

                if (patient.SmsNotificationEnabled)
                    _notificationSubject.Attach(new SmsObserver(patient.PhoneNumber, patient.PatientId));

                if (patient.PushNotificationEnabled)
                    _notificationSubject.Attach(new PushObserver(patient.PatientId));

                _notificationSubject.Notify($"New patient registered: {patient.Name}");

                pnlSuccess.Visible = true;
                litSuccess.Text = "Patient registered successfully!";
                ClearForm();
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                litError.Text = "Error registering patient: " + ex.Message;
            }

            chkPush.Checked = false;
            LoadPatients(); // Refresh
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
      
        // method to load patients 
        private void LoadPatients()
        {
            try
            {
                var patients = _dataAccess.GetAllPatients(); // Get all patients from the database
                gvPatients.DataSource = patients;
                gvPatients.DataBind();
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                litError.Text = "Error loading patients: " + ex.Message;
            }
        }
        //  
        protected void gvPatients_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPatients.EditIndex = e.NewEditIndex;
            LoadPatients();
        }

        protected void gvPatients_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPatients.EditIndex = -1;
            LoadPatients();
        }

        protected void gvPatients_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string email = gvPatients.DataKeys[e.RowIndex].Value.ToString();

            GridViewRow row = gvPatients.Rows[e.RowIndex];
            bool emailSub = ((CheckBox)row.Cells[3].Controls[0]).Checked;
            bool smsSub = ((CheckBox)row.Cells[4].Controls[0]).Checked;
            bool pushSub = ((CheckBox)row.Cells[5].Controls[0]).Checked;

            // Update in DB
            _dataAccess.UpdatePatientSubscription(email, emailSub, smsSub, pushSub);

            gvPatients.EditIndex = -1;
            LoadPatients();
        }

        protected void gvPatients_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            pnlSuccess.Visible = false;
            pnlError.Visible = false; 
            
            try
            {
                string email = gvPatients.DataKeys[e.RowIndex].Value.ToString();
                _dataAccess.DeletePatient(email);

                pnlSuccess.Visible = true;
                litSuccess.Text = "Patient removed successfully!";
                LoadPatients();
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                litError.Text = "Error deleting patient: " + ex.Message;
            }

        } 

    }
} 