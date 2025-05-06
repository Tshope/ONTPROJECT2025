using ASP.NET_Web_Application__.NET_Framework_.Data;
using System;
using System.Web.UI;

namespace ASP.NET_Web_Application__.NET_Framework_
{
    public partial class PatientDashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user email is stored in Session
                if (Session["UserEmail"] != null)
                {
                    lblPatientName.Text = Session["UserEmail"].ToString();
                }
                else
                {
                    // Redirect to login if session has expired or not set
                    Response.Redirect("PatientLogin.aspx");
                }
            }
        }
    }
}
