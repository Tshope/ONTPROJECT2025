using ASP.NET_Web_Application__.NET_Framework_.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
namespace ASP.NET_Web_Application__.NET_Framework_.Models.Notifications
{
    public class PushNotification : INotification
    {
        
            private readonly string serverKey = ConfigurationManager.AppSettings["FCMServerKey"];

            public string Send(string message, string recipientToken)
            {
                try
                {
                    var request = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Headers.Add($"Authorization: key={serverKey}");

                    var payload = new
                    {
                        to = recipientToken,
                        notification = new
                        {
                            title = "New Notification",
                            body = message,
                            sound = "default"
                        },
                        priority = "high"
                    };

                    string jsonPayload = JsonConvert.SerializeObject(payload);
                    byte[] byteArray = Encoding.UTF8.GetBytes(jsonPayload);

                    using (Stream dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }

                    using (var response = request.GetResponse())
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseFromServer = reader.ReadToEnd();
                        return $"Push sent successfully: {responseFromServer}";
                    }
                }
                catch (Exception ex)
                {
                    return $"Error sending push: {ex.Message}";
                }
            }
        
    }
}
