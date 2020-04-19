using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingEngine.Logic.Common;

namespace TradingEngine.Logic.Domain.User
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdIncludingBalanceAsync(int id);
        Task<bool> UpdateBalanceAsync(User user);
    }
}
