using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel
{
    public interface INotificationStrategy
    {
        void SendNotification(Patient patient, string message);
    }

}
