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
                if (!IsValidEmail(recipient))
                {
                    LogError("Invalid email format", recipient);  // Log error
                    return "Failed to send email: Invalid email format.";
                }

                var smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
                var smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUser);
                mail.To.Add(new MailAddress(recipient));
                mail.Subject = "Healthcare Appointment Reminder";
                mail.Body = message;

                SmtpClient smtp = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPassword),
                    EnableSsl = true
                };

                smtp.Send(mail);
                return $"Email sent to {recipient}";
            }
            catch (Exception ex)
            {
                LogError(ex.Message, recipient);  // Log exception details
                return $"Failed to send email: {ex.Message}";
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
<<<<<<< HEAD

        private void LogError(string message, string recipient)
        {
            // Add your logging implementation here (e.g., log to a file or database)
            Console.WriteLine($"Error: {message}, Recipient: {recipient}");
        }
=======
>>>>>>> e686e17639e2233c6b4344fb37643f262cc39eab
    }

}

