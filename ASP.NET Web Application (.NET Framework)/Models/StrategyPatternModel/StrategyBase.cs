using ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel.ASP.NET_Web_Application__.NET_Framework_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.StrategyPatternModel
{
    public abstract class StrategyBase
    {
        public abstract string StrategyName { get; }
        public abstract void Deliver(List<Notification> notifications);
    }
}