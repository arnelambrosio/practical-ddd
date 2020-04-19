using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingEngine.Api.DTOs.Response
{
    public class GetUserBalance
    {
        public string Currency { get; private set; }
        public decimal Amount { get; private set; }

        public GetUserBalance(string currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;
        }
    }
}
