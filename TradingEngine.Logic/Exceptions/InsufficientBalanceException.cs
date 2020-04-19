using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Logic.Exceptions
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException():base("Insufficient balance")
        {

        }
    }
}
