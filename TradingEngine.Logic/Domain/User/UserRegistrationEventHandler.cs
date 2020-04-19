using System;
using System.Collections.Generic;
using System.Text;
using TradingEngine.Logic.Common.Events;

namespace TradingEngine.Logic.Domain.User
{
    public class UserRegistrationEventHandler : ISubscriber<UserRegistrationEventArgs>
    {
        public void Handle(UserRegistrationEventArgs e)
        {
            //Process the new registered user here...
            //throw new NotImplementedException();
        }
    }
}
