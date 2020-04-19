using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngine.Logic.Utils
{
    public static class Initer
    {
        public static string DbConnectionString { get; private set; }
        public static void InitializeDapperConnectionString(string connectionString)
        {
            DbConnectionString = connectionString;
        }
    }
}
