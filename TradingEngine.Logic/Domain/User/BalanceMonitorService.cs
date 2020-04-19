using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngine.Logic.Domain.User
{
    public class BalanceMonitorService : IBalanceMonitorService
    {
        public Task<bool> ChargeCeiling(User userToMonitor, Currency currencyToMonitor, decimal ceilingAmount)
        {
            throw new NotImplementedException();
        }
    }
}
