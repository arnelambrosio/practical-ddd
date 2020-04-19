using System;
using System.Collections.Generic;
using System.Text;
using TradingEngine.Logic.Common;

namespace TradingEngine.Logic.Domain.Currencies
{
    public class CurrencyRepository : Repository<Currency>
    {
        public CurrencyRepository():base("Currency")
        {

        }
    }
}
