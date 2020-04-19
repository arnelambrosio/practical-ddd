using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingEngine.Logic.Common;
using TradingEngine.Logic.Domain;
using TradingEngine.Logic.Domain.Currencies;
using TradingEngine.Logic.Domain.User;

namespace TradingEngine.Api.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection InitializeRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Currency>, CurrencyRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            return services;
        }

    }
}
