using System;
using System.Collections.Generic;
using System.Text;
using TradingEngine.Logic.Common.Events;

namespace TradingEngine.Logic.Domain.User
{
    public class UserRegisteredEventDispatcher
    {
        public static void DispatchOnUserRegistrationEvent(UserRegistrationEventArgs eventArgs)
        {
            var ea = new EventAggregator();
            ea.Register(new UserRegistrationEventHandler());
            ea.Raise(eventArgs);
        }
    }
}
