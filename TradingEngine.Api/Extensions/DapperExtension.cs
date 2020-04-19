using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingEngine.Logic.Utils;

namespace TradingEngine.Api.Extensions
{
    public static class DapperExtension
    {
        public static IServiceCollection InitializeDapperConnectionString(this IServiceCollection services, string connectionString)
        {
            Initer.InitializeDapperConnectionString(connectionString);

            return services;
        }
    }
}
