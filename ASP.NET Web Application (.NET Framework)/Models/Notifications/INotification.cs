namespace ASP.NET_Web_Application__.NET_Framework_.Models.Notifications
{
    public interface INotification
    {
        string Send(string message, string recipient);
    }
}
