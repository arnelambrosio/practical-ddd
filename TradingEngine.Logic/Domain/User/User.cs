using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using TradingEngine.Logic.Common;
using TradingEngine.Logic.SharedKernel;
using TradingEngine.Logic.Utils;

namespace TradingEngine.Logic.Domain.User
{
    public class User : AggregateRoot
    {
        public virtual string Username { get; protected set; }

        [Description("ignore")]
        public virtual Balance Balance { get; protected set; }

        public User(string userName, Balance balance) : this()
        {
            Username = userName;
            Balance = balance;
        }

        private User()
        {

        }

        public virtual void Exchange(Money money, Currency to)
        {
            Balance.Exchange(money, to);
        }

        public virtual void ReceiveMoney(Money money)
        {
            Balance.AddMoney(money);
        }

        public virtual Dictionary<string, decimal> GetBalance()
        {
            throw new NotImplementedException();
        }

        public virtual void SendMoney(User userTo, Money money)
        {
            Balance.ChargeMoney(money);
        }
    }
}
