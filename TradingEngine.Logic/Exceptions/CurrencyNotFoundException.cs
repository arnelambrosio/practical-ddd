using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Logic.Exceptions
{
    public class CurrencyNotFoundException : Exception
    {
        public CurrencyNotFoundException():base("Currency not found")
        {

        }
    }
}
