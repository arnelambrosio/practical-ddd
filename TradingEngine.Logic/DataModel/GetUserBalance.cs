using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Logic.DataModel
{
    public class GetUserBalance
    {
        public int UserId { get; protected set; }
        public string Username { get; set; }
        public int CurrencyId { get; protected set; }
        public string CurrencyName { get; protected set; }
        public decimal Ratio { get; protected set; }
        public decimal Amount { get; protected set; }
    }
}
