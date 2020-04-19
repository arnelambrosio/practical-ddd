using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingEngine.Api.DTOs.Request
{
    public class ExchangeMoney
    {
        public int FromCurrencyId { get; set; }
        public decimal Amount { get; set; }
        public int ToCurrencyId { get; set; }
    }
}
