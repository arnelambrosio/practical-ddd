using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingEngine.Logic.Common;
using TradingEngine.Logic.DataModel;
using TradingEngine.Logic.SharedKernel;


namespace TradingEngine.Logic.Domain.User
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository() : base("User")
        {

        }

        public override async Task<int> InsertAsync(User t)
        {
            string sql = "INSERT INTO [User] (Username)VALUES(@Username);SELECT CAST(SCOPE_IDENTITY() as int)";
            var createdUserId = 0;
            using (var connection = CreateConnection())
            {
                createdUserId = (await connection.QueryAsync<int>(sql, t)).Single();
            }

            if (createdUserId > 0)
            {
                //delegate the response here to raise an event, best practice is to delegate it to the persistence event
                UserRegisteredEventDispatcher.DispatchOnUserRegistrationEvent(new UserRegistrationEventArgs(createdUserId, t.Username));

            }

            return createdUserId;
        }

        public async Task<User> GetByIdIncludingBalanceAsync(int id)
        {
            string sql = @"SELECT        dbo.Balance.Id, dbo.Balance.UserId,dbo.[User].Username,dbo.Balance.CurrencyId, dbo.Currency.Name AS [CurrencyName], dbo.Balance.Amount, dbo.Currency.Ratio
                            FROM            dbo.Balance INNER JOIN
                                dbo.Currency ON dbo.Balance.CurrencyId = dbo.Currency.Id INNER JOIN
                                      dbo.[User] ON dbo.Balance.UserId = dbo.[User].Id
                                            WHERE UserId=@id";

            IEnumerable<GetUserBalance> result = null;

            var userInfo = await GetAsync(id);

            using (var connection = CreateConnection())
            {
                result = await connection.QueryAsync<GetUserBalance>(sql, new { id = id });
            }

            var balance = new Balance();

            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    balance.AddMoney(
                        new Money(new Currency(item.CurrencyName, item.Ratio) { Id = item.CurrencyId }, item.Amount));
                }
            }

            return new User(userInfo.Username, balance) { Id = id };
        }

        public async Task<bool> UpdateBalanceAsync(User user)
        {
            var currentBalance = user.Balance.GetAllMoney();
            var update = "";
            var insert = "";

            try
            {
                foreach (var item in currentBalance)
                {
                    var currencyExist = await CurrencyExist(user.Id, item.Currency.Name);

                    if (currencyExist)
                    {
                        update = $"UPDATE [Balance] SET Amount = @amount WHERE UserId = @userId AND CurrencyId = @currencyId";
                        using (var connection = CreateConnection())
                        {
                            await connection.ExecuteAsync(update, new { amount = item.Amount, userId = user.Id, currencyId = item.Currency.Id });
                        }

                    }
                    else
                    {
                        insert = $"INSERT INTO [Balance] (UserId,CurrencyId,Amount)VALUES(@userId,@currencyId,@amount)";
                        using (var connection = CreateConnection())
                        {
                            await connection.ExecuteAsync(insert, new { userId = user.Id, currencyId = item.Currency.Id, amount = item.Amount });
                        }
                    }

                }

                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<bool> CurrencyExist(int userId, string currency)
        {
            string sql = @"SELECT   COUNT(dbo.Currency.Name) AS Expr1
                            FROM dbo.Balance INNER JOIN
                                    dbo.Currency ON dbo.Balance.CurrencyId = dbo.Currency.Id
                                        WHERE(dbo.Balance.UserId = @userId) AND(dbo.Currency.Name = @currency)";

            using (var connection = CreateConnection())
            {
                var result = (await connection.QueryAsync<int>(sql, new { userId, currency })).FirstOrDefault();

                return result > 0 ? true : false;
            }
        }
    }
}
