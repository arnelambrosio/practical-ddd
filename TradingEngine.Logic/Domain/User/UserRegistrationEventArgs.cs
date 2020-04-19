using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Logic.Domain.User
{
    public class UserRegistrationEventArgs : EventArgs
    {
        public int UserId { get;}
        public string Username { get;}

        public UserRegistrationEventArgs(int userId, string userName)
        {
            UserId = userId;
            Username = userName;
        }
    }
}
