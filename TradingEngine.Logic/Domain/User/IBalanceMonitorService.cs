using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngine.Logic.Domain.User
{
    public interface IBalanceMonitorService
    {
        Task<bool> ChargeCeiling(User userToMonitor,Currency currencyToMonitor,decimal ceilingAmount);
    }
}
