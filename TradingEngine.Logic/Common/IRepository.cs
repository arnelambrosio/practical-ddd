using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingEngine.Logic.Common
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteRowAsync(int id);
        Task<T> GetAsync(int id);
        Task<int> SaveRangeAsync(IEnumerable<T> list);
        Task<int> UpdateAsync(T t);
        Task<int> InsertAsync(T t);
    }
}
