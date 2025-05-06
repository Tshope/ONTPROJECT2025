using ASP.NET_Web_Application__.NET_Framework_.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP.NET_Web_Application__.NET_Framework_
{
    public partial class PatientLogin : System.Web.UI.Page
    {
        private readonly NotificationDataAccess _dataAccess;

        public PatientLogin()
        {
            _dataAccess = new NotificationDataAccess();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any existing session
                Session.Clear();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            try
            {
                if (_dataAccess.ValidatePatientEmail(email))
                {
                    // store the email
                    Session["UserEmail"] = email;

                    // Create authentication ticket
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        email,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        false,
                        "Patient"
                    );

                    // Encrypt the ticket
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    // Create the cookie
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    // Redirect to the patient dashboard 
                    Response.Redirect("~/PatientDashboard.aspx");
                }
                else
                {
                    lblMessage.Text = "Email not found. Please check your email or contact our admin.";
                }
            }
            catch (Exception)
            {
                lblMessage.Text = "An error occurred during login. Please try again later.";
            }
        }

    }
}