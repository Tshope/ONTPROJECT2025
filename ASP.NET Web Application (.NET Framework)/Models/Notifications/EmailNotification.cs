using ASP.NET_Web_Application__.NET_Framework_.Models;
using System;
using System.Net.Mail;
using System.Configuration;
using System.Net;
namespace ASP.NET_Web_Application__.NET_Framework_.Models.Notifications
{
    public class EmailNotification : INotification
    {
        public string Send(string recipient, string message)
        {
            try
            {
                var smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
                var smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUser);
                mail.To.Add(recipient);
                mail.Subject = "Healthcare Appointment Reminder";
                mail.Body = message;

                SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                smtp.EnableSsl = true;

                smtp.Send(mail);

                return $"Email sent to {recipient}";
            }
            catch (Exception ex)
            {
                return $"Failed to send email: {ex.Message}";
            }
        }
    }
}
