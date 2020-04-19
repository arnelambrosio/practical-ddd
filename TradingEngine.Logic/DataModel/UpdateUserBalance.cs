using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Logic.DataModel
{
    public class UpdateUserBalance
    {
        public int Id { get;}
        public int UserId { get;}
        public int CurrencyId { get;}
        public decimal Amount { get;}

        public UpdateUserBalance(int id, int userId, int currencyId, decimal amount)
        {
            Id = id;
            UserId = userId;
            CurrencyId = currencyId;
            Amount = amount;
        }
    }
}
