using ASP.NET_Web_Application__.NET_Framework_.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http.Headers;
namespace ASP.NET_Web_Application__.NET_Framework_.Models.Notifications
{
    public class SmsNotification : INotification
    {
        public string Send(string recipient, string message)
        {
            try
            {
                // Convert to international format (e.g. 27821234567)
                if (recipient.StartsWith("0"))
                    recipient = "27" + recipient.Substring(1);

                var result = SendSmsAsync(recipient, message).Result;
                return result;
            }
            catch (Exception ex)
            {
                return $"Error processing SMS: {ex.Message}";
            }
        }

        private async Task<string> SendSmsAsync(string to, string message)
        {
            try
            {
                var apiKey = ConfigurationManager.AppSettings["InfobipApiKey"];
                var baseUrl = ConfigurationManager.AppSettings["InfobipBaseUrl"];
                var sender = ConfigurationManager.AppSettings["InfobipSender"];

                var client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("App", apiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var payload = new
                {
<<<<<<< HEAD
                    messages = new[] {
=======
>>>>>>> e686e17639e2233c6b4344fb37643f262cc39eab
                new {
                    from = sender,
                    destinations = new[] { new { to = to } },
                    text = message
                }
            }
<<<<<<< HEAD
                };
=======
            };
>>>>>>> e686e17639e2233c6b4344fb37643f262cc39eab

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/sms/2/text/advanced", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return $"SMS sent to {to}";
                else
                {
                    LogError("SMS API error: " + responseContent, to);  // Log error
                    return $"Failed: {responseContent}";
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, to);  // Log exception details
                return $"Failed to send SMS: {ex.Message}";
            }
        }

        private void LogError(string message, string recipient)
        {
            // Add your logging implementation here (e.g., log to a file or database)
            Console.WriteLine($"Error: {message}, Recipient: {recipient}");
        }

    }
}
