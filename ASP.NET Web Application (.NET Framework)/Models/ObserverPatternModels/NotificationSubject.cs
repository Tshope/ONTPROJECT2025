using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Web_Application__.NET_Framework_.Models.ObserverPatternModels
{
        public class NotificationSubject : ISubject
        {
            private List<IObserver> _observers = new List<IObserver>(); // a list of observer object

        public void Attach(IObserver observer)
            {
                _observers.Add(observer);
            }

            public void Detach(IObserver observer)
            {
                _observers.Remove(observer);
            }

            public void Notify(string message)
            {
                foreach (var observer in _observers)
                {
                    observer.Update(message);
                }
            }
        public void ClearObservers()  // additional methd to prevent duplicates 
        {
            _observers.Clear();
        }

    }
  

}
 
